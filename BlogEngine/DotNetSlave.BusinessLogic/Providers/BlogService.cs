#region Using

using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Providers
{
  /// <summary>
  /// The proxy class for communication between
  /// the business objects and the providers.
  /// </summary>
  public static class BlogService
  {

    #region Provider model

    private static BlogProvider _provider;
    private static BlogProviderCollection _providers;
    private static object _lock = new object();

    /// <summary>
    /// Gets the current provider.
    /// </summary>
    public static BlogProvider Provider
    {
      get { return _provider; }
    }

    /// <summary>
    /// Gets a collection of all registered providers.
    /// </summary>
    public static BlogProviderCollection Providers
    {
      get { return _providers; }
    }

    /// <summary>
    /// Load the providers from the web.config.
    /// </summary>
    private static void LoadProviders()
    {
      // Avoid claiming lock if providers are already loaded
      if (_provider == null)
      {
        lock (_lock)
        {
          // Do this again to make sure _provider is still null
          if (_provider == null)
          {
            // Get a reference to the <blogProvider> section
            BlogProviderSection section = (BlogProviderSection)WebConfigurationManager.GetSection("BlogEngine/blogProvider");

            // Load registered providers and point _provider
            // to the default provider
            _providers = new BlogProviderCollection();
            ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(BlogProvider));
            _provider = _providers[section.DefaultProvider];

            if (_provider == null)
              throw new ProviderException("Unable to load default BlogProvider");
          }
        }
      }
    }

    #endregion

    #region Posts

    /// <summary>
    /// Returns a Post based on the specified id.
    /// </summary>
    public static Post SelectPost(Guid id)
    {
      LoadProviders();
      return _provider.SelectPost(id);
    }

    /// <summary>
    /// Persists a new Post in the current provider.
    /// </summary>
    public static void InsertPost(Post post)
    {
      LoadProviders();
      _provider.InsertPost(post);
    }

    /// <summary>
    /// Updates an exsiting Post.
    /// </summary>
    public static void UpdatePost(Post post)
    {
      LoadProviders();
      _provider.UpdatePost(post);
    }

    /// <summary>
    /// Deletes the specified Post from the current provider.
    /// </summary>
    public static void DeletePost(Post post)
    {
      LoadProviders();
      _provider.DeletePost(post);
    }

    #endregion

    #region Pages

    /// <summary>
    /// Returns a Page based on the specified id.
    /// </summary>
    public static Page SelectPage(Guid id)
    {
      LoadProviders();
      return _provider.SelectPage(id);
    }

    /// <summary>
    /// Persists a new Page in the current provider.
    /// </summary>
    public static void InsertPage(Page page)
    {
      LoadProviders();
      _provider.InsertPage(page);
    }

    /// <summary>
    /// Updates an exsiting Page.
    /// </summary>
    public static void UpdatePage(Page page)
    {
      LoadProviders();
      _provider.UpdatePage(page);
    }

    /// <summary>
    /// Deletes the specified Page from the current provider.
    /// </summary>
    public static void DeletePage(Page page)
    {
      LoadProviders();
      _provider.DeletePage(page);
    }

    #endregion

    #region Categories

    /// <summary>
    /// Retrieves all the categories.
    /// </summary>
    public static CategoryDictionary SelectCategories()
    {
      LoadProviders();
      return _provider.LoadCategories();
    }

    /// <summary>
    /// Saves the specified categories.
    /// </summary>
    public static void SaveCategories(CategoryDictionary categories)
    {
      LoadProviders();
      _provider.SaveCategories(categories);
    }

    #endregion

  }
}
