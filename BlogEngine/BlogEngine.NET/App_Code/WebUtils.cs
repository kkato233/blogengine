namespace App_Code
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using BlogEngine.Core;

    public static class WebUtils
    {
        /// <summary>
        /// Checks to see if the current user has the rights to access an
        /// admin settings page.
        /// </summary>
        /// <param name="checkOnly">
        /// If true, check only. If false and rights are insufficient, user
        /// will be redirected to the login page.
        /// </param>
        /// <returns>True if user has sufficient rights</returns>
        public static bool CheckRightsForAdminSettingsPage(bool checkOnly)
        {
            if (checkOnly)
            {
                return
                    Security.IsAuthorizedTo(AuthorizationCheck.HasAll,
                        BlogEngine.Core.Rights.AccessAdminPages,
                        BlogEngine.Core.Rights.AccessAdminSettingsPages);
            }
            else
            {
                Security.DemandUserHasRight(AuthorizationCheck.HasAll, true,
                    BlogEngine.Core.Rights.AccessAdminPages,
                    BlogEngine.Core.Rights.AccessAdminSettingsPages);
            }

            return true;
        }

    }
}