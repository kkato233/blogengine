using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Profile;

namespace BlogEngine.Core.Providers
{
    /// <summary>
    /// Generic Db Profile Provider
    /// </summary>
    public class DbProfileProvider : ProfileProvider
    {
        private string connStringName;
        private string tablePrefix;
        private string parmPrefix;
        private string applicationName;

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="name">Configuration name</param>
        /// <param name="config">Configuration settings</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "DbMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Generic Database Membership Provider");
            }

            base.Initialize(name, config);

            if (config["connectionStringName"] == null)
            {
                // default to BlogEngine
                config["connectionStringName"] = "BlogEngine";
            }
            connStringName = config["connectionStringName"];
            config.Remove("connectionStringName");

            if (config["tablePrefix"] == null)
            {
                // default
                config["tablePrefix"] = "be_";
            }
            tablePrefix = config["tablePrefix"];
            config.Remove("tablePrefix");

            if (config["parmPrefix"] == null)
            {
                // default
                config["parmPrefix"] = "@";
            }
            parmPrefix = config["parmPrefix"];
            config.Remove("parmPrefix");

            if (config["applicationName"] == null)
            {
                // default to BlogEngine
                config["applicationName"] = "BlogEngine";
            }
            applicationName = config["applicationName"];
            config.Remove("applicationName");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                   DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                        DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption,
                                                             int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption,
                                                                     DateTime userInactiveSinceDate, int pageIndex,
                                                                     int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption,
                                                                     string usernameToMatch, int pageIndex, int pageSize,
                                                                     out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

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
            //    Dictionary<string, object> usersProperties = GetUserProfile(username);

            //    foreach (SettingsProperty property in collection)
            //    {
            //        // Indicate that provider-specific serialized properties should be
            //        // serialized as strings for primitive types and as XML for non-primitive types
            //        if (property.SerializeAs == SettingsSerializeAs.ProviderSpecific)
            //            if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(String))
            //                property.SerializeAs = SettingsSerializeAs.String;
            //            else
            //                property.SerializeAs = SettingsSerializeAs.Xml;

            //        // Create a new SettingsPropertyValue based on the current SettingsProperty object
            //        SettingsPropertyValue setting = new SettingsPropertyValue(property);

            //        if (usersProperties != null)
            //        {
            //            setting.IsDirty = false;

            //            if (usersProperties.ContainsKey(property.Name))
            //            {
            //                setting.SerializedValue = usersProperties[property.Name];
            //                setting.Deserialized = false;
            //            }
            //        }

            //        settings.Add(setting); // Add the settings value to the collection
            //    }
            }

            return settings; // Return the settings collection
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
            //using (StreamWriter sw = new StreamWriter(GetProfileFilePath(username), false))
            //{
            //    using (XmlTextWriter writer = new XmlTextWriter(sw))
            //    {
            //        writer.Formatting = Formatting.Indented;

            //        writer.WriteStartDocument();
            //        writer.WriteStartElement("profileData");

            //        foreach (SettingsPropertyValue setting in collection)
            //        {
            //            // If the user is not authenticated and the property does not allow anonymous access, skip serializing it
            //            if (!userIsAuthenticated && !(bool)setting.Property.Attributes["AllowAnonymous"])
            //                continue;

            //            // Skip the current property if it's not dirty and is currently assigned its default value
            //            if (!setting.IsDirty && setting.UsingDefaultValue)
            //                continue;

            //            // Serialize data based on property's SerializeAs type
            //            if (setting.Property.SerializeAs == SettingsSerializeAs.String)
            //                writer.WriteElementString(setting.Name, Convert.ToString(setting.SerializedValue));
            //            else if (setting.Property.SerializeAs == SettingsSerializeAs.Xml)
            //            {
            //                // strip out the <?xml ... ?> portion from the serialized string
            //                string xmlData = setting.SerializedValue as string;
            //                xmlData = Regex.Replace(xmlData, @"^<\?xml .*?\?>", string.Empty);

            //                writer.WriteRaw(string.Format("<{0}>{1}</{0}>", setting.Name, xmlData));
            //            }
            //            else if (setting.Property.SerializeAs == SettingsSerializeAs.Binary)
            //            {
            //                // encode the binary data using base64 encoding
            //                string encodedBinaryData = Convert.ToBase64String(setting.SerializedValue as byte[]);
            //                writer.WriteStartElement(setting.Name);
            //                writer.WriteCData(encodedBinaryData);
            //                writer.WriteEndElement();
            //            }
            //            else
            //                // unknown serialize type!
            //                throw new ProviderException(
            //                    string.Format(
            //                        "Invalid value for SerializeAs; expected String, Xml, or Binary, received {0}",
            //                        Enum.GetName(setting.Property.SerializeAs.GetType(), setting.Property.SerializeAs)));
            //        }

            //        writer.WriteEndElement();
            //        writer.WriteEndDocument();

            //        writer.Close();
            //    }

            //    sw.Close();
            //}
        }

        /// <summary>
        /// Returns the application name as set in the web.config
        /// otherwise returns BlogEngine.  Set will throw an error.
        /// </summary>
        public override string ApplicationName
        {
            get { return applicationName; }
            set { throw new NotImplementedException(); }
        }

        private bool ExistsDirtyProperty(SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue setting in collection)
                if (setting.IsDirty)
                    return true;

            // If we reach here, none are dirty
            return false;
        }
    }
}
