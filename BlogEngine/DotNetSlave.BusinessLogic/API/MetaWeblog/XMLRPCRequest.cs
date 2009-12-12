using System;
using System.Collections.Generic;
using System.Globalization;
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
    internal class XMLRPCRequest
    {
      #region Contructors
      /// <summary>
      /// Loads XMLRPCRequest object from HttpContext
      /// </summary>
      /// <param name="input">incoming HttpContext</param>
      public XMLRPCRequest(HttpContext input)
      {
          string inputXML = ParseRequest(input);
          //LogMetaWeblogCall(inputXML);
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
      private MWAPage _page;
      private string _pageID;

      #endregion

      #region Public Properties
      /// <summary>
      /// Name of Called Metaweblog Function
      /// </summary>
      public string MethodName
      {
        get { return _methodName; }
      }

      /// <summary>
      /// AppKey is a key generated by the calling application.  It is sent with blogger API calls.
      /// </summary>
      /// <remarks>
      /// BlogEngine.NET doesn't require specific AppKeys for API calls.  It is no longer standard practive.
      /// </remarks>
      public string AppKey
      {
        get { return _appKey; }
      }

      /// <summary>
      /// ID of the Blog to call the function on.  Since BlogEngine supports only a single blog instance,
      /// this incoming parameter is not used.
      /// </summary>
      public string BlogID
      {
        get { return _blogID; }
      }

      /// <summary>
      /// MediaObject is a struct sent by the metaWeblog.newMediaObject function.
      /// It contains information about the media and the object in a bit array.
      /// </summary>
      public MWAMediaObject MediaObject
      {
        get { return _media; }
      }

      /// <summary>
      /// Number of post request by the metaWeblog.getRecentPosts function
      /// </summary>
      public int NumberOfPosts
      {
        get { return _numberOfPosts; }
      }

      /// <summary>
      /// Password for user validation
      /// </summary>
      public string Password
      {
        get { return _password; }
      }

      /// <summary>
      /// Metaweblog Post struct containing information post including title, content, and categories.
      /// </summary>
      public MWAPost Post
      {
        get { return _post; }
      }

      /// <summary>
      /// Metaweblog Page Struct
      /// </summary>
      public MWAPage Page
      {
        get { return _page; }
      }

      /// <summary>
      /// The PostID Guid in string format
      /// </summary>
      public string PostID
      {
        get { return _postID; }
      }

      /// <summary>
      /// PageID Guid in string format
      /// </summary>
      public string PageID
      {
        get { return _pageID; }
      }

      /// <summary>
      /// Publish determines wheter or not a post will be marked as published by BlogEngine.
      /// </summary>
      public bool Publish
      {
        get { return _publish; }
      }

      /// <summary>
      /// Login for user validation
      /// </summary>
      public string UserName
      {
        get { return _userName; }
      }

      #endregion
        
      #region Private Methods

      /// <summary>
      /// Retrieves the content of the input stream
      /// and return it as plain text.
      /// </summary>
      private string ParseRequest(HttpContext context)
      {
        byte[] buffer = new byte[context.Request.InputStream.Length];
        context.Request.InputStream.Read(buffer, 0, buffer.Length);

        return System.Text.Encoding.UTF8.GetString(buffer);
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
          if (!(xml.StartsWith("<?xml") || xml.StartsWith("<method"))) 
          {
						xml = xml.Substring(xml.IndexOf("<?xml"));
          }
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
          case "wp.getAuthors":
          case "wp.getPageList":
          case "wp.getPages":
          case "wp.getTags":
            _blogID = _inputParams[0].InnerText;
            _userName = _inputParams[1].InnerText;
            _password = _inputParams[2].InnerText;
            break;
          case "metaWeblog.getRecentPosts":
            _blogID = _inputParams[0].InnerText;
            _userName = _inputParams[1].InnerText;
            _password = _inputParams[2].InnerText;
            _numberOfPosts = Int32.Parse(_inputParams[3].InnerText, CultureInfo.InvariantCulture);
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
          case "wp.newPage":
            _blogID = _inputParams[0].InnerText;
            _userName = _inputParams[1].InnerText;
            _password = _inputParams[2].InnerText;
            _page = GetPage(_inputParams[3]);
            if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
              _publish = false;
            else
              _publish = true;
            break;
          case "wp.getPage":
            _blogID = _inputParams[0].InnerText;
            _pageID = _inputParams[1].InnerText;
            _userName = _inputParams[2].InnerText;
            _password = _inputParams[3].InnerText;
            break;
          case "wp.editPage":
            _blogID = _inputParams[0].InnerText;
            _pageID = _inputParams[1].InnerText;
            _userName = _inputParams[2].InnerText;
            _password = _inputParams[3].InnerText;
            _page = GetPage(_inputParams[4]);
            if (_inputParams[5].InnerText == "0" || _inputParams[5].InnerText == "false")
              _publish = false;
            else
              _publish = true;
            break;
          case "wp.deletePage":
            _blogID = _inputParams[0].InnerText;
            _userName = _inputParams[1].InnerText;
            _password = _inputParams[2].InnerText;
            _pageID = _inputParams[3].InnerText;
            break;
          default:
            throw new MetaWeblogException("02", "Unknown Method. (" + _methodName + ")");

        }

      }

      /// <summary>
      /// Creates a Metaweblog Post object from the XML struct
      /// </summary>
      /// <param name="node">XML contains a Metaweblog Post Struct</param>
      /// <returns>Metaweblog Post Struct Obejct</returns>
      private MWAPost GetPost(XmlNode node)
      {
        MWAPost temp = new MWAPost();
        List<string> cats = new List<string>();
        List<string> tags = new List<string>();
        
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

        if (node.SelectSingleNode("value/struct/member[name='mt_allow_comments']") == null)
          temp.commentPolicy = "";
        else
          temp.commentPolicy = node.SelectSingleNode("value/struct/member[name='mt_allow_comments']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='mt_excerpt']") == null)
          temp.excerpt = "";
        else
          temp.excerpt = node.SelectSingleNode("value/struct/member[name='mt_excerpt']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='wp_slug']") == null)
          temp.slug = "";
        else
          temp.slug = node.SelectSingleNode("value/struct/member[name='wp_slug']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='wp_author_id']") == null)
            temp.author = "";
        else
            temp.author = node.SelectSingleNode("value/struct/member[name='wp_author_id']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='categories']") != null)
        {
          XmlNode categoryArray = node.SelectSingleNode("value/struct/member[name='categories']").LastChild;
          foreach (XmlNode catnode in categoryArray.SelectNodes("array/data/value/string"))
          {
            cats.Add(catnode.InnerText);
          }
        }
        temp.categories = cats;

        // postDate has a few different names to worry about
        if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
        {
            try
            {
                string tempDate = node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
                temp.postDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            catch
            {
                // Ignore PubDate Error
            }
        }
        else if (node.SelectSingleNode("value/struct/member[name='pubDate']") != null)
        {
          try
          {
            string tempPubDate = node.SelectSingleNode("value/struct/member[name='pubDate']").LastChild.InnerText;
            temp.postDate = DateTime.ParseExact(tempPubDate, "yyyyMMdd'T'HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
          }
          catch
          {
            // Ignore PubDate Error
          }
        }

        // WLW tags implementation using mt_keywords
        if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") != null)
        {
          string tagsList = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;
          foreach (string item in tagsList.Split(','))
          {
              if (string.IsNullOrEmpty(tags.Find(delegate(string t) { return t.Equals(item.Trim(), StringComparison.OrdinalIgnoreCase); })))
              {
                  tags.Add(item.Trim());
              }
          }
        }
        temp.tags = tags;

        return temp;
      }

      /// <summary>
      /// Creates a Metaweblog Page object from the XML struct
      /// </summary>
      /// <param name="node">XML contains a Metaweblog Page Struct</param>
      /// <returns>Metaweblog Page Struct Obejct</returns>
      private MWAPage GetPage(XmlNode node)
      {
        MWAPage temp = new MWAPage();

        // Require Title and Description
        try
        {
          temp.title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
          temp.description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
        }
        catch (Exception ex)
        {
          throw new MetaWeblogException("06", "Page Struct Element, Title or Description,  not Sent. (" + ex.Message + ")");
        }
        if (node.SelectSingleNode("value/struct/member[name='link']") == null)
          temp.link = "";
        else
          temp.link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
        {
          try
          {
            string tempDate = node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
            temp.pageDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
          }
          catch
          {
            // Ignore PubDate Error
          }
        }
        
        //Keywords
        if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") == null)
          temp.mt_keywords = "";
        else
          temp.mt_keywords = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;

        if (node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']") != null)
          temp.pageParentID = node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']").LastChild.InnerText;

        return temp;
      }

      /// <summary>
      /// Creates a Metaweblog Media object from the XML struct
      /// </summary>
      /// <param name="node">XML contains a Metaweblog MediaObject Struct</param>
      /// <returns>Metaweblog MediaObject Struct Obejct</returns>
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

      private void LogMetaWeblogCall(string message)
      {
          string saveFolder = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);
          string saveFile = Path.Combine(saveFolder, "lastmetaweblogcall.txt");

          try
          {
              // Save message to file
              using (FileStream fileWrtr = new FileStream(saveFile, FileMode.OpenOrCreate, FileAccess.Write))
              {
                  using (StreamWriter streamWrtr = new StreamWriter(fileWrtr))
                  {
                      streamWrtr.WriteLine(message);
                      streamWrtr.Close();
                  }
                  fileWrtr.Close();
              }
          }
          catch
          {
              // Ignore all errors
          }
          
      }

      #endregion

    }
}
