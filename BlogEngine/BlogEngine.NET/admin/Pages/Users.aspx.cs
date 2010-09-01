using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Security;
using BlogEngine.Core;

public partial class admin_newuser : System.Web.UI.Page
{
    JsonResponse _response;

	protected void Page_Load(object sender, EventArgs e)
	{
        _response = new JsonResponse();
	}

    [WebMethod]
    public static List<MembershipUser> GetUsers()
    {
        int count = 0;
        MembershipUserCollection userCollection =  Membership.Provider.GetAllUsers(0, 999, out count);
        List<MembershipUser> users = new List<MembershipUser>();

        foreach (MembershipUser user in userCollection)
        {
            users.Add(user);
        }

        users.Sort(delegate(MembershipUser u1, MembershipUser u2)
        { return string.Compare(u1.UserName, u2.UserName); });

        return users;
    }
}