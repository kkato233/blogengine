<%@ WebService Language="C#" Class="Comments" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

using BlogEngine.Core;
using BlogEngine.Core.Json;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class Comments : WebService
{
    #region Constants and Fields

    /// <summary>
    ///     JSON object that will be return back to client
    /// </summary>
    private readonly JsonResponse response;

    protected static int CurrentPage = 1;

    protected static int LastPage = 1;

    protected static int CommCnt = 1;

    #endregion

    #region Constructors and Destructors

    public Comments()
    {
        this.response = new JsonResponse();
    }

    #endregion

    /// <summary>
    ///     Reject selected comments
    /// </summary>
    /// <param name = "vals">Array of comments</param>
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
            foreach (var p in Post.Posts.ToArray())
            {
                foreach (var c in from c in p.Comments.ToArray() from t in vals where c.Id == new Guid(t) select c)
                {
                    CommentHandlers.AddIpToFilter(c.IP, true);
                    CommentHandlers.ReportMistake(c);

                    c.ModeratedBy = this.User.Identity.Name;
                    p.DisapproveComment(c);
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
    ///     Restore selected comments
    /// </summary>
    /// <param name = "vals">Array of comments</param>
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
            var toapprove = from p in Post.Posts
                            from c in p.Comments
                            join t in vals on c.Id equals new Guid(t)
                            select new { p, c };

            foreach (var t in toapprove)
            {
                CommentHandlers.AddIpToFilter(t.c.IP, false);
                CommentHandlers.ReportMistake(t.c);

                t.c.ModeratedBy = this.User.Identity.Name;
                t.p.ApproveComment(t.c);
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
    ///     Delete selected comments
    /// </summary>
    /// <param name = "vals">Array of comments</param>
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
            var tmp = new List<Comment>();

            foreach (var post in Post.Posts)
            {
                var post1 = post;
                tmp.AddRange(vals.Select(t => post1.Comments.Find(c => c.Id == new Guid(t))).Where(comment => comment != null));
            }

            foreach (var c in tmp)
            {
                this.RemoveComment(c);
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
    ///     Delete all spam comments
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
            this.DeleteAllComments();
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

    [WebMethod]
    public static JsonComment SaveComment(string id, string author, string email, string website, string cont)
    {
        var gId = new Guid(id);
        var jc = new JsonComment();

        foreach (var p in Post.Posts.ToArray())
        {
            foreach (var c in p.Comments.Where(c => c.Id == gId).ToArray())
            {
                c.Author = author;
                c.Email = email;
                c.Website = new Uri(website);
                c.Content = cont;

                p.Save();
                return JsonComments.GetComment(gId);
            }
        }
        return jc;
    }

    protected void RemoveComment(Comment comment)
    {
        var toremove = from p in Post.Posts from c in p.Comments where c.Id == comment.Id select new { p, c };
        foreach (var t in toremove)
        {
            t.p.RemoveComment(t.c);
        }
    }

    protected void DeleteAllComments()
    {
        if (Post.Posts.Count <= 0)
        {
            return;
        }

        // loop backwards to avoid "collection was modified" error
        for (var i = Post.Posts.Count - 1; i >= 0; i--)
        {
            if (Post.Posts[i].Comments.Count <= 0)
            {
                continue;
            }

            for (var j = Post.Posts[i].Comments.Count - 1; j >= 0; j--)
            {
                var comment = Post.Posts[i].Comments[j];

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
