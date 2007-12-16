#region Using

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using BlogEngine.Core;

#endregion

public partial class _default : BlogEngine.Core.Web.Controls.BlogBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Page.IsCallback)
			return;

		Page frontPage = BlogEngine.Core.Page.GetFrontPage();
		if (Request.QueryString.Count == 0 && frontPage != null)
		{
			Server.Transfer(Utils.RelativeWebRoot + "page.aspx?id=" + frontPage.Id);
		}
		else if (Request.RawUrl.ToLowerInvariant().Contains("/category/"))
		{
			DisplayCategories();
		}
		else if (Request.RawUrl.ToLowerInvariant().Contains("/author/"))
		{
			DisplayAuthors();
		}
		else if (Request.RawUrl.ToLowerInvariant().Contains("?tag="))
		{
			DisplayTags();
		}
		else if (Request.PathInfo.StartsWith("/CALENDAR", StringComparison.OrdinalIgnoreCase))
		{
			calendar.Visible = true;
			PostList1.Visible = false;
			Title = Server.HtmlEncode(BlogSettings.Instance.Name);
		}
		else if (Request.PathInfo.Length > 6 && Request.PathInfo.Length < 9 && YEAR_MONTH.IsMatch(Request.PathInfo))
		{
			Match match = YEAR_MONTH.Match(Request.PathInfo);
			string year = match.Groups[1].Value;
			string month = match.Groups[2].Value;
			DisplayDateRange(year, month, null);
		}
		else if (Request.PathInfo.Length > 9 && YEAR_MONTH_DAY.IsMatch(Request.PathInfo))
		{
			Match match = YEAR_MONTH_DAY.Match(Request.PathInfo);
			string year = match.Groups[1].Value;
			string month = match.Groups[2].Value;
			string day = match.Groups[3].Value;
			DisplayDateRange(year, month, day);
		}
		else if (Request.QueryString["year"] != null || Request.QueryString["date"] != null || Request.QueryString["calendar"] != null)
		{
			DisplayDateRange();
		}
		else if (Request.QueryString.Count == 0 || !string.IsNullOrEmpty(Request.QueryString["page"]) || !string.IsNullOrEmpty(Request.QueryString["theme"]) || !string.IsNullOrEmpty(Request.QueryString["blog"]))
		{
			PostList1.Posts = Post.Posts;
			Page.Title = Server.HtmlEncode(BlogSettings.Instance.Name);
			if (!string.IsNullOrEmpty(BlogSettings.Instance.Description))
				Page.Title += " - " + Server.HtmlEncode(BlogSettings.Instance.Description);
		}

		AddMetaKeywords();
		base.AddMetaTag("description", Server.HtmlEncode(BlogSettings.Instance.Description));
		base.AddMetaTag("author", Server.HtmlEncode(BlogSettings.Instance.AuthorName));
	}

	private static readonly Regex YEAR_MONTH = new Regex("/([0-9][0-9][0-9][0-9])/([0-1][0-9])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	private static readonly Regex YEAR_MONTH_DAY = new Regex("/([0-9][0-9][0-9][0-9])/([0-1][0-9])/([0-3][0-9])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	/// <summary>
	/// Adds the post's tags as meta keywords.
	/// </summary>
	private void AddMetaKeywords()
	{
		if (Category.Categories.Count > 0)
		{
			string[] categories = new string[Category.Categories.Count];
			for (int i = 0; i < Category.Categories.Count; i++)
			{
				categories[i] = Category.Categories[i].Title;
			}
			base.AddMetaTag("keywords", Server.HtmlEncode(string.Join(",", categories)));
		}
	}

	private void DisplayCategories()
	{
		if (!String.IsNullOrEmpty(Request.QueryString["id"]))
		{
			Guid categoryId = new Guid(Request.QueryString["id"]);
			PostList1.Posts = Post.GetPostsByCategory(categoryId);
			Page.Title = BlogSettings.Instance.Name + " - " + Category.GetCategory(categoryId);
		}
	}

	private void DisplayAuthors()
	{
		if (!string.IsNullOrEmpty(Request.QueryString["name"]))
		{
			PostList1.Posts = Post.GetPostsByAuthor(Request.QueryString["name"]); ;
			Title = BlogSettings.Instance.Name + " - All posts by " + Request.QueryString["name"];
		}
	}

	private void DisplayTags()
	{
		if (!string.IsNullOrEmpty(Request.QueryString["tag"]))
		{
			PostList1.Posts = Post.GetPostsByTag(Request.QueryString["tag"].Substring(1)); ;
			base.Title = BlogSettings.Instance.Name + " - All posts tagged '" + Request.QueryString["tag"].Substring(1) + "'";
			base.AddMetaTag("description", Server.HtmlEncode(BlogSettings.Instance.Description));
		}
	}

	[Obsolete("Use DisplayDateRange(year, month, day) instead")]
	private void DisplayDateRange()
	{
		string year = Request.QueryString["year"];
		string month = Request.QueryString["month"];
		string specificDate = Request.QueryString["date"];

		if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
		{
			DisplayDateRange(year, month, null);
		}
		else if (!string.IsNullOrEmpty(specificDate) && specificDate.Length == 10)
		{
			DateTime date = DateTime.Parse(specificDate, CultureInfo.InvariantCulture);
			DisplayDateRange(date.Year.ToString(), date.Month.ToString(), date.Day.ToString());
			//PostList1.Posts = Post.GetPostsByDate(date, date);
			//Title = BlogSettings.Instance.Name + " - " + date.ToString("MMMM d. yyyy");
		}
		else if (!string.IsNullOrEmpty(Request.QueryString["calendar"]))
		{
			calendar.Visible = true;
			PostList1.Visible = false;
			Title = Server.HtmlEncode(BlogSettings.Instance.Name);
		}
	}

	private void DisplayDateRange(string year, string month, string day)
	{
		if (string.IsNullOrEmpty(day))
		{
			DateTime dateFrom = DateTime.Parse(year + "-" + month + "-01", CultureInfo.InvariantCulture);
			DateTime dateTo = dateFrom.AddMonths(1).AddMilliseconds(-1);
			PostList1.Posts = Post.GetPostsByDate(dateFrom, dateTo);
			Title = BlogSettings.Instance.Name + " - " + dateFrom.ToString("MMMM yyyy");
		}
		else
		{
			DateTime date = DateTime.Parse(year + "-" + month + "-" + day, CultureInfo.InvariantCulture);
			PostList1.Posts = Post.GetPostsByDate(date, date);
			Title = BlogSettings.Instance.Name + " - " + date.ToString("MMMM d. yyyy");
		}
	}

}
