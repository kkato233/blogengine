using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using BlogEngine.Core;

namespace BlogEngine.Core.API.MetaWeblog
{
    /// <summary>
    /// HTTP Handler for MetaWeblog API
    /// </summary>
    public class MetaWeblogHandler: IHttpHandler, IMetaWeblogAPI
    {
        private HttpContext _request;

        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Process the HTTP Request.  Create XMLRPC request, find method call, process it and create response object and sent it back.
        /// This is the heart of the MetaWeblog API
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            _request = context;

            try
            {
                XMLRPCRequest input = new XMLRPCRequest(context);
                XMLRPCResponse output = new XMLRPCResponse(input.MethodName);

                switch (input.MethodName)
                {
                    case "metaWeblog.newPost":
                        output.PostID = NewPost(input.BlogID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.editPost":
                        output.Completed = EditPost(input.PostID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.getPost":
                        output.Post = GetPost(input.PostID, input.UserName, input.Password);
                        break;
                    case "metaWeblog.newMediaObject":
                        output.MediaInfo = NewMediaObject(input.BlogID, input.UserName, input.Password, input.MediaObject);
                        break;
                    case "metaWeblog.getCategories":
                        output.Categories = GetCategories(input.BlogID, input.UserName, input.Password);
                        break;
                    case "metaWeblog.getRecentPosts":
                        output.Posts = GetRecentPosts(input.BlogID, input.UserName, input.Password, input.NumberOfPosts);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        output.Blogs = GetUserBlogs(input.AppKey, input.UserName, input.Password);
                        break;
                    case "blogger.deletePost":
                        output.Completed = DeletePost(input.AppKey, input.PostID, input.UserName, input.Password, input.Publish);
                        break;
                    case "blogger.getUserInfo":
                        //TODO: Implement getUserInfo
                        throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");

                }
                output.Response(context);
            }
            catch (MetaWeblogException mex)
            {
                XMLRPCResponse output = new XMLRPCResponse("fault");
                MWAFault fault = new MWAFault();
                fault.faultCode = mex.Code;
                fault.faultString = mex.Message;
                output.Fault = fault;
                output.Response(context);
            }
            catch (Exception ex)
            {
                XMLRPCResponse output = new XMLRPCResponse("fault");
                MWAFault fault = new MWAFault();
                fault.faultCode = "0";
                fault.faultString = ex.Message;
                output.Fault = fault;
                output.Response(context);
            }
        }

        #endregion

        #region IMetaWeblogAPI Members

        /// <summary>
        /// metaWeblog.newPost
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="sentPost">struct with post details</param>
        /// <param name="publish">mark as published?</param>
        /// <returns>postID as string</returns>
        public string NewPost(string blogID, string userName, string password, MWAPost sentPost, bool publish)
        {
            ValidateRequest(userName, password);

            Post post = new Post();

            post.Author = userName;
            post.Title = sentPost.title;
            post.Content = sentPost.description;
            post.IsPublished = publish;
            post.Categories.Clear();
            foreach (string item in sentPost.categories)
            {
                Guid key;
                if (LookupCategoryGuidByName(item, out key))
                    post.Categories.Add(key);
            }
            post.Tags.Clear();
            foreach (string item in sentPost.tags)
            {
                post.Tags.Add(item);
            }

            post.Save();

            return post.Id.ToString();
        }

        /// <summary>
        /// metaWeblog.editPost
        /// </summary>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="sentPost">struct with post details</param>
        /// <param name="publish">mark as published?</param>
        /// <returns>1 if successful</returns>
        public bool EditPost(string postID, string userName, string password, MWAPost sentPost, bool publish)
        {
            ValidateRequest(userName, password);

            Post post = Post.GetPost(new Guid(postID));

            post.Author = userName;
            post.Title = sentPost.title;
            post.Content = sentPost.description;
            post.IsPublished = publish;
            post.Categories.Clear();
            foreach (string item in sentPost.categories)
            {
                // Ignore categories not found (as per spec)
                Guid key;
                if (LookupCategoryGuidByName(item, out key))
                    post.Categories.Add(key);
            }
            post.Tags.Clear();
            foreach (string item in sentPost.tags)
            {
              post.Tags.Add(item);
            }

            post.Save();

            return true;
        }

        /// <summary>
        /// metaWeblog.getPost
        /// </summary>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>struct with post details</returns>
        public MWAPost GetPost(string postID, string userName, string password)
        {
            ValidateRequest(userName, password);

            MWAPost sendPost = new MWAPost();
            Post post = Post.GetPost(new Guid(postID));

            sendPost.postID = post.Id.ToString();
            sendPost.postDate = post.DateCreated;
            sendPost.title = post.Title;
            sendPost.description = post.Content;
            sendPost.link = post.AbsoluteLink.AbsoluteUri; 
            sendPost.publish = post.IsPublished;

            List<string> cats = new List<string>();
            for (int i = 0; i < post.Categories.Count; i++)
            {
                cats.Add(CategoryDictionary.Instance[post.Categories[i]]);
            }
            sendPost.categories = cats;

            List<string> tags = new List<string>();
            for (int i = 0; i < post.Tags.Count; i++)
            {
              tags.Add(post.Tags[i]);
            }
            sendPost.tags = tags;

            return sendPost;
        }

        /// <summary>
        /// metaWeblog.newMediaObject
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="mediaObject">struct with media details</param>
        /// <returns>struct with url to media</returns>
        public MWAMediaInfo NewMediaObject(string blogID, string userName, string password, MWAMediaObject mediaObject)
        {
            ValidateRequest(userName, password);

            MWAMediaInfo mediaInfo = new MWAMediaInfo();

            string rootPath = BlogSettings.Instance.StorageLocation + "files/";
            string serverPath = _request.Server.MapPath(rootPath);
            string saveFolder = serverPath;
            string fileName = mediaObject.name;

            // Check/Create Folders & Fix fileName
            if (mediaObject.name.LastIndexOf('/') > -1)
            {
                saveFolder += mediaObject.name.Substring(0, mediaObject.name.LastIndexOf('/'));
                saveFolder = saveFolder.Replace('/', '\\');
                

                fileName = mediaObject.name.Substring(mediaObject.name.LastIndexOf('/') + 1);
            }
            else
            {
                if (saveFolder.EndsWith("\\"))
                    saveFolder = saveFolder.Substring(0, saveFolder.Length - 1);
            }
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
            saveFolder += "\\";

            // Save File
            FileStream fs = new FileStream(saveFolder + fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(mediaObject.bits);
            bw.Close();

            // Set Url
            string rootUrl = _request.Request.Url.ToString().Substring(0, _request.Request.Url.ToString().IndexOf("metaweblog.axd"));

            string mediaType = mediaObject.type;
            if (mediaType.IndexOf('/') > -1)
                mediaType = mediaType.Substring(0, mediaType.IndexOf('/'));
            switch (mediaType)
            {
                case "image":
                case "notsent": // If there wasn't a type, let's pretend it is an image.  (Thanks Zoundry.  This is for you.)
                    rootUrl += "image.axd?picture=";
                    break;
                default:
                    rootUrl += "file.axd?file=";
                    break;
            }
            
            mediaInfo.url = rootUrl + mediaObject.name;
            return mediaInfo;
        }

        /// <summary>
        /// metaWeblog.getCategories
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>array of category structs</returns>
        public List<MWACategory> GetCategories(string blogID, string userName, string password)
        {
            List<MWACategory> categories = new List<MWACategory>();

            ValidateRequest(userName, password);

            string rootUrl = _request.Request.Url.ToString().Substring(0, _request.Request.Url.ToString().IndexOf("metaweblog.axd"));

            foreach (KeyValuePair<Guid, string> kvp in CategoryDictionary.Instance)
            {
                MWACategory temp = new MWACategory();
                temp.description = kvp.Value;
                temp.htmlUrl = rootUrl + "category/" + kvp.Value + ".aspx";
                temp.rssUrl = rootUrl + "category/syndication.axd?category=" + kvp.Key.ToString();
                categories.Add(temp);

            }

            return categories;
        }

        /// <summary>
        /// metaWeblog.getRecentPosts
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="numberOfPosts">number of posts to return</param>
        /// <returns>array of post structs</returns>
        public List<MWAPost> GetRecentPosts(string blogID, string userName, string password, int numberOfPosts)
        {
            ValidateRequest(userName, password);

            List<MWAPost> sendPosts = new List<MWAPost>();
            List<Post> posts = Post.Posts;

            // Set End Point
            int stop = numberOfPosts;
            if (stop > posts.Count)
                stop = posts.Count;

            foreach (Post post in posts.GetRange(0, stop))
            {
                MWAPost tempPost = new MWAPost();
                List<string> tempCats = new List<string>();
                List<string> tempTags = new List<string>();

                tempPost.postID = post.Id.ToString();
                tempPost.postDate = post.DateCreated;
                tempPost.title = post.Title;
                tempPost.description = post.Content;
                tempPost.link = post.AbsoluteLink.AbsoluteUri; //post.PermaLink.AbsoluteUri;
                tempPost.publish = post.IsPublished;
                for (int i = 0; i < post.Categories.Count; i++)
                {
                    tempCats.Add(CategoryDictionary.Instance[post.Categories[i]]);
                }
                tempPost.categories = tempCats;
                
                for (int i = 0; i < post.Tags.Count; i++)
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
        /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>array of blog structs</returns>
        public List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password)
        {
            List<MWABlogInfo> blogs = new List<MWABlogInfo>();

            ValidateRequest(userName, password);

            MWABlogInfo temp = new MWABlogInfo();
            temp.url = _request.Request.Url.ToString().Substring(0, _request.Request.Url.ToString().IndexOf("metaweblog.axd"));
            temp.blogID = "1000";
            temp.blogName = BlogSettings.Instance.Name;
            blogs.Add(temp);

            return blogs;
        }

        /// <summary>
        /// blogger.deletePost
        /// </summary>
        /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="publish">mark as published?</param>
        /// <returns></returns>
        public bool DeletePost(string appKey, string postID, string userName, string password, bool publish)
        {
            ValidateRequest(userName, password);
            try
            {
                Post post = Post.GetPost(new Guid(postID));
                post.Delete();
                post.Save();
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("12", "DeletePost failed.  Error: " + ex.Message);
            }

            return true;
        }

        /// <summary>
        /// blogger.getUserInfo
        /// </summary>
        /// <remarks>
        /// This is never called in anything I've tested.
        /// </remarks>
        /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>struct with user data</returns>
        public MWAUserInfo GetUserInfo(string appKey, string userName, string password)
        {
            throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks username and password.  Throws error if validation fails.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void ValidateRequest(string userName, string password)
        {
            if (!Membership.ValidateUser(userName, password))
            {
                throw new MetaWeblogException("11", "User authentication failed");
            }
        }

        /// <summary>
        /// Returns Category Guid from Category name.
        /// </summary>
        /// <remarks>
        /// Reverse dictionary lookups are ugly.
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool LookupCategoryGuidByName(string name, out Guid key)
        {
            key = new Guid();
            foreach (KeyValuePair<Guid, string> kvp in CategoryDictionary.Instance)
            {
                if (kvp.Value == name)
                {
                    key = kvp.Key;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
