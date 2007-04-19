#region Using

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.IO;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

namespace Controls
{

  public class RelatedPosts : Control
  {

    #region Properties

    private Post _Post;

    public Post Post
    {
      get { return _Post; }
      set { _Post = value; }
    }

    private int _MaxResults = 3;

    public int MaxResults
    {
      get { return _MaxResults; }
      set { _MaxResults = value; }
    }

    private bool _ShowDescription;

    public bool ShowDescription
    {
      get { return _ShowDescription; }
      set { _ShowDescription = value; }
    }

    private int _DescriptionMaxLength = 100;

    public int DescriptionMaxLength
    {
      get { return _DescriptionMaxLength; }
      set { _DescriptionMaxLength = value; }
    }

    private string _Headline = "Related posts";

    public string Headline
    {
      get { return _Headline; }
      set { _Headline = value; }
    }

    /// <summary>
    /// It is a long running process to built the search catalog,
    /// so we only built it once an then updates it when new posts 
    /// are added or old ones updated.
    /// </summary>
    private Dictionary<Guid, string> Cache
    {
      get
      {
        lock (_SyncRoot)
        {
          if (_Cache == null)
          {
            _Cache = new Dictionary<Guid, string>();
            Post.Saved += delegate { _Cache = null; };
          }
        }

        return _Cache;
      }
    }

    #endregion

    #region Private fiels

    private static Dictionary<Guid, string> _Cache;
    private static object _SyncRoot = new object();

    #endregion    

    public override void RenderControl(HtmlTextWriter writer)
    {
      if (!BlogSettings.Instance.EnableRelatedPosts || Post == null)
        return;

      if (!Cache.ContainsKey(Post.Id))
      {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Post> relatedPosts = SearchForPosts();
        if (relatedPosts.Count <= 1)
          return;

        string link = "<a href=\"{0}\">{1}</a>";
        string desc = "<span>{0}</span>";
        sb.Append("<h1>" + this.Headline + "</h1>");
        sb.Append("<div id=\"relatedPosts\">");

        int count = 0;
        foreach (Post post in relatedPosts)
        {
          if (post != this.Post)
          {
            sb.Append(string.Format(link, post.RelativeLink, post.Title));
            if (ShowDescription)
            {
              string description = post.Description;
              if (description != null && description.Length > DescriptionMaxLength)
                description = description.Substring(0, DescriptionMaxLength) + "...";

              sb.Append(string.Format(desc, description));
            }
            count++;
          }

          if (count == MaxResults)
            break;
        }

        sb.Append("</div>");
        Cache.Add(Post.Id, sb.ToString());
      }

      writer.Write(Cache[Post.Id]);
    }

    private List<Post> SearchForPosts()
    {
      //TODO: Base the comparison on more than the title alone.
      return Search.Hits(Post.Posts, this.Post.Title, false);
    }
  }
}