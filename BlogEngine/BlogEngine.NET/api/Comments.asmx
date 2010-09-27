<%@ WebService Language="C#" Class="Comments" %>

using System;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;

using BlogEngine.Core;
using BlogEngine.Core.Json;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Comments  : System.Web.Services.WebService {

    #region Constants and Fields

    /// <summary>
    ///     JSON object that will be return back to client
    /// </summary>
    private readonly JsonResponse response;

    protected static int currentPage = 1;
    protected static int lastPage = 1;
    protected static int commCnt = 1;

    #endregion
    
    #region Constructors and Destructors

    public Comments()
    {
        this.response = new JsonResponse();
    }

    #endregion

    /// <summary>
    /// Reject selected comments
    /// </summary>
    /// <param name="vals">Array of comments</param>
    /// <returns>Json response</returns>
    [WebMethod]
    public JsonResponse Reject(string[] vals)
    {
        this.response.Success = false;

        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Message = "Not authorized";
            return this.response;
        }

        if (string.IsNullOrEmpty(vals[0]))
        {
            return this.response;
        }

        try
        {         
            foreach (Post p in Post.Posts.ToArray())
            {
                foreach (Comment c in p.Comments.ToArray())
                {
                    for (int i = 0; i < vals.Length; i++)
                    {
                        if (c.Id == new Guid(vals[i]))
                        {
                            CommentHandlers.AddIpToFilter(c.IP, true);
                            CommentHandlers.ReportMistake(c);

                            c.ModeratedBy = this.User.Identity.Name;
                            p.DisapproveComment(c);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Api.Comments.Reject: {0}", ex.Message));
            this.response.Message = "Error rejecting comment";
            return this.response;
        }
        this.response.Success = true;
        this.response.Message = "Selected comments rejected";
        return this.response;
    }

    /// <summary>
    /// Restore selected comments
    /// </summary>
    /// <param name="vals">Array of comments</param>
    /// <returns>Json response</returns>
    [WebMethod]
    public JsonResponse Approve(string[] vals)
    {
        this.response.Success = false;

        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Message = "Not authorized";
            return this.response;
        }

        if (string.IsNullOrEmpty(vals[0]))
        {
            return this.response;
        }

        try
        {
            foreach (Post p in Post.Posts.ToArray())
            {
                foreach (Comment c in p.Comments.ToArray())
                {
                    for (int i = 0; i < vals.Length; i++)
                    {
                        if (c.Id == new Guid(vals[i]))
                        {
                            CommentHandlers.AddIpToFilter(c.IP, false);
                            CommentHandlers.ReportMistake(c);

                            c.ModeratedBy = this.User.Identity.Name;
                            p.ApproveComment(c);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Api.Comments.Reject: {0}", ex.Message));
            this.response.Message = string.Format("Could not restore comment: {0}", vals[0]);
            return this.response;
        }
        this.response.Success = true;
        this.response.Message = "Selected comments restored";
        return this.response;
    }

    /// <summary>
    /// Delete selected comments
    /// </summary>
    /// <param name="vals">Array of comments</param>
    /// <returns>Json response</returns>
    [WebMethod]
    public JsonResponse Delete(string[] vals)
    {
        this.response.Success = false;

        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Message = "Not authorized";
            return this.response;
        }

        if (string.IsNullOrEmpty(vals[0]))
        {
            return this.response;
        }

        try
        {
            List<Comment> tmp = new List<Comment>();

            foreach (Post post in Post.Posts)
            {
                for (int i = 0; i < vals.Length; i++)
                {
                    Comment comment = post.Comments.Find(
                            delegate(Comment c)
                            {
                                return c.Id == new Guid(vals[i]);
                            });

                    if (comment != null) tmp.Add(comment);
                }
            }

            foreach (Comment c in tmp)
            {
                RemoveComment(c);
            }
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Api.Comments.Delete: {0}", ex.Message));
            this.response.Message = string.Format("Could not delete comment: {0}", ex.Message);
            return this.response;
        }
        this.response.Success = true;
        this.response.Message = "Selected comments deleted";
        return this.response;
    }

    /// <summary>
    /// Delete all spam comments
    /// </summary>
    /// <returns>Json response</returns>
    [WebMethod]
    public JsonResponse DeleteAll()
    {
        this.response.Success = false;

        if (!this.User.IsInRole(BlogSettings.Instance.AdministratorRole))
        {
            this.response.Message = "Not authorized";
            return this.response;
        }
        try
        {
            DeleteAllComments();
        }
        catch (Exception ex)
        {
            Utils.Log(string.Format("Api.Comments.DeleteAll: {0}", ex.Message));
            this.response.Message = string.Format("Could not delete all comment: {0}", ex.Message);
            return this.response;
        }
        this.response.Success = true;
        this.response.Message = "Comments deleted";
        return this.response;
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
            if (found) break;
        }
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

    protected void DeleteAllComments()
    {
        if (Post.Posts.Count > 0)
        {
            // loop backwards to avoid "collection was modified" error
            for (int i = Post.Posts.Count - 1; i >= 0; i--)
            {
                if (Post.Posts[i].Comments.Count > 0)
                {
                    for (int j = Post.Posts[i].Comments.Count - 1; j >= 0; j--)
                    {
                        Comment comment = Post.Posts[i].Comments[j];
                        // spam comments should never have children but
                        // be on a safe side insure we won't create
                        // orphan comment with deleted parent
                        if (!comment.IsApproved && comment.Comments.Count == 0)
                        {
                            Post.Posts[i].RemoveComment(comment);
                        }
                    }
                }
            }
        }
    }


}
