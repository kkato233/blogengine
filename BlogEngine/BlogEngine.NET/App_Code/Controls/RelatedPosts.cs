#region Using

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.IO;
using BlogEngine.Core;

#endregion

namespace Controls
{

  public class RelatedPosts : Control
  {

		public RelatedPosts()
		{
			Post.Saved += new EventHandler<SavedEventArgs>(Post_Saved);
		}

		void Post_Saved(object sender, SavedEventArgs e)
		{
			if (e.Action == SaveAction.Update)
			{
				Post post = (Post)sender;
				if (_Cache.ContainsKey(post.Id))
					_Cache.Remove(post.Id);
			}
		}

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

    private string _Headline = Resources.labels.relatedPosts;

    public string Headline
    {
      get { return _Headline; }
      set { _Headline = value; }
    }

    #endregion

    #region Private fiels

    private static Dictionary<Guid, string> _Cache = new Dictionary<Guid,string>();
    private static object _SyncRoot = new object();

    #endregion    

    public override void RenderControl(HtmlTextWriter writer)
    {
      if (!BlogSettings.Instance.EnableRelatedPosts || Post == null)
        return;
      
      if (!_Cache.ContainsKey(Post.Id))
      {
        lock (_SyncRoot)
        {
          if (!_Cache.ContainsKey(Post.Id))
          {            
            List<Post> relatedPosts = SearchForPosts();
            if (relatedPosts.Count <= 1)
              return;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

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
            _Cache.Add(Post.Id, sb.ToString());
          }
        }
      }

      writer.Write(_Cache[Post.Id]);
    }

    private List<Post> SearchForPosts()
    {
      List<IPublishable> list = Search.FindRelatedPosts(this.Post);
      List<Post> posts = new List<Post>();
      foreach (IPublishable item in list)
      {
        if (item is Post)
          posts.Add((Post)item);
      }

      return posts;
    }
  }
}