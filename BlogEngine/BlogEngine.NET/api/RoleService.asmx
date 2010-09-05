<%@ WebService Language="C#" Class="RoleService" %>

using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
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
    public JsonResponse Add(string roleName)
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
        //_response.Data = string.Format(row, roleName);
        return _response;
    }
    
    [WebMethod]
    public JsonResponse Delete(string id)
    {
        if (!IsAdmin())
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(id))
        {
            _response.Success = false;
            _response.Message = "Role name is required field";
            return _response;
        }

        try
        {
            Roles.DeleteRole(id);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.AddRole: " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not delete the role: " + id;
            return _response;
        }

        _response.Success = true;
        _response.Message = "Role \"" + id + "\" has been deleted";
        return _response;
    }

    [WebMethod]
    public JsonResponse Edit(string id, string bg, string[] vals)
    {
        string ptrn = "<tr id=\"{0}\" bgcolor=\"#{1}\"><td><input type=\"checkbox\" class\"chk\"/></td>";
        ptrn += "<td class='editable'>{0}</td><td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"editButton\">edit</a></td>";
	    ptrn += "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"deleteButton\">delete</a></td></tr>";

        _response.Success = false;
        _response.Data = string.Format(ptrn, vals[0], bg);
        
        if (!IsAdmin())
        {
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(vals[0]))
        {
            _response.Message = "Role name is required field";
            return _response;
        }

        try
        {
            Roles.DeleteRole(id);
            Roles.CreateRole(vals[0]);
        }
        catch (Exception ex)
        {
            Utils.Log("Roles.UpdateRole: " + ex.Message);
            _response.Message = "Could not update the role: " + vals[0];
            return _response;
        }

        _response.Success = true;
        _response.Message = string.Format("Role updated from \"{0}\" to \"{1}\"", id, vals[0]);
        return _response;
    }

    private bool IsAdmin()
    {
        return User.IsInRole(BlogSettings.Instance.AdministratorRole);
    }
}