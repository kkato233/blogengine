#region Using

using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using BlogEngine.Core.Providers;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// A post is an entry on the blog - a blog post.
  /// </summary>
  [Serializable]
  public class Post : BusinessBase<Post, Guid>, IComparable<Post>
  {

    #region Constructor

    /// <summary>
    /// The default contstructor assign default values.
    /// </summary>
    public Post()
    {
      base.Id = Guid.NewGuid();
      _Comments = new List<Comment>();
      _Categories = new StateCollection<Guid>();
      _Tags = new StateCollection<string>();
      DateCreated = DateTime.Now;
      _IsPublished = true;
      _IsCommentsEnabled = true;
    }

    #endregion

    #region Properties

    private string _Author;
    /// <summary>
    /// Gets or sets the Author or the post.
    /// </summary>
    public string Author
    {
      get { return _Author; }
      set
      {
        if (_Author != value) MarkDirty("Author");
        _Author = value;
      }
    }

    private string _Title;
    /// <summary>
    /// Gets or sets the Title or the post.
    /// </summary>
    public string Title
    {
      get { return _Title; }
      set
      {
        if (_Title != value) MarkDirty("Title");
        _Title = value;
      }
    }

    private string _Description;
    /// <summary>
    /// Gets or sets the Description or the post.
    /// </summary>
    public string Description
    {
      get { return _Description; }
      set
      {
        if (_Description != value) MarkDirty("Description");
        _Description = value;
      }
    }

    private string _Content;
    /// <summary>
    /// Gets or sets the Content or the post.
    /// </summary>
    public string Content
    {
      get { return _Content; }
      set
      {
        if (_Content != value) MarkDirty("Content");
        _Content = value;
      }
    }

    private List<Comment> _Comments;
    /// <summary>
    /// A collection of comments sorted by date.
    /// </summary>
    public List<Comment> Comments
    {
      get { return _Comments; }
    }

    private StateCollection<Guid> _Categories;
    /// <summary>
    /// An unsorted collection of categories.
    /// </summary>
    public StateCollection<Guid> Categories
    {
      get { return _Categories; }
    }

    private StateCollection<string> _Tags;
    /// <summary>
    /// An unsorted collection of tags.
    /// </summary>
    public StateCollection<string> Tags
    {
      get { return _Tags; }
    }

    private bool _IsCommentsEnabled;
    /// <summary>
    /// Gets or sets the EnableComments or the object.
    /// </summary>
    public bool IsCommentsEnabled
    {
      get { return _IsCommentsEnabled; }
      set
      {
        if (_IsCommentsEnabled != value) MarkDirty("IsCommentsEnabled");
        _IsCommentsEnabled = value;
      }
    }

    private bool _IsPublished;
    /// <summary>
    /// Gets or sets the IsPublished or the object.
    /// </summary>
    public bool IsPublished
    {
      get { return _IsPublished; }
      set
      {
        if (_IsPublished != value) MarkDirty("IsPublished");
        _IsPublished = value;
      }
    }

    #endregion

    #region Links

    /// <summary>
    /// The absolute permanent link to the post.
    /// </summary>
    public Uri PermaLink
    {
      get { return new Uri(Utils.AbsoluteWebRoot.ToString() + "post.aspx?id=" + Id.ToString()); }
    }

    /// <summary>
    /// A relative-to-the-site-root path to the post.
    /// Only for in-site use.
    /// </summary>
    public Uri RelativeLink
    {
      get { return new Uri(VirtualPathUtility.ToAbsolute("~/post/" + Utils.RemoveIlegalCharacters(Title) + ".aspx"), UriKind.Relative); }
    }

    /// <summary>
    /// The absolute link to the post.
    /// </summary>
    public Uri AbsoluteLink
    {
      get { return new Uri(Utils.AbsoluteWebRoot.ToString() + "post/" + Utils.RemoveIlegalCharacters(Title) + ".aspx"); }
    }

    /// <summary>
    /// The trackback link to the post.
    /// </summary>
    public Uri TrackbackLink
    {
      get { return new Uri(Utils.AbsoluteWebRoot.ToString() + "trackback.axd?id=" + Id.ToString()); }
    }

    #endregion

    #region Methods

    private static object _SyncRoot = new object();
    private static List<Post> _Posts;
    /// <summary>
    /// A sorted collection of all posts in the blog.
    /// Sorted by date.
    /// </summary>
    public static List<Post> Posts
    {
      get
      {
        lock (_SyncRoot)
        {
          if (_Posts == null)
            FillPosts();
          return _Posts;
        }
      }
    }

    /// <summary>
    /// Returns all posts in the specified category
    /// </summary>
    public static List<Post> GetPostsByCategory(Guid categoryId)
    {
      List<Post> col = new List<Post>();
      foreach (Post post in Posts)
      {
        if (post.Categories.Contains(categoryId))
          col.Add(post);
      }

      col.Sort();
      return col;
    }

    /// <summary>
    /// Returs a post based on the specified id.
    /// </summary>
    public static Post GetPost(Guid id)
    {
      foreach (Post post in Posts)
      {
        if (post.Id == id)
          return post;
      }

      return null;
    }

    private static void FillPosts()
    {
      string folder = CategoryDictionary._Folder + "posts\\";
      _Posts = new List<Post>();

      foreach (string file in Directory.GetFiles(folder, "*.xml", SearchOption.TopDirectoryOnly))
      {
        FileInfo info = new FileInfo(file);
        string id = info.Name.Replace(".xml", string.Empty);
        Post post = Post.Load(new Guid(id));
        _Posts.Add(post);
      }

      _Posts.Sort();
    }

    /// <summary>
    /// Checks to see if the specified title has already been used
    /// by another post.
    /// <remarks>
    /// Titles must be unique because the title is part of the URL.
    /// </remarks>
    /// </summary>
    public static bool IsTitleUnique(string title)
    {
      foreach (Post post in Posts)
      {
        if (Utils.RemoveIlegalCharacters(post.Title).Equals(Utils.RemoveIlegalCharacters(title), StringComparison.OrdinalIgnoreCase))
          return false;
      }

      return true;
    }

    /// <summary>
    /// Returns a post based on it's title.
    /// </summary>
    public static Post GetPostByName(string title)
    {
      foreach (Post post in Post.Posts)
      {
        string legalTitle = Utils.RemoveIlegalCharacters(post.Title);
        if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          return post;
        }
      }

      return null;
    }

    /// <summary>
    /// Returns all posts written by the specified author.
    /// </summary>
    public static List<Post> GetPostsByAuthor(string author)
    {
      List<Post> list = new List<Post>();
      foreach (Post post in Post.Posts)
      {
        string legalTitle = Utils.RemoveIlegalCharacters(post.Author);
        if (Utils.RemoveIlegalCharacters(author).Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          list.Add(post);
        }
      }

      return list;
    }

    /// <summary>
    /// Returns all posts tagged with the specified tag.
    /// </summary>
    public static List<Post> GetPostsByTag(string tag)
    {
      List<Post> list = new List<Post>();
      foreach (Post post in Post.Posts)
      {
        if (post.Tags.Contains(tag))
          list.Add(post);
      }

      return list;
    }

    /// <summary>
    /// Returns all posts published between the two dates.
    /// </summary>
    public static List<Post> GetPostsByDate(DateTime dateFrom, DateTime dateTo)
    {
      List<Post> list = new List<Post>();
      foreach (Post post in Post.Posts)
      {
        if (post.DateCreated.Date >= dateFrom && post.DateCreated.Date <= dateTo)
          list.Add(post);
      }

      return list;
    }

    #endregion

    #region Base overrides

    /// <summary>
    /// Validates the Post instance.
    /// </summary>
    protected override void ValidationRules()
    {
      AddRule("Title", "Title must be set", String.IsNullOrEmpty(Title));
      AddRule("Content", "Content must be set", String.IsNullOrEmpty(Content));
    }

    /// <summary>
    /// Returns a Post based on the specified id.
    /// </summary>
    protected override Post DataSelect(Guid id)
    {
      return BlogService.SelectPost(id);
    }

    /// <summary>
    /// Updates the Post.
    /// </summary>
    protected override void DataUpdate()
    {
      BlogService.UpdatePost(this);
    }

    /// <summary>
    /// Inserts a new post to the current BlogProvider.
    /// </summary>
    protected override void DataInsert()
    {
      BlogService.InsertPost(this);

      if (this.IsNew)
      {
        Posts.Add(this);
        Posts.Sort();
      }    
    }

    /// <summary>
    /// Deletes the Post from the current BlogProvider.
    /// </summary>
    protected override void DataDelete()
    {
      BlogService.DeletePost(this);
      if (Posts.Contains(this))
        Posts.Remove(this);      
    }

    /// <summary>
    /// Gets if the Post have been changed.
    /// </summary>
    public override bool IsDirty
    {
      get
      {
        if (base.IsDirty)
          return true;

        if (Categories.IsChanged || Tags.IsChanged)
          return true;

        return false;
      }
    }

    #endregion

    #region IComparable<Post> Members

    /// <summary>
    /// Compares this post to another instance.
    /// </summary>
    public int CompareTo(Post other)
    {
      return other.DateCreated.CompareTo(this.DateCreated);
    }

    #endregion
  }
}
