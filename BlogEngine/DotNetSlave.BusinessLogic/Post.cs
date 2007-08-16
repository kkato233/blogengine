#region Using

using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Net.Mail;
using System.Globalization;
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
      _Categories =   new List<Category>();
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

    private List<Category> _Categories;
    /// <summary>
    /// An unsorted List of categories.
    /// </summary>
      public List<Category> Categories
    {
        get { return _Categories; }
    }

      private bool _IsCategoriesChanged;
      /// <summary>
      /// Gets or sets to check if the List has changed.
      /// </summary>
      public bool IsCategoriesChanged
      {
          get{ return _IsCategoriesChanged;}
          set { _IsCategoriesChanged = value; }
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

    private float _Rating;
    /// <summary>
    /// Gets or sets the rating or the post.
    /// </summary>
    public float Rating
    {
      get { return _Rating; }
      set
      {
        if (_Rating != value) MarkDirty("Rating");
        _Rating = value;
      }
    }

    private int _Raters;
    /// <summary>
    /// Gets or sets the number of raters or the object.
    /// </summary>
    public int Raters
    {
      get { return _Raters; }
      set
      {
        if (_Raters != value) MarkDirty("Raters");
        _Raters = value;
      }
    }

    private string _Slug;
    /// <summary>
    /// Gets or sets the Slug of the Post.
    /// A Slug is the relative URL used by the posts.
    /// </summary>
    public string Slug
    {
      get
      {
        if (string.IsNullOrEmpty(_Slug))
          return Title;

        return _Slug;
      }
      set { _Slug = value; }
    }

    private StringCollection _NotificationEmails;
    /// <summary>
    /// Gets a collection of email addresses that is signed up for 
    /// comment notification on the specific post.
    /// </summary>
    public StringCollection NotificationEmails
    {
      get 
      {
        if (_NotificationEmails == null)
          _NotificationEmails = new StringCollection();

        return _NotificationEmails; 
      }
    }

    /// <summary>
    /// Gets whether or not the post is visible or not.
    /// </summary>
    public bool IsVisible
    {
      get
      {
        if (IsPublished && DateCreated <= DateTime.Now)
          return true;

        return false;
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
      get { return new Uri(VirtualPathUtility.ToAbsolute("~/post/" + Utils.RemoveIlegalCharacters(Slug) + ".aspx"), UriKind.Relative); }
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
        if (_Posts == null)
        {
          lock (_SyncRoot)
          {
            if (_Posts == null)
              _Posts = BlogService.FillPosts();
          }
        }

        return _Posts;
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
        if (post.Categories.Contains(Category.GetCategory(categoryId)))
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
    public static Post GetPostBySlug(string slug)
    {
      foreach (Post post in Post.Posts)
      {
        if (slug.Equals(Utils.RemoveIlegalCharacters(post.Slug), StringComparison.OrdinalIgnoreCase))
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

    /// <summary>
    /// Adds a rating to the post.
    /// </summary>
    public void Rate(int rating)
    {
      if (Raters > 0)
      {
        float total = Raters * Rating;
        total += rating;
        Raters++;
        Rating = (float)(total / Raters);
      }
      else
      {
        Raters = 1;
        Rating = rating;
      }

      DataUpdate();
      OnRated(this);
    }

    /// <summary>
    /// Adds a comment to the collection and saves the post.
    /// </summary>
    /// <param name="comment">The comment to add to the post.</param>
    public void AddComment(Comment comment)
    {
      OnAddingComment(this);
      Comments.Add(comment);
      DataUpdate();
      OnCommentAdded(this);
      SendNotifications(comment);
    }

    /// <summary>
    /// Sends a notification to all visitors  that has registered
    /// to retrieve notifications for the specific post.
    /// </summary>
    private void SendNotifications(Comment comment)
    {
      if (NotificationEmails.Count == 0)
        return;

      MailMessage mail = new MailMessage();
      mail.From = new MailAddress(BlogSettings.Instance.Email, BlogSettings.Instance.Name);
      mail.Subject = "New comment on " + Title;
      mail.Body = "Comment by " + comment.Author + Environment.NewLine + Environment.NewLine;
      mail.Body += comment.Content + "\n\n" + PermaLink.ToString();

      foreach (string email in NotificationEmails)
      {
        if (email != comment.Email)
        {
          mail.To.Clear();
          mail.To.Add(email);
          Utils.SendMailMessageAsync(mail);
        }
      }
    }

    /// <summary>
    /// Removes a comment from the collection and saves the post.
    /// </summary>
    /// <param name="comment">The comment to remove from the post.</param>
    public void RemoveComment(Comment comment)
    {
      OnRemovingComment(this);
      Comments.Remove(comment);
      DataUpdate();
      OnCommentRemoved(this);
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
      Posts.Sort();
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

        if (IsCategoriesChanged || Tags.IsChanged)
          return true;

        return false;
      }
    }

    #endregion

    #region IComparable<Post> Members

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the 
    /// objects being compared. The return value has the following meanings: 
    /// Value Meaning Less than zero This object is less than the other parameter.Zero 
    /// This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    public int CompareTo(Post other)
    {
      return other.DateCreated.CompareTo(this.DateCreated);
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs before a new comment is added.
    /// </summary>
    public static event EventHandler<EventArgs> AddingComment;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnAddingComment(Post post)
    {
      if (AddingComment != null)
      {
        AddingComment(post, new EventArgs());
      }
    }

    /// <summary>
    /// Occurs when a comment is added.
    /// </summary>
    public static event EventHandler<EventArgs> CommentAdded;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnCommentAdded(Post post)
    {
      if (CommentAdded != null)
      {
        CommentAdded(post, new EventArgs());
      }
    }

    /// <summary>
    /// Occurs before a new comment is added.
    /// </summary>
    public static event EventHandler<EventArgs> RemovingComment;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnRemovingComment(Post post)
    {
      if (RemovingComment != null)
      {
        RemovingComment(post, new EventArgs());
      }
    }

    /// <summary>
    /// Occurs when a comment is added.
    /// </summary>
    public static event EventHandler<EventArgs> CommentRemoved;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnCommentRemoved(Post post)
    {
      if (CommentRemoved != null)
      {
        CommentRemoved(post, new EventArgs());
      }
    }

    /// <summary>
    /// Occurs when a visitor rates the post.
    /// </summary>
    public static event EventHandler<EventArgs> Rated;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    protected virtual void OnRated(Post post)
    {
      if (Rated != null)
      {
        Rated(post, new EventArgs());
      }
    }

    /// <summary>
    /// Occurs when the post is being served to the output stream.
    /// </summary>
    public static event EventHandler<ServingEventArgs> Serving;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    public static void OnServing(Post post, ServingEventArgs arg)
    {
      if (Serving != null)
      {
        Serving(post, arg);
      }
    }

    #endregion

  }
}
