﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Script.Serialization;

namespace BlogEngine.Core.Web
{
    /// <summary>
    /// Represents the i18n culture used by blog.js. Used by ResourceHandler.
    /// </summary>
    public sealed class BlogCulture
    {
        #region "Fields"

        private readonly CultureInfo cultureInfo;
        private readonly Dictionary<string, string> translationDict = new Dictionary<string, string>();

        #endregion

        /// <summary>
        /// Creates a new JsonCulture instance from the supplied CultureInfo.
        /// </summary>
        /// <param name="cultureInfo">The CultureInfo needed to get the proper translations for this JsonCulture instance</param>
        /// <param name="resourceType">Type of resource (blog or admin)</param>
        /// <remarks>
        /// 
        /// This class uses a dictionary as its basis for storing/caching its information. This makes it incredibly easy to extend
        /// without having to create/remove properties.
        /// 
        /// </remarks>
        public BlogCulture(CultureInfo cultureInfo, ResourceType resourceType)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo");
            }
            this.cultureInfo = cultureInfo;

            if (resourceType == ResourceType.Admin)
            {
                try
                {
                    var filePath = System.Web.HttpContext.Current.Server.MapPath(
                        System.IO.Path.Combine(BlogConfig.StorageLocation, "labels.txt"));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.StreamReader reader = System.IO.File.OpenText(filePath);
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrEmpty(line) && line.Trim().Length > 0)
                            {
                                AddResource(line);
                            }
                        }
                    }
                    else
                    {
                        AddJavaScriptResources();
                    }
                }
                catch (Exception ex)
                {
                    Utils.Log("Error loading admin labels from App_Data/labels.txt", ex);
                }
            }
            else
            {
                AddResource("hasRated");
                AddResource("savingTheComment");
                AddResource("comments");
                AddResource("commentWasSaved");
                AddResource("commentWaitingModeration");
                AddResource("cancel");
                AddResource("filter");
                AddResource("apmlDescription");
                AddResource("beTheFirstToRate");
                AddResource("currentlyRated");
                AddResource("ratingHasBeenRegistered");
                AddResource("rateThisXStars");
            }
        }

        /// <summary>
        /// Type of language resources
        /// </summary>
        public enum ResourceType
        {
            /// <summary>
            ///     Resources added to Blog
            /// </summary>
            Blog = 0,

            /// <summary>
            ///     Resources added to Admin
            /// </summary>
            Admin = 1
        }

        #region "Methods"

        /// <summary>
        /// Adds a new translatable string resource to this JsonCulture.
        /// </summary>
        /// <param name="resourceLabelKey">The key used to retrieve the translated value from global resource labels.</param>
        /// <returns>The translated string.</returns>
        public string AddResource(string resourceLabelKey)
        {
            var translation = Utils.Translate(resourceLabelKey, null, cultureInfo);
            // "delete" label has a name conflict with jQuery under IE7/8, rename to "doDelete"
            translationDict.Add(resourceLabelKey == "delete" ? "doDelete" : resourceLabelKey, translation);
            return translation;
        }

        /// <summary>
        /// Returns a JSON formatted string repressentation of this JsonCulture instance's culture labels.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            return new JavaScriptSerializer().Serialize(this.translationDict);
        }

        // this is fall back if file with JS
        // resources not in the app_data folder
        void AddJavaScriptResources()
        {
            AddResource("addToPages");
            AddResource("addToWhiteList");
            AddResource("admin");
            AddResource("administrationPanel");
            AddResource("advanced");
            AddResource("advancedSettings");
            AddResource("all");
            AddResource("allow");
            AddResource("allowMobileTheme");
            AddResource("allowRemoteFileDownloads");
            AddResource("allowRemoteFileDownloadsDescription");
            AddResource("AllPostsBy");
            AddResource("AllPostsTagged");
            AddResource("alphaNumeric");
            AddResource("alreadyHaveAccount");
            AddResource("alternateFeedUrl");
            AddResource("alwaysTrust");
            AddResource("anotherEmail");
            AddResource("anotherUserName");
            AddResource("antiSpamServices");
            AddResource("apmlDescription");
            AddResource("appearance");
            AddResource("applyChanges");
            AddResource("approve");
            AddResource("approveallcomments");
            AddResource("approved");
            AddResource("approvedComments");
            AddResource("approveSelected");
            AddResource("archive");
            AddResource("areYouSure");
            AddResource("areYouSureDeleteComment");
            AddResource("areYouSureDeleteEmail");
            AddResource("areYouSureDeletePage");
            AddResource("areYouSureDeleteRow");
            AddResource("attachFile");
            AddResource("author");
            AddResource("authorApproved");
            AddResource("authorBlocked");
            AddResource("authorDispalyCount");
            AddResource("authorDisplayPattern");
            AddResource("authorDisplayPatternAggregated");
            AddResource("authorRejected");
            AddResource("automatically");
            AddResource("automoderation");
            AddResource("available");
            AddResource("avatars");
            AddResource("basic");
            AddResource("beTheFirstToRate");
            AddResource("birthday");
            AddResource("blacklistIpOnCommentRejection");
            AddResource("block");
            AddResource("blogAdded");
            AddResource("blogDeleted");
            AddResource("blogMLDescription");
            AddResource("blogroll");
            AddResource("blogs");
            AddResource("buttonText");
            AddResource("calendar");
            AddResource("cancel");
            AddResource("cancelReply");
            AddResource("cannotDeleteLastAdmin");
            AddResource("categories");
            AddResource("category");
            AddResource("categoryAlreadyExists");
            AddResource("changePassword");
            AddResource("cheked");
            AddResource("chooseFromExistingTags");
            AddResource("chooseOtherName");
            AddResource("cityTown");
            AddResource("clear");
            AddResource("clickTag");
            AddResource("clickToDisable");
            AddResource("clickToEnable");
            AddResource("close");
            AddResource("closeCommetsAfter");
            AddResource("comment");
            AddResource("commentLabelText");
            AddResource("commentNotificationUnsubscribe");
            AddResource("commentNotificationUnsubscriptionHeader");
            AddResource("commentNotificationUnsubscriptionText");
            AddResource("commentRSS");
            AddResource("comments");
            AddResource("commentsAddedSince");
            AddResource("commentsApprovedByAdmin");
            AddResource("commentsAreClosed");
            AddResource("commentsAuto");
            AddResource("commentsBlacklist");
            AddResource("commentsBlockOnDelete");
            AddResource("commentsDeleted");
            AddResource("commentSettings");
            AddResource("commentsPerPage");
            AddResource("commentsUnmodApproved");
            AddResource("commentsWhere");
            AddResource("commentWaitingModeration");
            AddResource("commentWasSaved");
            AddResource("commonControls");
            AddResource("company");
            AddResource("compressWebResource");
            AddResource("compressWebResourceDescription");
            AddResource("configuration");
            AddResource("Configuration_BtnTestSmtpClick_Test_successfull");
            AddResource("Configuration_OnInit_Please_specify_a_number");
            AddResource("confirmNewPassword");
            AddResource("confirmPassword");
            AddResource("confirmPasswordIsRequired");
            AddResource("confirmResetCounters");
            AddResource("contact");
            AddResource("contactAuthorInformation");
            AddResource("contactDetails");
            AddResource("contactForm");
            AddResource("contactIPAddress");
            AddResource("contactUserAgent");
            AddResource("contains");
            AddResource("content");
            AddResource("controls");
            AddResource("copyRightsFrom");
            AddResource("couldNotCreateRole");
            AddResource("couldNotCreateUser");
            AddResource("couldNotDeleteBlog");
            AddResource("couldNotDeleteComment");
            AddResource("couldNotDeleteComments");
            AddResource("couldNotDeletePage");
            AddResource("couldNotDeletePost");
            AddResource("couldNotDeleteRole");
            AddResource("couldNotDeleteTag");
            AddResource("couldNotDeleteUser");
            AddResource("couldNotUpdateProfile");
            AddResource("couldNotUpdateRole");
            AddResource("couldNotUpdateTag");
            AddResource("couldNotUpdateUser");
            AddResource("count");
            AddResource("country");
            AddResource("countryCode");
            AddResource("createAccount");
            AddResource("createBlogOnSelfRegistration");
            AddResource("createdOn");
            AddResource("createNewUser");
            AddResource("createNow");
            AddResource("createRole");
            AddResource("createUser");
            AddResource("currentlyRated");
            AddResource("currentTheme");
            AddResource("custom");
            AddResource("customCode");
            AddResource("dashboard");
            AddResource("date");
            AddResource("days");
            AddResource("defaultAnonymousRole");
            AddResource("defaultEditorsRole");
            AddResource("defaultFeedOutput");
            AddResource("defaultTextShownInSearchField");
            AddResource("delete");
            AddResource("deleteAll");
            AddResource("deleteKeepReplies");
            AddResource("deletePlusReplies");
            AddResource("deleteSelected");
            AddResource("description");
            AddResource("developmentMode");
            AddResource("developmentModeCheckboxMessage");
            AddResource("disable");
            AddResource("disabled");
            AddResource("displayCommentsOnRecentPosts");
            AddResource("displayName");
            AddResource("displayNameIsRequired");
            AddResource("displayRatingsOnRecentPosts");
            AddResource("disqusSettings");
            AddResource("disqusShortName");
            AddResource("disqusSignupMessage");
            AddResource("doNotPassRightsWithFalseValue");
            AddResource("dontHaveAccount");
            AddResource("down");
            AddResource("download");
            AddResource("downloadOPML");
            AddResource("downloads");
            AddResource("draftPages");
            AddResource("draftPosts");
            AddResource("drafts");
            AddResource("edit");
            AddResource("editExistingBlog");
            AddResource("editingRightsForRole");
            AddResource("editor");
            AddResource("editPage");
            AddResource("email");
            AddResource("emailAddress");
            AddResource("emailArgumentInvalid");
            AddResource("emailIsInvalid");
            AddResource("emailIsRequired");
            AddResource("emailNotExist");
            AddResource("emailSubjectPrefix");
            AddResource("emptyLog");
            AddResource("emptyTrash");
            AddResource("enable");
            AddResource("enableAttachments");
            AddResource("enableCoComments");
            AddResource("enableCommentNesting");
            AddResource("enableCommentNestingDescription");
            AddResource("enableComments");
            AddResource("enableCommentsDescription");
            AddResource("enableCommentSearch");
            AddResource("enableCommentsModeration");
            AddResource("enabled");
            AddResource("enableEnclosures");
            AddResource("enableErrorLogging");
            AddResource("enableErrorLoggingDescription");
            AddResource("enableGravatars");
            AddResource("enableHttpCompression");
            AddResource("enableHttpCompressionDescription");
            AddResource("enableOpenSearch");
            AddResource("enableOpenSearchDescription");
            AddResource("enableOptimization");
            AddResource("enableOptimizationDesc");
            AddResource("enablePasswordReset");
            AddResource("enablePingbacks");
            AddResource("enableQuickNotes");
            AddResource("enableRating");
            AddResource("enableRecaptcha");
            AddResource("enableReferrerTracking");
            AddResource("enableSearchHighlight");
            AddResource("enableSearchHighlightDescription");
            AddResource("enableSelfRegistration");
            AddResource("enableSsl");
            AddResource("enableTrackbacks");
            AddResource("endorsment");
            AddResource("enforce");
            AddResource("enterDate");
            AddResource("enterEmail");
            AddResource("enterEmailAddress");
            AddResource("enterTitle");
            AddResource("enterValidDate");
            AddResource("enterValidEmail");
            AddResource("enterValidName");
            AddResource("enterValidNumber");
            AddResource("enterValidNumberPositiveNegative");
            AddResource("enterValidUrl");
            AddResource("eqls");
            AddResource("Error");
            AddResource("errorApprovingComment");
            AddResource("errorRejectingComment");
            AddResource("excerpt");
            AddResource("existingBlogToCreateNewBlogFrom");
            AddResource("export");
            AddResource("exportIntoBlogML");
            AddResource("extensions");
            AddResource("extractFromTitle");
            AddResource("feed");
            AddResource("fileManager");
            AddResource("filestorage");
            AddResource("filter");
            AddResource("filterByApml");
            AddResource("filterByApmlDescription");
            AddResource("filterName");
            AddResource("filters");
            AddResource("findPosition");
            AddResource("firstName");
            AddResource("followTwitter");
            AddResource("forceMainTheme");
            AddResource("forgotPassword");
            AddResource("formMessage");
            AddResource("fullName");
            AddResource("gallery");
            AddResource("galleryFeedUrl");
            AddResource("galleryFeedUrlDescription");
            AddResource("gender");
            AddResource("goToFrontPage");
            AddResource("goToPage");
            AddResource("goToPost");
            AddResource("groupByYear");
            AddResource("handleWwwSubdomain");
            AddResource("hasRated");
            AddResource("home");
            AddResource("hostName");
            AddResource("htmlHeadSection");
            AddResource("idArgumentNull");
            AddResource("ignore");
            AddResource("import");
            AddResource("importFromBlogML");
            AddResource("importFromFile");
            AddResource("importWithClickOnce");
            AddResource("inbox");
            AddResource("incorrectSimpleCaptcha");
            AddResource("industry");
            AddResource("insertImage");
            AddResource("insertVideo");
            AddResource("install");
            AddResource("installed");
            AddResource("interests");
            AddResource("invalid");
            AddResource("invalidBlogId");
            AddResource("invalidPageId");
            AddResource("invalidPostId");
            AddResource("invalidRoleName");
            AddResource("ip");
            AddResource("isAlreadyExists");
            AddResource("isForSiteAggregation");
            AddResource("isFrontPage");
            AddResource("isPrivate");
            AddResource("isRequiredField");
            AddResource("itemsPerPage");
            AddResource("keywords");
            AddResource("LangDirection");
            AddResource("language");
            AddResource("languageCode");
            AddResource("lastName");
            AddResource("latestFromTheGallery");
            AddResource("latitude");
            AddResource("link");
            AddResource("ListIsEmpty");
            AddResource("livePreview");
            AddResource("login");
            AddResource("loginFailed");
            AddResource("loginNow");
            AddResource("logoff");
            AddResource("longtitude");
            AddResource("makeDonation");
            AddResource("manual");
            AddResource("manually");
            AddResource("maximumRemoteFileSize");
            AddResource("maximumRemoteFileSizeDescription");
            AddResource("maxLengthOfItems");
            AddResource("message");
            AddResource("middleName");
            AddResource("minPassLengthInChars");
            AddResource("mistakes");
            AddResource("mobileTheme");
            AddResource("moderatedByDisqus");
            AddResource("moderation");
            AddResource("moderator");
            AddResource("more");
            AddResource("move");
            AddResource("moveWidgetTo");
            AddResource("myProfile");
            AddResource("name");
            AddResource("never");
            AddResource("newAndConfirmPasswordMismatch");
            AddResource("newest");
            AddResource("newPassword");
            AddResource("newPasswordIsRequired");
            AddResource("newUserRoles");
            AddResource("nextPost");
            AddResource("nextPosts");
            AddResource("noApprovedComments");
            AddResource("noBlogsWereFound");
            AddResource("noFilesFound");
            AddResource("none");
            AddResource("noPackagesToDisplay");
            AddResource("noParent");
            AddResource("noPendingComments");
            AddResource("noPingbacksTrackbacks");
            AddResource("noPostsMatchedYourCriteria");
            AddResource("noRightExists");
            AddResource("noSpamHere");
            AddResource("notAuthorized");
            AddResource("notifiedPublished");
            AddResource("notifyMe");
            AddResource("notifyOnNewComments");
            AddResource("notRatedYet");
            AddResource("noValidNumber");
            AddResource("nowUnsubscribed");
            AddResource("numberOfCharacters");
            AddResource("numberOfComments");
            AddResource("numberOfDaysToKeep");
            AddResource("numberOfDisplayedItems");
            AddResource("numberOfPosts");
            AddResource("occupation");
            AddResource("oldPassword");
            AddResource("oldPasswordIsRequired");
            AddResource("onlyLoggedInCanView");
            AddResource("options");
            AddResource("or");
            AddResource("orderBy");
            AddResource("otherSettings");
            AddResource("page");
            AddResource("pageDeleted");
            AddResource("pageIdRequired");
            AddResource("PageNotFound");
            AddResource("pages");
            AddResource("parent");
            AddResource("password");
            AddResource("passwordAndConfirmPasswordIsMatch");
            AddResource("passwordAndConfirmPasswordMismatch");
            AddResource("passwordArgumentInvalid");
            AddResource("passwordChangeSuccess");
            AddResource("passwordIsRequried");
            AddResource("passwordMinimumCharacters");
            AddResource("passwordNotChanged");
            AddResource("passwordRetrieval");
            AddResource("passwordRetrievalInstructionMessage");
            AddResource("passwordSent");
            AddResource("pending");
            AddResource("pendingApproval");
            AddResource("personalDetails");
            AddResource("phoneFax");
            AddResource("phoneMain");
            AddResource("phoneMobile");
            AddResource("photoURL");
            AddResource("pingbacksAndTrackbacks");
            AddResource("pingService");
            AddResource("pingServiceNotUnique");
            AddResource("pingServiceUrl");
            AddResource("pingServiceUrlBatch");
            AddResource("portNumber");
            AddResource("portNumberDescription");
            AddResource("possibleSpam");
            AddResource("post");
            AddResource("postDeleted");
            AddResource("postPerPage");
            AddResource("posts");
            AddResource("postsPerFeed");
            AddResource("preview");
            AddResource("previousPost");
            AddResource("previousPosts");
            AddResource("primary");
            AddResource("priority");
            AddResource("profile");
            AddResource("profiles");
            AddResource("profileUpdated");
            AddResource("publish");
            AddResource("published");
            AddResource("purge");
            AddResource("purgeAll");
            AddResource("purgeSelected");
            AddResource("quickAddNewCategory");
            AddResource("raters");
            AddResource("rateThisXStars");
            AddResource("rating");
            AddResource("ratingHasBeenRegistered");
            AddResource("recaptchaConfigureReminder");
            AddResource("receive");
            AddResource("recentComments");
            AddResource("recentDatesAtTop");
            AddResource("recentPendingComments");
            AddResource("recentPosts");
            AddResource("redirectToRemoveFileExtension");
            AddResource("redirectToRemoveFileExtensionDesc");
            AddResource("referrer");
            AddResource("referrers");
            AddResource("Referrers_BindTable_Total");
            AddResource("regionState");
            AddResource("register");
            AddResource("reject");
            AddResource("rejectSelected");
            AddResource("relatedPosts");
            AddResource("rememberMe");
            AddResource("remoteTimeout");
            AddResource("remoteTimeoutDescription");
            AddResource("remove");
            AddResource("removeExtensionsFromUrls");
            AddResource("removeExtensionsFromUrlsDesc");
            AddResource("replyTo");
            AddResource("replyToBase");
            AddResource("replyToThis");
            AddResource("reportMistakesToService");
            AddResource("required");
            AddResource("requiredPasswordLength");
            AddResource("requireSslForMetaWeblogApi");
            AddResource("requireSslForMetaWeblogApiDescription");
            AddResource("reset");
            AddResource("resetCounters");
            AddResource("restore");
            AddResource("restoreSelected");
            AddResource("returnToBlog");
            AddResource("rights");
            AddResource("rightsCanNotBeNull");
            AddResource("rightsUpdatedForRole");
            AddResource("roleAlreadyExists");
            AddResource("roleHasBeenCreated");
            AddResource("roleHasBeenDeleted");
            AddResource("roleNameArgumentNull");
            AddResource("roleNameIsRequired");
            AddResource("roles");
            AddResource("roleUpdatedFromTo");
            AddResource("rules");
            AddResource("runClickOnceApplicationToImport");
            AddResource("save");
            AddResource("saveComment");
            AddResource("savedByAtTime");
            AddResource("savePage");
            AddResource("savePost");
            AddResource("saveProfile");
            AddResource("saveSettings");
            AddResource("savingTheComment");
            AddResource("search");
            AddResource("searchField");
            AddResource("searchFieldText");
            AddResource("searchResultsFor");
            AddResource("securitySettings");
            AddResource("select");
            AddResource("selectDay");
            AddResource("selectedCommentDeleted");
            AddResource("selectedCommentRejected");
            AddResource("selectedCommentRestored");
            AddResource("selectedComments");
            AddResource("selectParent");
            AddResource("selectSavedBlogMlFle");
            AddResource("selfRegistrationInitialRole");
            AddResource("send");
            AddResource("sendCommentEmail");
            AddResource("separateTagsWitComma");
            AddResource("setAsCurrentTheme");
            AddResource("setAsMobileTheme");
            AddResource("setPublishDate");
            AddResource("settings");
            AddResource("shortBiography");
            AddResource("show");
            AddResource("showCountryChooser");
            AddResource("showCountryChooserDescription");
            AddResource("showDescriptionInPostList");
            AddResource("showDescriptionInPostListForPostsByTagOrCategory");
            AddResource("showEnableWebsiteInComments");
            AddResource("showEnableWebsiteInCommentsDescription");
            AddResource("showIncludeComments");
            AddResource("showInList");
            AddResource("showLivePreview");
            AddResource("showPingBacks");
            AddResource("showPostNavigation");
            AddResource("showRelatedPosts");
            AddResource("showSelector");
            AddResource("siteUnavailableConfirm");
            AddResource("size");
            AddResource("slug");
            AddResource("smtpServer");
            AddResource("source");
            AddResource("sourceViewer");
            AddResource("spam");
            AddResource("spamProtection");
            AddResource("specifyPingService");
            AddResource("stats");
            AddResource("status");
            AddResource("storageContainerName");
            AddResource("subject");
            AddResource("subscribe");
            AddResource("switchUserProfile");
            AddResource("Tag");
            AddResource("tagChangedFromTo");
            AddResource("tagHasBeenDeleted");
            AddResource("tagIsRequired");
            AddResource("tags");
            AddResource("testEmailSettings");
            AddResource("textBeforeHostName");
            AddResource("thankYou");
            AddResource("thankYouMessage");
            AddResource("theComment");
            AddResource("theme");
            AddResource("themeCookieName");
            AddResource("themes");
            AddResource("thePost");
            AddResource("thereAreXDrafts");
            AddResource("theValuesSaved");
            AddResource("thumbnailServiceProvider");
            AddResource("thumbnailServiceProviderHelp");
            AddResource("timeStampPostLinks");
            AddResource("timezone");
            AddResource("title");
            AddResource("toDisableSetTo0");
            AddResource("tools");
            AddResource("total");
            AddResource("tracking");
            AddResource("trackingScript");
            AddResource("trackingScriptDescription");
            AddResource("trash");
            AddResource("trashIsEmpty");
            AddResource("trimStylesheet");
            AddResource("trimStylesheetDescription");
            AddResource("trustAuthenticated");
            AddResource("turnDisqusOnOff");
            AddResource("type");
            AddResource("unapprove");
            AddResource("unapproved");
            AddResource("unapprovedcomments");
            AddResource("uncategorized");
            AddResource("unmoderated");
            AddResource("up");
            AddResource("update");
            AddResource("updateFrequenzy");
            AddResource("upload");
            AddResource("uploadFile");
            AddResource("uploadImage");
            AddResource("uploadVideo");
            AddResource("useBlogNameInPageTitles");
            AddResource("useBlogNameInPageTitlesDescription");
            AddResource("useDisqusAsCommentProvider");
            AddResource("userAlreadyExists");
            AddResource("userArgumentInvalid");
            AddResource("useRawHtmlEditor");
            AddResource("userHasBeenCreated");
            AddResource("userHasBeenDeleted");
            AddResource("userName");
            AddResource("usernameAlreadyTaken");
            AddResource("userNameIsRequired");
            AddResource("userProfiles");
            AddResource("users");
            AddResource("userUpdated");
            AddResource("userWithEmailExists");
            AddResource("valsArgumentNull");
            AddResource("version");
            AddResource("view");
            AddResource("viewAll");
            AddResource("viewLargeCalendar");
            AddResource("viewSource");
            AddResource("virtualPath");
            AddResource("visitorHi");
            AddResource("website");
            AddResource("welcome");
            AddResource("welcomeBack");
            AddResource("whitelistIpOnCommentApproval");
            AddResource("widget");
            AddResource("widgets");
            AddResource("willShowGravatar");
            AddResource("writeNewPost");
            AddResource("writtenCommentsTotal");
            AddResource("wrote");
            AddResource("youAlreadyRated");
            AddResource("youDontHaveAnyDraftPages");
            AddResource("youDontHaveAnyDraftPosts");
            AddResource("youHaveNotAddedAnyUserSoFar");
            AddResource("youHaveNoTaggedPosts");
            AddResource("youHaveNotDefinedAnyRolesSoFar");
            AddResource("youHaveNotWrittenAnyPosts");
            AddResource("youHaveNoWrittenAnyPages");
        }

        #endregion
    }
}
