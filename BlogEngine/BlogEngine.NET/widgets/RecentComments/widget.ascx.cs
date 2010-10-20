#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using BlogEngine.Core;

using Resources;

#endregion

/// <summary>
/// The widgets_ recent comments_widget.
/// </summary>
public partial class widgets_RecentComments_widget : WidgetBase
{
    /// <summary>
    /// The defaul t_ numbe r_ o f_ comments.
    /// </summary>
    private const int DEFAULT_NUMBER_OF_COMMENTS = 10;

    /// <summary>
    /// Initializes static members of the <see cref="widgets_RecentComments_widget"/> class.
    /// </summary>
    static widgets_RecentComments_widget()
    {
        Post.CommentAdded += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
        Post.CommentRemoved += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
        BlogSettings.Changed += delegate { HttpRuntime.Cache.Remove("widget_recentcomments"); };
    }

    /// <summary>
    /// The load widget.
    /// </summary>
    public override void LoadWidget()
    {
        var settings = this.GetSettings();
        var numberOfComments = DEFAULT_NUMBER_OF_COMMENTS;
        if (settings.ContainsKey("numberofcomments"))
        {
            numberOfComments = int.Parse(settings["numberofcomments"]);
        }

        if (HttpRuntime.Cache["widget_recentcomments"] == null)
        {
            var comments = new List<Comment>();

            foreach (var post in Post.Posts)
            {
                if (post.IsVisible)
                {
                    comments.AddRange(
                        post.Comments.FindAll(delegate(Comment c) { return c.IsApproved && c.Email.Contains("@"); }));
                }
            }

            comments.Sort();
            comments.Reverse();

            var max = Math.Min(comments.Count, numberOfComments);
            var list = comments.GetRange(0, max);
            HttpRuntime.Cache.Insert("widget_recentcomments", list);
        }

        var content = this.RenderComments((List<Comment>)HttpRuntime.Cache["widget_recentcomments"], settings);

        var html = new LiteralControl(content);
            
            // new LiteralControl((string)HttpRuntime.Cache["widget_recentcomments"]);
        this.phPosts.Controls.Add(html);
    }

    /// <summary>
    /// The render comments.
    /// </summary>
    /// <param name="comments">
    /// The comments.
    /// </param>
    /// <param name="settings">
    /// The settings.
    /// </param>
    /// <returns>
    /// The render comments.
    /// </returns>
    private string RenderComments(List<Comment> comments, StringDictionary settings)
    {
        if (comments.Count == 0)
        {
            // HttpRuntime.Cache.Insert("widget_recentcomments", "<p>" + Resources.labels.none + "</p>");
            return "<p>" + labels.none + "</p>";
        }

        var ul = new HtmlGenericControl("ul");
        ul.Attributes.Add("class", "recentComments");
        ul.ID = "recentComments";

        foreach (var comment in comments)
        {
            if (comment.IsApproved)
            {
                var li = new HtmlGenericControl("li");

                // The post title
                var title = new HtmlAnchor();
                title.HRef = comment.Parent.RelativeLink;
                title.InnerText = comment.Parent.Title;
                title.Attributes.Add("class", "postTitle");
                li.Controls.Add(title);

                // The comment count on the post
                var count = new LiteralControl(string.Format(" ({0})<br />", ((Post)comment.Parent).ApprovedComments.Count));
                li.Controls.Add(count);

                // The author
                if (comment.Website != null)
                {
                    using (var author = new HtmlAnchor())
                    {
                        author.Attributes.Add("rel", "nofollow");
                        author.HRef = comment.Website.ToString();
                        author.InnerHtml = comment.Author;
                        li.Controls.Add(author);
                    }

                    using (var wrote = new LiteralControl(string.Format(" {0}: ", labels.wrote)))
                    {
                        li.Controls.Add(wrote);
                    }
                }
                else
                {
                    using (var author = new LiteralControl(string.Format("{0} {1}: ", comment.Author, labels.wrote)))
                    {
                        li.Controls.Add(author);
                    }
                }

                // The comment body
                var commentBody = Regex.Replace(comment.Content, @"\[(.*?)\]", string.Empty);
                var bodyLength = Math.Min(commentBody.Length, 50);

                commentBody = commentBody.Substring(0, bodyLength);
                if (commentBody.Length > 0)
                {
                    if (commentBody[commentBody.Length - 1] == '&')
                    {
                        commentBody = commentBody.Substring(0, commentBody.Length - 1);
                    }
                }

                commentBody += comment.Content.Length <= 50 ? " " : "... ";
                var body = new LiteralControl(commentBody);
                li.Controls.Add(body);

                // The comment link
                using (var link = new HtmlAnchor
                    {
                        HRef = string.Format("{0}#id_{1}", comment.Parent.RelativeLink, comment.Id), InnerHtml = string.Format("[{0}]", labels.more)
                    })
                {
                    link.Attributes.Add("class", "moreLink");
                    li.Controls.Add(link);
                }

                ul.Controls.Add(li);
            }
        }

        var sw = new StringWriter();
        ul.RenderControl(new HtmlTextWriter(sw));

        const string Ahref = "<a href=\"{0}syndication.axd?comments=true\">Comment RSS <img src=\"{0}pics/rssButton.gif\" alt=\"\" /></a>";
        var rss = string.Format(Ahref, Utils.RelativeWebRoot);
        sw.Write(rss);
        return sw.ToString();

        // HttpRuntime.Cache.Insert("widget_recentcomments", sw.ToString());
    }

    /// <summary>
    /// Gets Name.
    /// </summary>
    public override string Name
    {
        get
        {
            return "RecentComments";
        }
    }

    /// <summary>
    /// Gets a value indicating whether IsEditable.
    /// </summary>
    public override bool IsEditable
    {
        get
        {
            return true;
        }
    }
}