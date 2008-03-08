#region Using

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Providers;
using BlogEngine.Core;

#endregion

public partial class admin_profiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SetDDLUser();
            SetProfile(User.Identity.Name);
        }
    }

    private void SetProfile(string name)
    {
        ProfileCommon pc = new ProfileCommon().GetProfile(name);
        tbFirstName.Text = pc.FirstName;
        tbLastName.Text = pc.LastName;
    }

    private void SetDDLUser()
    {
        foreach (MembershipUser user in Membership.GetAllUsers())
        {
            ListItem li = new ListItem(user.UserName, user.UserName);
            ddlUserList.Items.Add(li);
        }
    }


    protected void lbSaveProfile_Click(object sender, EventArgs e)
    {
        string userProfileToSave = User.IsInRole("Administrator") ? ddlUserList.SelectedValue : User.Identity.Name;
        ProfileCommon pc = new ProfileCommon().GetProfile(userProfileToSave);
        pc.FirstName = tbFirstName.Text;
        pc.LastName = tbLastName.Text ;
        pc.Save();
    }

    protected void lbChangeUserProfile_Click(object sender, EventArgs e)
    {
        SetProfile(ddlUserList.SelectedValue);
    }
}
