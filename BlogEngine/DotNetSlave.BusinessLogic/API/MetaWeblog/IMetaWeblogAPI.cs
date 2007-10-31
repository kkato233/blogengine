using System;
using System.Collections.Generic;

namespace BlogEngine.Core.API.MetaWeblog
{
		///// <summary>
		///// Implements all required calls for MetaWeblog API
		///// http://www.xmlrpc.com/metaWeblogApi
		///// Also, includes the Blogger API calls that are needed.
		///// http://www.blogger.com/developers/api/1_docs/
		///// Interestingly, the blogger API docs miss one call that is needed.  Info found at MSDN.
		///// http://msdn2.microsoft.com/en-us/library/bb259697.aspx 
		///// http://msdn2.microsoft.com/en-us/library/aa905663.aspx
		///// </summary>
		//public interface IMetaWeblogAPI
		//{
		//    /// <summary>
		//    /// metaWeblog.newPost
		//    /// </summary>
		//    /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <param name="sentPost">struct with post details</param>
		//    /// <param name="publish">mark as published?</param>
		//    /// <returns>postID as string</returns>
		//    string NewPost(string blogID, string userName, string password, MWAPost sentPost, bool publish);

		//    /// <summary>
		//    /// metaWeblog.editPost
		//    /// </summary>
		//    /// <param name="postID">post guid in string format</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <param name="sentPost">struct with post details</param>
		//    /// <param name="publish">mark as published?</param>
		//    /// <returns>1 if successful</returns>
		//    bool EditPost(string postID, string userName, string password, MWAPost sentPost, bool publish);

		//    /// <summary>
		//    /// metaWeblog.getPost
		//    /// </summary>
		//    /// <param name="postID">post guid in string format</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <returns>struct with post details</returns>
		//    MWAPost GetPost(string postID, string userName, string password);

		//    /// <summary>
		//    /// metaWeblog.newMediaObject
		//    /// </summary>
		//    /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <param name="mediaObject">struct with media details</param>
		//    /// <returns>struct with url to media</returns>
		//    MWAMediaInfo NewMediaObject(string blogID, string userName, string password, MWAMediaObject mediaObject);

		//    /// <summary>
		//    /// metaWeblog.getCategories
		//    /// </summary>
		//    /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <returns>array of category structs</returns>
		//    List<MWACategory> GetCategories(string blogID, string userName, string password);

		//    /// <summary>
		//    /// metaWeblog.getRecentPosts
		//    /// </summary>
		//    /// <param name="blogID">always 1000 in BlogEngine since it is a singlar blog instance</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <param name="numberOfPosts">number of posts to return</param>
		//    /// <returns>array of post structs</returns>
		//    List<MWAPost> GetRecentPosts(string blogID, string userName, string password, int numberOfPosts);

		//    /// <summary>
		//    /// blogger.getUsersBlogs
		//    /// </summary>
		//    /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <returns>array of blog structs</returns>
		//    List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password);

		//    /// <summary>
		//    /// blogger.deletePost
		//    /// </summary>
		//    /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
		//    /// <param name="postID">post guid in string format</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <param name="publish">mark as published?</param>
		//    /// <returns></returns>
		//    bool DeletePost(string appKey, string postID, string userName, string password, bool publish);

		//    /// <summary>
		//    /// blogger.getUserInfo
		//    /// </summary>
		//    /// <remarks>
		//    /// This is never called in anything I've tested.
		//    /// </remarks>
		//    /// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
		//    /// <param name="userName">login username</param>
		//    /// <param name="password">login password</param>
		//    /// <returns>struct with user data</returns>
		//    MWAUserInfo GetUserInfo(string appKey, string userName, string password);

		//}

    /// <summary>
    /// Exception specifically for MetaWeblog API.  Error (or fault) responses 
    /// request a code value.  This is our chance to add one to the exceptions
    /// which can be used to produce a proper fault.
    /// </summary>
		[Serializable()]
    public class MetaWeblogException : Exception
    {
        /// <summary>
        /// Constructor to load properties
        /// </summary>
        /// <param name="code">Fault code to be returned in Fault Response</param>
        /// <param name="message">Message to be returned in Fault Response</param>
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
    internal struct MWACategory
    {
        /// <summary>
        /// Category title
        /// </summary>
        public string description;
        /// <summary>
        /// Url to thml display of category
        /// </summary>
        public string htmlUrl;
        /// <summary>
        /// Url to RSS for category
        /// </summary>
        public string rssUrl;
        /// <summary>
        /// The guid of the category
        /// </summary>
        public string id;
        /// <summary>
        /// The title/name of the category
        /// </summary>
        public string title;
    }

    /// <summary>
    /// MetaWeblog BlogInfo struct
    /// returned as an array from getUserBlogs
    /// </summary>
    internal struct MWABlogInfo
    {
        /// <summary>
        /// Blog Url
        /// </summary>
        public string url;
        /// <summary>
        /// Blog ID (Since BlogEngine.NET is single instance this number is always 10.
        /// </summary>
        public string blogID;
        /// <summary>
        /// Blog Title
        /// </summary>
        public string blogName;
    }

    /// <summary>
    /// MetaWeblog Fault struct
    /// returned when error occurs
    /// </summary>
    internal struct MWAFault
    {
        /// <summary>
        /// Error code of Fault Response
        /// </summary>
        public string faultCode;
        /// <summary>
        /// Message of Fault Response
        /// </summary>
        public string faultString;
    }

    /// <summary>
    /// MetaWeblog MediaObject struct
    /// passed in the newMediaObject call
    /// </summary>
    internal struct MWAMediaObject
    {
        /// <summary>
        /// Name of media object (filename)
        /// </summary>
        public string name;
        /// <summary>
        /// Type of file
        /// </summary>
        public string type;
        /// <summary>
        /// Media
        /// </summary>
        public byte[] bits;
    }

    /// <summary>
    /// MetaWeblog MediaInfo struct
    /// returned from NewMediaObject call
    /// </summary>
    internal struct MWAMediaInfo
    {
        /// <summary>
        /// Url that points to Saved MediaObejct
        /// </summary>
        public string url;
    }

    /// <summary>
    /// MetaWeblog Post struct
    /// used in newPost, editPost, getPost, recentPosts
    /// not all properties are used everytime.
    /// </summary>
    internal struct MWAPost
    {
        /// <summary>
        /// PostID Guid in string format
        /// </summary>
        public string postID;
        /// <summary>
        /// Title of Blog Post
        /// </summary>
        public string title;
        /// <summary>
        /// Link to Blog Post
        /// </summary>
        public string link;
        /// <summary>
        /// Content of Blog Post
        /// </summary>
        public string description;
        /// <summary>
        /// List of Categories assigned for Blog Post
        /// </summary>
        public List<string> categories;
        /// <summary>
        /// List of Tags assinged for Blog Post
        /// </summary>
        public List<string> tags;
        /// <summary>
        /// Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime postDate;
        /// <summary>
        /// Whether the Post is published or not.
        /// </summary>
        public bool publish;
    }

    /// <summary>
    /// MetaWeblog UserInfo struct
    /// returned from GetUserInfo call
    /// </summary>
    /// <remarks>
    /// Not used currently, but here for completeness.
    /// </remarks>
    internal struct MWAUserInfo
    {
        /// <summary>
        /// User Name Proper
        /// </summary>
        public string nickname;
        /// <summary>
        /// Login ID
        /// </summary>
        public string userID;
        /// <summary>
        /// Url to User Blog?
        /// </summary>
        public string url;
        /// <summary>
        /// Email address of User
        /// </summary>
        public string email;
        /// <summary>
        /// User LastName
        /// </summary>
        public string lastName;
        /// <summary>
        /// User First Name
        /// </summary>
        public string firstName;
    }
}
