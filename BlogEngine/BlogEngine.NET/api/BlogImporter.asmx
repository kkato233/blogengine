<%@ WebService Language="C#" Class="BlogImporter" %>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;

using BlogEngine.Core;

/// <summary>
///     Web Service API for Blog Importer
/// </summary>
[WebService(Namespace = "http://dotnetblogengine.net/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class BlogImporter : WebService
{
    #region Constants and Fields

    public AuthHeader AuthenticationHeader;

    #endregion

    #region Public Methods

    /// <summary>
    ///     Add Comment to specified post
    /// </summary>
    /// <param name = "postId">postId as string</param>
    /// <param name = "author">commenter username</param>
    /// <param name = "email">commenter email</param>
    /// <param name = "website">commenter url</param>
    /// <param name = "description">actual comment</param>
    /// <param name = "date">comment datetime</param>
    [SoapHeader("AuthenticationHeader")]
    [WebMethod]
    public void AddComment(
        string postId, string author, string email, string website, string description, DateTime date)
    {
        if (!this.IsAuthenticated())
        {
            throw new InvalidOperationException("Wrong credentials");
        }

        //Post post = Post.GetPost(new Guid(postID));
        var post = Post.Load(new Guid(postId));
        if (post == null)
        {
            return;
        }

        var comment = new Comment { Id = Guid.NewGuid(), Author = author, Email = email };
        Uri url;
        if (Uri.TryCreate(website, UriKind.Absolute, out url))
        {
            comment.Website = url;
        }

        comment.Content = description;
        comment.DateCreated = date;
        comment.Parent = post;
        comment.IsApproved = true;
        post.ImportComment(comment);
        post.Import();
    }

    /// <summary>
    ///     Add new blog post to system
    /// </summary>
    /// <param name = "import">ImportPost object</param>
    /// <param name = "previousUrl">Old Post Url (for Url re-writing)</param>
    /// <param name = "removeDuplicate">Search for duplicate post and remove?</param>
    /// <returns>string containing unique post identifier</returns>
    [SoapHeader("AuthenticationHeader")]
    [WebMethod]
    public string AddPost(ImportPost import, string previousUrl, bool removeDuplicate)
    {
        if (!this.IsAuthenticated())
        {
            throw new InvalidOperationException("Wrong credentials");
        }

        if (removeDuplicate)
        {
            if (!Post.IsTitleUnique(import.Title))
            {
                // Search for matching post (by date and title) and delete it
                foreach (var temp in
                    Post.GetPostsByDate(import.PostDate.AddDays(-2), import.PostDate.AddDays(2)).Where(
                        temp => temp.Title == import.Title))
                {
                    temp.Delete();
                    temp.Import();
                }
            }
        }

        var post = new Post
            {
                Title = import.Title,
                Author = import.Author,
                DateCreated = import.PostDate,
                DateModified = import.PostDate,
                Content = import.Content,
                Description = import.Description,
                Published = import.Publish
            };
        //TODO: Save Previous Url?

        AddCategories(import.Categories, post);

        //Tag Support:
        //No tags. Use categories. 
        post.Tags.AddRange(import.Tags.Count == 0 ? import.Categories : import.Tags);

        post.Import();

        return post.Id.ToString();
    }

    /// <summary>
    ///     Relative File Handler path
    /// </summary>
    /// <returns>file handler path as string</returns>
    [WebMethod]
    public string BlogFileHandler()
    {
        return "file.axd?file=";
    }

    /// <summary>
    ///     Relative Image Handler path
    /// </summary>
    /// <returns>image handler path as string</returns>
    [WebMethod]
    public string BlogImageHandler()
    {
        return "image.axd?picture=";
    }

    /// <summary>
    ///     Name/Type of Blog Software
    /// </summary>
    /// <returns>Blog Software name</returns>
    [WebMethod]
    public string BlogType()
    {
        return "BlogEngine.NET";
    }

    /// <summary>
    ///     Version Number of the Blog
    /// </summary>
    /// <returns>Version number in string</returns>
    [WebMethod]
    public string BlogVersion()
    {
        return BlogSettings.Instance.Version();
    }

    /// <summary>
    ///     Force Reload of all posts
    /// </summary>
    [SoapHeader("AuthenticationHeader")]
    [WebMethod]
    public void ForceReload()
    {
        if (!this.IsAuthenticated())
        {
            throw new InvalidOperationException("Wrong credentials");
        }

        Post.Reload();
    }

    /// <summary>
    ///     Downloads specified file to specified location
    /// </summary>
    /// <param name = "source">source file path</param>
    /// <param name = "destination">relative destination path</param>
    /// <returns></returns>
    [SoapHeader("AuthenticationHeader")]
    [WebMethod]
    public bool GetFile(string source, string destination)
    {
        bool response;
        try
        {
            var rootPath = string.Format("{0}files/", BlogSettings.Instance.StorageLocation);
            var serverPath = this.Server.MapPath(rootPath);
            var saveFolder = serverPath;
            var fileName = destination;

            // Check/Create Folders & Fix fileName
            if (fileName.LastIndexOf('/') <= -1 && saveFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                saveFolder = saveFolder.Substring(0, saveFolder.Length - 1);
            }
            else
            {
                saveFolder += fileName.Substring(0, fileName.LastIndexOf('/'));
                saveFolder = saveFolder.Replace('/', Path.DirectorySeparatorChar);

                fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            }

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            saveFolder += Path.DirectorySeparatorChar;

            using (var client = new WebClient())
            {
                client.DownloadFile(source, saveFolder + fileName);
            }
            response = true;
        }
        catch (Exception)
        {
            // The file probably didn't exist. No action needed.
            response = false;
        }

        return response;
    }

    #endregion

    #region Methods

    private static void AddCategories(IEnumerable<string> categories, IPublishable post)
    {
        try
        {
            foreach (var category in categories)
            {
                var added = false;
                var category1 = category;
                foreach (var cat in
                    Category.Categories.Where(cat => cat.Title.Equals(category1, StringComparison.OrdinalIgnoreCase)))
                {
                    post.Categories.Add(cat);
                    added = true;
                }

                if (added)
                {
                    continue;
                }

                var newCat = new Category(category, string.Empty);
                newCat.Save();
                post.Categories.Add(newCat);
            }
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
    }

    private bool IsAuthenticated()
    {
        return Membership.ValidateUser(this.AuthenticationHeader.Username, this.AuthenticationHeader.Password);
    }

    #endregion

    public class AuthHeader : SoapHeader
    {
        #region Constants and Fields

        public string Password;

        public string Username;

        #endregion
    }

    public class ImportPost
    {
        #region Constants and Fields

        public string Author;

        public Collection<string> Categories;

        public string Content;

        public string Description;

        public DateTime PostDate;

        public bool Publish;

        public Collection<string> Tags;

        public string Title;

        #endregion
    }
}

