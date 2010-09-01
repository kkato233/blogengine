using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Security;
using BlogEngine.Core;

public partial class admin_Account_Roles : System.Web.UI.Page
{
    JsonResponse _response;

    protected void Page_Load(object sender, EventArgs e)
    {
        _response = new JsonResponse();
    }

    [WebMethod]
    public static List<Role> GetRoles()
    {
        List<Role> roles = new List<Role>();
        string[] sRoles = Roles.GetAllRoles();

        for (int i = 0; i <= sRoles.GetUpperBound(0); i++)
        {
            Role r = new Role();
            r.RoleName = sRoles[i];
            roles.Add(r);
        }

        roles.Sort(delegate(Role r1, Role r2)
        { return string.Compare(r1.RoleName, r2.RoleName); });

        return roles;
    }

    private bool IsAdmin()
    {
        return User.IsInRole(BlogSettings.Instance.AdministratorRole);
    }
}

public class Role
{
    public string RoleName;
}