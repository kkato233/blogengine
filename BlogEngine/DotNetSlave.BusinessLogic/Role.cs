namespace BlogEngine.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// The role class.
    /// </summary>
    public class Role
    {
        #region Constants and Fields

        private readonly object synclock = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">
        /// A name of the role.
        /// </param>
        public Role(string name) : this(name, new List<string>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Role" /> class.
        /// </summary>
        public Role() : this(null, new List<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">
        /// A name of the role.
        /// </param>
        /// <param name="userNames">
        /// A list of users in role.
        /// </param>
        public Role(string name, List<string> userNames)
        {
            if (userNames == null)
            {
                throw new System.ArgumentNullException("userNames");
            }
            else
            {
                this.Name = name;
                this.Users = userNames;
                this.UpdateRights(new List<Right>()); // Creates the initial empty list of Rights.
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name of the role.</value>
        public string Name { get; set; }


        #region Rights IEnumerable property
        /// <summary>
        /// Gets the rights or permissions associated with this Role.
        /// </summary>
        public IEnumerable<Right> Rights { get { return this._publicRights; } }

        /// <summary>
        /// This holds the private set of rights this role belongs to. This shouldn't be returned to any
        /// other instance directly. Copies should be made to prevent outside access from modifying it illegally.
        /// </summary>
        private ReadOnlyCollection<Right> _publicRights;

        #endregion

        /// <summary>
        ///     Gets the users.
        /// </summary>
        /// <value>The users.</value>
        public List<string> Users { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Call this method when the internal rights list has been dirtied. This updates the public facing read only collection.
        /// </summary>
        internal void UpdateRights(IEnumerable<Right> updatedRights)
        {
            lock (this.synclock)
            {
                if (updatedRights == null)
                {
                    updatedRights = new List<Right>();
                }

                var newRights = new List<Right>();

                if (!updatedRights.Any())
                {
                    newRights.Add(Right.GetRightByFlag(RightFlags.None));
                }
                else
                {
                    newRights.AddRange(updatedRights);
                }

                this._publicRights = new ReadOnlyCollection<Right>(newRights);

            }

        }

        /// <summary>
        /// Sets this Role's Rights collection to the Rights found in the given value.
        /// </summary>
        /// <param name="rights"></param>
        internal void SetRights(IEnumerable<Right> rights)
        {

            this.UpdateRights(rights.Distinct().ToList());

        }

        #endregion

    }
}