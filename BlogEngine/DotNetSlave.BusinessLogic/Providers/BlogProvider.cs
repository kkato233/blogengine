#region Using

using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

namespace BlogEngine.Core.Providers
{
  /// <summary>
  /// A base class for all custom providers to inherit from.
  /// </summary>
  public abstract class BlogProvider : ProviderBase
  {
    // Post
    public abstract Post SelectPost(Guid id);
    public abstract void InsertPost(Post post);
    public abstract void UpdatePost(Post post);
    public abstract void DeletePost(Post post);

    // Page
    public abstract Page SelectPage(Guid id);
    public abstract void InsertPage(Page page);
    public abstract void UpdatePage(Page page);
    public abstract void DeletePage(Page page);

    // Category
    public abstract CategoryDictionary LoadCategories();
    public abstract void SaveCategories(CategoryDictionary categories);
  }

  /// <summary>
  /// A collection of all registered providers.
  /// </summary>
  public class BlogProviderCollection : ProviderCollection
  {
    /// <summary>
    /// Gets a provider by its name.
    /// </summary>
    public new BlogProvider this[string name]
    {
      get { return (BlogProvider)base[name]; }
    }

    /// <summary>
    /// Add a provider to the collection.
    /// </summary>
    public override void Add(ProviderBase provider)
    {
      if (provider == null)
        throw new ArgumentNullException("provider");

      if (!(provider is BlogProvider))
        throw new ArgumentException
            ("Invalid provider type", "provider");

      base.Add(provider);
    }
  }

}
