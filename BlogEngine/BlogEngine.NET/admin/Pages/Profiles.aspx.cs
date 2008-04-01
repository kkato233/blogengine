#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using BlogEngine.Core;
using Page=System.Web.UI.Page;

#endregion

public partial class admin_profiles : Page
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
        cbIsPublic.Checked = pc.IsPrivate;
        tbRegionState.Text = pc.RegionState;
        tbCityTown.Text = pc.CityTown;
        ddlCountry.SelectedValue = pc.Country;
        tbAboutMe.Text = pc.AboutMe;
        tbInterests.Text = pc.Interests;
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
        pc.LastName = tbLastName.Text;
        pc.IsPrivate = cbIsPublic.Checked;
        pc.RegionState = tbRegionState.Text;
        pc.CityTown = tbCityTown.Text;
        pc.Country = ddlCountry.SelectedValue;
        pc.AboutMe = tbAboutMe.Text;
        pc.Interests = tbInterests.Text;
        pc.Save();
    }

    protected void lbChangeUserProfile_Click(object sender, EventArgs e)
    {
        SetProfile(ddlUserList.SelectedValue);
    }


    /// <summary>
    /// Binds the country dropdown list with countries retrieved
    /// from the .NET Framework.
    /// </summary>
    public void BindCountries()
    {
        StringDictionary dic = new StringDictionary();
        List<string> col = new List<string>();

        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            RegionInfo ri = new RegionInfo(ci.Name);
            if (!dic.ContainsKey(ri.EnglishName))
                dic.Add(ri.EnglishName, ri.TwoLetterISORegionName.ToLowerInvariant());

            if (!col.Contains(ri.EnglishName))
                col.Add(ri.EnglishName);
        }

        // Add custom cultures
        if (!dic.ContainsValue("bd"))
        {
            dic.Add("Bangladesh", "bd");
            col.Add("Bangladesh");
        }

        col.Sort();

        ddlCountry.Items.Add(new ListItem("[Not specified]", ""));
        foreach (string key in col)
        {
            ddlCountry.Items.Add(new ListItem(key, dic[key]));
        }

        if (ddlCountry.SelectedIndex == 0 && Request.UserLanguages != null && Request.UserLanguages[0].Length == 5)
        {
            ddlCountry.SelectedValue = Request.UserLanguages[0].Substring(3);
            SetFlagImageUrl();
        }
    }

    private void SetFlagImageUrl()
    {
        if (!string.IsNullOrEmpty(ddlCountry.SelectedValue))
        {
            imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/flags/" + ddlCountry.SelectedValue + ".png";
        }
        else
        {
            imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/pixel.png";
        }
    }
}