<%@ WebService Language="C#" Class="Profile" %>

using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Security;
using BlogEngine.Core;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Profile  : System.Web.Services.WebService {

    /// <summary>
    /// JSON object that will be return back to client
    /// </summary>
    JsonResponse _response;

    public Profile()
    {
        _response = new JsonResponse();
    }

    [WebMethod]
    public JsonResponse Save(string id, string[] vals, string[] roles)
    {
        _response.Success = false;

        if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            _response.Message = "Not authorized";
            return _response;
        }
        
        if (string.IsNullOrEmpty(vals[0]))
        {
            _response.Message = "Display name is required field";
            return _response;
        }

        AuthorProfile pf = AuthorProfile.GetProfile(id);
        if (pf == null) pf = new AuthorProfile(id);

        try
        {
            pf.DisplayName = vals[0];
            pf.FirstName = vals[1];
            pf.MiddleName = vals[2];
            pf.LastName = vals[3];
            pf.EmailAddress = vals[4];

            DateTime date;
            if (vals[5].Length == 0) vals[5] = "1/1/1001";
            if (DateTime.TryParse(vals[5], out date))
            {
                pf.Birthday = date;
            }
            else
            {
                _response.Message = "Date must be in format mm/dd/yyyy";
                return _response;
            }

            pf.PhotoURL = vals[6];
            pf.IsPrivate = false;
            
            bool prv = false;
            if (bool.TryParse(vals[7], out prv)) pf.IsPrivate = prv;
            
            pf.PhoneMobile = vals[8];
            pf.PhoneMain = vals[9];
            pf.PhoneFax = vals[10];

            pf.CityTown = vals[11];
            pf.RegionState = vals[12];
            pf.Country = vals[13]; // ddlCountry.SelectedValue;
            
            //pf.Company = tbCompany.Text;
            pf.AboutMe = vals[14];

            pf.Save();

            // remove all user roles and add only checked
            Roles.RemoveUserFromRoles(id, Roles.GetAllRoles());
            if (roles.GetLength(0) > 0)
            {
                Roles.AddUserToRoles(id, roles);
            }
        }
        catch (Exception ex)
        {
            Utils.Log("Profile.Edit: " + ex.Message);
            _response.Message = "Could not update the profile: " + vals[0];
            return _response;
        }

        _response.Success = true;
        _response.Message = string.Format("Pforile {0} updated", vals[0]);
        return _response;
    }
    
}