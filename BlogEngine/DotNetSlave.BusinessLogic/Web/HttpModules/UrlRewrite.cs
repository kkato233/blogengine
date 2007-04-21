#region Using

using System;
using System.Text;
using System.Web;
using System.Collections.Generic;
using BlogEngine.Core.Entities;

#endregion

namespace BlogEngine.Core.Web.HttpModules
{
  /// <summary>
  /// Summary description for UrlRewrite
  /// </summary>
  public class UrlRewrite : IHttpModule
  {
    #region IHttpModule Members

    public void Dispose()
    {
      // Nothing to dispose
    }

    public void Init(HttpApplication context)
    {
      context.BeginRequest += new EventHandler(context_BeginRequest);
    }

    #endregion

    private void context_BeginRequest(object sender, EventArgs e)
    {
      HttpContext context = ((HttpApplication)sender).Context;
      if (context.Request.RawUrl.ToLowerInvariant().Contains(".aspx"))
      {
        if (context.Request.RawUrl.ToLowerInvariant().Contains("/post/"))
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
          string author = ExtractTitle(context, "/author/");
          context.RewritePath("~/default.aspx?name=" + author + GetQueryString(context), false);
        }
        else if (context.Request.RawUrl.ToLowerInvariant().Contains("/tag/"))
        {
          string tag = ExtractTitle(context, "/tag/");
          context.RewritePath("~/default.aspx?name=" + tag + GetQueryString(context), false);
        }
      }
    }

    private static void RewritePost(HttpContext context)
    {
      string title = ExtractTitle(context, "/post/");
      Post post = Post.GetPostByName(title);
      if (post != null)
        context.RewritePath("~/post.aspx?id=" + post.Id.ToString() + GetQueryString(context), false);
    }

    private static void RewriteCategory(HttpContext context)
    {
      string title = ExtractTitle(context, "/category/");
      foreach (KeyValuePair<Guid, string> entry in CategoryDictionary.Instance)
      {
        string legalTitle = Utils.RemoveIlegalCharacters(entry.Value).ToLowerInvariant();
        if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          context.RewritePath("~/default.aspx?id=" + entry.Key.ToString() + GetQueryString(context), false);
          break;
        }
      }
    }

    private static void RewritePage(HttpContext context)
    {
      string title = ExtractTitle(context, "/page/");
      foreach (Page page in Page.Pages)
      {
        string legalTitle = Utils.RemoveIlegalCharacters(page.Title).ToLowerInvariant();
        if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
        {
          context.RewritePath("~/page.aspx?id=" + page.Id + GetQueryString(context), false);
          break;
        }
      }
    }

    public static string ExtractTitle(HttpContext context, string lookFor)
    {
      int index = context.Request.RawUrl.ToLowerInvariant().LastIndexOf(lookFor) + lookFor.Length;
      int stop = context.Request.RawUrl.ToLowerInvariant().LastIndexOf(".aspx");
      string title = context.Request.RawUrl.Substring(index, stop - index).Replace(".aspx", string.Empty).ToLowerInvariant();
      return title;
    }

    public static string GetQueryString(HttpContext context)
    {
      string query = context.Request.QueryString.ToString();
      if (!string.IsNullOrEmpty(query))
        return "&" + query;

      return string.Empty;
    }
  }
}