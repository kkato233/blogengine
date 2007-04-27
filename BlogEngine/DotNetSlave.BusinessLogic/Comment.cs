using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Core
{
  /// <summary>
  /// Represents a comment to a blog post.
  /// </summary>
  [Serializable]
  public struct Comment : IComparable<Comment>
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
    /// Gets or sets the Author or the object.
    /// </summary>
    public string Author
    {
      get { return _Author; }
      set { _Author = value; }
    }

    private string _Email;
    /// <summary>
    /// Gets or sets the Email or the object.
    /// </summary>
    public string Email
    {
      get { return _Email; }
      set { _Email = value; }
    }

    private Uri _Website;
    /// <summary>
    /// Gets or sets the Website or the object.
    /// </summary>
    public Uri Website
    {
      get { return _Website; }
      set { _Website = value; }
    }

    private string _Content;
    /// <summary>
    /// Gets or sets the Content or the object.
    /// </summary>
    public string Content
    {
      get { return _Content; }
      set { _Content = value; }
    }

    private string _Country;
      
    /// <summary>
    /// 
    /// </summary>
    public string Country
    {
      get { return _Country; }
      set { _Country = value; }
    }

    private string _IP;

    /// <summary>
    /// 
    /// </summary>
    public string IP
    {
      get { return _IP; }
      set { _IP = value; }
    }

    private DateTime _DateCreated;
    /// <summary>
    /// Gets or sets when the comment was created.
    /// </summary>
    public DateTime DateCreated
    {
      get { return _DateCreated; }
      set { _DateCreated = value; }
    }

    #endregion

    #region IComparable<Comment> Members

    /// <summary>
    /// Compares the comment to another.
    /// </summary>
    public int CompareTo(Comment other)
    {
      return this.DateCreated.CompareTo(other.DateCreated);
    }

    #endregion
  }
}
