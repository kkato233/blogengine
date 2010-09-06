namespace BlogEngine.Core
{
    using System;
    using System.Collections.Generic;

    using BlogEngine.Core.Providers;

    /// <summary>
    /// The author profile.
    /// </summary>
    public class AuthorProfile : BusinessBase<AuthorProfile, string>
    {
        #region Constants and Fields

        /// <summary>
        /// The sync root.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The profiles.
        /// </summary>
        private static List<AuthorProfile> profiles;

        /// <summary>
        /// The about me.
        /// </summary>
        private string aboutMe;

        /// <summary>
        /// The birthday.
        /// </summary>
        private DateTime birthday;

        /// <summary>
        /// The city town.
        /// </summary>
        private string cityTown;

        /// <summary>
        /// The company.
        /// </summary>
        private string company;

        /// <summary>
        /// The country.
        /// </summary>
        private string country;

        /// <summary>
        /// The display name.
        /// </summary>
        private string displayName;

        /// <summary>
        /// The email address.
        /// </summary>
        private string emailAddress;

        /// <summary>
        /// The first name.
        /// </summary>
        private string firstName;

        /// <summary>
        /// The is private.
        /// </summary>
        private bool isprivate;

        /// <summary>
        /// The last name.
        /// </summary>
        private string lastName;

        /// <summary>
        /// The middle name.
        /// </summary>
        private string middleName;

        /// <summary>
        /// The phone fax.
        /// </summary>
        private string phoneFax;

        /// <summary>
        /// The phone main.
        /// </summary>
        private string phoneMain;

        /// <summary>
        /// The phone mobile.
        /// </summary>
        private string phoneMobile;

        /// <summary>
        /// The photo url.
        /// </summary>
        private string photoUrl;

        /// <summary>
        /// The region state.
        /// </summary>
        private string regionState;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorProfile"/> class.
        /// </summary>
        public AuthorProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorProfile"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        public AuthorProfile(string username)
        {
            this.Id = username;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets an unsorted list of all pages.
        /// </summary>
        public static List<AuthorProfile> Profiles
        {
            get
            {
                if (profiles == null)
                {
                    lock (SyncRoot)
                    {
                        if (profiles == null)
                        {
                            profiles = BlogService.FillProfiles();
                        }
                    }
                }

                return profiles;
            }
        }

        /// <summary>
        /// Gets or sets AboutMe.
        /// </summary>
        public string AboutMe
        {
            get
            {
                return this.aboutMe;
            }

            set
            {
                if (value != this.aboutMe)
                {
                    this.MarkChanged("AboutMe");
                }

                this.aboutMe = value;
            }
        }

        /// <summary>
        /// Gets or sets Birthday.
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return this.birthday;
            }

            set
            {
                if (value != this.birthday)
                {
                    this.MarkChanged("Birthday");
                }

                this.birthday = value;
            }
        }

        // private string _Address1;
        // public string Address1
        // {
        // get { return _Address1; }
        // set { _Address1 = value; }
        // }

        // private string _Address2;

        // public string Address2
        // {
        // get { return _Address2; }
        // set { _Address2 = value; }
        // }

        /// <summary>
        /// Gets or sets CityTown.
        /// </summary>
        public string CityTown
        {
            get
            {
                return this.cityTown;
            }

            set
            {
                if (value != this.cityTown)
                {
                    this.MarkChanged("CityTown");
                }

                this.cityTown = value;
            }
        }

        /// <summary>
        /// Gets or sets Company.
        /// </summary>
        public string Company
        {
            get
            {
                return this.company;
            }

            set
            {
                if (value != this.company)
                {
                    this.MarkChanged("Company");
                }

                this.company = value;
            }
        }

        /// <summary>
        /// Gets or sets Country.
        /// </summary>
        public string Country
        {
            get
            {
                return this.country;
            }

            set
            {
                if (value != this.country)
                {
                    this.MarkChanged("Country");
                }

                this.country = value;
            }
        }

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }

            set
            {
                if (value != this.displayName)
                {
                    this.MarkChanged("DisplayName");
                }

                this.displayName = value;
            }
        }

        /// <summary>
        /// Gets or sets EmailAddress.
        /// </summary>
        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }

            set
            {
                if (value != this.emailAddress)
                {
                    this.MarkChanged("EmailAddress");
                }

                this.emailAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                if (value != this.firstName)
                {
                    this.MarkChanged("FirstName");
                }

                this.firstName = value;
            }
        }

        /// <summary>
        /// Gets FullName.
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName).Replace("  ", " ");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Private.
        /// </summary>
        public bool Private
        {
            get
            {
                return this.isprivate;
            }

            set
            {
                if (value != this.isprivate)
                {
                    this.MarkChanged("Private");
                }

                this.isprivate = value;
            }
        }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                if (value != this.lastName)
                {
                    this.MarkChanged("LastName");
                }

                this.lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets MiddleName.
        /// </summary>
        public string MiddleName
        {
            get
            {
                return this.middleName;
            }

            set
            {
                if (value != this.middleName)
                {
                    this.MarkChanged("MiddleName");
                }

                this.middleName = value;
            }
        }

        /// <summary>
        /// Gets or sets PhoneFax.
        /// </summary>
        public string PhoneFax
        {
            get
            {
                return this.phoneFax;
            }

            set
            {
                if (value != this.phoneFax)
                {
                    this.MarkChanged("PhoneFax");
                }

                this.phoneFax = value;
            }
        }

        /// <summary>
        /// Gets or sets PhoneMain.
        /// </summary>
        public string PhoneMain
        {
            get
            {
                return this.phoneMain;
            }

            set
            {
                if (value != this.phoneMain)
                {
                    this.MarkChanged("PhoneMain");
                }

                this.phoneMain = value;
            }
        }

        /// <summary>
        /// Gets or sets PhoneMobile.
        /// </summary>
        public string PhoneMobile
        {
            get
            {
                return this.phoneMobile;
            }

            set
            {
                if (value != this.phoneMobile)
                {
                    this.MarkChanged("PhoneMobile");
                }

                this.phoneMobile = value;
            }
        }

        /// <summary>
        /// Gets or sets PhotoURL.
        /// </summary>
        public string PhotoUrl
        {
            get
            {
                return this.photoUrl;
            }

            set
            {
                if (value != this.photoUrl)
                {
                    this.MarkChanged("PhotoURL");
                }

                this.photoUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets RegionState.
        /// </summary>
        public string RegionState
        {
            get
            {
                return this.regionState;
            }

            set
            {
                if (value != this.regionState)
                {
                    this.MarkChanged("RegionState");
                }

                this.regionState = value;
            }
        }

        /// <summary>
        /// Gets RelativeLink.
        /// </summary>
        public string RelativeLink
        {
            get
            {
                return string.Format("{0}author/{1}.aspx", Utils.RelativeWebRoot, this.Id);
            }
        }

        /// <summary>
        /// Gets UserName.
        /// </summary>
        public string UserName
        {
            get
            {
                return this.Id;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The AuthorProfile.</returns>
        public static AuthorProfile GetProfile(string username)
        {
            return
                Profiles.Find(p => p.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.FullName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Datas the delete.
        /// </summary>
        protected override void DataDelete()
        {
            BlogService.DeleteProfile(this);
            if (Profiles.Contains(this))
            {
                Profiles.Remove(this);
            }
        }

        /// <summary>
        /// Datas the insert.
        /// </summary>
        protected override void DataInsert()
        {
            BlogService.InsertProfile(this);

            if (this.New)
            {
                Profiles.Add(this);
            }
        }

        /// <summary>
        /// Datas the select.
        /// </summary>
        /// <param name="id">The AuthorProfile id.</param>
        /// <returns>The AuthorProfile.</returns>
        protected override AuthorProfile DataSelect(string id)
        {
            return BlogService.SelectProfile(id);
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected override void DataUpdate()
        {
            BlogService.UpdateProfile(this);
        }

        /// <summary>
        /// Validates based on rules.
        /// </summary>
        protected override void ValidationRules()
        {
            this.AddRule(
                "Id", 
                "Id must be set to the username of the user who the profile belongs to", 
                string.IsNullOrEmpty(this.Id));
        }

        #endregion
    }
}