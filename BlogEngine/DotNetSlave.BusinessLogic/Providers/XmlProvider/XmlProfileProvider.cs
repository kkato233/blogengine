#region Using

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text.RegularExpressions;

#endregion

namespace BlogEngine.Core.Providers
{       
    /// <summary>
    /// XML implementation of the ASP.NET ProfileProvider.
    /// </summary>
    public class XmlProfileProvider : ProfileProvider
    {
        #region Properties

        private XmlDocument _ProfilesXml;

        protected virtual XmlDocument ProfilesXml
        {
            get
            {
                if (this._ProfilesXml == null)
                    this._ProfilesXml = GetProfilesXmlDocument();

                return _ProfilesXml;
            }
        }

        public override string ApplicationName
        {
            get { return "BlogEngine.NET"; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string XmlFilePath
        {
            get
            {
                //return HttpContext.Current.Server.MapPath("~/App_Data/profiles.xml");
                return HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation + "profiles.xml");
                
            }
        }

        #endregion 

        #region Helper Methods

        protected virtual XmlDocument GetProfilesXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            return doc;
        }

        protected virtual bool HasDirtyProperty(SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue setting in collection)
            {
                if (setting.IsDirty)
                    return true;
            }

            // None are dirty
            return false;
        }

        /// <summary>
        /// Searches the <see cref="ProfileFile"/> file for a &lt;user&gt; section with a matching username
        /// and returns this content as a string dictionary.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>A string dictionary object representing the user's settings. If the supplied username is not found,
        /// <b>null</b> is returned.</returns>
        protected virtual Dictionary<string, object> GetUserProfile(string username)
        {
            XmlNode profileXml = GetProfileXmlNode(username.ToLowerInvariant());

            if (profileXml == null)
                return null;        // No profile for the specified username
            else
            {
                Dictionary<string, object> propertyValues = new Dictionary<string, object>();

                foreach (XmlNode xmlProperty in profileXml.ChildNodes)
                {
                    if (xmlProperty.InnerXml.StartsWith("<![CDATA"))
                        propertyValues.Add(xmlProperty.Name, Convert.FromBase64String(xmlProperty.InnerText));
                    else
                        propertyValues.Add(xmlProperty.Name, xmlProperty.InnerXml);
                }

                return propertyValues;
            }
        }

        protected virtual XmlNode CreateUserProfile(string username, DateTime lastUpdatedTime)
        {
            XmlNode node = ProfilesXml.CreateNode(XmlNodeType.Element, "profile", "");
            XmlAttribute usernameAttribute = ProfilesXml.CreateAttribute("userName");
            XmlAttribute lastUpdatedTimeAttribute = ProfilesXml.CreateAttribute("lastUpdatedTime");

            usernameAttribute.Value = username.ToLowerInvariant();
            lastUpdatedTimeAttribute.Value = lastUpdatedTime.ToString();

            node.Attributes.Append(usernameAttribute);
            node.Attributes.Append(lastUpdatedTimeAttribute);

            return node;
        }

        protected virtual XmlNode GetProfileXmlNode(string username)
        {
            XmlNode profile = ProfilesXml.SelectSingleNode("/profiles/profile[@userName=\"" + username.ToLowerInvariant() + "\"]");

            return profile;
        }

        protected virtual void SaveNode(XmlNode node)
        {
            // TODO: Check if node already exists. If so, Replace. Else Add.
        }

        protected virtual void Save()
        {
            ProfilesXml.Save(XmlFilePath);
        }

        #endregion

        #region Initialize Method

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            //ApplicationName = config["applicationName"];

            // TODO: Replace with profilePath
            //if (!string.IsNullOrEmpty(config["profileFolder"]))
            //  ProfileFolder = config["profileFolder"];

            //if (string.IsNullOrEmpty(ApplicationName))
            //    throw new ProviderException("You _must_ provide the 'applicationName' setting when using the XmlProfileProvider");

            // Make sure that there are no unknown settings
            //config.Remove("applicationName");
            //config.Remove("profileFolder");

            if (config.Count > 0)
                throw new ProviderException("Unrecognized attribute: " + config.GetKey(0));
        }

        #endregion

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            int totalRecords = 0;
            ProfileInfoCollection inactiveProfiles = GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, 1, Int32.MaxValue, out totalRecords);

            return DeleteProfiles(inactiveProfiles);
        }

        public override int DeleteProfiles(string[] usernames)
        {
            int deletedProfilesCount = 0;

            foreach (string username in usernames)
            {
                bool wasProfileDeleted = DeleteProfileByUserName(username);

                if (wasProfileDeleted)
                    deletedProfilesCount++;
            }

            return deletedProfilesCount;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            string[] usernames = new string[profiles.Count];

            int i = 0;
            foreach (ProfileInfo profile in profiles)
            {
                usernames[i] = profile.UserName;
                i++;
            }

            return DeleteProfiles(usernames);
        }

        private bool DeleteProfileByUserName(string username)
        {
            XmlNode profile = GetProfileXmlNode(username);

            if (profile != null)
            {
                ProfilesXml.DocumentElement.RemoveChild(profile);
                Save();

                return true;
            }

            return false;
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            int totalRecords = 0;
            ProfileInfoCollection profiles = GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, 1, Int32.MaxValue, out totalRecords);

            return totalRecords;
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection settings = new SettingsPropertyValueCollection();

            // Make sure we have an entry for this username in the XML data
            string username = context["UserName"] as string;
            if (!string.IsNullOrEmpty(username))
            {
                // Make sure username doesn't contain .. in the username (like '../SomeOtherUser/')
                if (username.IndexOf("..") >= 0)
                    throw new ProviderException("Cannot access profile data for users with a username that contains '..'");

                // Get the profile values for the user
                Dictionary<string, object> usersProperties = GetUserProfile(username.ToLowerInvariant());

                foreach (SettingsProperty property in collection)
                {
                    // Indicate that provider-specific serialized properties should be
                    // serialized as strings for primitive types and as XML for non-primitive types
                    if (property.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(String))
                            property.SerializeAs = SettingsSerializeAs.String;
                        else
                            property.SerializeAs = SettingsSerializeAs.Xml;

                    // Create a new SettingsPropertyValue based on the current SettingsProperty object
                    SettingsPropertyValue setting = new SettingsPropertyValue(property);

                    if (usersProperties != null)
                    {
                        setting.IsDirty = false;

                        if (usersProperties.ContainsKey(property.Name))
                        {
                            setting.SerializedValue = usersProperties[property.Name];
                            setting.Deserialized = false;
                        }
                    }

                    settings.Add(setting);      // Add the settings value to the collection
                }

            }

            return settings;    // Return the settings collection
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string username = context["UserName"] as string;
            bool userIsAuthenticated = (bool)context["IsAuthenticated"];

            // If no username is specified, or if no properties are to be saved, exit
            if (string.IsNullOrEmpty(username) || collection.Count == 0)
                return;

            // If ALL properties are non-dirty, then exit
            if (!HasDirtyProperty(collection))
                return;

            XmlNode newProfile = CreateUserProfile(username.ToLowerInvariant(), DateTime.Now);

            foreach (SettingsPropertyValue setting in collection)
            {
                // If the user is not authenticated and the property does not allow anonymous access, skip serializing it
                if (!userIsAuthenticated && !(bool)setting.Property.Attributes["AllowAnonymous"])
                    continue;

                // Skip the current property if it's not dirty and is currently assigned its default value
                if (!setting.IsDirty && setting.UsingDefaultValue)
                    continue;

                XmlNode propertyNode = ProfilesXml.CreateNode(XmlNodeType.Element, setting.Name, String.Empty);

                // Serialize data based on property's SerializeAs type
                if (setting.Property.SerializeAs == SettingsSerializeAs.String)
                    propertyNode.InnerText = Convert.ToString(setting.SerializedValue);
                else if (setting.Property.SerializeAs == SettingsSerializeAs.Xml)
                {
                    // strip out the <?xml ... ?> portion from the serialized string
                    string xmlData = setting.SerializedValue as string;
                    xmlData = Regex.Replace(xmlData, @"^<\?xml .*?\?>", string.Empty);

                    propertyNode.InnerText = string.Format("<{0}>{1}</{0}>", setting.Name, xmlData);
                }
                else if (setting.Property.SerializeAs == SettingsSerializeAs.Binary)
                {
                    // encode the binary data using base64 encoding
                    string encodedBinaryData = Convert.ToBase64String(setting.SerializedValue as byte[]);

                    propertyNode.InnerText = encodedBinaryData;
                }
                else
                    // unknown serialize type!
                    throw new ProviderException(string.Format("Invalid value for SerializeAs; expected String, Xml, or Binary, received {0}", System.Enum.GetName(setting.Property.SerializeAs.GetType(), setting.Property.SerializeAs)));

                newProfile.AppendChild(propertyNode);
            }
            Save();
        }
        
        #region Not Implemented

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


    } 
}