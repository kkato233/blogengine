<%@ WebService Language="C#" Class="RssImporter" %>

using System;
using System.Net;
using System.Web;
using System.Collections.ObjectModel;
using System.Web.Services;
using System.Web.Security;
using System.Web.Services.Protocols;
using System.Text.RegularExpressions;
using DotNetSlave.BlogEngine.BusinessLogic;

[WebService(Namespace = "http://madskristensen.dk/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class RssImporter : System.Web.Services.WebService
{

  [SoapHeader("AuthenticationHeader")]
  [WebMethod]
  public Guid CreatePost(string author, string title, string content, Collection<string> categories, DateTime dateCreated, string fileLocation, bool downloadFiles)
  {
    if (!IsAuthenticated())
      throw new InvalidOperationException("Wrong credentials");

    content = ChangeFiles(content, fileLocation, downloadFiles);
    content = ChangeImages(content, fileLocation, downloadFiles);

    if (Post.IsTitleUnique(title))
    {
      Post post = new Post();
      post.Author = author;
      post.Title = title;
      post.Content = content;
      post.DateCreated = dateCreated;

      AddCategories(categories, post);
      post.Save();
      return post.Id;
    }

    return Guid.Empty;
  }

  [SoapHeader("AuthenticationHeader")]
  [WebMethod]
  public void AddComment(Guid postId, string author, string email, string website, string description, DateTime date)
  {
    if (!IsAuthenticated())
      throw new InvalidOperationException("Wrong credentials");
    
    Post post = Post.GetPost(postId);
    if (post != null)
    {
      Comment comment = new Comment();
      comment.Author = author;
      comment.Email = email;
      Uri url;
      if (Uri.TryCreate(website, UriKind.Absolute, out url))
        comment.Website = url;
      comment.Content = description;
      comment.DateCreated = date;
      post.Comments.Add(comment);
      post.Save();
    }
  }

  private bool IsAuthenticated()
  {
    return Membership.ValidateUser(AuthenticationHeader.Username, AuthenticationHeader.Password);
  }

  public AuthHeader AuthenticationHeader;

  public class AuthHeader : SoapHeader
  {
    public string Username;
    public string Password;
  }

  private static void AddCategories(Collection<string> categories, Post post)
  {
    foreach (string category in categories)
    {
      if (CategoryDictionary.Instance.ContainsValue(category))
      {
        foreach (Guid key in CategoryDictionary.Instance.Keys)
        {
          if (CategoryDictionary.Instance[key].Equals(category, StringComparison.OrdinalIgnoreCase))
          {
            post.Categories.Add(key);
          }
        }
      }
      else
      {
        Guid id = CategoryDictionary.Instance.Add(category);
        CategoryDictionary.Instance.Save();
        post.Categories.Add(id);
      }
    }
  }

  private string ChangeImages(string content, string fileLocation, bool downloadFiles)
  {
    Regex regex = new Regex("src=\\\"(.*)\\\"", RegexOptions.IgnoreCase);

    foreach (Match match in regex.Matches(content))
    {
      string src = match.Groups[1].Value;
      int index = src.IndexOf(fileLocation) + fileLocation.Length;
      if (index > fileLocation.Length)
      {
        if (IsImage(src.ToLowerInvariant()))
        {
          content = content.Replace(src, VirtualPathUtility.ToAbsolute("~/") + "image.axd?picture=" + src.Substring(index));
          if (downloadFiles)
            DownloadFile(src);
        }
      }
    }

    return content;
  }

  private string ChangeFiles(string content, string fileLocation, bool downloadFiles)
  {
    Regex regex = new Regex("href=\\\"(.*)\\\"", RegexOptions.IgnoreCase);

    foreach (Match match in regex.Matches(content))
    {
      string src = match.Groups[1].Value;
      int index = src.IndexOf(fileLocation) + fileLocation.Length;
      if (index > fileLocation.Length)
      {
        if (!IsImage(src.ToLowerInvariant()))
        {
          content = content.Replace(src, VirtualPathUtility.ToAbsolute("~/") + "file.axd?file=" + src.Substring(index));
          if (downloadFiles)
            DownloadFile(src);
        }
      }
    }

    return content;
  }

  private bool IsImage(string text)
  {
    return text.Contains(".jpg") || text.Contains(".gif") || text.Contains(".png");
  }

  private void DownloadFile(string url)
  {
    try
    {
      using (WebClient client = new WebClient())
      {
        int index = url.LastIndexOf("/") + 1;
        client.DownloadFile(url, Server.MapPath(BlogSettings.Instance.StorageLocation) + "files/" + url.Substring(index));
      }
    }
    catch (Exception)
    {
      // The file probably didn't exist. No action needed.
    }
  }

}