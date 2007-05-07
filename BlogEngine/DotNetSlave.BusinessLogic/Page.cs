#region Using

using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using BlogEngine.Core.Providers;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// A page is much like a post, but is not part of the
  /// blog chronology and is more static in nature.
  /// <remarks>
  /// Pages can be used for "About" pages or other static
  /// information.
  /// </remarks>
  /// </summary>
  public class Page : BusinessBase<Page, Guid>
  {

    #region Constructor

    /// <summary>
    /// The contructor sets default values.
    /// </summary>
    public Page()
    {
      base.Id = Guid.NewGuid();
      DateCreated = DateTime.Now;
    }

    #endregion

    #region Properties

    private string _Title;
    /// <summary>
    /// Gets or sets the Title or the object.
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

    private string _Content;
    /// <summary>
    /// Gets or sets the Description or the object.
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

    private string _Description;
    /// <summary>
    /// Gets or sets the Description or the object.
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

    private string _Keywords;
    /// <summary>
    /// Gets or sets the Keywords or the object.
    /// </summary>
    public string Keywords
    {
      get { return _Keywords; }
      set
      {
        if (_Keywords != value) MarkDirty("Keywords");
        _Keywords = value;
      }
    }

    /// <summary>
    /// The relative URI to the page. For in-site use only.
    /// </summary>
    public Uri RelativeLink
    {
      get { return new Uri(VirtualPathUtility.ToAbsolute("~/page/" + Utils.RemoveIlegalCharacters(Title) + ".aspx"), UriKind.Relative); }
    }

    /// <summary>
    /// The absolute URI to the path.
    /// </summary>
    public Uri AbsoluteLink
    {
      get { return new Uri(Utils.AbsoluteWebRoot.ToString() + "page/" + Utils.RemoveIlegalCharacters(Title) + ".aspx"); }
    }

    private static object _SyncRoot = new object();
    private static List<Page> _Pages;
    /// <summary>
    /// Gets an unsorted list of all pages.
    /// </summary>
    public static List<Page> Pages
    {
      get
      {
        lock (_SyncRoot)
        {
          if (_Pages == null)
              _Pages = BlogService.FillPages();
          return _Pages;
        }
      }
    }

    /// <summary>
    /// Returns a page based on the specified id.
    /// </summary>
    public static Page GetPage(Guid id)
    {
      foreach (Page page in Pages)
      {
        if (page.Id == id)
          return page;
      }

      return null;
    }

    #endregion

    #region Base overrides

    /// <summary>
    /// Validates the properties on the Page.
    /// </summary>
    protected override void ValidationRules()
    {
      AddRule("Title", "Title must be set", string.IsNullOrEmpty(Title));
      AddRule("Content", "Content must be set", string.IsNullOrEmpty(Content));
    }

    /// <summary>
    /// Retrieves a page form the BlogProvider
    /// based on the specified id.
    /// </summary>
    protected override Page DataSelect(Guid id)
    {
      return BlogService.SelectPage(id);
    }

    /// <summary>
    /// Updates the page.
    /// </summary>
    protected override void DataUpdate()
    {
      BlogService.UpdatePage(this);
    }

    /// <summary>
    /// Inserts a new page to current BlogProvider.
    /// </summary>
    protected override void DataInsert()
    {
      BlogService.InsertPage(this);

      if (IsNew)
        Pages.Add(this);      
    }

    /// <summary>
    /// Deletes the page from the current BlogProvider.
    /// </summary>
    protected override void DataDelete()
    {
      BlogService.DeletePage(this);
      if (Pages.Contains(this))
        Pages.Remove(this);      
    }

    #endregion
  }
}
