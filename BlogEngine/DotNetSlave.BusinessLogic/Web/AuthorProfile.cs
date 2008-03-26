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
            get { return (bool) base["IsPrivate"]; }
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
        public string PhotoURL
        {
            get { return base["PhotoURL"].ToString(); }
            set { base["PhotoURL"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Gender
        {
            get { return base["Gender"].ToString(); }
            set { base["Gender"] = value; }
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
        public string Industry
        {
            get { return base["Industry"].ToString(); }
            set { base["Industry"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Occupation
        {
            get { return base["Occupation"].ToString(); }
            set { base["Occupation"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SettingsAllowAnonymous(false)]
        public string Interests
        {
            get { return base["Interests"].ToString(); }
            set { base["Interests"] = value; }
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