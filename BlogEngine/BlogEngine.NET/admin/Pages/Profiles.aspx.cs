namespace admin.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    using BlogEngine.Core;

    using Resources;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The admin_profiles.
    /// </summary>
    public partial class admin_profiles : Page
    {
        #region Public Methods

        /// <summary>
        /// Binds the country dropdown list with countries retrieved
        ///     from the .NET Framework.
        /// </summary>
        public void BindCountries()
        {
            var dic = new StringDictionary();
            var col = new List<string>();

            foreach (var ri in
                CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(ci => new RegionInfo(ci.Name)))
            {
                if (!dic.ContainsKey(ri.EnglishName))
                {
                    dic.Add(ri.EnglishName, ri.TwoLetterISORegionName.ToLowerInvariant());
                }

                if (!col.Contains(ri.EnglishName))
                {
                    col.Add(ri.EnglishName);
                }
            }

            // Add custom cultures
            if (!dic.ContainsValue("bd"))
            {
                dic.Add("Bangladesh", "bd");
                col.Add("Bangladesh");
            }

            col.Sort();

            this.ddlCountry.Items.Add(new ListItem("[Not specified]", string.Empty));
            foreach (var key in col)
            {
                this.ddlCountry.Items.Add(new ListItem(key, dic[key]));
            }

            this.SetDefaultCountry();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.ClearFormControls();
                this.BindCountries();
                this.SetDdlUser();
                this.SetProfile(this.User.Identity.Name);
                this.dropdown.Visible = this.Page.User.IsInRole(BlogSettings.Instance.AdministratorRole);
            }

            this.lbSaveProfile.Text = labels.saveProfile;
            this.Page.Title = labels.profile;
        }

        /// <summary>
        /// Handles the Click event of the lbChangeUserProfile control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void lbChangeUserProfile_Click(object sender, EventArgs e)
        {
            this.SetProfile(this.ddlUserList.SelectedValue);
        }

        /// <summary>
        /// Handles the Click event of the lbSaveProfile control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void lbSaveProfile_Click(object sender, EventArgs e)
        {
            var userProfileToSave = this.ViewState["selectedProfile"] as string;
            var pc = AuthorProfile.GetProfile(userProfileToSave) ?? new AuthorProfile(userProfileToSave);

            pc.Private = this.cbIsPublic.Checked;
            pc.DisplayName = this.tbDisplayName.Text;
            pc.FirstName = this.tbFirstName.Text;
            pc.MiddleName = this.tbMiddleName.Text;
            pc.LastName = this.tbLastName.Text;

            DateTime date;
            if (DateTime.TryParse(this.tbBirthdate.Text, out date))
            {
                pc.Birthday = date;
            }

            pc.PhotoUrl = this.tbPhotoUrl.Text;
            pc.PhoneMain = this.tbPhoneMain.Text;
            pc.PhoneMobile = this.tbPhoneMobile.Text;
            pc.PhoneFax = this.tbPhoneFax.Text;
            pc.EmailAddress = this.tbEmailAddress.Text;
            pc.CityTown = this.tbCityTown.Text;
            pc.RegionState = this.tbRegionState.Text;
            pc.Country = this.ddlCountry.SelectedValue;
            pc.Company = this.tbCompany.Text;
            pc.AboutMe = this.tbAboutMe.Text;

            pc.Save();
        }

        /// <summary>
        /// Returns the ListItem that has a value matching the Value passed in
        ///     via a OrdinalIgnoreCase search.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="value">
        /// The Value.
        /// </param>
        /// <returns>
        /// The list item.
        /// </returns>
        private static ListItem FindItemInListControlByValue(ListControl control, string value)
        {
            return
                control.Items.Cast<ListItem>().FirstOrDefault(
                    li => string.Equals(value, li.Value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Sets the flag image URL.
        /// </summary>
        private static void SetFlagImageUrl()
        {
            // if (!string.IsNullOrEmpty(ddlCountry.SelectedValue))
            // {
            // imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/flags/" + ddlCountry.SelectedValue + ".png";
            // }
            // else
            // {
            // imgFlag.ImageUrl = Utils.RelativeWebRoot + "pics/pixel.png";
            // }
        }

        /// <summary>
        /// The clear form controls.
        /// </summary>
        private void ClearFormControls()
        {
            this.cbIsPublic.Checked = false;
            this.tbDisplayName.Text = string.Empty;
            this.tbFirstName.Text = string.Empty;
            this.tbMiddleName.Text = string.Empty;
            this.tbLastName.Text = string.Empty;
            this.tbBirthdate.Text = string.Empty;
            this.tbPhotoUrl.Text = string.Empty;
            this.tbPhoneMain.Text = string.Empty;
            this.tbPhoneMobile.Text = string.Empty;
            this.tbPhoneFax.Text = string.Empty;
            this.tbEmailAddress.Text = string.Empty;
            this.tbCityTown.Text = string.Empty;
            this.tbRegionState.Text = string.Empty;
            this.tbCompany.Text = string.Empty;
            this.tbAboutMe.Text = string.Empty;

            if (this.ddlCountry.Items.Count > 0)
            {
                this.ddlCountry.SelectedIndex = 0;
            }

            this.SetDefaultCountry();
        }

        /// <summary>
        /// The set ddl user.
        /// </summary>
        private void SetDdlUser()
        {
            foreach (var li in
                Membership.GetAllUsers().Cast<MembershipUser>().Select(
                    user => new ListItem(user.UserName, user.UserName)))
            {
                this.ddlUserList.Items.Add(li);
            }
        }

        /// <summary>
        /// The set default country.
        /// </summary>
        private void SetDefaultCountry()
        {
            if (this.ddlCountry.SelectedIndex != 0 || this.Request.UserLanguages == null ||
                this.Request.UserLanguages[0].Length != 5)
            {
                return;
            }

            // Do a case-insensitive search for the country name.  Some browsers (like Chrome) report
            // their UserLanguages in uppercase.
            var countryToSelect = FindItemInListControlByValue(
                this.ddlCountry, this.Request.UserLanguages[0].Substring(3));
            if (countryToSelect != null)
            {
                this.ddlCountry.SelectedValue = countryToSelect.Value;
            }

            SetFlagImageUrl();
        }

        /// <summary>
        /// The set profile.
        /// </summary>
        /// <param name="name">
        /// The profile name.
        /// </param>
        private void SetProfile(string name)
        {
            var pc = AuthorProfile.GetProfile(name);
            if (pc != null)
            {
                this.cbIsPublic.Checked = pc.Private;
                this.tbDisplayName.Text = pc.DisplayName;
                this.tbFirstName.Text = pc.FirstName;
                this.tbMiddleName.Text = pc.MiddleName;
                this.tbLastName.Text = pc.LastName;

                if (pc.Birthday != DateTime.MinValue)
                {
                    this.tbBirthdate.Text = pc.Birthday.ToString("yyyy-MM-dd");
                }

                this.tbPhotoUrl.Text = pc.PhotoUrl;
                this.tbPhoneMain.Text = pc.PhoneMain;
                this.tbPhoneMobile.Text = pc.PhoneMobile;
                this.tbPhoneFax.Text = pc.PhoneFax;
                this.tbEmailAddress.Text = pc.EmailAddress;
                this.tbCityTown.Text = pc.CityTown;
                this.tbRegionState.Text = pc.RegionState;
                this.ddlCountry.SelectedValue = pc.Country;
                this.tbCompany.Text = pc.Company;
                this.tbAboutMe.Text = pc.AboutMe;
            }
            else
            {
                // Clear any data in the form controls remaining from the last profile selected.
                this.ClearFormControls();
            }

            // Sync the dropdownlist user with the selected profile user.  This is
            // particularily needed on the initial page load (!IsPostBack).
            var selectedUser = FindItemInListControlByValue(this.ddlUserList, name);
            if (selectedUser != null)
            {
                selectedUser.Selected = true;
            }

            // Store the selected profile name so changes are saved to this
            // profile and not another profile the user may later select and
            // forget to click the lbChangeUserProfile button for.
            this.ViewState["selectedProfile"] = name;
        }

        #endregion
    }
}