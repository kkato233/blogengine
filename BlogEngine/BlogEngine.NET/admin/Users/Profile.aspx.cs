using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using BlogEngine.Core;

public partial class admin_Pages_Profile : System.Web.UI.Page
{
    static string _id = "";
    static protected string RolesList 
    { 
        get 
        {
            string ret = "";
            string ptrn = "<input type=\"checkbox\" id=\"{0}\" class=\"chkRole\" {1} /><span class=\"lbl\">{0}</span>";
            string[] sRoles = Roles.GetAllRoles();
            
            for (int i = 0; i <= sRoles.GetUpperBound(0); i++)
            {
                string chkd = "";            
                if(Roles.IsUserInRole(_id, sRoles[i]))
                {
                    chkd = "checked";
                }
                ret += string.Format(ptrn, sRoles[i], chkd);
            }
            return ret;
        } 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            _id = Request.QueryString["id"];
        }
    }

    [WebMethod]
    public static AuthorProfile GetProfile(string id)
    {
        AuthorProfile pf = AuthorProfile.GetProfile(id);
        if (pf == null)
        {
            pf = new AuthorProfile();
            pf.DisplayName = "";
            pf.FirstName = "";
            pf.MiddleName = "";
            pf.LastName = "";
            pf.Birthday = new DateTime(1001, 1, 1);
            pf.PhotoURL = "";
            pf.EmailAddress = "";
            pf.PhoneMobile = "";
            pf.PhoneMain = "";
            pf.PhoneFax = "";
            pf.CityTown = "";
            pf.RegionState = "";
            pf.Country = "";
            pf.AboutMe = "";
        }
        return pf;
    }
}