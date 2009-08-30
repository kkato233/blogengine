using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.ComponentModel;

namespace BlogEngine.Core
{
    ///<summary>
    ///</summary>
    public static class CommentHandlers
    {
        static ExtensionSettings _filters;

        ///<summary>
        /// Initiate adding comment event listener
        ///</summary>
        public static void Listen()
        {
            Post.AddingComment += PostAddingComment;
            InitFilters();
            InitCustomFilters();
        }

        #region Event handlers

        static void PostAddingComment(object sender, CancelEventArgs e)
        {
            if(!BlogSettings.Instance.IsCommentsEnabled) return;
            if(!BlogSettings.Instance.EnableCommentsModeration) return;

            Comment comment = (Comment)sender;

            if(!ModeratedByRule(comment))
            {
                if(!ModeratedByFilter(comment))
                {
                    RunCustomModerators(comment);
                }
            }
        }

        #endregion

        static bool ModeratedByRule(Comment comment)
        {
            // trust authenticated users
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                comment.IsApproved = true;
                comment.ModeratedBy = Comment.Moderator.Rule;
                return true;
            }

            int blackCnt = 0;
            int whiteCnt = 0;

            foreach (Post p in Post.Posts)
            {
                foreach (Comment c in p.Comments)
                {
                    if (c.Author == comment.Author)
                    {
                        if (c.IsApproved)
                            whiteCnt++;
                        else
                            blackCnt++;
                    }
                }
            }

            // user is in the white list - approve comment
            if (whiteCnt >= BlogSettings.Instance.CommentWhiteListCount)
            {
                comment.IsApproved = true;
                comment.ModeratedBy = Comment.Moderator.Rule;
                return true;
            }

            // user is in the black list - reject comment
            if (blackCnt >= BlogSettings.Instance.CommentBlackListCount)
            {
                comment.IsApproved = false;
                comment.ModeratedBy = Comment.Moderator.Rule;
                return true;
            }
            return false;
        }

        static bool ModeratedByFilter(Comment comment)
        {
            DataTable dt = _filters.GetDataTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string action = row["Action"].ToString();
                    string subject = row["Subject"].ToString();
                    string oper = row["Operator"].ToString();
                    string filter = row["Filter"].ToString().Trim().ToLower(CultureInfo.InvariantCulture);

                    string comm = comment.Content.ToLower(CultureInfo.InvariantCulture);
                    string auth = comment.Author.ToLower(CultureInfo.InvariantCulture);
                    string wsite = comment.Website.ToString().ToLower(CultureInfo.InvariantCulture);
                    string email = comment.Email.ToLower(CultureInfo.InvariantCulture);

                    bool match = false;

                    if (oper == "Equals")
                    {
                        if (subject == "IP")
                        {
                            if (comment.IP == filter)
                                match = true;
                        }
                        if (subject == "Author")
                        {
                            if (auth == filter)
                                match = true;
                        }
                        if (subject == "Website")
                        {
                            if (wsite == filter)
                                match = true;
                        }
                        if (subject == "Email")
                        {
                            if (email == filter)
                                match = true;
                        }
                        if (subject == "Comment")
                        {
                            if (comm == filter)
                                match = true;
                        }
                    }
                    else
                    {
                        if (subject == "IP")
                        {
                            if (comment.IP.Contains(filter))
                                match = true;
                        }
                        if (subject == "Author")
                        {
                            if (auth.Contains(filter))
                                match = true;
                        }
                        if (subject == "Website")
                        {
                            if (wsite.Contains(filter))
                                match = true;
                        }
                        if (subject == "Email")
                        {
                            if (email.Contains(filter))
                                match = true;
                        }
                        if (subject == "Comment")
                        {
                            if (comm.Contains(filter))
                                match = true;
                        }
                    }

                    if (match)
                    {
                        comment.IsApproved = action != "Block";
                        comment.ModeratedBy = Comment.Moderator.Filter;
                        return true;
                    }
                }
            }
            return false;
        }

        static void RunCustomModerators(Comment comment)
        {
            DataTable dt = _filters.GetDataTable();

            foreach (DataRow row in dt.Rows)
            {
                string fileterName = row[0].ToString();

                ICustomFilter customFilter = GetCustomFilter(fileterName);

                if (customFilter.Initialize("a","b"))
                {
                    comment.IsApproved = !customFilter.Check(comment);
                    comment.ModeratedBy = Comment.Moderator.Service;
                }
            }
        }

        #region Settings
        static void InitFilters()
        {
            ExtensionSettings settings = new ExtensionSettings("BeCommentFilters");

            settings.AddParameter("ID", "ID", 20, true, true, ParameterType.Integer);
            settings.AddParameter("Action");
            settings.AddParameter("Subject");
            settings.AddParameter("Operator");
            settings.AddParameter("Filter");

            ExtensionManager.ImportSettings("MetaExtension", settings);

            _filters = ExtensionManager.GetSettings("MetaExtension", "BeCommentFilters");
        }

        static void InitCustomFilters()
        {
            ExtensionSettings settings = new ExtensionSettings("BeCustomFilters");

            settings.AddParameter("FullName", "Name", 100, true, true);
            settings.AddParameter("Name");
            settings.AddParameter("SiteUrl");
            settings.AddParameter("ApiKey");
            settings.AddParameter("Checked");
            settings.AddParameter("Cought");
            settings.AddParameter("Reported");

            ExtensionManager.ImportSettings("MetaExtension", settings);

            _filters = ExtensionManager.GetSettings("MetaExtension", "BeCustomFilters");

            if(_filters != null)
            {
                DataTable dt = _filters.GetDataTable();
                ArrayList codeAssemblies = Utils.CodeAssemblies();

                foreach (Assembly a in codeAssemblies)
                {
                    Type[] types = a.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("BlogEngine.Core.ICustomFilter") != null)
                        {
                            bool found = false;
                            foreach (DataRow row in dt.Rows)
                            {
                                if(row[0].ToString() == type.Name)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if(!found)
                            {
                                _filters.AddValues(new string[] { type.FullName, type.Name, "YourSite.com", "123456789", "0", "0", "0" });
                            }
                        }
                    }
                }

                ExtensionManager.SaveSettings(_filters);
            }
        }
        #endregion

        public static ICustomFilter GetCustomFilter(string className)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type t = assembly.GetType(className);
                return (ICustomFilter)Activator.CreateInstance(t);
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}