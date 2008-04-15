using System.Web.Profile;

namespace BlogEngine.Core.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorProfile : ProfileBase
    {


        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public bool IsPrivate
        {
            get { return (bool)base["IsPrivate"]; }
            set { base["IsPrivate"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string FirstName
        {
            get { return base["FirstName"].ToString(); }
            set { base["FirstName"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string MiddleName
        {
            get { return base["MiddleName"].ToString(); }
            set { base["MiddleName"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string LastName
        {
            get { return base["LastName"].ToString(); }
            set { base["LastName"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string DisplayName
        {
            get { return base["DisplayName"].ToString(); }
            set { base["DisplayName"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Title
        {
            get { return base["Title"].ToString(); }
            set { base["Title"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string PhotoURL
        {
            get { return base["PhotoURL"].ToString(); }
            set { base["PhotoURL"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Birthday
        {
            get { return base["Birthday"].ToString(); }
            set { base["Birthday"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Address1
        {
            get { return base["Address1"].ToString(); }
            set { base["Address1"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Address2
        {
            get { return base["Address2"].ToString(); }
            set { base["Address2"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string CityTown
        {
            get { return base["CityTown"].ToString(); }
            set { base["CityTown"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string RegionState
        {
            get { return base["RegionState"].ToString(); }
            set { base["RegionState"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Country
        {
            get { return base["Country"].ToString(); }
            set { base["Country"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string PhoneMain
        {
            get { return base["PhoneMain"].ToString(); }
            set { base["PhoneMain"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string PhoneFax
        {
            get { return base["PhoneFax"].ToString(); }
            set { base["PhoneFax"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string PhoneMobile
        {
            get { return base["PhoneMobile"].ToString(); }
            set { base["PhoneMobile"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string EmailAddress
        {
            get { return base["EmailAddress"].ToString(); }
            set { base["EmailAddress"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Industry
        {
            get { return base["Industry"].ToString(); }
            set { base["Industry"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Company
        {
            get { return base["Company"].ToString(); }
            set { base["Company"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string AboutMe
        {
            get { return base["AboutMe"].ToString(); }
            set { base["AboutMe"] = value; }
        }

    }
}