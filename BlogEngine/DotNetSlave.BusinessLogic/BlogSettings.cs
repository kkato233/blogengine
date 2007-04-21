#region Using

using System;
using System.Xml;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.ComponentModel;

#endregion

/// <summary>
/// Settings used by the blog engine.
/// </summary>
public class BlogSettings
{

  #region Constructor

  private BlogSettings()
  {
    Load();
  }

  #endregion

  private static BlogSettings _Instance;
  /// <summary>
  /// The singleton instance of the class.
  /// </summary>
  public static BlogSettings Instance
  {
    get
    {
      if (_Instance == null)
        _Instance = new BlogSettings();
      return _Instance;
    }
  }

  #region Properties

  private string _Name;
  /// <summary>
  /// Gets or sets the name of the blog.
  /// </summary>
  public string Name
  {
    get { return _Name; }
    set { _Name = value; }
  }

  private string _Description;
  /// <summary>
  /// Gets or sets the description of the blog.
  /// Is also used for the description meta tag.
  /// </summary>
  public string Description
  {
    get { return _Description; }
    set { _Description = value; }
  }

  private int _PostsPerPage;
  /// <summary>
  /// Gets or sets the number of posts to show an each page.
  /// </summary>
  public int PostsPerPage
  {
    get { return _PostsPerPage; }
    set { _PostsPerPage = value; }
  }

  private string _StorageLocation;

  public string StorageLocation
  {
    get { return _StorageLocation; }
    set { _StorageLocation = value; }
  }

  private int _BlogrollMaxLength;
  /// <summary>
  /// Gets or sets the character length of the items.
  /// </summary>
  public int BlogrollMaxLength
  {
    get { return _BlogrollMaxLength; }
    set { _BlogrollMaxLength = value; }
  }

  private int _BlogrollVisiblePosts;
  /// <summary>
  /// Gets or sets the number of items to show.
  /// </summary>
  public int BlogrollVisiblePosts
  {
    get { return _BlogrollVisiblePosts; }
    set { _BlogrollVisiblePosts = value; }
  }

  private string _Theme;
  /// <summary>
  /// Gets or sets which theme to use.
  /// </summary>
  public string Theme
  {
    get { return _Theme; }
    set { _Theme = value; }
  }

  private bool _EnableRelatedPosts;
  /// <summary>
  /// Gets or sets whether or not related posts should be shown.
  /// </summary>
  public bool EnableRelatedPosts
  {
    get { return _EnableRelatedPosts; }
    set { _EnableRelatedPosts = value; }
  }

  // Email

  private string _Email;
  /// <summary>
  /// Gets or sets the e-mail address.
  /// </summary>
  public string Email
  {
    get { return _Email; }
    set { _Email = value; }
  }

  private string _SmtpServer;
  /// <summary>
  /// Gets or sets the address to the SMTP server.
  /// </summary>
  public string SmtpServer
  {
    get { return _SmtpServer; }
    set { _SmtpServer = value; }
  }

  private string _SmtpUsername;
  /// <summary>
  /// Gets or sets the username for the SMTP server.
  /// </summary>
  public string SmtpUsername
  {
    get { return _SmtpUsername; }
    set { _SmtpUsername = value; }
  }

  private string _SmtpPassword;
  /// <summary>
  /// Gets or sets the password to the SMTP server.
  /// </summary>
  public string SmtpPassword
  {
    get { return _SmtpPassword; }
    set { _SmtpPassword = value; }
  }

  private bool _SendMailOnComment;
  /// <summary>
  /// Gets or sets whether or not to be notified by mail
  /// when a new comment is added to a post.
  /// </summary>
  public bool SendMailOnComment
  {
    get { return _SendMailOnComment; }
    set { _SendMailOnComment = value; }
  }

  private bool _IsCommentsEnabled;
  /// <summary>
  /// Gets or sets whether or not comments are enabled.
  /// </summary>
  public bool IsCommentsEnabled
  {
    get { return _IsCommentsEnabled; }
    set { _IsCommentsEnabled = value; }
  }

  private bool _EnableCountryInComments;

  public bool EnableCountryInComments
  {
    get { return _EnableCountryInComments; }
    set { _EnableCountryInComments = value; }
  }

  private bool _ShowLivePreview;

  public bool ShowLivePreview
  {
    get { return _ShowLivePreview; }
    set { _ShowLivePreview = value; }
  }

  private bool _IsCoCommentEnabled;

  public bool IsCoCommentEnabled
  {
    get { return _IsCoCommentEnabled; }
    set { _IsCoCommentEnabled = value; }
  }

  private int _DaysCommentsAreEnabled;

  public int DaysCommentsAreEnabled
  {
    get { return _DaysCommentsAreEnabled; }
    set { _DaysCommentsAreEnabled = value; }
  }

  private int _NumberOfRencentPosts;

  public int NumberOfRecentPosts
  {
    get { return _NumberOfRencentPosts; }
    set { _NumberOfRencentPosts = value; }
  }

  private string _SearchButtonText;

  public string SearchButtonText
  {
    get { return _SearchButtonText; }
    set { _SearchButtonText = value; }
  }

  private string _SearchDefaultText;

  public string SearchDefaultText
  {
    get { return _SearchDefaultText; }
    set { _SearchDefaultText = value; }
  }

  private bool _EnableCommentSearch;

  public bool EnableCommentSearch
  {
    get { return _EnableCommentSearch; }
    set { _EnableCommentSearch = value; }
  }

  private string _SearchCommentLabelText;

  public string SearchCommentLabelText
  {
    get { return _SearchCommentLabelText; }
    set { _SearchCommentLabelText = value; }
  }

  #endregion

  #region Methods

  private void Load()
  {
    string fileName = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SettingsFile"]);
    Type type = this.GetType();
    XmlDocument doc = new XmlDocument();
    doc.Load(fileName);

    foreach (XmlNode node in doc.SelectSingleNode("settings").ChildNodes)
    {
      string name = node.Name;
      string value = node.InnerText;

      foreach (PropertyInfo info in type.GetProperties())
      {
        if (info.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
          switch (info.PropertyType.ToString())
          {
            case "System.String":
              info.SetValue(this, value, null);
              break;
            case "System.Int32":
              info.SetValue(this, int.Parse(value), null);
              break;
            case "System.DateTime":
              info.SetValue(this, DateTime.Parse(value), null);
              break;
            case "System.Double":
              info.SetValue(this, double.Parse(value), null);
              break;
            case "System.Int64":
              info.SetValue(this, long.Parse(value), null);
              break;
            case "System.Boolean":
              info.SetValue(this, bool.Parse(value), null);
              break;
            default:
              throw new InvalidCastException("The BlogSettings does not allow properties of type '" + info.PropertyType + "'");
          }

          break;
        }
      }
    }
  }

  /// <summary>
  /// Saves the settings to disk.
  /// </summary>
  public void Save()
  {
    string fileName = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SettingsFile"]);
    Type type = this.GetType();
    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;

    using (XmlWriter writer = XmlWriter.Create(fileName, settings))
    {
      writer.WriteStartElement("settings");

      foreach (PropertyInfo info in type.GetProperties())
      {
        if (info.Name != "Instance")
          writer.WriteElementString(info.Name, info.GetValue(this, null).ToString());
      }

      writer.WriteEndElement();
    }

    OnChanged();
  }

  #endregion

  #region Events

  /// <summary>
  /// Occurs when the settings have been changed.
  /// </summary>
  public static event EventHandler<EventArgs> Changed;  
  private static void OnChanged()
  {
    if (Changed != null)
    {
      Changed(null, new EventArgs());
    }
  }        

  #endregion

}
