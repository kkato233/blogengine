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
			string url = context.Request.RawUrl.ToUpperInvariant();

      if (url.Contains(BlogSettings.Instance.FileExtension.ToUpperInvariant()) && !url.Contains("ERROR404.ASPX"))
      {
        if (context.Request.Path.Contains("/blog.aspx"))
        {
          context.RewritePath(Utils.RelativeWebRoot + "default.aspx?blog=true");
        }
        else if (url.Contains("/POST/"))
        {
          RewritePost(context);
        }
        else if (url.Contains("/CATEGORY/"))
        {
          RewriteCategory(context);
        }
        else if (url.Contains("/PAGE/"))
        {
          RewritePage(context);
        }
				else if (url.Contains("/CALENDAR/"))
				{
					context.RewritePath(Utils.RelativeWebRoot + "default.aspx?calendar=show", false);
				}
				else if (url.EndsWith("DEFAULT.ASPX"))
				{
					RewriteDefault(context);
				}
				else if (url.Contains("/AUTHOR/"))
				{
					string author = ExtractTitle(context);
					context.RewritePath(Utils.RelativeWebRoot + "default.aspx?name=" + author + GetQueryString(context), false);
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
        context.RewritePath(Utils.RelativeWebRoot + "post.aspx?id=" + post.Id.ToString() + GetQueryString(context), false);
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
          context.RewritePath(Utils.RelativeWebRoot + "default.aspx?id=" + cat.Id.ToString() + GetQueryString(context), false);
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
          context.RewritePath(Utils.RelativeWebRoot + "page.aspx?id=" + page.Id + GetQueryString(context), false);
          break;
        }
      }
    }

		private static readonly Regex YEAR = new Regex("/([0-9][0-9][0-9][0-9])/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex YEAR_MONTH = new Regex("/([0-9][0-9][0-9][0-9])/([0-1][0-9])/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex YEAR_MONTH_DAY = new Regex("/([0-9][0-9][0-9][0-9])/([0-1][0-9])/([0-3][0-9])/", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static void RewriteDefault(HttpContext context)
		{
			string url = context.Request.RawUrl;
			if (YEAR_MONTH_DAY.IsMatch(url))
			{
				Match match = YEAR_MONTH_DAY.Match(url);
				string year = match.Groups[1].Value;
				string month = match.Groups[2].Value;
				string day = match.Groups[3].Value;
				string date = year + "-" + month + "-" + day;
				context.RewritePath(Utils.RelativeWebRoot + "default.aspx?date=" + date, false);
			}
			else if (YEAR_MONTH.IsMatch(url))
			{
				Match match = YEAR_MONTH.Match(url);
				string year = match.Groups[1].Value;
				string month = match.Groups[2].Value;
				string path = string.Format("default.aspx?year={0}&month={1}", year, month);
				context.RewritePath(Utils.RelativeWebRoot + path, false);
			}
			else if (YEAR.IsMatch(url))
			{
				Match match = YEAR.Match(url);
				string year = match.Groups[1].Value;
				string path = string.Format("default.aspx?year={0}", year);
				context.RewritePath(Utils.RelativeWebRoot + path, false);
			}
		}

    /// <summary>
    /// Extracts the title from the requested URL.
    /// </summary>
    private static string ExtractTitle(HttpContext context)
    {
			string url = context.Request.RawUrl.ToLowerInvariant();
			if (url.Contains(BlogSettings.Instance.FileExtension) && url.EndsWith("/"))
			{
				url = url.Substring(0, url.Length - 1);
				context.Response.AppendHeader("location", url);
				context.Response.StatusCode = 301;
			}

			url = url.Substring(0, url.IndexOf(BlogSettings.Instance.FileExtension));
			int index = url.LastIndexOf("/") + 1;
			string title = url.Substring(index);
      return context.Server.UrlEncode(title);
    }

    private static readonly Regex _Regex = new Regex("/([0-9][0-9][0-9][0-9])/([01][0-9])/", RegexOptions.Compiled);
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