#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Xml;
using System.IO;
using System.Text;
using BlogEngine.Core;

#endregion

public partial class widgets_RecentPosts_widget : WidgetBase
{

	private const int DEFAULT_NUMBER_OF_POSTS = 10;
	private const bool DEFAULT_SHOW_COMMENTS = true;
	private const bool DEFAULT_SHOW_RATING = true;

	static widgets_RecentPosts_widget()
	{
		Post.Saved += delegate { HttpRuntime.Cache.Remove("widget_recentposts"); };
		Post.CommentAdded += delegate { HttpRuntime.Cache.Remove("widget_recentposts"); };
		Post.CommentRemoved += delegate { HttpRuntime.Cache.Remove("widget_recentposts"); };
		Post.Rated += delegate { HttpRuntime.Cache.Remove("widget_recentposts"); };
		BlogSettings.Changed += delegate { HttpRuntime.Cache.Remove("widget_recentposts"); };
	}

	public override void LoadWidget()
	{
		if (HttpRuntime.Cache["widget_recentposts"] == null)
		{
			StringDictionary settings = (StringDictionary)GetSettings(ObjectType.StringDictionary);
			
			int numberOfPosts = DEFAULT_NUMBER_OF_POSTS;
			if (settings.ContainsKey("numberofposts"))
				numberOfPosts = int.Parse(settings["numberofposts"]);

			List<Post> visiblePosts = Post.Posts.FindAll(delegate(Post p)
			{
				return p.IsVisible;
			});

			int max = Math.Min(visiblePosts.Count, numberOfPosts);
			List<Post> list = visiblePosts.GetRange(0, max);
			RenderPosts(list, settings);
		}

		LiteralControl html = new LiteralControl((string)HttpRuntime.Cache["widget_recentposts"]);
		phPosts.Controls.Add(html);
	}

	private void RenderPosts(List<Post> posts, StringDictionary settings)
	{
		if (posts.Count == 0)
		{
			HttpRuntime.Cache.Insert("widget_recentposts", "<p>" + Resources.labels.none + "</p>");
			return;
		}

		StringBuilder sb = new StringBuilder();
		sb.Append("<ul class=\"recentPosts\" id=\"recentPosts\">");

		foreach (Post post in posts)
		{
			if (!post.IsVisible)
				continue;

			string rating = Math.Round(post.Rating, 1).ToString(System.Globalization.CultureInfo.InvariantCulture);

			string link = "<li><a href=\"{0}\">{1}</a>{2}{3}</li>";
			string comments = string.Format("<span>{0}: {1}</span>", Resources.labels.comments, post.ApprovedComments.Count);
			string rate = string.Format("<span>{0}: {1} / {2}</span>", Resources.labels.rating, rating, post.Raters);

			bool showComments = DEFAULT_SHOW_COMMENTS;
			bool showRating = DEFAULT_SHOW_RATING;
			if (settings.ContainsKey("showcomments") && settings["showcomments"].Equals("false", StringComparison.OrdinalIgnoreCase))
				showComments = false;

			if (settings.ContainsKey("showrating") && settings["showrating"].Equals("false", StringComparison.OrdinalIgnoreCase))
				showRating = false;

			if (!showComments || !BlogSettings.Instance.IsCommentsEnabled)
				comments = null;

			if (!showRating || !BlogSettings.Instance.EnableRating)
				rate = null;

			if (post.Raters == 0)
				rating = Resources.labels.notRatedYet;

			sb.AppendFormat(link, post.RelativeLink, HttpUtility.HtmlEncode(post.Title), comments, rate);
		}

		sb.Append("</ul>");
		HttpRuntime.Cache.Insert("widget_recentposts", sb.ToString());
	}

	public override string Name
	{
		get { return "RecentPosts"; }
	}

	public override bool IsEditable
	{
		get { return true; }
	}

}
