namespace Admin.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BlogEngine.Core;

    public partial class Rights : System.Web.UI.Page
    {

        private string roleName;
        protected string RoleName
        {
            get { return this.roleName; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            // Immediately throws a security exception if user doesn't have this right.
            Security.DemandUserHasRight(BlogEngine.Core.Rights.EditRoles);

            this.roleName = this.Request.QueryString["role"];

            // If an invalid role is requested, send the user back to the Roles page since it has a list of all the roles.
            if (Utils.StringIsNullOrWhitespace(this.roleName) || !System.Web.Security.Roles.RoleExists(this.roleName)) {
                this.Response.Redirect("~/admin/Users/Roles.aspx");
            }

        }

        protected string GetRightsJson()
        {
            var role = this.roleName;

            if (Utils.StringIsNullOrWhitespace(role))
            {
                return "null";
            }
            else
            {
                var jsonDict = new Dictionary<string, bool>();

                foreach (var right in BlogEngine.Core.Right.GetAllRights())
                {
                    // The None flag isn't meant to be set specifically, so 
                    // don't render it out.
                    if (right.Flag != BlogEngine.Core.Rights.None)
                    {
                        jsonDict.Add(right.Name, right.Roles.Contains(role));
                    }
                }

                return Utils.ConvertToJson(jsonDict);
            }
        }

    }
}