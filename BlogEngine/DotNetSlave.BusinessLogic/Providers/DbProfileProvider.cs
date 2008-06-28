using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
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

            string username = context["UserName"] as string;
            if (!string.IsNullOrEmpty(username))
            {
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
            string connString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            string providerName = ConfigurationManager.ConnectionStrings[connStringName].ProviderName;
            DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);

            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connString;

                using (DbCommand cmd = conn.CreateCommand())
                {
                    string sqlQuery = "SELECT SettingName, SettingValue " +
                                      "FROM " + tablePrefix + "Profiles " +
                                      "WHERE UserName = " + parmPrefix + "username";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandType = CommandType.Text;

                    DbParameter dpID = provider.CreateParameter();
                    dpID.ParameterName = parmPrefix + "username";
                    dpID.Value = username;
                    cmd.Parameters.Add(dpID);

                    conn.Open();

                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while(rdr.Read())
                        {
                            if (rdr.GetString(1).StartsWith("<![CDATA"))
                                propertyValues.Add(rdr.GetString(0), Convert.FromBase64String(rdr.GetString(1)));
                            else
                                propertyValues.Add(rdr.GetString(0), rdr.GetString(1));
                        }
                    }
                }
            }

            return propertyValues;
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
            // we're going to blow away the data and just build it up based on the
            // values passed in...
            string connString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            string providerName = ConfigurationManager.ConnectionStrings[connStringName].ProviderName;
            DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);

            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    string sqlQuery = "DELETE FROM " + tablePrefix + "Profiles " +
                                      "WHERE UserName = " + parmPrefix + "username";
                    cmd.CommandText = sqlQuery;
                    cmd.CommandType = CommandType.Text;

                    DbParameter dpID = provider.CreateParameter();
                    dpID.ParameterName = parmPrefix + "username";
                    dpID.Value = username;
                    cmd.Parameters.Add(dpID);

                    cmd.ExecuteNonQuery();

                    foreach (SettingsPropertyValue setting in collection)
                    {
                        //If the user is not authenticated and the property does not allow anonymous access, skip serializing it
                        if (!userIsAuthenticated && !(bool)setting.Property.Attributes["AllowAnonymous"])
                            continue;

                        // Skip the current property if it's not dirty and is currently assigned its default value
                        if (!setting.IsDirty && setting.UsingDefaultValue)
                            continue;

                        // Serialize data based on property's SerializeAs type
                        string serializedData;
                        if (setting.Property.SerializeAs == SettingsSerializeAs.String)
                        {
                            serializedData = Convert.ToString(setting.SerializedValue);   
                        }
                        else if (setting.Property.SerializeAs == SettingsSerializeAs.Xml)
                        {
                            // strip out the <?xml ... ?> portion from the serialized string
                            serializedData = setting.SerializedValue as string;
                            serializedData = Regex.Replace(serializedData, @"^<\?xml .*?\?>", string.Empty);
                        }
                        else if (setting.Property.SerializeAs == SettingsSerializeAs.Binary)
                        {
                            // encode the binary data using base64 encoding
                            serializedData = Convert.ToBase64String(setting.SerializedValue as byte[]);
                        }
                        else
                            // unknown serialize type!
                            throw new ProviderException(
                                string.Format(
                                    "Invalid value for SerializeAs; expected String, Xml, or Binary, received {0}",
                                    Enum.GetName(setting.Property.SerializeAs.GetType(), setting.Property.SerializeAs)));

                        sqlQuery = "INSERT INTO " + tablePrefix + "Profiles (UserName, SettingName, SettingValue) " +
                                   "VALUES (" + parmPrefix + "username, " + parmPrefix + "name, " + parmPrefix + "value)";
                        cmd.CommandText = sqlQuery;
                        cmd.Parameters.Clear();

                        DbParameter dpUserName = provider.CreateParameter();
                        dpUserName.ParameterName = parmPrefix + "username";
                        dpUserName.Value = username;
                        cmd.Parameters.Add(dpUserName);

                        DbParameter dpName = provider.CreateParameter();
                        dpName.ParameterName = parmPrefix + "name";
                        dpName.Value = setting.Name;
                        cmd.Parameters.Add(dpName);

                        DbParameter dpValue = provider.CreateParameter();
                        dpValue.ParameterName = parmPrefix + "value";
                        dpValue.Value = serializedData;
                        cmd.Parameters.Add(dpValue);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
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
