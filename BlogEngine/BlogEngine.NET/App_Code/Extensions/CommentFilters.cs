using System;
using System.Data;
using System.Globalization;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

/// <summary>
/// Comment filters
/// </summary>
[Extension("Comment Filters", "1.0", "<a href=\"http://dotnetblogengine.net\">BlogEngine.NET</a>")]
public class CommentFilters
{
    static protected ExtensionSettings _filters;

    public CommentFilters()
    {
        Post.AddingComment += Post_AddingComment;
        InitFilters();
    }

    void Post_AddingComment(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Comment comment = (Comment)sender;
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
                    if(subject == "Website")
                    {
                        if (wsite == filter)
                            match = true;
                    }
                    if(subject == "Email")
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
                }
            }
        }
    }

    protected void InitFilters()
    {
        ExtensionSettings settings = new ExtensionSettings("Filters");
        string id = Guid.NewGuid().ToString();

        settings.AddParameter("ID", "ID", 20, true, true, ParameterType.Integer);
        settings.AddParameter("Action");
        settings.AddParameter("Subject");
        settings.AddParameter("Operator");
        settings.AddParameter("Filter");

        ExtensionManager.ImportSettings("CommentFilters", settings);
        _filters = ExtensionManager.GetSettings("CommentFilters", "Filters");
    }
}