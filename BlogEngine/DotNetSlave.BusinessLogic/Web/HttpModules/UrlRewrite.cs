#region Using

using System;
using System.Text;
using System.Web;
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
      if (context.Request.RawUrl.ToLowerInvariant().Contains(".aspx") && !context.Request.RawUrl.Contains("error404.aspx"))
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
          string author = ExtractTitle(context, "/author/");
          context.RewritePath("~/default.aspx?name=" + author + GetQueryString(context), false);
        }
        //else if (context.Request.RawUrl.ToLowerInvariant().Contains("/tag.aspx/"))
        //{
        //  string tag = ExtractTitle(context, "/tag.aspx/");
        //  context.RewritePath("~/default.aspx?name=" + tag + GetQueryString(context), false);
        //}
      }
    }

    private static void RewritePost(HttpContext context)
    {
      string slug = ExtractTitle(context, "/post/");
      Post post = Post.GetPostBySlug(slug);
      if (post != null)
        context.RewritePath("~/post.aspx?id=" + post.Id.ToString() + GetQueryString(context), false);
    }

      //todo:  Need to rewrite for Category BO
    private static void RewriteCategory(HttpContext context)
    {
      string title = ExtractTitle(context, "/category/");
        foreach (Category cat in Category.Categories)
        {
            string legalTitle = Utils.RemoveIlegalCharacters(cat.Title).ToLowerInvariant();
            if (title.Equals(legalTitle, StringComparison.OrdinalIgnoreCase))
            {
                context.RewritePath("~/default.aspx?id=" + cat.Id.ToString() + GetQueryString(context), false);
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

    /// <summary>
    /// Extracts the title from the requested URL.
    /// </summary>
    private static string ExtractTitle(HttpContext context, string lookFor)
    {
      int index = context.Request.RawUrl.ToLowerInvariant().LastIndexOf(lookFor) + lookFor.Length;
      int stop = context.Request.RawUrl.ToLowerInvariant().LastIndexOf(".aspx");
      string title = context.Request.RawUrl.Substring(index, stop - index).Replace(".aspx", string.Empty);
      return context.Server.UrlEncode(title);
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