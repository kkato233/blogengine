// Written by: Roman D. Clarkson
// http://www.romanclarkson.com  inspirit@romanclarkson.com


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Xml;

namespace BlogEngine.Core.Providers
{
    ///<summary>
    ///</summary>
    public class XmlRoleProvider : System.Web.Security.RoleProvider
    {

        #region Properties

        private string _XmlFileName;
        private List<Role> _Roles = new List<Role>();


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
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
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
            List<string> UsersInRole = new List<string>();
            lock (this)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_XmlFileName);
                XmlNodeList nodes = doc.GetElementsByTagName("role");
                foreach (XmlNode roleNode in nodes)
                {
                    if (roleNode.InnerText.ToLower() == roleName.ToLower())
                    {
                        foreach (XmlNode userNode in doc.SelectNodes("role/users"))
                        {
                            UsersInRole.Add(userNode.InnerText.ToLower());
                        }
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
            lock (this)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_XmlFileName);

                foreach (XmlNode roleNode in doc.SelectNodes("roles/role"))
                {
                    if (roleNode.SelectSingleNode("name").InnerText.ToLower() == roleName.ToLower())
                    {
                        foreach (XmlNode userNode in roleNode.SelectNodes("users/user"))
                        {
                            if (userNode.InnerText.ToLower() == username.ToLower())
                                return true;
                        }
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
            List<string> rolesForUser = new List<string>(); ;
            lock (this)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_XmlFileName);

                foreach (XmlNode roleNode in doc.SelectNodes("roles/role"))
                {
                    foreach (XmlNode userNode in roleNode.SelectNodes("users/user"))
                    {

                        if (userNode.InnerText.ToLower() == username.ToLower())
                        {
                            rolesForUser.Add(roleNode.SelectSingleNode("name").InnerText);
                        }

                    }

                }
            }
            return rolesForUser.ToArray();
        }



        #endregion


        #region Supported methods

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

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }

            ReadRoleDataStore();
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
                XmlNodeList nodes = doc.GetElementsByTagName("role/name");

                foreach (XmlNode node in nodes)
                {
                    _Roles.Add(new Role(node.InnerText.ToLower()));
                }

            }
        }

      ///<summary>
        ///</summary>
        public void Save()
        {

        }

        #endregion


        #region Unsupported methods

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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        ///<summary>
        ///Adds a new role to the data source for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        ///<summary>
        ///Adds the specified user names to the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of the role names to add the specified user names to. </param>
        ///<param name="usernames">A string array of user names to be added to the specified roles. </param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        ///<summary>
        ///Removes the specified user names from the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of role names to remove the specified user names from. </param>
        ///<param name="usernames">A string array of user names to be removed from the specified roles. </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        private string _Name;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Name"></param>
        public Role(string _Name)
        {
            this._Name = _Name;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


    }
}