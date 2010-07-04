<%@ WebService Language="C#" Class="RoleService" %>

using System;
using System.Web.Services;
using System.Web.Security;
using BlogEngine.Core;

/// <summary>
/// Membership service to support AJAX calls
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class RoleService : WebService
{
    [WebMethod]
    public String AddRole(string roleName)
    {
        if (!IsAdmin())
            return "warning: Not authorized";
        
        if (string.IsNullOrEmpty(roleName))
        {
            return "warning: Role name is required field";
        }

        try
        {
            Roles.CreateRole(roleName);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.AddRole: " + ex.Message);
            return "warning: Could not create the role: " + roleName;
        }

        return "success: Role \"" + roleName + "\" has been created";
    }

    [WebMethod]
    public String DeleteRole(string roleName)
    {
        if (!IsAdmin())
            return "warning: Not authorized";

        if (string.IsNullOrEmpty(roleName))
        {
            return "warning: Role name is required field";
        }

        try
        {
            Roles.DeleteRole(roleName);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.AddRole: " + ex.Message);
            return "warning: Could not delete the role: " + roleName;
        }

        return "success: Role \"" + roleName + "\" has been deleted";
    }

    [WebMethod]
    public String UpdateRole(string oldRole, string  newRole)
    {
        if (!IsAdmin())
            return "warning: Not authorized";

        if (string.IsNullOrEmpty(newRole))
        {
            return "warning: Role name is required field";
        }

        try
        {
            Roles.DeleteRole(oldRole);
            Roles.CreateRole(newRole);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.UpdateRole: " + ex.Message);
            return "warning: Could not update the role: " + newRole;
        }

        return "success: Role \"" + newRole + "\" has been updated";
    }

    private bool IsAdmin()
    {
        return User.IsInRole(BlogSettings.Instance.AdministratorRole);
    }

}