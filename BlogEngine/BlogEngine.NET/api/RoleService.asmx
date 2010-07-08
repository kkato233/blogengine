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
    /// <summary>
    /// JSON object that will be return back to client
    /// </summary>
    JsonResponse _response;
    
    public RoleService()
    {
        _response = new JsonResponse();
    }
    
    [WebMethod]
    public JsonResponse AddRole(string roleName)
    {
        if (!IsAdmin())
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(roleName))
        {
            _response.Success = false;
            _response.Message = "Role name is required field";
            return _response;
        }

        string[] roles = Roles.GetAllRoles();
        if (roles.GetUpperBound(0) > 0)
        {
            for (int i = 0; i <= roles.GetUpperBound(0); i++)
            {
                if (roles[i].ToLowerInvariant() == roleName.ToLowerInvariant())
                {
                    _response.Success = false;
                    _response.Message = string.Format("Role \"{0}\" already exists", roleName);
                    return _response;
                }
            }
        }

        try
        {
            Roles.CreateRole(roleName);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.AddRole: " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not create the role: " + roleName;
            return _response;
        }

        _response.Success = true;
        _response.Message = "Role \"" + roleName + "\" has been created";
        return _response;
    }

    [WebMethod]
    public JsonResponse DeleteRole(string roleName)
    {
        if (!IsAdmin())
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(roleName))
        {
            _response.Success = false;
            _response.Message = "Role name is required field";
            return _response;
        }

        try
        {
            Roles.DeleteRole(roleName);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.AddRole: " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not delete the role: " + roleName;
            return _response;
        }

        _response.Success = true;
        _response.Message = "Role \"" + roleName + "\" has been deleted";
        return _response;
    }

    [WebMethod]
    public JsonResponse UpdateRole(string oldRole, string newRole)
    {
        if (!IsAdmin())
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(newRole))
        {
            _response.Success = false;
            _response.Message = "Role name is required field";
            return _response;
        }

        try
        {
            Roles.DeleteRole(oldRole);
            Roles.CreateRole(newRole);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.UpdateRole: " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not update the role: " + newRole;
            return _response;
        }

        _response.Success = true;
        _response.Message = string.Format("Role updated from \"{0}\" to \"{1}\"", oldRole, newRole);
        return _response;
    }

    private bool IsAdmin()
    {
        return User.IsInRole(BlogSettings.Instance.AdministratorRole);
    }
}