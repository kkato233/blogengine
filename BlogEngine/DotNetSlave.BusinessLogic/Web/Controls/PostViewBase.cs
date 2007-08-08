#region Using

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.Controls
{
  /// <summary>
  /// The PostView.ascx that is located in the themes folder
  /// has to inherit from this class. 
  /// <remarks>
  /// It provides the basic functionaly needed to display a post.
  /// </remarks>
  /// </summary>
  public class PostViewBase : UserControl
  {
    /// <summary>
    /// Lets process our .Body content and build up our controls collection
    /// inside the 'BodyContent' placeholder.
    /// 
    /// User controls are insterted into the blog in the following format..
    /// [UserControl:~/path/usercontrol.ascx]
    /// 
    /// TODO : Expose user control parameters.
    /// 
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Our content placeholder.
        PlaceHolder bodyContent;

        try
        {
            bodyContent = (PlaceHolder)FindControl("BodyContent");
            if (bodyContent == null)
            {
                throw new ApplicationException("Missing 'Bodycontent' Placeholder from theme PostView.ascx file");
            }
        }
        catch
        {
            // Rethrow exception.  Need to implement a proper application wide error handler...
            throw;
        }

        MatchCollection myMatches = _BodyRegex.Matches(Body);
        int currentPosition = 0;

        foreach (Match myMatch in myMatches)
        {
            // Add literal for content before custom tag should it exist.
            if (myMatch.Index > currentPosition)
            {
                bodyContent.Controls.Add(new LiteralControl(Body.Substring(currentPosition, myMatch.Index - currentPosition)));
            }

            // Now lets add our user control.
            try
            {
                bodyContent.Controls.Add(LoadControl(myMatch.Groups[1].Value));

                // Now we will update our position.
                currentPosition = myMatch.Index + myMatch.Groups[0].Length;
            }
            catch
            {
                // Whoopss, can't load that control so lets output something that tells the developer that theres a problem.
                bodyContent.Controls.Add(new LiteralControl("ERROR - UNABLE TO LOAD CONTROL : " + myMatch.Groups[1].Value));

                // Now we will update our position based on our error message as opposed to our control text as we would normally.
                currentPosition = myMatch.Index + ((string)("ERROR - UNABLE TO LOAD CONTROL : " + myMatch.Groups[1].Value)).Length;
            }
        }

        // Finally we add any trailing static text.
        bodyContent.Controls.Add(new LiteralControl(Body.Substring(currentPosition, Body.Length - currentPosition)));
    }
    private static Regex _BodyRegex = new Regex(@"\[UserControl:(.*?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Manages the deletion of posts.
    /// </summary>
    private void Page_Init(object sender, EventArgs e)
    {
      if (!Post.IsPublished && !Page.User.Identity.IsAuthenticated)
      {
        this.Visible = false;
      }
    }

    /// <summary>
    /// The Post object that is displayed through the PostView.ascx control.
    /// </summary>
    /// <value>The Post object that has to be displayed.</value>
    public virtual Post Post
    {
      get { return (Post)(ViewState["Post"] ?? default(Post)); }
      set { ViewState["Post"] = value; }
    }

    private bool _ShowExcerpt;
    /// <summary>
    /// Gets or sets whether or not to show the entire post or just the excerpt/description.
    /// </summary>
    public bool ShowExcerpt
    {
      get { return _ShowExcerpt; }
      set { _ShowExcerpt = value; }
    }

    /// <summary>
    /// Gets the body of the post. Important: use this instead of Post.Content.
    /// </summary>
    public string Body
    {
      get
      {
        string body = Post.Content;
        if (ShowExcerpt)
        {
          string link = " <a href=\"" + Post.RelativeLink.ToString() + "\">[" + (Page as BlogBasePage).Translate("more") + "]</a>";

          if (!string.IsNullOrEmpty(Post.Description))
          {
            body = Post.Description + "." + link;
          }
          else
          {
            body = StripHtml(Post.Content);
            if (body.Length > 300)
              body = body.Substring(0, 300) + "..." + link;
          }
        }
        ServingEventArgs arg = new ServingEventArgs(body);
        Post.OnServing(Post, arg);

        return arg.Body;
      }
    }

    private static Regex _Regex = new Regex("<[^>]*>", RegexOptions.Compiled);

    private static string StripHtml(string html)
    {
      if (string.IsNullOrEmpty(html))
        return string.Empty;

      return _Regex.Replace(html, string.Empty);
    } 

    /// <summary>
    /// Gets the comment feed link.
    /// </summary>
    /// <value>The comment feed.</value>
    public string CommentFeed
    {
      get { return Utils.RelativeWebRoot + "commentfeed.axd?id=" + Post.Id.ToString(); }
    }

    #region Protected methods

    /// <summary>
    /// Displays the Post's categories seperated by the specified string.
    /// </summary>
    protected virtual string CategoryLinks(string separator)
    {
      string[] keywords = new string[Post.Categories.Count];
      string link = "<a href=\"{0}{1}.aspx\">{2}</a>";
      string path = VirtualPathUtility.ToAbsolute("~/category/");
      for (int i = 0; i < Post.Categories.Count; i++)
      {
        if (CategoryDictionary.Instance.ContainsKey(Post.Categories[i]))
        {
          string category = CategoryDictionary.Instance[Post.Categories[i]];
          keywords[i] = string.Format(link, path, Utils.RemoveIlegalCharacters(category), category);
        }
      }

      return string.Join(separator, keywords);
    }

    /// <summary>
    /// Displays the Post's tags seperated by the specified string.
    /// </summary>
    protected virtual string TagLinks(string separator)
    {
      if (Post.Tags.Count == 0)
        return null;

      string[] tags = new string[Post.Tags.Count];
      string link = "<a href=\"{0}/{1}\" rel=\"tag\">{1}</a>";
      string path = Utils.RelativeWebRoot + "?tag=";
      for (int i = 0; i < Post.Tags.Count; i++)
      {
        string tag = HttpUtility.HtmlEncode(Post.Tags[i]);
        tags[i] = string.Format(link, path, tag);
      }

      return string.Join(separator, tags);
    }

    /// <summary>
    /// Displays an Edit and Delete link to any 
    /// authenticated user.
    /// </summary>
    protected virtual string AdminLinks
    {
      get
      {
        if (Page.User.Identity.IsAuthenticated)
        {
          BlogBasePage page = (BlogBasePage)Page;
          string confirmDelete = string.Format(page.Translate("areYouSure"), page.Translate("delete").ToLowerInvariant(), page.Translate("thePost"));
          StringBuilder sb = new StringBuilder();
          sb.AppendFormat("<a href=\"{0}\">{1}</a> | ", VirtualPathUtility.ToAbsolute("~/") + "admin/pages/add_entry.aspx?id=" + Post.Id.ToString(), page.Translate("edit"));
          sb.AppendFormat("<a href=\"{0}?deletepost={1}\" onclick=\"return confirm('{2}')\">{3}</a> | ", Post.RelativeLink, Post.Id.ToString(), confirmDelete, page.Translate("delete"));
          return sb.ToString();
        }

        return string.Empty;
      }
    }

    /// <summary>
    /// Enable visitors to rate the post.
    /// </summary>
    protected virtual string Rating
    {
      get
      {
        if (!BlogSettings.Instance.EnableRating)
          return string.Empty;

        float rating = Post.Rating / 5 * 100;
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=\"rating\">");

        BlogBasePage page = (BlogBasePage)Page;

        if (Post.Raters > 0)
          sb.AppendFormat("<p>" + page.Translate("currentlyRated") + "</p>", Post.Rating.ToString("#.0"), Post.Raters);
        else
          sb.Append("<p>Be the first to rate this post</p>");

        string script = "Rate('{0}', {1});";
        if (Request.Cookies["rating"] != null && Request.Cookies["rating"].Value.Contains(Post.Id.ToString()))
          script = "alert('" + page.Translate("youAlreadyRated") + "');";

        sb.Append("<ul class=\"star-rating small-star\">");
        sb.AppendFormat("<li class=\"current-rating\" style=\"width:{0}%\">Currently {1}/5 Stars.</li>", Math.Round(rating, 0), Post.Rating);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 1 star out of 5\" class=\"one-star\">1</a></li>", Post.Id.ToString(), 1);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 2 stars out of 5\" class=\"two-stars\">2</a></li>", Post.Id.ToString(), 2);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 3 stars out of 5\" class=\"three-stars\">3</a></li>", Post.Id.ToString(), 3);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 4 stars out of 5\" class=\"four-stars\">4</a></li>", Post.Id.ToString(), 4);
        sb.AppendFormat("<li><a href=\"javascript:" + script + "void(0)\" title=\"Rate this 5 stars out of 5\" class=\"five-stars\">5</a></li>", Post.Id.ToString(), 5);
        sb.Append("</ul>");
        sb.Append("</div>");
        return sb.ToString();
      }
    }

    #endregion
  }
}