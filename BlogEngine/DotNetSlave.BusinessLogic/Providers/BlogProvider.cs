namespace BlogEngine.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    using BlogEngine.Core.DataStore;
    using BlogEngine.Core.FileSystem;

    /// <summary>
    /// A base class for all custom providers to inherit from.
    /// </summary>
    public abstract class BlogProvider : ProviderBase
    {
        // Post
        #region Public Methods

        /// <summary>
        /// Deletes a BlogRoll from the data store specified by the provider.
        /// </summary>
        /// <param name="blogRollItem">
        /// The blog Roll Item to delete.
        /// </param>
        public abstract void DeleteBlogRollItem(BlogRollItem blogRollItem);

        /// <summary>
        /// Deletes a Blog from the data store specified by the provider.
        /// </summary>
        /// <param name="blog">
        /// The blog to delete.
        /// </param>
        public abstract void DeleteBlog(Blog blog);

        /// <summary>
        /// Deletes a Blog's storage container from the data store specified by the provider.
        /// </summary>
        /// <param name="blog">
        /// The blog to delete the storage container of.
        /// </param>
        public abstract bool DeleteBlogStorageContainer(Blog blog);

        /// <summary>
        /// Deletes a Category from the data store specified by the provider.
        /// </summary>
        /// <param name="category">
        /// The category to delete.
        /// </param>
        public abstract void DeleteCategory(Category category);

        /// <summary>
        /// Deletes a Page from the data store specified by the provider.
        /// </summary>
        /// <param name="page">
        /// The page to delete.
        /// </param>
        public abstract void DeletePage(Page page);

        /// <summary>
        /// Deletes a Post from the data store specified by the provider.
        /// </summary>
        /// <param name="post">
        /// The post to delete.
        /// </param>
        public abstract void DeletePost(Post post);

        /// <summary>
        /// Deletes a Page from the data store specified by the provider.
        /// </summary>
        /// <param name="profile">
        /// The profile to delete.
        /// </param>
        public abstract void DeleteProfile(AuthorProfile profile);

        /// <summary>
        /// Retrieves all BlogRolls from the provider and returns them in a list.
        /// </summary>
        /// <returns>A list of BlogRollItem.</returns>
        public abstract List<BlogRollItem> FillBlogRoll();

        /// <summary>
        /// Retrieves all Blogs from the provider and returns them in a list.
        /// </summary>
        /// <returns>A list of Blogs.</returns>
        public abstract List<Blog> FillBlogs();

        /// <summary>
        /// Retrieves all Categories from the provider and returns them in a List.
        /// </summary>
        /// <returns>A list of Category.</returns>
        public abstract List<Category> FillCategories();

        /// <summary>
        /// Retrieves all Pages from the provider and returns them in a List.
        /// </summary>
        /// <returns>A list of Page.</returns>
        public abstract List<Page> FillPages();

        /// <summary>
        /// Retrieves all Posts from the provider and returns them in a List.
        /// </summary>
        /// <returns>A list of Post.</returns>
        public abstract List<Post> FillPosts();

        /// <summary>
        /// Retrieves all Pages from the provider and returns them in a List.
        /// </summary>
        /// <returns>A list of AuthorProfile.</returns>
        public abstract List<AuthorProfile> FillProfiles();

        /// <summary>
        /// Deletes a Referrer from the data store specified by the provider.
        /// </summary>
        /// <returns>A list of Referrer.</returns>
        public abstract List<Referrer> FillReferrers();

        /// <summary>
        /// Returns a dictionary representing rights and the roles that allow them.
        /// </summary>
        /// <returns>
        /// 
        /// The key must be a string of the name of the Rights enum of the represented Right.
        /// The value must be an IEnumerable of strings that includes only the role names of
        /// roles the right represents.
        /// 
        /// Inheritors do not need to worry about verifying that the keys and values are valid.
        /// This is handled in the Right class.
        /// 
        /// </returns>
        public abstract IDictionary<string, IEnumerable<String>> FillRights();

        /// <summary>
        /// Inserts a new BlogRoll into the data store specified by the provider.
        /// </summary>
        /// <param name="blogRollItem">
        /// The blog Roll Item.
        /// </param>
        public abstract void InsertBlogRollItem(BlogRollItem blogRollItem);

        /// <summary>
        /// Inserts a new Blog into the data store specified by the provider.
        /// </summary>
        /// <param name="blog">
        /// The blog.
        /// </param>
        public abstract void InsertBlog(Blog blog);

        /// <summary>
        /// Inserts a new Category into the data store specified by the provider.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        public abstract void InsertCategory(Category category);

        /// <summary>
        /// Inserts a new Page into the data store specified by the provider.
        /// </summary>
        /// <param name="page">
        /// The page to insert.
        /// </param>
        public abstract void InsertPage(Page page);

        /// <summary>
        /// Inserts a new Post into the data store specified by the provider.
        /// </summary>
        /// <param name="post">
        /// The post to insert.
        /// </param>
        public abstract void InsertPost(Post post);

        /// <summary>
        /// Inserts a new Page into the data store specified by the provider.
        /// </summary>
        /// <param name="profile">
        /// The profile to insert.
        /// </param>
        public abstract void InsertProfile(AuthorProfile profile);

        /// <summary>
        /// Inserts a new Referrer into the data store specified by the provider.
        /// </summary>
        /// <param name="referrer">
        /// The referrer to insert.
        /// </param>
        public abstract void InsertReferrer(Referrer referrer);

        /// <summary>
        /// Loads settings from data store
        /// </summary>
        /// <param name="extensionType">
        /// Extension Type
        /// </param>
        /// <param name="extensionId">
        /// Extensio Id
        /// </param>
        /// <returns>
        /// Settings as stream
        /// </returns>
        public abstract object LoadFromDataStore(ExtensionType extensionType, string extensionId);

        /// <summary>
        /// Loads the ping services.
        /// </summary>
        /// <returns>
        /// A StringCollection.
        /// </returns>
        public abstract StringCollection LoadPingServices();

        /// <summary>
        /// Loads the settings from the provider.
        /// </summary>
        /// <returns>A StringDictionary.</returns>
        public abstract StringDictionary LoadSettings();

        /// <summary>
        /// Loads the stop words used in the search feature.
        /// </summary>
        /// <returns>
        /// A StringCollection.
        /// </returns>
        public abstract StringCollection LoadStopWords();

        /// <summary>
        /// Removes settings from data store
        /// </summary>
        /// <param name="extensionType">
        /// Extension Type
        /// </param>
        /// <param name="extensionId">
        /// Extension Id
        /// </param>
        public abstract void RemoveFromDataStore(ExtensionType extensionType, string extensionId);

        /// <summary>
        /// Saves the ping services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public abstract void SavePingServices(StringCollection services);

        /// <summary>
        /// Saves all of the Rights and the roles that coorespond with them.
        /// </summary>
        /// <param name="rights"></param>
        public abstract void SaveRights(IEnumerable<Right> rights);

        /// <summary>
        /// Saves the settings to the provider.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public abstract void SaveSettings(StringDictionary settings);

        /// <summary>
        /// Saves settings to data store
        /// </summary>
        /// <param name="extensionType">
        /// Extension Type
        /// </param>
        /// <param name="extensionId">
        /// Extension Id
        /// </param>
        /// <param name="settings">
        /// Settings object
        /// </param>
        public abstract void SaveToDataStore(ExtensionType extensionType, string extensionId, object settings);

        /// <summary>
        /// Retrieves a BlogRoll from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Blog Roll Item Id.</param>
        /// <returns>A BlogRollItem.</returns>
        public abstract BlogRollItem SelectBlogRollItem(Guid id);

        /// <summary>
        /// Retrieves a Blog from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Blog Id.</param>
        /// <returns>A Blog.</returns>
        public abstract Blog SelectBlog(Guid id);

        /// <summary>
        /// Retrieves a Category from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Category id.</param>
        /// <returns>A Category.</returns>
        public abstract Category SelectCategory(Guid id);

        /// <summary>
        /// Retrieves a Page from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Page id.</param>
        /// <returns>The Page object.</returns>
        public abstract Page SelectPage(Guid id);

        /// <summary>
        /// Retrieves a Post from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Post id.</param>
        /// <returns>A Post object.</returns>
        public abstract Post SelectPost(Guid id);

        /// <summary>
        /// Retrieves a Page from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The AuthorProfile id.</param>
        /// <returns>An AuthorProfile.</returns>
        public abstract AuthorProfile SelectProfile(string id);

        /// <summary>
        /// Retrieves a Referrer from the provider based on the specified id.
        /// </summary>
        /// <param name="id">The Referrer Id.</param>
        /// <returns>A Referrer.</returns>
        public abstract Referrer SelectReferrer(Guid id);

        /// <summary>
        /// Sets up the required storage files/tables for a new Blog instance, from an existing blog instance.
        /// </summary>
        /// <param name="existingBlog">The existing blog instance to base the new blog instance off of.</param>
        /// <param name="newBlog">The new blog instance.</param>
        /// <returns>A boolean indicating if the setup process was successful.</returns>
        public abstract bool SetupBlogFromExistingBlog(Blog existingBlog, Blog newBlog);

        /// <summary>
        /// Updates an existing BlogRollItem in the data store specified by the provider.
        /// </summary>
        /// <param name="blogRollItem">
        /// The blogroll item to update.
        /// </param>
        public abstract void UpdateBlogRollItem(BlogRollItem blogRollItem);

        /// <summary>
        /// Updates an existing Blog in the data store specified by the provider.
        /// </summary>
        /// <param name="blog">
        /// The blog to update.
        /// </param>
        public abstract void UpdateBlog(Blog blog);

        /// <summary>
        /// Updates an existing Category in the data store specified by the provider.
        /// </summary>
        /// <param name="category">
        /// The category to update.
        /// </param>
        public abstract void UpdateCategory(Category category);

        /// <summary>
        /// Updates an existing Page in the data store specified by the provider.
        /// </summary>
        /// <param name="page">
        /// The page to update.
        /// </param>
        public abstract void UpdatePage(Page page);

        /// <summary>
        /// Updates an existing Post in the data store specified by the provider.
        /// </summary>
        /// <param name="post">
        /// The post to update.
        /// </param>
        public abstract void UpdatePost(Post post);

        /// <summary>
        /// Updates an existing Page in the data store specified by the provider.
        /// </summary>
        /// <param name="profile">
        /// The profile to update.
        /// </param>
        public abstract void UpdateProfile(AuthorProfile profile);

        /// <summary>
        /// Updates an existing Referrer in the data store specified by the provider.
        /// </summary>
        /// <param name="referrer">
        /// The referrer to update.
        /// </param>
        public abstract void UpdateReferrer(Referrer referrer);

        #endregion

        #region FileSystem 

        /// <summary>
        /// Creates a directory at a specific path
        /// </summary>
        /// <param name="VirtualPath">The virtual path to be created</param>
        /// <returns>the new Directory object created</returns>
        /// <remarks>
        /// Virtual path is the path starting from the /files/ containers
        /// The entity is created against the current blog id
        /// </remarks>
        internal abstract Directory CreateDirectory(string VirtualPath);

        /// <summary>
        /// Deletes a spefic directory from a virtual path
        /// </summary>
        /// <param name="VirtualPath">The path to delete</param>
        /// <remarks>
        /// Virtual path is the path starting from the /files/ containers
        /// The entity is queried against to current blog id
        /// </remarks>
        public abstract void DeleteDirectory(string VirtualPath);

        /// <summary>
        /// Returns wether or not the specific directory by virtual path exists
        /// </summary>
        /// <param name="VirtualPath">The virtual path to query</param>
        /// <returns>boolean</returns>
        public abstract bool DirectoryExists(string VirtualPath);

        /// <summary>
        /// gets a directory by the virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path</param>
        /// <returns>the directory object or null for no directory found</returns>
        public abstract Directory GetDirectory(string VirtualPath);

        /// <summary>
        /// gets a directory by a basedirectory and a string array of sub path tree
        /// </summary>
        /// <param name="BaseDirectory">the base directory object</param>
        /// <param name="SubPath">the params of sub path</param>
        /// <returns>the directory found, or null for no directory found</returns>
        public abstract Directory GetDirectory(Directory BaseDirectory, params string[] SubPath);

        /// <summary>
        /// gets all the directories underneath a base directory. Only searches one level.
        /// </summary>
        /// <param name="BaseDirectory">the base directory</param>
        /// <returns>collection of Directory objects</returns>
        public abstract IEnumerable<Directory> GetDirectories(Directory BaseDirectory);

        /// <summary>
        /// gets all the files in a directory, only searches one level
        /// </summary>
        /// <param name="BaseDirectory">the base directory</param>
        /// <returns>collection of File objects</returns>
        public abstract IEnumerable<File> GetFiles(Directory BaseDirectory);

        /// <summary>
        /// gets a specific file by virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path of the file</param>
        /// <returns></returns>
        public abstract File GetFile(string VirtualPath);

        /// <summary>
        /// boolean wether a file exists by its virtual path
        /// </summary>
        /// <param name="VirtualPath">the virtual path</param>
        /// <returns>boolean</returns>
        public abstract bool FileExists(string VirtualPath);

        /// <summary>
        /// deletes a file by virtual path
        /// </summary>
        /// <param name="VirtualPath">virtual path</param>
        public abstract void DeleteFile(string VirtualPath);

        /// <summary>
        /// uploads a file to the provider container
        /// </summary>
        /// <param name="FileBinary">file contents as byte array</param>
        /// <param name="FileName">the file name</param>
        /// <param name="BaseDirectory">directory object that is the owner</param>
        /// <returns>the new file object</returns>
        public abstract File UploadFile(byte[] FileBinary, string FileName, Directory BaseDirectory);

        /// <summary>
        /// uploads a file to the provider container
        /// </summary>
        /// <param name="FileBinary">the contents of the file as a byte array</param>
        /// <param name="FileName">the file name</param>
        /// <param name="BaseDirectory">the directory object that is the owner</param>
        /// <param name="Overwrite">boolean wether to overwrite the file if it exists.</param>
        /// <returns>the new file object</returns>
        public abstract File UploadFile(byte[] FileBinary, string FileName, Directory BaseDirectory, bool Overwrite);

        /// <summary>
        /// gets the file contents via Lazy load, however in the DbProvider the Contents are loaded when the initial object is created to cut down on DbReads
        /// </summary>
        /// <param name="BaseFile">the baseFile object to fill</param>
        /// <returns>the original file object</returns>
        internal abstract File GetFileContents(File BaseFile);
        #endregion
    }
}