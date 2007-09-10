#region Using

using System;
using System.Globalization;
using System.Collections.Generic;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// Represents a comment to a blog post.
  /// </summary>
  [Serializable]
  public class Comment : IComparable<Comment>, IPublishable
  {

    #region Properties

    private Guid _Id;
    /// <summary>
    /// The Id of the comment.
    /// </summary>
    public Guid Id
    {
      get { return _Id; }
      set { _Id = value; }
    }

    private string _Author;
    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    /// <value>The author.</value>
    public string Author
    {
      get { return _Author; }
      set { _Author = value; }
    }

    private string _Email;
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    public string Email
    {
      get { return _Email; }
      set { _Email = value; }
    }

    private Uri _Website;

    /// <summary>
    /// Gets or sets the website.
    /// </summary>
    /// <value>The website.</value>
    public Uri Website
    {
      get { return _Website; }
      set { _Website = value; }
    }

    private string _Content;
    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>The content.</value>
    public string Content
    {
      get { return _Content; }
      set { _Content = value; }
    }

    private string _Country;
    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>The country.</value>
    public string Country
    {
      get { return _Country; }
      set { _Country = value; }
    }

    private string _IP;
    /// <summary>
    /// Gets or sets the IP address.
    /// </summary>
    /// <value>The IP.</value>
    public string IP
    {
      get { return _IP; }
      set { _IP = value; }
    }

    private Post _Post;

    /// <summary>
    /// 
    /// </summary>
    public Post Post
    {
      get { return _Post; }
      set { _Post = value; }
    }

    private bool _approved;
    /// <summary>
    /// Gets or sets the Comment approval status
    /// </summary>
    public bool Approved
    {
      get { return _approved; }
      set { _approved = value; }
    }

    private DateTime _DateCreated = DateTime.MinValue;
    /// <summary>
    /// Gets or sets when the comment was created.
    /// </summary>
    public DateTime DateCreated
    {
      get
      {
        if (_DateCreated == DateTime.MinValue)
          return _DateCreated;

        return _DateCreated.AddHours(BlogSettings.Instance.Timezone);

      }
      set { _DateCreated = value; }
    }

    DateTime IPublishable.DateModified
    {
      get { return DateCreated; }
    }

    /// <summary>
    /// Gets the title of the object
    /// </summary>
    /// <value></value>
    public String Title
    {
      get { return Author + " on " + Post.Title; }
    }

    /// <summary>
    /// Gets the relative link of the comment.
    /// </summary>
    /// <value>The relative link.</value>
    public Uri RelativeLink
    {
      get { return new Uri(Post.RelativeLink.ToString() + "#id_" + Id, UriKind.Relative); }
    }

    /// <summary>
    /// Gets the description. Returns always string.empty.
    /// </summary>
    /// <value>The description.</value>
    public String Description
    {
      get { return string.Empty; }
    }

    List<Category> IPublishable.Categories
    {
      get { return null; }
    }

    #endregion

    #region IComparable<Comment> Members

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the 
    /// objects being compared. The return value has the following meanings: 
    /// Value Meaning Less than zero This object is less than the other parameter.
    /// Zero This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    public int CompareTo(Comment other)
    {
      return this.DateCreated.CompareTo(other.DateCreated);
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the post is being served to the output stream.
    /// </summary>
    public static event EventHandler<ServingEventArgs> Serving;
    /// <summary>
    /// Raises the event in a safe way
    /// </summary>
    public static void OnServing(Comment comment, ServingEventArgs arg)
    {
      if (Serving != null)
      {
        Serving(comment, arg);
      }
    }

    /// <summary>
    /// Raises the Serving event
    /// </summary>
    public void OnServing(ServingEventArgs arg)
    {
      if (Serving != null)
      {
        Serving(this, arg);
      }
    }

    #endregion
  }
}
