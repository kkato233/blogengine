using System;
using System.Collections.Generic;
using System.Web;
using BlogEngine.Core;
using System.Web.UI.WebControls;

public partial class admin_Comments_DataGrid : System.Web.UI.UserControl
{
    #region Private members

    static protected List<Comment> Comments;
    private bool _autoModerated;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        gridComments.RowCommand += GridCommentsRowCommand;
        gridComments.RowDataBound += gridComments_RowDataBound;

        gridComments.PageSize = 15;
        _autoModerated = false;

        if (!BlogSettings.Instance.EnableCommentsModeration)
            btnApproveAll.Visible = false;
        if(_autoModerated)
            btnApproveAll.Text = "Spam";
        else
            btnApproveAll.Text = "Approve";

        if (Request.Path.ToLower().Contains("approved.aspx"))
            btnApproveAll.Text = "Reject";

        if (Request.Path.ToLower().Contains("spam.aspx"))
            btnApproveAll.Text = "Restore";

        if (!Page.IsPostBack)
        {
            BindComments();
        }
    }

    protected void ddCommentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindComments();
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        BindComments();
        gridComments.PageIndex = e.NewPageIndex;
        gridComments.DataBind();
    }

    protected void gridComments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[6].Text = string.Format("Total : {0} comments", Comments.Count);
        }
    }

    #region Binding

    protected void BindComments()
    {
        LoadData();
        gridComments.DataSource = Comments;
        gridComments.DataBind();
    }

    #endregion

    #region Data handling

    protected void LoadData()
    {
        List<Comment> comments = new List<Comment>();
        string comType = "All"; // ddCommentType.SelectedValue;

        foreach (Post p in Post.Posts)
        {
            foreach (Comment c in p.Comments)
            {
                if (!BlogSettings.Instance.EnableCommentsModeration)
                {
                    comments.Add(c);
                }
                else
                {
                    if(Request.Path.ToLower().Contains("approved.aspx"))
                    {
                        if (c.IsApproved) comments.Add(c);
                    }
                    else if(Request.Path.ToLower().Contains("spam.aspx"))
                    {
                        if (!c.IsApproved) comments.Add(c);
                    }
                    else
                    {
                        // if auto-moderated, inbox has approved comments
                        if (_autoModerated && c.IsApproved) comments.Add(c);

                        // if manual moderation inbox has unapproved comments
                        if (!_autoModerated && !c.IsApproved) comments.Add(c);
                    }
                }
            }
        }
        // sort in descending order
        comments.Sort(delegate(Comment c1, Comment c2)
        { return DateTime.Compare(c2.DateCreated, c1.DateCreated); });
        Comments = comments;
    }

    protected void ApproveComment(Comment comment)
    {
        comment.IsApproved = true;
        comment.ModeratedBy = Comment.Moderator.Admin;
        UpdateComment(comment);
    }

    protected void RejectComment(Comment comment)
    {
        comment.IsApproved = false;
        comment.ModeratedBy = Comment.Moderator.Admin;
        UpdateComment(comment);
    }

    protected void RemoveComment(Comment comment)
    {
        bool found = false;
        for (int i = 0; i < Post.Posts.Count; i++)
        {
            for (int j = 0; j < Post.Posts[i].Comments.Count; j++)
            {
                if (Post.Posts[i].Comments[j].Id == comment.Id)
                {
                    Post.Posts[i].RemoveComment(Post.Posts[i].Comments[j]);
                    found = true;
                    break;
                }
            }
            if (found) { break; }
        }
    }

    protected void UpdateComment(Comment comment)
    {
        bool found = false;
        // Cast ToArray so the original collection isn't modified. 
        foreach (Post p in Post.Posts.ToArray())
        {
            // Cast ToArray so the original collection isn't modified. 
            foreach (Comment c in p.Comments.ToArray())
            {
                if (c.Id == comment.Id)
                {
                    c.Content = comment.Content;
                    c.IsApproved = comment.IsApproved;
                    c.ModeratedBy = comment.ModeratedBy;

                    // Need to mark post as "changed" for it to get saved into the data store. 
                    string desc = p.Description;
                    p.Description = (desc ?? string.Empty) + " ";
                    p.Description = desc;

                    p.Save();
                    found = true;
                    break;
                }
            }
            if(found) break;
        }
        BindComments();
    }

    #endregion

    #region Grid buttons

    void GridCommentsRowCommand(object sender, GridViewCommandEventArgs e)
    {
        System.Web.UI.Page pg = (System.Web.UI.Page)HttpContext.Current.CurrentHandler;
        bool x = pg.IsPostBack;

        GridView grid = (GridView)sender;
        int index = Convert.ToInt32(e.CommandArgument);

        if (grid.PageIndex > 0)
        {
            index = grid.PageIndex * grid.PageSize + index;
        }

        Comment comment;

        if (e.CommandName == "btnInspect")
        {
            comment = Comments[index];

            commId.InnerHtml = comment.Id.ToString();
            popAuthor.InnerHtml = "Author: " + comment.Author;

            string postTmpl = "<a rel=\"bookmark\" title=\"Post permalink\" href=\"{0}\" target=\"_new\">{1}</a>";
            popPost.InnerHtml = "Post: " + string.Format(postTmpl, comment.Parent.RelativeLink, comment.Parent.Title);
            popIp.InnerHtml = "<a href=\"http://www.domaintools.com/go/?service=whois&q=" + comment.IP + "\" target=\"_new\">" + comment.IP + "</a>";
            popEmail.InnerHtml = "<a href=\"mailto:" + comment.Email + "\">" + comment.Email + "</a>";
            txtArea.Value = comment.Content;

            popWebsite.InnerHtml = "";
            if (comment.Website != null && comment.Website.ToString().Length > 0)
            {
                popWebsite.InnerHtml = string.Format("Website: <a href=\"{0}\" target=\"_new\">{1}</a>", comment.Website, comment.Website);
            }
            if (!string.IsNullOrEmpty(comment.Country))
            {
                popCountry.InnerHtml = "Country: " + comment.Country.ToUpper();
            }
            pop1.Visible = true;
        }

        BindComments();
    }

    #endregion

    #region Popup buttons

    protected void btnSaveTxt_Click(object sender, EventArgs e)
    {
        foreach (Comment com in Comments)
        {
            if (com.Id.ToString() == commId.InnerHtml)
            {
                com.Content = txtArea.Value;
                UpdateComment(com);
            }
        }
        BindComments();
        pop1.Visible = false;
    }

    protected void btnCancelPop_Click(object sender, EventArgs e)
    {
        BindComments();
        pop1.Visible = false;
    }

    protected void btnApprovePop_Click(object sender, EventArgs e)
    {
        foreach (Comment com in Comments)
        {
            if (com.Id.ToString() == commId.InnerHtml)
            {
                ApproveComment(com);
            }
        }
        BindComments();
        pop1.Visible = false;
    }

    protected void btnDeletePop_Click(object sender, EventArgs e)
    {
        foreach (Comment com in Comments)
        {
            if (com.Id.ToString() == commId.InnerHtml)
            {
                RemoveComment(com);
                BindComments();
            }
        }
        BindComments();
        pop1.Visible = false;
    }

    #endregion

    #region Footer buttons

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        BindComments();
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (cb != null && cb.Enabled)
            {
                cb.Checked = true;
            }
        }
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        BindComments();
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("chkSelect");
            if (cb != null && cb.Enabled)
            {
                cb.Checked = false;
            }
        }
    }

    protected void btnApproveAll_Click(object sender, EventArgs e)
    {
        if (Request.Path.ToLower().Contains("approved.aspx"))
            ProcessSelected("unapprove");
        else
            ProcessSelected("approve");
    }

    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        ProcessSelected("delete");
    }

    protected void ProcessSelected(string action)
    {
        foreach (GridViewRow row in gridComments.Rows)
        {
            CheckBox cbx = (CheckBox)row.FindControl("chkSelect");
            if (cbx != null && cbx.Checked)
            {
                int index = row.RowIndex;
                if (gridComments.PageIndex > 0)
                {
                    index = gridComments.PageIndex * gridComments.PageSize + index;
                }
                Comment comment = Comments[index];

                if (action == "approve")
                {
                    if (!comment.IsApproved)
                    {
                        ApproveComment(comment);
                    }
                }
                else if (action == "unapprove")
                {
                    comment.IsApproved = false;
                    RejectComment(comment);
                }
                if (action == "delete")
                {
                    RemoveComment(comment);
                }
            }
        }

        BindComments();
    }

    public static bool HasNoChildren(Guid comId)
    {
        foreach (Post p in Post.Posts)
        {
            // Cast ToArray so the original collection isn't modified. 
            foreach (Comment c in p.Comments)
            {
                if (c.ParentId == comId)
                {
                    return false;
                }
            }
        }
        return true;
    }

    #endregion
}