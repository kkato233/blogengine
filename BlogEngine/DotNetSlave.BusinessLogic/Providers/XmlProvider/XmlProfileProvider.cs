using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Profile;
using System.Xml;

namespace BlogEngine.Core.Providers
{
    // Many bits of code and the overall concept were taken from:
    //  Profile Providers
    //  http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspp/html/aspnetprovmod_prt5.asp
    //
    internal class XmlProfileProvider : ProfileProvider
    {
        #region Properties

        private readonly string _AppName = "BlogEngine.Net";

        private string _ProfileFolder = BlogSettings.Instance.StorageLocation;

        public override string ApplicationName
        {
            get
            {
                return _AppName;
            }
            set
            {
                //
            }
        }

        public string ProfileFolder
        {
            get { return _ProfileFolder; }
            set { _ProfileFolder = value; }
        }

        #endregion

        #region Initialize Method

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            ApplicationName = config["applicationName"];
            if (!string.IsNullOrEmpty(config["profileFolder"]))
                ProfileFolder = config["profileFolder"];

            if (string.IsNullOrEmpty(ApplicationName))
                throw new ProviderException(
                    "You _must_ provide the 'applicationName' setting when using the XmlProfileProvider");

            // Make sure that there are no unknown settings
            config.Remove("applicationName");
            config.Remove("profileFolder");

            if (config.Count > 0)
                throw new ProviderException("Unrecognized attribute: " + config.GetKey(0));
        }

        #endregion

        #region Get and Set Properties Methods

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
                                                                          SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection settings = new SettingsPropertyValueCollection();

            // Make sure we have an entry for this username in the XML data
            string username = context["UserName"] as string;
            if (!string.IsNullOrEmpty(username))
            {
                // Make sure username doesn't contain .. in the username (like '../SomeOtherUser/')
                if (username.IndexOf("..") >= 0)
                    throw new ProviderException(
                        "Cannot access profile data for users with a username that contains '..'");

                // Get the profile values for the user
                Dictionary<string, object> usersProperties = GetUserProfile(username);

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

                    settings.Add(setting); // Add the settings value to the collection
                }
            }

            return settings; // Return the settings collection
        }

        /// <summary>
        /// Searches the users profile file for a &lt;user&gt; section with a matching username
        /// and returns this content as a string dictionary.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>A string dictionary object representing the user's settings. If the supplied username is not found,
        /// <b>null</b> is returned.</returns>
        protected virtual Dictionary<string, object> GetUserProfile(string username)
        {
            Dictionary<string, object> propertyValues = new Dictionary<string, object>();
            // Open the XML file            
            if (!File.Exists(GetProfileFilePath(username)))
                CreateInitialProfile(username);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetProfileFilePath(username));
            XmlNode rootNode = xmlDoc.DocumentElement;

            foreach (XmlNode xmlProperty in rootNode.ChildNodes)
            {
                if (xmlProperty.InnerXml.StartsWith("<![CDATA"))
                    propertyValues.Add(xmlProperty.Name, Convert.FromBase64String(xmlProperty.InnerText));
                else
                    propertyValues.Add(xmlProperty.Name, xmlProperty.InnerXml);
            }

            return propertyValues;
        }

        private void CreateInitialProfile(string username)
        {
            string profileFilePath = Path.Combine(HttpContext.Current.Server.MapPath(ProfileFolder),
                            string.Format(@"{0}\{1}.xml", "profiles", username));
            using (StreamWriter sw = new StreamWriter(profileFilePath, false))
            {
                using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("profileData");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    writer.Close();
                }
                sw.Close();
            }

        }


        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string username = context["UserName"] as string;
            bool userIsAuthenticated = (bool)context["IsAuthenticated"];

            // If no username is specified, or if no properties are to be saved, exit
            if (string.IsNullOrEmpty(username) || collection.Count == 0)
                return;

            // If ALL properties are non-dirty, then exit
            if (!ExistsDirtyProperty(collection))
                return;

            // Otherwise, save the entire set of data, regardless of IsDirty.
            // That is, rather than trying to piecemeal together the changes into an existing XML file,
            // we're going to blow away the XML file (if it already exists) and just build it up based on the
            // values passed in...
            using (StreamWriter sw = new StreamWriter(GetProfileFilePath(username), false))
            {
                using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartDocument();
                    writer.WriteStartElement("profileData");

                    foreach (SettingsPropertyValue setting in collection)
                    {
                        // If the user is not authenticated and the property does not allow anonymous access, skip serializing it
                        if (!userIsAuthenticated && !(bool)setting.Property.Attributes["AllowAnonymous"])
                            continue;

                        // Skip the current property if it's not dirty and is currently assigned its default value
                        if (!setting.IsDirty && setting.UsingDefaultValue)
                            continue;

                        // Serialize data based on property's SerializeAs type
                        if (setting.Property.SerializeAs == SettingsSerializeAs.String)
                            writer.WriteElementString(setting.Name, Convert.ToString(setting.SerializedValue));
                        else if (setting.Property.SerializeAs == SettingsSerializeAs.Xml)
                        {
                            // strip out the <?xml ... ?> portion from the serialized string
                            string xmlData = setting.SerializedValue as string;
                            xmlData = Regex.Replace(xmlData, @"^<\?xml .*?\?>", string.Empty);

                            writer.WriteRaw(string.Format("<{0}>{1}</{0}>", setting.Name, xmlData));
                        }
                        else if (setting.Property.SerializeAs == SettingsSerializeAs.Binary)
                        {
                            // encode the binary data using base64 encoding
                            string encodedBinaryData = Convert.ToBase64String(setting.SerializedValue as byte[]);
                            writer.WriteStartElement(setting.Name);
                            writer.WriteCData(encodedBinaryData);
                            writer.WriteEndElement();
                        }
                        else
                            // unknown serialize type!
                            throw new ProviderException(
                                string.Format(
                                    "Invalid value for SerializeAs; expected String, Xml, or Binary, received {0}",
                                    Enum.GetName(setting.Property.SerializeAs.GetType(), setting.Property.SerializeAs)));
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    writer.Close();
                }

                sw.Close();
            }
        }

        protected virtual string GetProfileFilePath(string username)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath(ProfileFolder),
                             string.Format(@"{0}\{1}.xml", "profiles", username));
        }

        protected virtual bool ExistsDirtyProperty(SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue setting in collection)
                if (setting.IsDirty)
                    return true;

            // If we reach here, none are dirty
            return false;
        }

        #endregion

        #region Not Implemented

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                   DateTime userInactiveSinceDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption,
                                                                     string usernameToMatch, int pageIndex, int pageSize,
                                                                     out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                                     DateTime userInactiveSinceDate, int pageIndex,
                                                                     int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption,
                                                             int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                        DateTime userInactiveSinceDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}