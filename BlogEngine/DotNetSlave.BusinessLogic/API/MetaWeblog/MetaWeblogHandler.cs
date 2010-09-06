namespace BlogEngine.Core.API.MetaWeblog
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Security;

    /// <summary>
    /// HTTP Handler for MetaWeblog API
    /// </summary>
    internal class MetaWeblogHandler : IHttpHandler
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref = "T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref = "T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        /// <summary>
        /// Process the HTTP Request.  Create XMLRPC request, find method call, process it and create response object and sent it back.
        ///     This is the heart of the MetaWeblog API
        /// </summary>
        /// <param name="context">
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var rootUrl = Utils.AbsoluteWebRoot.ToString();
                    
                    // context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().IndexOf("metaweblog.axd"));
                var input = new XMLRPCRequest(context);
                var output = new XMLRPCResponse(input.MethodName);

                switch (input.MethodName)
                {
                    case "metaWeblog.newPost":
                        output.PostID = this.NewPost(
                            input.BlogID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.editPost":
                        output.Completed = this.EditPost(
                            input.PostID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.getPost":
                        output.Post = this.GetPost(input.PostID, input.UserName, input.Password);
                        break;
                    case "metaWeblog.newMediaObject":
                        output.MediaInfo = this.NewMediaObject(
                            input.BlogID, input.UserName, input.Password, input.MediaObject, context);
                        break;
                    case "metaWeblog.getCategories":
                        output.Categories = this.GetCategories(input.BlogID, input.UserName, input.Password, rootUrl);
                        break;
                    case "metaWeblog.getRecentPosts":
                        output.Posts = this.GetRecentPosts(
                            input.BlogID, input.UserName, input.Password, input.NumberOfPosts);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        output.Blogs = this.GetUserBlogs(input.AppKey, input.UserName, input.Password, rootUrl);
                        break;
                    case "blogger.deletePost":
                        output.Completed = this.DeletePost(
                            input.AppKey, input.PostID, input.UserName, input.Password, input.Publish);
                        break;
                    case "blogger.getUserInfo":

                        // Not implemented.  Not planned.
                        throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
                    case "wp.newPage":
                        output.PageID = this.NewPage(
                            input.BlogID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.getPageList":
                    case "wp.getPages":
                        output.Pages = this.GetPages(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getPage":
                        output.Page = this.GetPage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.editPage":
                        output.Completed = this.EditPage(
                            input.BlogID, input.PageID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.deletePage":
                        output.Completed = this.DeletePage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.getAuthors":
                        output.Authors = this.GetAuthors(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getTags":
                        output.Keywords = this.GetKeywords(input.BlogID, input.UserName, input.Password);
                        break;
                }

                output.Response(context);
            }
            catch (MetaWeblogException mex)
            {
                var output = new XMLRPCResponse("fault");
                var fault = new MWAFault();
                fault.faultCode = mex.Code;
                fault.faultString = mex.Message;
                output.Fault = fault;
                output.Response(context);
            }
            catch (Exception ex)
            {
                var output = new XMLRPCResponse("fault");
                var fault = new MWAFault();
                fault.faultCode = "0";
                fault.faultString = ex.Message;
                output.Fault = fault;
                output.Response(context);
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The delete page.
        /// </summary>
        /// <param name="blogID">
        /// The blog id.
        /// </param>
        /// <param name="pageID">
        /// The page id.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The delete page.
        /// </returns>
        /// <exception cref="MetaWeblogException">
        /// </exception>
        internal bool DeletePage(string blogID, string pageID, string userName, string password)
        {
            this.ValidateRequest(userName, password);
            try
            {
                var page = Page.GetPage(new Guid(pageID));
                page.Delete();
                page.Save(userName, password);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("15", "DeletePage failed.  Error: " + ex.Message);
            }

            return true;
        }

        /// <summary>
        /// blogger.deletePost
        /// </summary>
        /// <param name="appKey">
        /// Key from application.  Outdated methodology that has no use here.
        /// </param>
        /// <param name="postID">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// The delete post.
        /// </returns>
        internal bool DeletePost(string appKey, string postID, string userName, string password, bool publish)
        {
            this.ValidateRequest(userName, password);
            try
            {
                var post = Post.GetPost(new Guid(postID));
                post.Delete();
                post.Save(userName, password);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("12", "DeletePost failed.  Error: " + ex.Message);
            }

            return true;
        }

        /// <summary>
        /// The edit page.
        /// </summary>
        /// <param name="blogID">
        /// The blog id.
        /// </param>
        /// <param name="pageID">
        /// The page id.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="mPage">
        /// The m page.
        /// </param>
        /// <param name="publish">
        /// The publish.
        /// </param>
        /// <returns>
        /// The edit page.
        /// </returns>
        internal bool EditPage(
            string blogID, string pageID, string userName, string password, MWAPage mPage, bool publish)
        {
            this.ValidateRequest(userName, password);

            var page = Page.GetPage(new Guid(pageID));

            page.Title = mPage.title;
            page.Content = mPage.description;
            page.Keywords = mPage.mt_keywords;
            page.ShowInList = publish;
            page.Published = publish;
            if (mPage.pageParentID != "0")
            {
                page.Parent = new Guid(mPage.pageParentID);
            }

            page.Save();

            return true;
        }

        /// <summary>
        /// metaWeblog.editPost
        /// </summary>
        /// <param name="postID">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="sentPost">
        /// struct with post details
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// 1 if successful
        /// </returns>
        internal bool EditPost(string postID, string userName, string password, MWAPost sentPost, bool publish)
        {
            this.ValidateRequest(userName, password);

            var post = Post.GetPost(new Guid(postID));

            if (String.IsNullOrEmpty(sentPost.author))
            {
                post.Author = userName;
            }
            else
            {
                post.Author = sentPost.author;
            }

            post.Title = sentPost.title;
            post.Content = sentPost.description;
            post.Published = publish;
            post.Slug = sentPost.slug;
            post.Description = sentPost.excerpt;

            if (sentPost.commentPolicy != string.Empty)
            {
                if (sentPost.commentPolicy == "1")
                {
                    post.HasCommentsEnabled = true;
                }
                else
                {
                    post.HasCommentsEnabled = false;
                }
            }

            post.Categories.Clear();
            foreach (var item in sentPost.categories)
            {
                Category cat;
                if (this.LookupCategoryGuidByName(item, out cat))
                {
                    post.Categories.Add(cat);
                }
                else
                {
                    // Allowing new categories to be added.  (This breaks spec, but is supported via WLW)
                    var newcat = new Category(item, string.Empty);
                    newcat.Save();
                    post.Categories.Add(newcat);
                }
            }

            post.Tags.Clear();
            foreach (var item in sentPost.tags)
            {
                if (item != null && item.Trim() != string.Empty)
                {
                    post.Tags.Add(item);
                }
            }

            if (sentPost.postDate != new DateTime())
            {
                post.DateCreated = sentPost.postDate.AddHours(-BlogSettings.Instance.Timezone);
            }

            post.Save();

            return true;
        }

        /// <summary>
        /// The get authors.
        /// </summary>
        /// <param name="blogID">
        /// The blog id.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// </returns>
        internal List<MWAAuthor> GetAuthors(string blogID, string userName, string password)
        {
            this.ValidateRequest(userName, password);

            var authors = new List<MWAAuthor>();

            if (Roles.IsUserInRole(userName, BlogSettings.Instance.AdministratorRole))
            {
                var total = 0;
                var count = 0;
                var users = Membership.Provider.GetAllUsers(0, 999, out total);

                foreach (MembershipUser user in users)
                {
                    count++;
                    var temp = new MWAAuthor();
                    temp.user_id = user.UserName;
                    temp.user_login = user.UserName;
                    temp.display_name = user.UserName;
                    temp.user_email = user.Email;
                    temp.meta_value = string.Empty;
                    authors.Add(temp);
                }
            }
            else
            {
                // If not admin, just add that user to the options.
                var single = Membership.GetUser(userName);
                var temp = new MWAAuthor();
                temp.user_id = single.UserName;
                temp.user_login = single.UserName;
                temp.display_name = single.UserName;
                temp.user_email = single.Email;
                temp.meta_value = string.Empty;
                authors.Add(temp);
            }

            return authors;
        }

        /// <summary>
        /// metaWeblog.getCategories
        /// </summary>
        /// <param name="blogID">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="rootUrl">
        /// The root URL.
        /// </param>
        /// <returns>
        /// array of category structs
        /// </returns>
        internal List<MWACategory> GetCategories(string blogID, string userName, string password, string rootUrl)
        {
            var categories = new List<MWACategory>();

            this.ValidateRequest(userName, password);

            foreach (var cat in Category.Categories)
            {
                var temp = new MWACategory();
                temp.title = cat.Title;
                temp.description = cat.Title; // cat.Description;
                temp.htmlUrl = cat.AbsoluteLink.ToString();
                temp.rssUrl = cat.FeedAbsoluteLink.ToString();
                categories.Add(temp);
            }

            return categories;
        }

        /// <summary>
        /// wp.getTags
        /// </summary>
        /// <param name="blogID">
        /// </param>
        /// <param name="userName">
        /// </param>
        /// <param name="password">
        /// </param>
        /// <returns>
        /// list of tags
        /// </returns>
        internal List<string> GetKeywords(string blogID, string userName, string password)
        {
            var keywords = new List<string>();

            this.ValidateRequest(userName, password);

            foreach (var post in Post.Posts)
            {
                if (post.Visible)
                {
                    foreach (var tag in post.Tags)
                    {
                        if (!keywords.Contains(tag))
                        {
                            keywords.Add(tag);
                        }
                    }
                }
            }

            keywords.Sort();

            return keywords;
        }

        /// <summary>
        /// wp.getPage
        /// </summary>
        /// <param name="blogID">
        /// blogID in string format
        /// </param>
        /// <param name="pageID">
        /// page guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// struct with post details
        /// </returns>
        internal MWAPage GetPage(string blogID, string pageID, string userName, string password)
        {
            this.ValidateRequest(userName, password);

            var sendPage = new MWAPage();
            var page = Page.GetPage(new Guid(pageID));

            sendPage.pageID = page.Id.ToString();
            sendPage.title = page.Title;
            sendPage.description = page.Content;
            sendPage.mt_keywords = page.Keywords;
            sendPage.pageDate = page.DateCreated;
            sendPage.link = page.AbsoluteLink.AbsoluteUri;
            sendPage.mt_convert_breaks = "__default__";
            if (page.Parent != null)
            {
                sendPage.pageParentID = page.Parent.ToString();
            }

            return sendPage;
        }

        /// <summary>
        /// wp.getPages
        /// </summary>
        /// <param name="blogID">
        /// blogID in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// </returns>
        internal List<MWAPage> GetPages(string blogID, string userName, string password)
        {
            this.ValidateRequest(userName, password);

            var pages = new List<MWAPage>();

            foreach (var page in Page.Pages)
            {
                var mPage = new MWAPage();
                mPage.pageID = page.Id.ToString();
                mPage.title = page.Title;
                mPage.description = page.Content;
                mPage.mt_keywords = page.Keywords;
                mPage.pageDate = page.DateCreated;
                mPage.link = page.AbsoluteLink.AbsoluteUri;
                mPage.mt_convert_breaks = "__default__";
                mPage.pageParentID = page.Parent.ToString();

                pages.Add(mPage);
            }

            return pages;
        }

        /// <summary>
        /// metaWeblog.getPost
        /// </summary>
        /// <param name="postID">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// struct with post details
        /// </returns>
        internal MWAPost GetPost(string postID, string userName, string password)
        {
            this.ValidateRequest(userName, password);

            var sendPost = new MWAPost();
            var post = Post.GetPost(new Guid(postID));

            sendPost.postID = post.Id.ToString();
            sendPost.postDate = post.DateCreated;
            sendPost.title = post.Title;
            sendPost.description = post.Content;
            sendPost.link = post.AbsoluteLink.AbsoluteUri;
            sendPost.slug = post.Slug;
            sendPost.excerpt = post.Description;
            if (post.HasCommentsEnabled)
            {
                sendPost.commentPolicy = "1";
            }
            else
            {
                sendPost.commentPolicy = "0";
            }

            sendPost.publish = post.Published;

            var cats = new List<string>();
            for (var i = 0; i < post.Categories.Count; i++)
            {
                cats.Add(Category.GetCategory(post.Categories[i].Id).ToString());
            }

            sendPost.categories = cats;

            var tags = new List<string>();
            for (var i = 0; i < post.Tags.Count; i++)
            {
                tags.Add(post.Tags[i]);
            }

            sendPost.tags = tags;

            return sendPost;
        }

        /// <summary>
        /// metaWeblog.getRecentPosts
        /// </summary>
        /// <param name="blogID">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="numberOfPosts">
        /// number of posts to return
        /// </param>
        /// <returns>
        /// array of post structs
        /// </returns>
        internal List<MWAPost> GetRecentPosts(string blogID, string userName, string password, int numberOfPosts)
        {
            this.ValidateRequest(userName, password);

            var sendPosts = new List<MWAPost>();
            var posts = Post.Posts;

            // Set End Point
            var stop = numberOfPosts;
            if (stop > posts.Count)
            {
                stop = posts.Count;
            }

            foreach (var post in posts.GetRange(0, stop))
            {
                var tempPost = new MWAPost();
                var tempCats = new List<string>();
                var tempTags = new List<string>();

                tempPost.postID = post.Id.ToString();
                tempPost.postDate = post.DateCreated;
                tempPost.title = post.Title;
                tempPost.description = post.Content;
                tempPost.link = post.AbsoluteLink.AbsoluteUri;
                tempPost.slug = post.Slug;
                tempPost.excerpt = post.Description;
                if (post.HasCommentsEnabled)
                {
                    tempPost.commentPolicy = string.Empty;
                }
                else
                {
                    tempPost.commentPolicy = "0";
                }

                tempPost.publish = post.Published;
                for (var i = 0; i < post.Categories.Count; i++)
                {
                    tempCats.Add(Category.GetCategory(post.Categories[i].Id).ToString());
                }

                tempPost.categories = tempCats;

                for (var i = 0; i < post.Tags.Count; i++)
                {
                    tempTags.Add(post.Tags[i]);
                }

                tempPost.tags = tempTags;

                sendPosts.Add(tempPost);
            }

            return sendPosts;
        }

        /// <summary>
        /// blogger.getUsersBlogs
        /// </summary>
        /// <param name="appKey">
        /// Key from application.  Outdated methodology that has no use here.
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="rootUrl">
        /// The root URL.
        /// </param>
        /// <returns>
        /// array of blog structs
        /// </returns>
        internal List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password, string rootUrl)
        {
            var blogs = new List<MWABlogInfo>();

            this.ValidateRequest(userName, password);

            var temp = new MWABlogInfo();
            temp.url = rootUrl;
            temp.blogID = "1000";
            temp.blogName = BlogSettings.Instance.Name;
            blogs.Add(temp);

            return blogs;
        }

        /// <summary>
        /// metaWeblog.newMediaObject
        /// </summary>
        /// <param name="blogID">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="mediaObject">
        /// struct with media details
        /// </param>
        /// <param name="request">
        /// The HTTP request.
        /// </param>
        /// <returns>
        /// struct with url to media
        /// </returns>
        internal MWAMediaInfo NewMediaObject(
            string blogID, string userName, string password, MWAMediaObject mediaObject, HttpContext request)
        {
            this.ValidateRequest(userName, password);

            var mediaInfo = new MWAMediaInfo();

            var rootPath = BlogSettings.Instance.StorageLocation + "files/";
            var serverPath = request.Server.MapPath(rootPath);
            var saveFolder = serverPath;
            var fileName = mediaObject.name;
            var mediaFolder = string.Empty;

            // Check/Create Folders & Fix fileName
            if (mediaObject.name.LastIndexOf('/') > -1)
            {
                mediaFolder = mediaObject.name.Substring(0, mediaObject.name.LastIndexOf('/'));
                saveFolder += mediaFolder;
                mediaFolder += "/";
                saveFolder = saveFolder.Replace('/', Path.DirectorySeparatorChar);
                fileName = mediaObject.name.Substring(mediaObject.name.LastIndexOf('/') + 1);
            }
            else
            {
                if (saveFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    saveFolder = saveFolder.Substring(0, saveFolder.Length - 1);
                }
            }

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            saveFolder += Path.DirectorySeparatorChar;

            if (File.Exists(saveFolder + fileName))
            {
                // Find unique fileName
                for (var count = 1; count < 30000; count++)
                {
                    var tempFileName = fileName.Insert(fileName.LastIndexOf('.'), "_" + count);
                    if (!File.Exists(saveFolder + tempFileName))
                    {
                        fileName = tempFileName;
                        break;
                    }
                }
            }

            // Save File
            var fs = new FileStream(saveFolder + fileName, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(mediaObject.bits);
            bw.Close();

            // Set Url
            var rootUrl = Utils.AbsoluteWebRoot.ToString();
            if (BlogSettings.Instance.RequireSslMetaWeblogApi)
            {
                rootUrl = rootUrl.Replace("https://", "http://");
            }

            var mediaType = mediaObject.type;
            if (mediaType.IndexOf('/') > -1)
            {
                mediaType = mediaType.Substring(0, mediaType.IndexOf('/'));
            }

            switch (mediaType)
            {
                case "image":
                case "notsent":
                    // If there wasn't a type, let's pretend it is an image.  (Thanks Zoundry.  This is for you.)
                    rootUrl += "image.axd?picture=";
                    break;
                default:
                    rootUrl += "file.axd?file=";
                    break;
            }

            mediaInfo.url = rootUrl + mediaFolder + fileName;
            return mediaInfo;
        }

        /// <summary>
        /// wp.newPage
        /// </summary>
        /// <param name="blogID">
        /// blogID in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="mPage">
        /// </param>
        /// <param name="publish">
        /// </param>
        /// <returns>
        /// The new page.
        /// </returns>
        internal string NewPage(string blogID, string userName, string password, MWAPage mPage, bool publish)
        {
            this.ValidateRequest(userName, password);

            var page = new Page();
            page.Title = mPage.title;
            page.Content = mPage.description;
            page.Description = string.Empty; // Can not be set from WLW
            page.Keywords = mPage.mt_keywords;
            if (mPage.pageDate != new DateTime())
            {
                page.DateCreated = mPage.pageDate;
            }

            page.ShowInList = publish;
            page.Published = publish;
            if (mPage.pageParentID != "0")
            {
                page.Parent = new Guid(mPage.pageParentID);
            }

            page.Save();

            return page.Id.ToString();
        }

        /// <summary>
        /// metaWeblog.newPost
        /// </summary>
        /// <param name="blogID">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="sentPost">
        /// struct with post details
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// postID as string
        /// </returns>
        internal string NewPost(string blogID, string userName, string password, MWAPost sentPost, bool publish)
        {
            this.ValidateRequest(userName, password);

            var post = new Post();

            if (String.IsNullOrEmpty(sentPost.author))
            {
                post.Author = userName;
            }
            else
            {
                post.Author = sentPost.author;
            }

            post.Title = sentPost.title;
            post.Content = sentPost.description;
            post.Published = publish;
            post.Slug = sentPost.slug;
            post.Description = sentPost.excerpt;

            if (sentPost.commentPolicy != string.Empty)
            {
                if (sentPost.commentPolicy == "1")
                {
                    post.HasCommentsEnabled = true;
                }
                else
                {
                    post.HasCommentsEnabled = false;
                }
            }

            post.Categories.Clear();
            foreach (var item in sentPost.categories)
            {
                Category cat;
                if (this.LookupCategoryGuidByName(item, out cat))
                {
                    post.Categories.Add(cat);
                }
                else
                {
                    // Allowing new categories to be added.  (This breaks spec, but is supported via WLW)
                    var newcat = new Category(item, string.Empty);
                    newcat.Save();
                    post.Categories.Add(newcat);
                }
            }

            post.Tags.Clear();
            foreach (var item in sentPost.tags)
            {
                if (item != null && item.Trim() != string.Empty)
                {
                    post.Tags.Add(item);
                }
            }

            if (sentPost.postDate != new DateTime())
            {
                post.DateCreated = sentPost.postDate;
            }

            post.Save();

            return post.Id.ToString();
        }

        /// <summary>
        /// Returns Category Guid from Category name.
        /// </summary>
        /// <remarks>
        /// Reverse dictionary lookups are ugly.
        /// </remarks>
        /// <param name="name">
        /// </param>
        /// <param name="cat">
        /// </param>
        /// <returns>
        /// The lookup category guid by name.
        /// </returns>
        private bool LookupCategoryGuidByName(string name, out Category cat)
        {
            cat = new Category();
            foreach (var item in Category.Categories)
            {
                if (item.Title == name)
                {
                    cat = item;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks username and password.  Throws error if validation fails.
        /// </summary>
        /// <param name="userName">
        /// </param>
        /// <param name="password">
        /// </param>
        private void ValidateRequest(string userName, string password)
        {
            if (!Membership.ValidateUser(userName, password))
            {
                throw new MetaWeblogException("11", "User authentication failed");
            }
        }

        #endregion
    }

    /// <summary>
    /// Exception specifically for MetaWeblog API.  Error (or fault) responses 
    ///     request a code value.  This is our chance to add one to the exceptions
    ///     which can be used to produce a proper fault.
    /// </summary>
    [Serializable]
    public class MetaWeblogException : Exception
    {
        #region Constants and Fields

        /// <summary>
        /// The code.
        /// </summary>
        private readonly string _code;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaWeblogException"/> class. 
        /// Constructor to load properties
        /// </summary>
        /// <param name="code">
        /// Fault code to be returned in Fault Response
        /// </param>
        /// <param name="message">
        /// Message to be returned in Fault Response
        /// </param>
        public MetaWeblogException(string code, string message)
            : base(message)
        {
            this._code = code;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Code is actually for Fault Code.  It will be passed back in the 
        ///     response along with the error message.
        /// </summary>
        public string Code
        {
            get
            {
                return this._code;
            }
        }

        #endregion
    }

    /// <summary>
    /// MetaWeblog Category struct
    ///     returned as an array from GetCategories
    /// </summary>
    internal struct MWACategory
    {
        #region Constants and Fields

        /// <summary>
        ///     Category title
        /// </summary>
        public string description;

        /// <summary>
        ///     Url to thml display of category
        /// </summary>
        public string htmlUrl;

        /// <summary>
        ///     The guid of the category
        /// </summary>
        public string id;

        /// <summary>
        ///     Url to RSS for category
        /// </summary>
        public string rssUrl;

        /// <summary>
        ///     The title/name of the category
        /// </summary>
        public string title;

        #endregion
    }

    /// <summary>
    /// MetaWeblog BlogInfo struct
    ///     returned as an array from getUserBlogs
    /// </summary>
    internal struct MWABlogInfo
    {
        #region Constants and Fields

        /// <summary>
        ///     Blog ID (Since BlogEngine.NET is single instance this number is always 10.
        /// </summary>
        public string blogID;

        /// <summary>
        ///     Blog Title
        /// </summary>
        public string blogName;

        /// <summary>
        ///     Blog Url
        /// </summary>
        public string url;

        #endregion
    }

    /// <summary>
    /// MetaWeblog Fault struct
    ///     returned when error occurs
    /// </summary>
    internal struct MWAFault
    {
        #region Constants and Fields

        /// <summary>
        ///     Error code of Fault Response
        /// </summary>
        public string faultCode;

        /// <summary>
        ///     Message of Fault Response
        /// </summary>
        public string faultString;

        #endregion
    }

    /// <summary>
    /// MetaWeblog MediaObject struct
    ///     passed in the newMediaObject call
    /// </summary>
    internal struct MWAMediaObject
    {
        #region Constants and Fields

        /// <summary>
        ///     Media
        /// </summary>
        public byte[] bits;

        /// <summary>
        ///     Name of media object (filename)
        /// </summary>
        public string name;

        /// <summary>
        ///     Type of file
        /// </summary>
        public string type;

        #endregion
    }

    /// <summary>
    /// MetaWeblog MediaInfo struct
    ///     returned from NewMediaObject call
    /// </summary>
    internal struct MWAMediaInfo
    {
        #region Constants and Fields

        /// <summary>
        ///     Url that points to Saved MediaObejct
        /// </summary>
        public string url;

        #endregion
    }

    /// <summary>
    /// MetaWeblog Post struct
    ///     used in newPost, editPost, getPost, recentPosts
    ///     not all properties are used everytime.
    /// </summary>
    internal struct MWAPost
    {
        #region Constants and Fields

        /// <summary>
        ///     wp_author_id
        /// </summary>
        public string author;

        /// <summary>
        ///     List of Categories assigned for Blog Post
        /// </summary>
        public List<string> categories;

        /// <summary>
        ///     CommentPolicy (Allow/Deny)
        /// </summary>
        public string commentPolicy;

        /// <summary>
        ///     Content of Blog Post
        /// </summary>
        public string description;

        /// <summary>
        ///     Excerpt
        /// </summary>
        public string excerpt;

        /// <summary>
        ///     Link to Blog Post
        /// </summary>
        public string link;

        /// <summary>
        ///     Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime postDate;

        /// <summary>
        ///     PostID Guid in string format
        /// </summary>
        public string postID;

        /// <summary>
        ///     Whether the Post is published or not.
        /// </summary>
        public bool publish;

        /// <summary>
        ///     Slug of post
        /// </summary>
        public string slug;

        /// <summary>
        ///     List of Tags assinged for Blog Post
        /// </summary>
        public List<string> tags;

        /// <summary>
        ///     Title of Blog Post
        /// </summary>
        public string title;

        #endregion
    }

    ///// <summary>
    ///// MetaWeblog UserInfo struct
    ///// returned from GetUserInfo call
    ///// </summary>
    ///// <remarks>
    ///// Not used currently, but here for completeness.
    ///// </remarks>
    // internal struct MWAUserInfo
    // {
    // /// <summary>
    // /// User Name Proper
    // /// </summary>
    // //public string nickname;
    // /// <summary>
    // /// Login ID
    // /// </summary>
    // //public string userID;
    // /// <summary>
    // /// Url to User Blog?
    // /// </summary>
    // //public string url;
    // /// <summary>
    // /// Email address of User
    // /// </summary>
    // public string email;
    // /// <summary>
    // /// User LastName
    // /// </summary>
    // public string lastName;
    // /// <summary>
    // /// User First Name
    // /// </summary>
    // public string firstName;
    // }

    /// <summary>
    /// wp Page Struct
    /// </summary>
    internal struct MWAPage
    {
        #region Constants and Fields

        /// <summary>
        ///     Content of Blog Post
        /// </summary>
        public string description;

        /// <summary>
        ///     Link to Blog Post
        /// </summary>
        public string link;

        /// <summary>
        ///     Convert Breaks
        /// </summary>
        public string mt_convert_breaks;

        /// <summary>
        ///     Page keywords
        /// </summary>
        public string mt_keywords;

        /// <summary>
        ///     Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime pageDate;

        /// <summary>
        ///     PostID Guid in string format
        /// </summary>
        public string pageID;

        /// <summary>
        ///     Page Parent ID
        /// </summary>
        public string pageParentID;

        /// <summary>
        ///     Title of Blog Post
        /// </summary>
        public string title;

        #endregion
    }

    /// <summary>
    /// wp Author struct
    /// </summary>
    internal struct MWAAuthor
    {
        #region Constants and Fields

        /// <summary>
        ///     display name
        /// </summary>
        public string display_name;

        /// <summary>
        ///     nothing to see here.
        /// </summary>
        public string meta_value;

        /// <summary>
        ///     user email
        /// </summary>
        public string user_email;

        /// <summary>
        ///     userID - Specs call for a int, but our ID is a string.
        /// </summary>
        public string user_id;

        /// <summary>
        ///     user login name
        /// </summary>
        public string user_login;

        #endregion
    }
}