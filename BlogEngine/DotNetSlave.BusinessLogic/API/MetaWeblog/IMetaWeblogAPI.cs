using System;
using System.Collections.Generic;

namespace BlogEngine.Core.API.MetaWeblog
{
    /// <summary>
    /// Implements all required calls for MetaWeblog API
    /// http://www.xmlrpc.com/metaWeblogApi
    /// Also, includes the Blogger API calls that are needed.
    /// http://www.blogger.com/developers/api/1_docs/
    /// Interestingly, the blogger API docs miss one call that is needed.  Info found at MSDN.
    /// http://msdn2.microsoft.com/en-us/library/bb259697.aspx 
    /// http://msdn2.microsoft.com/en-us/library/aa905663.aspx
    /// </summary>
    public interface IMetaWeblogAPI
    {
        /// <summary>
        /// metaWeblog.newPost
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="sentPost">struct with post details</param>
        /// <param name="publish">mark as published?</param>
        /// <returns>postID as string</returns>
        string NewPost(string blogID, string userName, string password, MWAPost sentPost, bool publish);

        /// <summary>
        /// metaWeblog.editPost
        /// </summary>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="sentPost">struct with post details</param>
        /// <param name="publish">mark as published?</param>
        /// <returns>1 if successful</returns>
        bool EditPost(string postID, string userName, string password, MWAPost sentPost, bool publish);

        /// <summary>
        /// metaWeblog.getPost
        /// </summary>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>struct with post details</returns>
        MWAPost GetPost(string postID, string userName, string password);

        /// <summary>
        /// metaWeblog.newMediaObject
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="mediaObject">struct with media details</param>
        /// <returns>struct with url to media</returns>
        MWAMediaInfo NewMediaObject(string blogID, string userName, string password, MWAMediaObject mediaObject);

        /// <summary>
        /// metaWeblog.getCategories
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>array of category structs</returns>
        List<MWACategory> GetCategories(string blogID, string userName, string password);

        /// <summary>
        /// metaWeblog.getRecentPosts
        /// </summary>
        /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="numberOfPosts">number of posts to return</param>
        /// <returns>array of post structs</returns>
        List<MWAPost> GetRecentPosts(string blogID, string userName, string password, int numberOfPosts);

        /// <summary>
        /// blogger.getUsersBlogs
        /// </summary>
        /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <returns>array of blog structs</returns>
        List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password);

        /// <summary>
        /// blogger.deletePost
        /// </summary>
        /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
        /// <param name="postID">post guid in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="publish">mark as published?</param>
        /// <returns></returns>
        bool DeletePost(string appKey, string postID, string userName, string password, bool publish);

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
        MWAUserInfo GetUserInfo(string appKey, string userName, string password);

    }

    /// <summary>
    /// Exception specifically for MetaWeblog API.  Error (or fault) responses 
    /// request a code value.  This is our chance to add one to the exceptions
    /// which can be used to produce a proper fault.
    /// </summary>
    public class MetaWeblogException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public MetaWeblogException(string code, string message)
            : base(message)
        {
            _code = code;
        }

        private string _code;

        /// <summary>
        /// Code is actually for Fault Code.  It will be passed back in the 
        /// response along with the error message.
        /// </summary>
        public string Code
        {
            get { return _code; }
        }
    }

    /// <summary>
    /// MetaWeblog Category struct
    /// returned as an array from GetCategories
    /// </summary>
    public struct MWACategory
    {
        /// <summary>
        /// 
        /// </summary>
        public string description;
        /// <summary>
        /// 
        /// </summary>
        public string htmlUrl;
        /// <summary>
        /// 
        /// </summary>
        public string rssUrl;
    }

    /// <summary>
    /// MetaWeblog BlogInfo struct
    /// returned as an array from getUserBlogs
    /// </summary>
    public struct MWABlogInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string url;
        /// <summary>
        /// 
        /// </summary>
        public string blogID;
        /// <summary>
        /// 
        /// </summary>
        public string blogName;
    }

    /// <summary>
    /// MetaWeblog Fault struct
    /// returned when error occurs
    /// </summary>
    public struct MWAFault
    {
        /// <summary>
        /// 
        /// </summary>
        public string faultCode;
        /// <summary>
        /// 
        /// </summary>
        public string faultString;
    }

    /// <summary>
    /// MetaWeblog MediaObject struct
    /// passed in the newMediaObject call
    /// </summary>
    public struct MWAMediaObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string name;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public byte[] bits;
    }

    /// <summary>
    /// MetaWeblog MediaInfo struct
    /// returned from NewMediaObject call
    /// </summary>
    public struct MWAMediaInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string url;
    }

    /// <summary>
    /// MetaWeblog Post struct
    /// used in newPost, editPost, getPost, recentPosts
    /// not all properties are used everytime.
    /// </summary>
    public struct MWAPost
    {
        /// <summary>
        /// 
        /// </summary>
        public string postID;
        /// <summary>
        /// 
        /// </summary>
        public string title;
        /// <summary>
        /// 
        /// </summary>
        public string link;
        /// <summary>
        /// 
        /// </summary>
        public string description;
        /// <summary>
        /// 
        /// </summary>
        public List<string> categories;
        /// <summary>
        /// 
        /// </summary>
        public DateTime postDate;
        /// <summary>
        /// 
        /// </summary>
        public bool publish;
    }

    /// <summary>
    /// MetaWeblog UserInfo struct
    /// returned from GetUserInfo call
    /// </summary>
    public struct MWAUserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string nickname;
        /// <summary>
        /// 
        /// </summary>
        public string userID;
        /// <summary>
        /// 
        /// </summary>
        public string url;
        /// <summary>
        /// 
        /// </summary>
        public string email;
        /// <summary>
        /// 
        /// </summary>
        public string lastName;
        /// <summary>
        /// 
        /// </summary>
        public string firstName;
    }
}
