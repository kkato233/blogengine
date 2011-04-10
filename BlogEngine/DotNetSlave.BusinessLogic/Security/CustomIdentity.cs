namespace BlogEngine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Security;
    using System.Security.Principal;
    using System.Runtime.Serialization;
    using System.Reflection;
    using System.Security;

    [Serializable]
    public class CustomIdentity : IIdentity, ISerializable
    {   
        [SecurityCriticalAttribute]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // With Cassini, we need to make CustomIdentity be serializable because
            // Cassini will cross app domain boundaries (for some reason).  When this
            // happens, User.Identity is automatically serialized by ASP.NET so it
            // is available in the other app domains.  This CustomIdentity type
            // will be an unknown type in other app domains.  Here, we serialize
            // this CustomIdentity object to a type known by other app domains --
            // GenericIdentity (this is part of .NET).
            // We can check if this is Cassini by seeing if context.State
            // is 'CrossAppDomain'.  If so, we create a GenericIdentity, put
            // our Name to it and the GenericIdentity is used.

            // Other than Cassini, this object should not need to be serialized.
            // If serialization is needed for non-Cassini cases, that needs
            // to be implemented.

            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                GenericIdentity gi = new GenericIdentity(this.Name, this.AuthenticationType);
                info.SetType(gi.GetType());

                MemberInfo[] members = FormatterServices.GetSerializableMembers(gi.GetType());
                object[] values = FormatterServices.GetObjectData(gi, members);

                for (int i = 0; i < members.Length; i++)
                {
                    info.AddValue(members[i].Name, values[i]);
                }
            }
        }

        public string AuthenticationType
        {
            get { return "BlogEngine.NET Custom Identity"; }
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public CustomIdentity(string username, bool isAuthenticated)
        {
            _name = username;
            _isAuthenticated = isAuthenticated;
        }

        public CustomIdentity(string username, string password)
        {
            if (Utils.StringIsNullOrWhitespace(username))
                throw new ArgumentNullException("username");

            if (Utils.StringIsNullOrWhitespace(password))
                throw new ArgumentNullException("password");

            if (!Membership.ValidateUser(username, password)) { return; }

            _isAuthenticated = true;
            _name = username;
        }
    }
}
