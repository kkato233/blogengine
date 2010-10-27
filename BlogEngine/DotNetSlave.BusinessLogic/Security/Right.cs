using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogEngine.Core
{
    /// <summary>
    /// A wrapper class for RightFlags enum values that allows for providing more information.
    /// </summary>
    public sealed class Right
    {

        #region "Static"

        #region "Fields"

        private static readonly Dictionary<string, Right> rightsByName = new Dictionary<string, Right>(StringComparer.OrdinalIgnoreCase);
        private static readonly IEnumerable<RightFlags> flagValues;
        private static readonly Dictionary<RightFlags, Right> flagDict = new Dictionary<RightFlags, Right>();

        #endregion

        static Right()
        {

            var flags = new List<RightFlags>();
            var flagType = typeof(RightFlags);


            // Create a Right instance for each value in the RightFlags enum.
            foreach (var flag in Enum.GetValues(flagType))
            {
                RightFlags curFlag = (RightFlags)flag;
                var flagName = Enum.GetName(flagType, curFlag);
                var curRight = new Right(curFlag);

                // Use the Add function so if there are multiple flags with the same
                // value they can be caught quickly at runtime.
                flagDict.Add(curFlag, curRight);

                flags.Add(curFlag);
                rightsByName.Add(flagName, curRight);
            }

            flagValues = flags;
        }

        #region "Methods"

        ///// <summary>
        ///// Returns an IEnumerable of Right objects based on the flags contained in the given value.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static IEnumerable<Right> GetRightsFromFlag(RightFlags value)
        //{
        //    // This entire method could probably be set up in a cacheable way.
        //    // It may also be better off returning a readonly dictionary too.

        //    var result = new List<Right>();

        //    if (flagDict.ContainsKey(value))
        //    {
        //        // If it's in the dictionary, then it's only a single value, so there's
        //        // no need to do any further processing.
        //        result.Add(flagDict[value]);
        //    }
        //    else
        //    {
        //        foreach (var flag in flagValues)
        //        {
        //            if (FlagContainsValue(value, flag))
        //            {
        //                result.Add(flagDict[flag]);
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Gets whether or not a combined flag value contains a specific flag.
        ///// </summary>
        ///// <param name="flags"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static bool FlagContainsValue(RightFlags flags, RightFlags value)
        //{
        //    var longFlags = Convert.ToUInt64(flags);
        //    var longCheckFlag = Convert.ToUInt16(value);

        //    return ((longFlags & longCheckFlag) == longCheckFlag);
        //}

        public static IEnumerable<Right> GetAllRights()
        {
            return flagDict.Values.ToList();
        }

        public static Right GetRightByName(string rightName)
        {
            if (Utils.StringIsNullOrWhitespace(rightName))
            {
                throw new ArgumentNullException("rightName");
            }
            else
            {
                Right right = null;
                if (rightsByName.TryGetValue(rightName.Trim(), out right))
                {
                    return right;
                }
                else
                {
                    throw new KeyNotFoundException("No Right exists by the name '" + rightName + "'");
                }
            }
        }

        public static Right GetRightByFlag(RightFlags flag)
        {

            Right right = null;
            if (flagDict.TryGetValue(flag, out right))
            {
                return right;
            }
            else
            {
                throw new KeyNotFoundException("Unable to find a cooresponding right for the given flag");
            }

        }

        #endregion

        #endregion

        #region "Instance"


        /// <summary>
        /// Private constructor for creating a Right instance.
        /// </summary>
        /// <param name="rightFlag"></param>
        private Right(RightFlags rightFlag)
        {
            this._flag = rightFlag;
        }

        /// <summary>
        /// Gets the RightFlag value for this Right instance.
        /// </summary>
        public RightFlags Flag
        {
            get
            {
                return this._flag;
            }
        }
        private readonly RightFlags _flag;

        #endregion

    }


    /// <summary>
    /// Enum that represents rights or permissions that are used through out BlogEngine.
    /// </summary>
    /// <remarks>
    /// 
    /// This was originally a Flag-based enum, but I decided to change that just because it would lead to really annoying
    /// conflicts if new values are added down the line either by other users or as part of the core library.
    /// 
    /// Also, at the moment this doesn't nearly represent all the current possible actions. This is just a few
    /// test values to play with.
    /// 
    /// I'd recommend using a common word pattern when used. Ie: Create/Edit/Delete/Publish as prefixes.
    /// 
    /// </remarks>
    public enum RightFlags
    {

        /// <summary>
        /// Represents a user that has no rights or permissions. This flag should not be used in combination with any other flag.
        /// </summary>
        None = 0,

        /// <summary>
        /// A user is allowed to create and submit comments for posts or pages.
        /// </summary>
        CreateComments,

        // Values pertaining to POSTS
        CreateNewPosts,

        EditOwnPosts,
        EditOtherUsersPosts,

        DeleteOwnPosts,
        DeleteOtherUsersPosts,

        PublishOwnPosts,
        PublishOtherUsersPosts,


        // Values pertaining to PAGES
        CreateNewPages,

        EditOwnPages,
        EditOtherUsersPages,

        DeleteOwnPages,
        DeleteOtherUsersPages,

        PublishOwnPages,
        PublicOtherUsersPages,

        /// <summary>
        /// User can approve, delete, or mark comments as spam.
        /// </summary>
        ModerateComments,

        // Roles
        CreateNewRoles,
        EditRoles,
        DeleteRoles
    }

}
