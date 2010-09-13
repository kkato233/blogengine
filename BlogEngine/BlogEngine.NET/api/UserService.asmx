<%@ WebService Language="C#" Class="UserService" %>

using System;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;

using BlogEngine.Core;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class UserService : WebService
{
    #region Constants and Fields

    private readonly JsonResponse response;

    #endregion

    #region Constructors and Destructors

    public UserService()
    {
        this.response = new JsonResponse();
    }

    #endregion

    #region Public Methods

    [WebMethod]
    public JsonResponse Add(string user, string pwd, string email)
    {
        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Success = false;
            this.response.Message = "Not authorized";
            return this.response;
        }

        if (string.IsNullOrEmpty(user))
        {
            this.response.Success = false;
            this.response.Message = "User name is required field";
            return this.response;
        }

        var checkUser = Membership.GetUser(user);
        if (checkUser != null)
        {
            this.response.Success = false;
            this.response.Message = string.Format("User \"{0}\" already exists", user);
            return this.response;
        }

        try
        {
            Membership.CreateUser(user, pwd, email);
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Users.AddUser: {0}", ex.Message));
            this.response.Success = false;
            this.response.Message = string.Format("Could not create user: {0} : {1}", user, ex.Message);
            return this.response;
        }

        this.response.Success = true;
        this.response.Message = string.Format("User \"{0}\" has been created", user);
        //_response.Data = string.Format(row, roleName);
        return this.response;
    }

    [WebMethod]
    public JsonResponse Delete(string id)
    {
        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Success = false;
            this.response.Message = "Not authorized";
            return this.response;
        }

        if (string.IsNullOrEmpty(id))
        {
            this.response.Success = false;
            this.response.Message = "User name is required field";
            return this.response;
        }

        try
        {
            Membership.DeleteUser(id);
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Users.Delete : {0}", ex.Message));
            this.response.Success = false;
            this.response.Message = string.Format("Could not delete user : {0}", id);
            return this.response;
        }

        this.response.Success = true;
        this.response.Message = string.Format("User \"{0}\" has been deleted", id);
        return this.response;
    }

    [WebMethod]
    public JsonResponse Edit(string id, string bg, string[] vals)
    {
        var ptrn = "<tr id=\"{0}\" bgcolor=\"#{1}\"><td><input type=\"checkbox\" class\"chk\"/></td>";
        ptrn += "<td>{0}</td><td class='editable'>{2}</td>";
        ptrn += "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"Profile.aspx?id={0}\">profile</a></td>";
        ptrn += "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"editButton\">edit</a></td>";
        ptrn +=
            "<td align=\"center\" style=\"vertical-align:middle\"><a href=\"#\" class=\"deleteButton\">delete</a></td></tr>";

        try
        {
            this.response.Success = false;
            this.response.Data = string.Format(ptrn, id, bg, vals[0]);

            if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                this.response.Message = "Email is required field";
                return this.response;
            }

            if (Membership.GetAllUsers().Cast<MembershipUser>().Any(u => u.Email.ToLowerInvariant() == vals[0].ToLowerInvariant()))
            {
                this.response.Message = "User with this email already exists";
                return this.response;
            }

            var usr = Membership.GetUser(id);
            if (usr != null)
            {
                usr.Email = vals[0];
                Membership.UpdateUser(usr);
            }

            this.response.Success = true;
            this.response.Message = string.Format("User \"{0}\" updated", id);
            return this.response;
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("UserService.Update: {0}", ex.Message));
            this.response.Message = string.Format("Could not update user: {0}", id);
            return this.response;
        }
    }

    #endregion
}