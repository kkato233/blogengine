using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace BlogEngine.Core.API.MetaWeblog
{
    /// <summary>
    /// Object is the outgoing XML-RPC response.  This objects properties are set
    /// and the Response method is called sending the response via the HttpContext Response.
    /// </summary>
    public class XMLRPCResponse
    {
        #region Contructors

        /// <summary>
        /// Constructor sets default value
        /// </summary>
        /// <param name="methodName">MethodName of called XML-RPC method</param>
        public XMLRPCResponse(string methodName)
        {
            _methodName = methodName;
            _blogs = new List<MWABlogInfo>();
            _categories = new List<MWACategory>();
            _posts = new List<MWAPost>();
        }
        #endregion

        #region Local Vars

        private string _methodName;

        private List<MWABlogInfo> _blogs;
        private List<MWACategory> _categories;
        private bool _completed;
        private MWAFault _fault;
        private MWAMediaInfo _mediaInfo;
        private MWAPost _post;
        private string _postID;
        private List<MWAPost> _posts;

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of Called Metaweblog Function
        /// </summary>
        public string MethodName
        {
            // Read Only
            get { return _methodName; }
        }

        /// <summary>
        /// List of blog structs.  Used by blogger.getUsersBlogs.
        /// </summary>
        public List<MWABlogInfo> Blogs
        {
            get { return _blogs; }
            set { _blogs = value; }
        }

        /// <summary>
        /// List of category structs. Used by metaWeblog.getCategories.
        /// </summary>
        public List<MWACategory> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        /// <summary>
        /// Marks whether function call was completed and successful.  
        /// Used by metaWeblog.editPost and blogger.deletePost.
        /// </summary>
        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; }
        }

        /// <summary>
        /// Fault Struct. Used by API to return error information
        /// </summary>
        public MWAFault Fault
        {
            get { return _fault; }
            set { _fault = value; }
        }

        /// <summary>
        /// MediaInfo Struct
        /// </summary>
        public MWAMediaInfo MediaInfo
        {
            get { return _mediaInfo; }
            set { _mediaInfo = value; }
        }

        /// <summary>
        /// Metaweblog Post Struct. Used by metaWeblog.getPost
        /// </summary>
        public MWAPost Post
        {
            get { return _post; }
            set { _post = value; }
        }

        /// <summary>
        /// Id of post that was just added.  Used by metaWeblog.newPost
        /// </summary>
        public string PostID
        {
            get { return _postID; }
            set { _postID = value; }
        }

        /// <summary>
        /// List of Metaweblog Post Structs.  Used by metaWeblog.getRecentPosts
        /// </summary>
        public List<MWAPost> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Response generates the XML-RPC response and returns it to the caller.
        /// </summary>
        /// <param name="context">httpContext.Response.OutputStream is used from the context</param>
        public void Response(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            using (XmlTextWriter data = new XmlTextWriter(context.Response.OutputStream, System.Text.Encoding.UTF8))
            {
                data.Formatting = Formatting.Indented;
                data.WriteStartDocument();
                data.WriteStartElement("methodResponse");
                if (_methodName == "fault")
                    data.WriteStartElement("fault");
                else
                    data.WriteStartElement("params");
                
                switch (_methodName)
                {
                    case "metaWeblog.newPost":
                        WriteNewPost(data);
                        break;
                    case "metaWeblog.editPost":
                        WriteBool(data);
                        break;
                    case "metaWeblog.getPost":
                        WritePost(data);
                        break;
                    case "metaWeblog.newMediaObject":
                        WriteMediaInfo(data);
                        break;
                    case "metaWeblog.getCategories":
                        WriteGetCategories(data);
                        break;
                    case "metaWeblog.getRecentPosts":
                        WritePosts(data);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        WriteGetUsersBlogs(data);
                        break;
                    case "blogger.deletePost":
                        WriteBool(data);
                        break;
                    case "blogger.getUserInfo":
                        //TODO: Implement getUserInfo
                        // This should not occur as it has not been implemented in the handler.
                        break;
                    case "fault":
                        WriteFault(data);
                        break;

                }
                
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndDocument();
                
            }
        }

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Writes Fault Parameters of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteFault(XmlTextWriter data)
        {
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // faultCode
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultCode");
            data.WriteElementString("value", _fault.faultCode);
            data.WriteEndElement();

            // faultString
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultString");
            data.WriteElementString("value", _fault.faultString);
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();

        }

        /// <summary>
        /// Writes Boolean parameter of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteBool(XmlTextWriter data)
        {
            string postValue = "0";
            if (_completed)
                postValue = "1";
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("boolean", postValue);
            data.WriteEndElement();
            data.WriteEndElement();
        }
        
        /// <summary>
        /// Writes the Array of Category structs parameters of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteGetCategories(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWACategory category in _categories)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", category.description);
                data.WriteEndElement();

                // categoryid
                data.WriteStartElement("member");
                data.WriteElementString("name", "categoryid");
                data.WriteElementString("value", category.description);
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteElementString("value", category.description);
                data.WriteEndElement();

                // htmlUrl 
                data.WriteStartElement("member");
                data.WriteElementString("name", "htmlUrl");
                data.WriteElementString("value", category.htmlUrl);
                data.WriteEndElement();

                // rssUrl
                data.WriteStartElement("member");
                data.WriteElementString("name", "rssUrl");
                data.WriteElementString("value", category.rssUrl);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }

        /// <summary>
        /// Writes the MediaInfo Struct of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteMediaInfo(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // url
            data.WriteStartElement("member");
            data.WriteElementString("name", "url");
            data.WriteStartElement("value");
            data.WriteElementString("string", _mediaInfo.url);
            data.WriteEndElement();
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }
        
        /// <summary>
        /// Writes the PostID string of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteNewPost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("string", _postID);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        /// Writes the Metaweblog Post Struct of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WritePost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // postid
            data.WriteStartElement("member");
            data.WriteElementString("name", "postid");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.postID);
            data.WriteEndElement();
            data.WriteEndElement();

            // title
            data.WriteStartElement("member");
            data.WriteElementString("name", "title");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.title);
            data.WriteEndElement();
            data.WriteEndElement();

            // description
            data.WriteStartElement("member");
            data.WriteElementString("name", "description");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.description);
            data.WriteEndElement();
            data.WriteEndElement();

            // link
            data.WriteStartElement("member");
            data.WriteElementString("name", "link");
            data.WriteStartElement("value");
            data.WriteElementString("string", _post.link);
            data.WriteEndElement();
            data.WriteEndElement();

            // tags (mt_keywords)
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_keywords");
            data.WriteStartElement("value");
            string[] tags = new string[_post.tags.Count];
            for (int i = 0; i < _post.tags.Count; i++)
            {
              tags[i] = _post.tags[i];
            }
            string tagList = string.Join(",", tags);
            data.WriteElementString("string", tagList);
            data.WriteEndElement();
            data.WriteEndElement();

            // categories
            if (_post.categories.Count > 0)
            {
                data.WriteStartElement("member");
                data.WriteElementString("name", "categories");
                data.WriteStartElement("value");
                data.WriteStartElement("array");
                data.WriteStartElement("data");
                foreach (string cat in _post.categories)
                {
                    data.WriteStartElement("value");
                    data.WriteElementString("string", cat);
                    data.WriteEndElement();
                }
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
            }

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        /// Writes the array of Metaweblog Post Structs of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WritePosts(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPost post in _posts)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // postid
                data.WriteStartElement("member");
                data.WriteElementString("name", "postid");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.postID);
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(post.postDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.title);
                data.WriteEndElement();
                data.WriteEndElement();

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", post.description);
                data.WriteEndElement();

                // link
                data.WriteStartElement("member");
                data.WriteElementString("name", "link");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.link);
                data.WriteEndElement();
                data.WriteEndElement();
                
                // tags (mt_keywords)
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_keywords");
                data.WriteStartElement("value");
                string[] tags = new string[post.tags.Count];
                for (int i = 0; i < post.tags.Count; i++)
                {
                  tags[i] = post.tags[i];
                }
                string tagList = string.Join(",", tags);
                data.WriteElementString("string", tagList);
                data.WriteEndElement();
                data.WriteEndElement();

                // publish
                data.WriteStartElement("member");
                data.WriteElementString("name", "publish");
                data.WriteStartElement("value");
                if (post.publish)
                    data.WriteElementString("boolean", "1");
                else
                    data.WriteElementString("boolean", "0");
                data.WriteEndElement();
                data.WriteEndElement();

                // categories
                if (post.categories.Count > 0)
                {
                    data.WriteStartElement("member");
                    data.WriteElementString("name", "categories");
                    data.WriteStartElement("value");
                    data.WriteStartElement("array");
                    data.WriteStartElement("data");
                    foreach (string cat in post.categories)
                    {
                        data.WriteStartElement("value");
                        data.WriteElementString("string", cat);
                        data.WriteEndElement();
                    }
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                }

                data.WriteEndElement();
                data.WriteEndElement();

            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();

        }
        
        /// <summary>
        /// Writes array of BlogInfo structs of Response
        /// </summary>
        /// <param name="data"></param>
        private void WriteGetUsersBlogs(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWABlogInfo blog in _blogs)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // url
                data.WriteStartElement("member");
                data.WriteElementString("name", "url");
                data.WriteElementString("value", blog.url);
                data.WriteEndElement();

                // blogid
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogid");
                data.WriteElementString("value", blog.blogID);
                data.WriteEndElement();

                // blogName
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogName");
                data.WriteElementString("value", blog.blogName);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();

            }
            
            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
                
        }

        /// <summary>
        /// Convert Date to format expected by MetaWeblog Response.
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns>ISO8601 date string</returns>
        private string ConvertDatetoISO8601(DateTime date)
        {
            string temp = date.Year.ToString() + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0') +
                "T" + date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') + ":" + date.Second.ToString().PadLeft(2, '0');
            return temp;
        }

        #endregion

    }
}
