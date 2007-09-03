// Written by: Roman D. Clarkson
// http://www.romanclarkson.com  inspirit@romanclarkson.com


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using BlogEngine.Core;

namespace BlogEngine.Core.Providers
{
    ///<summary>
    ///</summary>
    public class XmlRoleProvider : RoleProvider
    {
        private static string _Folder = HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);

        #region Properties

        private List<Role> _Roles = new List<Role>();
        private List<string> _UserNames;
        private string _XmlFileName;


        ///<summary>
        ///Gets or sets the name of the application to store and retrieve role information for.
        ///</summary>
        ///
        ///<returns>
        ///The name of the application to store and retrieve role information for.
        ///</returns>
        ///
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        ///<summary>
        ///Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        ///</returns>
        ///
        ///<param name="roleName">The name of the role to search for in the data source. </param>
        public override bool RoleExists(string roleName)
        {
            //ReadRoleDataStore();
            return _Roles.Contains(new Role(roleName.ToLower()));
        }

        ///<summary>
        ///Gets a list of all the roles for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the roles stored in the data source for the configured applicationName.
        ///</returns>
        ///
        public override string[] GetAllRoles()
        {
            //ReadRoleDataStore();
            List<string> allRoles = new List<string>();
            foreach (Role role in _Roles)
            {
                allRoles.Add(role.Name.ToLower());
            }
            return allRoles.ToArray();
        }

        ///<summary>
        ///Gets a list of users in the specified role for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        ///</returns>
        ///
        ///<param name="roleName">The name of the role to get the list of users for. </param>
        public override string[] GetUsersInRole(string roleName)
        {
            //  ReadRoleDataStore();
            List<string> UsersInRole = new List<string>();

            foreach (Role role in _Roles)
            {
                if (role.Name.ToLower() == roleName.ToLower())
                {
                    foreach (string user in role.Users)
                    {
                        UsersInRole.Add(user.ToLower());
                    }
                }
            }
            return UsersInRole.ToArray();
        }

        ///<summary>
        ///Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        ///</returns>
        ///
        ///<param name="username">The user name to search for.</param>
        ///<param name="roleName">The role to search in.</param>
        public override bool IsUserInRole(string username, string roleName)
        {
            //  ReadRoleDataStore();
            List<string> UsersInRole = new List<string>();

            foreach (Role role in _Roles)
            {
                if (role.Name.ToLower() == roleName.ToLower())
                {
                    foreach (string user in role.Users)
                    {
                        if (user == username)
                            return true;
                    }
                }
            }
            return false;
        }

        ///<summary>
        ///Gets a list of the roles that a specified user is in for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        ///</returns>
        ///
        ///<param name="username">The user to return a list of roles for.</param>
        public override string[] GetRolesForUser(string username)
        {
            //  ReadRoleDataStore();
            List<string> rolesForUser = new List<string>();

            foreach (Role role in _Roles)
            {
                foreach (string user in role.Users)
                {
                    if (user.ToLower() == username.ToLower())
                        rolesForUser.Add(role.Name.ToLower());
                }
            }
            return rolesForUser.ToArray();
        }

        #endregion

        #region Supported methods

        ///<summary>
        ///Gets an array of user names in a role where the user name contains the specified user name to match.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the users where the user name matches usernameToMatch and the user is a member of the specified role.
        ///</returns>
        ///
        ///<param name="usernameToMatch">The user name to search for.</param>
        ///<param name="roleName">The role to search in.</param>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            List<string> UsersInRole = new List<string>();
            if (IsUserInRole(usernameToMatch, roleName))
                UsersInRole.AddRange(_UserNames);
            return UsersInRole.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "XmlMembershipProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "XML role provider");
            }

            base.Initialize(name, config);

            // Initialize _XmlFileName and make sure the path
            // is app-relative
            string path = config["xmlFileName"];

            if (String.IsNullOrEmpty(path))
                path = BlogSettings.Instance.StorageLocation + "roles.xml";


            if (!VirtualPathUtility.IsAppRelative(path))
                throw new ArgumentException
                    ("xmlFileName must be app-relative");

            string fullyQualifiedPath = VirtualPathUtility.Combine
                (VirtualPathUtility.AppendTrailingSlash
                     (HttpRuntime.AppDomainAppVirtualPath), path);

            _XmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            config.Remove("xmlFileName");

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, _XmlFileName);
            permission.Demand();

            if (!System.IO.File.Exists(_XmlFileName))
                InstallMissingXMLFile(_XmlFileName);

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }

            ReadRoleDataStore();
        }

        ///<summary>
        ///Adds the specified user names to the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of the role names to add the specified user names to. </param>
        ///<param name="usernames">A string array of user names to be added to the specified roles. </param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            if (usernames.Length != 0 && roleNames.Length != 0)
            {
                foreach (Role role in _Roles)
                {
                    foreach (string _name in roleNames)
                    {
                        if (role.Name.ToLower() == _name)
                        {
                            foreach (string s in usernames)
                            {
                                if (!role.Users.Contains(s))
                                    role.Users.Add(s);
                            }
                        }
                    }
                }
            }
            Save();
        }

        ///<summary>
        ///Removes the specified user names from the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of role names to remove the specified user names from. </param>
        ///<param name="usernames">A string array of user names to be removed from the specified roles. </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            if (usernames.Length != 0 && roleNames.Length != 0)
            {
                foreach (Role role in _Roles)
                {
                    foreach (string _name in roleNames)
                    {
                        if (role.Name.ToLower() == _name)
                        {
                            foreach (string user in usernames)
                            {
                                if (role.Name == "administrators")
                                {
                                    if (role.Users.Count != 1)
                                    {
                                        if (role.Users.Contains(user))
                                            role.Users.Remove(user);
                                    }
                                }
                                else
                                {
                                    if (role.Users.Contains(user))
                                        role.Users.Remove(user);
                                }
                            }

                        }
                    }
                }
            }
            Save();
        }

        ///<summary>
        ///Removes a role from the data source for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the role was successfully deleted; otherwise, false.
        ///</returns>
        ///
        ///<param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
        ///<param name="roleName">The name of the role to delete.</param>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (roleName != "administrators")
            {
                _Roles.Remove(new Role(roleName));
                Save();
                return true;
            }
            return false;
        }

        ///<summary>
        ///Adds a new role to the data source for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            if (!_Roles.Contains(new Core.Role(roleName)))
            {
                _Roles.Add(new Core.Role(roleName));
                Save();
            }

        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Builds the internal cache of users.
        /// </summary>
        private void ReadRoleDataStore()
        {
            lock (this)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_XmlFileName);
                XmlNodeList nodes = doc.GetElementsByTagName("role");
                foreach (XmlNode roleNode in nodes)
                //foreach (XmlNode roleNode in doc.SelectNodes("roles/role"))
                {
                    Role tempRole = new Role(roleNode.SelectSingleNode("name").InnerText);
                    foreach (XmlNode userNode in roleNode.SelectNodes("users/user"))
                    {
                        tempRole.Users.Add(userNode.InnerText);
                    }
                    _Roles.Add(tempRole);

                }
            }
        }

        private void InstallMissingXMLFile(string _FileName)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            ReadMembershipDataStore();
            using (XmlWriter writer = XmlWriter.Create(_FileName, settings))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("roles");

                //Start by fixing the Administrators role.
                writer.WriteStartElement("role");
                writer.WriteElementString("name", "Administrators");
                writer.WriteStartElement("users");


                foreach (string username in _UserNames)
                {
                    writer.WriteElementString("user", username);
                }
                writer.WriteEndElement(); //closes Administrators users
                writer.WriteEndElement(); //closes Administrators role

                //End by fixing the Editors role.
                writer.WriteStartElement("role");
                writer.WriteElementString("name", "Editors");
                writer.WriteStartElement("users");

                foreach (string username in _UserNames)//.GetAllUsers(0, 0, out _usercount))
                {
                    writer.WriteElementString("user", username);
                }
                writer.WriteEndElement(); //closes Editors users
                writer.WriteEndElement(); //closes Editors role
            }
        }

        ///<summary>
        ///</summary>
        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(_XmlFileName, settings))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("roles");

                foreach (Role _role in _Roles)
                {
                    writer.WriteStartElement("role");
                    writer.WriteElementString("name", _role.Name);
                    writer.WriteStartElement("users");
                    foreach (string username in _role.Users)
                    {
                        writer.WriteElementString("user", username);
                    }
                    writer.WriteEndElement(); //closes users
                }
                writer.WriteEndElement(); //closes role

            }

        }

        #endregion

        /// <summary>
        /// Only so we can add users to the adminstrators and editors roles.
        /// </summary>
        private void ReadMembershipDataStore()
        {
            string fullyQualifiedPath = VirtualPathUtility.Combine
              (VirtualPathUtility.AppendTrailingSlash
              (HttpRuntime.AppDomainAppVirtualPath), BlogSettings.Instance.StorageLocation + "Users.xml");

            lock (this)
            {
                if (_UserNames == null)
                {
                    _UserNames = new List<string>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HostingEnvironment.MapPath(fullyQualifiedPath));
                    XmlNodeList nodes = doc.GetElementsByTagName("User");

                    foreach (XmlNode node in nodes)
                    {
                        _UserNames.Add(node["UserName"].InnerText);
                    }

                }
            }
        }

    }


}