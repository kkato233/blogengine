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
    /// I'd recommend using a common word pattern when used. Ie: Create/Edit/Delete/Publish as prefixes. The names
    /// should be very specific to what they allow in order to avoid confusion. For example, don't use a name like
    /// "ViewPosts". Use something that also specifies the kinds of posts, like ViewPublicPosts, ViewPrivatePosts, or
    /// ViewUnpublishedPosts.
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

        #region Misc

        /// <summary>
        /// A user is allowed to view exception messages.
        /// </summary>
        ViewDetailedErrorMessages,

        #endregion

        #region "Comments"

        /// <summary>
        /// A user is allowed to view comments on a post.
        /// </summary>
        ViewPublicComments,

        /// <summary>
        /// A user is allowed to view comments that have not been moderation yet.
        /// </summary>
        ViewUnmoderatedComments,


        /// <summary>
        /// A user is allowed to create and submit comments for posts or pages.
        /// </summary>
        CreateComments,

        /// <summary>
        /// User can approve, delete, or mark comments as spam.
        /// </summary>
        ModerateComments,

        #endregion

 
        #region Posts

        /// <summary>
        /// A user is allowed to view posts that are both published and public.
        /// </summary>
        ViewPublicPosts,

        /// <summary>
        /// A user is allowed to view unpublished posts.
        /// </summary>
        ViewUnpublishedPosts,

        /// <summary>
        /// A user is allowed to view non-public posts.
        /// </summary>
        ViewPrivatePosts,

        /// <summary>
        /// A user can create new posts. 
        /// </summary>
        CreateNewPosts,

        /// <summary>
        /// A user can edit their own posts. 
        /// </summary>
        EditOwnPosts,

        /// <summary>
        /// A user can edit posts created by other users.
        /// </summary>
        EditOtherUsersPosts,

        /// <summary>
        /// A user can delete their own posts.
        /// </summary>
        DeleteOwnPosts,

        /// <summary>
        /// A user can delete posts created by other users.
        /// </summary>
        DeleteOtherUsersPosts,

        /// <summary>
        /// A user can set whether or not their own posts are published.
        /// </summary>
        PublishOwnPosts,

        /// <summary>
        /// A user can set whether or not another user's posts are published.
        /// </summary>
        PublishOtherUsersPosts,

        #endregion

        #region Pages
        
        /// <summary>
        /// A user can view public, published pages.
        /// </summary>
        ViewPublicPages,

        /// <summary>
        /// A user can view unpublished pages.
        /// </summary>
        ViewUnpublishedPages,

        /// <summary>
        /// A user can create new pages.
        /// </summary>
        CreateNewPages,

        /// <summary>
        /// A user can edit pages they've created.
        /// </summary>
        EditOwnPages,

        /// <summary>
        /// A user can edit pages other users have created.
        /// </summary>
        EditOtherUsersPages,

        /// <summary>
        /// A user can delete pages they've created.
        /// </summary>
        DeleteOwnPages,

        /// <summary>
        /// A user can delete pages other users have created.
        /// </summary>
        DeleteOtherUsersPages,

        /// <summary>
        /// A user can set whether or not their own pages are published.
        /// </summary>
        PublishOwnPages,

        /// <summary>
        /// A user can set whether or not another user's pages are published.
        /// </summary>
        PublishOtherUsersPages,

        #endregion

        #region "Ratings"

            /// <summary>
            /// A user can view ratings on posts.
            /// </summary>
            ViewRatingsOnPosts,

            /// <summary>
            /// A user can submit ratings on posts.
            /// </summary>
            SubmitRatingsOnPosts,
        #endregion

        #region Roles

        /// <summary>
        /// A user can view roles.
        /// </summary>
        ViewRoles,

        /// <summary>
        /// A user can create new roles.
        /// </summary>
        CreateNewRoles,

        /// <summary>
        /// A user can edit existing roles.
        /// </summary>
        EditRoles,

        /// <summary>
        /// A user can delete existing roles.
        /// </summary>
        DeleteRoles,

        #endregion

        #region Users

        /// <summary>
        /// A user is allowed to register/create a new account. 
        /// </summary>
        CreateNewUsers,

        /// <summary>
        /// A user is allowed to delete their own account.
        /// </summary>
        DeleteUserSelf,

        /// <summary>
        /// A user is allowed to delete accounts they do not own.
        /// </summary>
        DeleteUsersOtherThanSelf,

        /// <summary>
        /// A user is allowed to edit their own account information.
        /// </summary>
        EditOwnUser,

        /// <summary>
        /// A user is allowed to edit the account information of other users.
        /// </summary>
        EditOtherUsers,

        #endregion
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