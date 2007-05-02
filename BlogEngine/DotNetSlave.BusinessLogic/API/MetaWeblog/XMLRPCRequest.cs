using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace BlogEngine.Core.API.MetaWeblog
{
    /// <summary>
    /// Obejct is the incoming XML-RPC Request.  Handles parsing the XML-RPC and 
    /// fills its properties with the values sent in the request.
    /// </summary>
    public class XMLRPCRequest
    {
        #region Contructors
        /// <summary>
        /// Loads XMLRPCRequest object from HttpContext
        /// </summary>
        /// <param name="input">incoming HttpContext</param>
        public XMLRPCRequest(HttpContext input)
        {
            string inputXML = ParseRequest(input);
            LoadXMLRequest(inputXML); // Loads Method Call and Associated Variables
        }

        /// <summary>
        /// Loads XMLRPCRequest object from string.  This is used by Unit Tests.
        /// </summary>
        /// <param name="inputXML">string containing incoming XML</param>
        public XMLRPCRequest(string inputXML)
        {
            LoadXMLRequest(inputXML); // Loads Method Call and Associated Variables
        }
        #endregion

        #region Local Vars

        private string _methodName;
        private List<XmlNode> _inputParams;

        private string _appKey;
        private string _blogID;
        private int _numberOfPosts;
        private string _password;
        private MWAPost _post;
        private string _postID;
        private bool _publish;
        private string _userName;
        private MWAMediaObject _media;

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string MethodName
        {
            // Read Only
            get { return _methodName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AppKey
        {
            // Read Only
            get { return _appKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BlogID
        {
            // Read Only
            get { return _blogID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MWAMediaObject MediaObject
        {
            // Read Only
            get { return _media; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfPosts
        {
            // Read Only
            get { return _numberOfPosts; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            // Read Only
            get { return _password; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MWAPost Post
        {
            // Read Only
            get { return _post; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PostID
        {
            // Read Only
            get { return _postID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Publish
        {
            // Read Only
            get { return _publish; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            // Read Only
            get { return _userName; }
        }

        #endregion

        #region Public Methods

        #endregion
        
        #region Private Methods

        /// <summary>
        /// Retrieves the content of the input stream
        /// and return it as plain text.
        /// </summary>
        //HACK: This function was found elsewhere in the project.
        private string ParseRequest(HttpContext context)
        {
            byte[] buffer = new byte[context.Request.InputStream.Length];
            context.Request.InputStream.Read(buffer, 0, buffer.Length);

            return System.Text.Encoding.Default.GetString(buffer);
        }

        /// <summary>
        /// Loads object properties with contents of passed xml
        /// </summary>
        /// <param name="xml">xml doc with methodname and parameters</param>
        private void LoadXMLRequest(string xml)
        {
            XmlDocument request = new XmlDocument();
            try
            {
                request.LoadXml(xml);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("01", "Invalid XMLRPC Request. (" + ex.Message + ")");
            }
            
            // Method name is always first
            _methodName = request.DocumentElement.ChildNodes[0].InnerText;

            // Parameters are next (and last)
            _inputParams = new List<XmlNode>();
            foreach (XmlNode node in request.SelectNodes("/methodCall/params/param"))
            {
                _inputParams.Add(node);
            }

            // Determine what params are what by method name
            switch (_methodName)
            {
                case "metaWeblog.newPost":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;

                    break;
                case "metaWeblog.editPost":
                    _postID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;

                    break;
                case "metaWeblog.getPost":
                    _postID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.newMediaObject":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _media = GetMediaObject(_inputParams[3]);
                    break;
                case "metaWeblog.getCategories":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.getRecentPosts":
                    _blogID = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    _numberOfPosts = Int32.Parse(_inputParams[3].InnerText);
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                    _appKey = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                case "blogger.deletePost":
                    _appKey = _inputParams[0].InnerText;
                    _postID = _inputParams[1].InnerText;
                    _userName = _inputParams[2].InnerText;
                    _password = _inputParams[3].InnerText;
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        _publish = false;
                    else
                        _publish = true;

                    break;
                case "blogger.getUserInfo":
                    _appKey = _inputParams[0].InnerText;
                    _userName = _inputParams[1].InnerText;
                    _password = _inputParams[2].InnerText;
                    break;
                default:
                    throw new MetaWeblogException("02", "Unknown Method. (" + _methodName + ")");

            }

        }

        private MWAPost GetPost(XmlNode node)
        {
            MWAPost temp = new MWAPost();
            List<string> cats = new List<string>();
            
            // Require Title and Description
            try
            {
                temp.title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
                temp.description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("05", "Post Struct Element, Title or Description,  not Sent. (" + ex.Message + ")");
            }
            if (node.SelectSingleNode("value/struct/member[name='link']") == null)
                temp.link = "";
            else
                temp.link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;
            
            if (node.SelectSingleNode("value/struct/member[name='categories']") != null)
            {
                XmlNode categoryArray = node.SelectSingleNode("value/struct/member[name='categories']").LastChild;
                foreach (XmlNode catnode in categoryArray.SelectNodes("array/data/value/string"))
                {
                    cats.Add(catnode.InnerText);
                }
            }

            temp.categories = cats;

            return temp;
        }

        private MWAMediaObject GetMediaObject(XmlNode node)
        {
            MWAMediaObject temp = new MWAMediaObject();
            temp.name = node.SelectSingleNode("value/struct/member[name='name']").LastChild.InnerText;
            if (node.SelectSingleNode("value/struct/member[name='type']") == null)
                temp.type = "notsent";
            else
                temp.type = node.SelectSingleNode("value/struct/member[name='type']").LastChild.InnerText;
            temp.bits = System.Convert.FromBase64String(node.SelectSingleNode("value/struct/member[name='bits']").LastChild.InnerText);

            return temp;
        }

        #endregion

    }
}
