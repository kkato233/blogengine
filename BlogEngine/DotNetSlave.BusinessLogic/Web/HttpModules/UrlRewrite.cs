#region Using

using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpModules
{
  /// <summary>
  /// Handles pretty URL's and redirects them to the permalinks.
  /// </summary>
  public class UrlRewrite : IHttpModule
  {
    #region IHttpModule Members

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
      // Nothing to dispose
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void Init(HttpApplication context)
    {
      context.BeginRequest += new EventHandler(context_BeginRequest);
    }

    #endregion

    /// <summary>
    /// Handles the BeginRequest event of the context control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void context_BeginRequest(object sender, EventArgs e)
    {
      HttpContext context = ((HttpApplication)sender).Context;
      if (context.Request.RawUrl.ToLowerInvariant().Contains(BlogSettings.Instance.FileExtension) && !context.Request.RawUrl.Contains("error404.aspx"))
      {
        if (context.Request.Path.Contains("/blog.aspx"))
        {
          context.RewritePath("~/default.aspx?blog=true");
        }
        else if (context.Request.RawUrl.ToLowerInvariant().Contains("/post/"))
        {
          RewritePost(context);
        }
        else if (context.Request.RawUrl.ToLowerInvariant().Contains("/category/"))
        {
          RewriteCategory(context);
        }
        else if (context.Request.RawUrl.ToLowerInvariant().Contains("/page/"))
        {
          RewritePage(context);
        }
        else if (context.Request.RawUrl.ToLowerInvariant().Contains("/author/"))
        {
          string author = ExtractTitle(context);
          context.RewritePath("~/default.aspx?name=" + author + GetQueryString(context), false);
        }
      }
    }

    private static void RewritePost(HttpContext context)
    {
      DateTime stamp = ExtractDate(context);
      string slug = ExtractTitle(context);
      Post post = Post.GetPostBySlug(slug, stamp);

      if (post != null)
      {
        context.RewritePath("~/post.aspx?id=" + post.Id.ToString() + GetQueryString(context), false);
      }
    }    

    //todo:  Need to rewrite for Category BO
    private static void RewriteCategory(HttpContext context)
    {
      string title = ExtractTitle(context);
      foreach (Category cat in Category.Categories)
      {
        string legalTitle = Utils.RemoveIllegalCharacters(cat.Title).ToLowerInvariant();
        if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          context.RewritePath("~/default.aspx?id=" + cat.Id.ToString() + GetQueryString(context), false);
          break;
        }
      }
    }

    private static void RewritePage(HttpContext context)
    {
      string title = ExtractTitle(context);
      foreach (Page page in Page.Pages)
      {
        string legalTitle = Utils.RemoveIllegalCharacters(page.Title).ToLowerInvariant();
        if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          context.RewritePath("~/page.aspx?id=" + page.Id + GetQueryString(context), false);
          break;
        }
      }
    }

    /// <summary>
    /// Extracts the title from the requested URL.
    /// </summary>
    private static string ExtractTitle(HttpContext context)
    {
      int index = context.Request.RawUrl.ToLowerInvariant().LastIndexOf("/") + 1;
      int stop = context.Request.RawUrl.ToLowerInvariant().LastIndexOf(BlogSettings.Instance.FileExtension);
      string title = context.Request.RawUrl.Substring(index, stop - index).Replace(BlogSettings.Instance.FileExtension, string.Empty);
      return context.Server.UrlEncode(title);
    }

    private static Regex _Regex = new Regex("/([0-9][0-9][0-9][0-9])/([01][0-9])/", RegexOptions.Compiled);
    /// <summary>
    /// Extracts the year and month from the requested URL and returns that as a DateTime.
    /// </summary>
    private static DateTime ExtractDate(HttpContext context)
    {
      if (!BlogSettings.Instance.TimeStampPostLinks)
        return DateTime.MinValue;

      Match match = _Regex.Match(context.Request.RawUrl);
      if (match.Success)
      {
        int year = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        int month = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        return new DateTime(year, month, 1);
      }

      return DateTime.MinValue;
    }

    /// <summary>
    /// Gets the query string from the requested URL.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    private static string GetQueryString(HttpContext context)
    {
      string query = context.Request.QueryString.ToString();
      if (!string.IsNullOrEmpty(query))
        return "&" + query;

      return string.Empty;
    }
  }
}