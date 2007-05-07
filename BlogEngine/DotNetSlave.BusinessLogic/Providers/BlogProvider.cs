#region Using

using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Providers
{
    /// <summary>
    /// A base class for all custom providers to inherit from.
    /// </summary>
    public abstract class BlogProvider : ProviderBase
    {
        // Post
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Post SelectPost(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        public abstract void InsertPost(Post post);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        public abstract void UpdatePost(Post post);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        public abstract void DeletePost(Post post);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract List<Post> FillPosts();

        // Page
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Page SelectPage(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        public abstract void InsertPage(Page page);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        public abstract void UpdatePage(Page page);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        public abstract void DeletePage(Page page);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract List<Page> FillPages();

        // Category
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract CategoryDictionary LoadCategories();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
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
