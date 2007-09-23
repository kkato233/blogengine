#region Using

using System;
using System.Collections.Generic;
using BlogEngine.Core.Providers;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// Categories are a way to organize posts. 
  /// A post can be in multiple categories.
  /// </summary>
  [Serializable]
  public class Category : BusinessBase<Category, Guid>
  {

    internal static string _Folder = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    public Category()
    {
      Id = Guid.NewGuid();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    public Category(string title, string description)
    {
      this.Id = Guid.NewGuid();
      this._Title = title;
      this._Description = description;
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

    /// <summary>
    /// Returns a category based on the specified id.
    /// </summary>
    public static Category GetCategory(Guid id)
    {
      foreach (Category category in Categories)
      {
        if (category.Id == id)
          return category;
      }

      return null;
    }

    private static object _SyncRoot = new object();
    private static List<Category> _Categories;
    /// <summary>
    /// Gets an unsorted list of all Categories.
    /// </summary>
    public static List<Category> Categories
    {
      get
      {
        if (_Categories == null)
        {
          lock (_SyncRoot)
          {
            if (_Categories == null)
              _Categories = BlogService.FillCategories();
          }
        }

        return _Categories;
      }
    }

    #endregion

    #region Base overrides

    /// <summary>
    /// Reinforces the business rules by adding additional rules to the
    /// broken rules collection.
    /// </summary>
    protected override void ValidationRules()
    {
      AddRule("Title", "Title must be set", string.IsNullOrEmpty(Title));
    }

    /// <summary>
    /// Retrieves the object from the data store and populates it.
    /// </summary>
    /// <param name="id">The unique identifier of the object.</param>
    /// <returns>
    /// True if the object exists and is being populated successfully
    /// </returns>
    protected override Category DataSelect(Guid id)
    {
      return BlogService.SelectCategory(id);
    }

    /// <summary>
    /// Updates the object in its data store.
    /// </summary>
    protected override void DataUpdate()
    {
      if (IsDirty)
        BlogService.UpdateCategory(this);
    }

    /// <summary>
    /// Inserts a new object to the data store.
    /// </summary>
    protected override void DataInsert()
    {
      if (IsNew)
        BlogService.InsertCategory(this);
    }

    /// <summary>
    /// Deletes the object from the data store.
    /// </summary>
    protected override void DataDelete()
    {
      if (IsDeleted)
        BlogService.DeleteCategory(this);
      if (Categories.Contains(this))
        Categories.Remove(this);
    }
    
    /// <summary>
    /// Saves the object to the database.
    /// </summary>
   public override void Save()
    {
      if (this.IsDeleted)
        BlogService.DeleteCategory(this);
    
     if (this.IsDirty && !this.IsDeleted && !this.IsNew)
        BlogService.UpdateCategory(this);
     
     if (this.IsNew)
        BlogService.InsertCategory(this);
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString()
    {
      return Title;
    }

    #endregion
  }
}
