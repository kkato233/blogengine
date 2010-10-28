using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Security;

namespace BlogEngine.Core
{

    /// <summary>
    /// Enum that represents rights or permissions that are used through out BlogEngine.
    /// </summary>
    /// <remarks>
    /// 
    /// Each Rights enum value is wrapped by an associated Right instance that contains information about roles/descriptions/etc.
    /// 
    /// When a Rights value is serialized to persistant storage, the enum's string name should be used in order to prevent
    /// conflicts with value changes due to new values being added(either through updates or customization).
    /// 
    /// Also, at the moment this doesn't nearly represent all the current possible actions. This is just a few
    /// test values to play with.
    /// 
    /// I'd recommend using a common word pattern when used. Ie: Create/Edit/Delete/Publish as prefixes.
    /// 
    /// </remarks>
    public enum Rights
    {

        /// <summary>
        /// Represents a user that has no rights or permissions. This flag should not be used in combination with any other flag.
        /// </summary>
        /// <remarks>
        /// 
        /// This value isn't meant for public consumption.
        /// 
        /// </remarks>
        None = 0,


        #region "Comments"

        ViewComments,

        /// <summary>
        /// A user is allowed to create and submit comments for posts or pages.
        /// </summary>
        CreateComments,

        /// <summary>
        /// User can approve, delete, or mark comments as spam.
        /// </summary>
        ModerateComments,

        #endregion

        /// <summary>
        /// A user is allowed to register/create a new account. 
        /// </summary>
        CreateNewAccounts,

        #region Posts

        ViewPosts,

        CreateNewPosts,

        EditOwnPosts,
        EditOtherUsersPosts,

        DeleteOwnPosts,
        DeleteOtherUsersPosts,

        PublishOwnPosts,
        PublishOtherUsersPosts,

        #endregion

        // Values pertaining to PAGES
        ViewPages,
        CreateNewPages,

        EditOwnPages,
        EditOtherUsersPages,

        DeleteOwnPages,
        DeleteOtherUsersPages,

        PublishOwnPages,
        PublishOtherUsersPages,


        // Roles
        ViewRoles,
        CreateNewRoles,
        EditRoles,
        DeleteRoles,
    }


    /// <summary>
    /// Attribute used to provide extra information about a Rights enum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=false, Inherited=false)]
    public sealed class RightDetailsAttribute : Attribute
    {
        public RightDetailsAttribute()
        {

        }

        #region "Properties"

        public string DescriptionResourceLabelKey { get; set; }
        public string NameResourceLabelKey { get; set; }

        #endregion

    }
}