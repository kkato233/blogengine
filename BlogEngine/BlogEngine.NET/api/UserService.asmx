<%@ WebService Language="C#" Class="UserService" %>

using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Security;
using BlogEngine.Core;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserService  : System.Web.Services.WebService {

    JsonResponse _response;
    
    public UserService()
    {
        _response = new JsonResponse();
    }

    [WebMethod]
    public JsonResponse Add(string user, string pwd, string email)
    {
        if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(user))
        {
            _response.Success = false;
            _response.Message = "User name is required field";
            return _response;
        }

        MembershipUser checkUser = Membership.GetUser(user);
        if (checkUser != null)
        {
            _response.Success = false;
            _response.Message = string.Format("User \"{0}\" already exists", user);
            return _response;
        }

        try
        {
            Membership.CreateUser(user, pwd, email);
        }
        catch (Exception ex)
        {
            Utils.Log("Users.AddUser: " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not create user: " + user + " : " + ex.Message;
            return _response;
        }

        _response.Success = true;
        _response.Message = "User \"" + user + "\" has been created";
        //_response.Data = string.Format(row, roleName);
        return _response;
    }

    [WebMethod]
    public JsonResponse Edit(string id, string bg, string[] vals)
    {
        string ptrn = "<tr id=\"{0}\" bgcolor=\"#{1}\"><td><input type=\"checkbox\" class\"chk\"/></td>";
        ptrn += "<td>{0}</td><td class='editable'>{2}</td>";
        ptrn += "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"editButton\">edit</a></td>";
        ptrn += "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"deleteButton\">delete</a></td></tr>";

        try
        {
            _response.Success = false;
            _response.Data = string.Format(ptrn, id, bg, vals[0]);

            if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                _response.Message = "Not authorized";
                return _response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                _response.Message = "Email is required field";
                return _response;
            }

            foreach (MembershipUser u in Membership.GetAllUsers())
	        {
                if (u.Email.ToLowerInvariant() == vals[0].ToLowerInvariant())
                {
                    _response.Message = "User with this email already exists";
                    return _response;
                }
	        } 
            
            MembershipUser usr = Membership.GetUser(id);
            usr.Email = vals[0];
            Membership.UpdateUser(usr);

            _response.Success = true;
            _response.Message = string.Format("User \"{0}\" updated", id);
            return _response;
        }
        catch (Exception ex)
        {
            Utils.Log("UserService.Update: " + ex.Message);
            _response.Message = "Could not update user: " + id;
            return _response;
        }
    }

    [WebMethod]
    public JsonResponse Delete(string id)
    {
        if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            _response.Success = false;
            _response.Message = "Not authorized";
            return _response;
        }

        if (string.IsNullOrEmpty(id))
        {
            _response.Success = false;
            _response.Message = "User name is required field";
            return _response;
        }

        try
        {
            Membership.DeleteUser(id);
        }
        catch (Exception ex)
        {
            Utils.Log("Users.Delete : " + ex.Message);
            _response.Success = false;
            _response.Message = "Could not delete user : " + id;
            return _response;
        }

        _response.Success = true;
        _response.Message = "User \"" + id + "\" has been deleted";
        return _response;
    }
    
}