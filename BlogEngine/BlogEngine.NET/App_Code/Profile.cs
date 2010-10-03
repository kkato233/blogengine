namespace App_Code
{
    using System;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    /// <summary>
    /// The profile.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Profile : WebService
    {
        #region Constants and Fields

        /// <summary>
        ///     JSON object that will be return back to client
        /// </summary>
        private readonly JsonResponse response;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
        {
            this.response = new JsonResponse();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the specified id.
        /// </summary>
        /// <param name="id">The profile id.</param>
        /// <param name="vals">The values.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>JSON response.</returns>
        [WebMethod]
        public JsonResponse Save(string id, string[] vals, string[] roles)
        {
            this.response.Success = false;

            if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
            {
                this.response.Message = "Not authorized";
                return this.response;
            }

            if (string.IsNullOrEmpty(vals[0]))
            {
                this.response.Message = "Display name is required field";
                return this.response;
            }

            var pf = AuthorProfile.GetProfile(id) ?? new AuthorProfile(id);

            try
            {
                pf.DisplayName = vals[0];
                pf.FirstName = vals[1];
                pf.MiddleName = vals[2];
                pf.LastName = vals[3];
                pf.EmailAddress = vals[4];

                DateTime date;
                if (vals[5].Length == 0)
                {
                    vals[5] = "1/1/1001";
                }

                if (DateTime.TryParse(vals[5], out date))
                {
                    pf.Birthday = date;
                }
                else
                {
                    this.response.Message = "Date must be in format mm/dd/yyyy";
                    return this.response;
                }

                pf.PhotoUrl = vals[6];
                pf.Private = false;

                bool prv;
                if (bool.TryParse(vals[7], out prv))
                {
                    pf.Private = prv;
                }

                pf.PhoneMobile = vals[8];
                pf.PhoneMain = vals[9];
                pf.PhoneFax = vals[10];

                pf.CityTown = vals[11];
                pf.RegionState = vals[12];
                pf.Country = vals[13]; // ddlCountry.SelectedValue;

                // pf.Company = tbCompany.Text;
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
                Utils.Log(string.Format("Profile.Edit: {0}", ex.Message));
                this.response.Message = string.Format("Could not update the profile: {0}", vals[0]);
                return this.response;
            }

            this.response.Success = true;
            this.response.Message = string.Format("Pforile {0} updated", vals[0]);
            return this.response;
        }

        #endregion
    }
}