using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using System.Web;
using System.Web.Security;

namespace BlogEngine.Core.Providers.Store
{

    /// <summary>
    /// 
    /// </summary>
    public class XmlRoleStore : Persistable<List<XmlRole>> {

        #region Static Fields ///////////////////////////////////////////////////////////

        static Dictionary<string, XmlRoleStore> _RegisteredStores;

        #endregion

        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static XmlRoleStore GetStore(string fileName) {

            if (_RegisteredStores == null)
                _RegisteredStores = new Dictionary<string, XmlRoleStore>();
            if (!_RegisteredStores.ContainsKey(fileName))
                _RegisteredStores.Add(fileName, new XmlRoleStore(fileName));
            ///
            return _RegisteredStores[fileName];
        }
        #endregion

        #region Properties  /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public List<XmlRole> Roles {
            get { return base.Value; }
        }
        #endregion

        #region Construct  //////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRoleStore"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private XmlRoleStore(string fileName)
            : base(fileName) {
        }
        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public XmlRole GetRole(string roleName) {

            return Roles.Find(
                delegate(XmlRole role) {
                    return role.Name.Equals(
                        roleName, StringComparison.OrdinalIgnoreCase);
                }
            );
        }

        /// <summary>
        /// Gets the roles for user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public List<XmlRole> GetRolesForUser(string userName) {

            List<XmlRole> Results = new List<XmlRole>();
            foreach (XmlRole r in Roles) {
                if (r.Users.Contains(userName))
                    Results.Add(r);
            }
            return Results;
        }

        /// <summary>
        /// Gets the users in role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public string[] GetUsersInRole(string roleName) {

            XmlRole role = GetRole(roleName);
            if (role != null) {
                string[] Results = new string[role.Users.Count];
                role.Users.CopyTo(Results, 0);
                return Results;
            }
            else {
                throw new Exception(string.Format("Role with name {0} does not exist!", roleName));
            }
        }
        #endregion
    }
}
