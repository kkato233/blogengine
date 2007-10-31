#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
using BlogEngine.Core;

#endregion

namespace BlogEngine.Core.Web.HttpHandlers
{
	/// <summary>
	/// Implements a custom handler to synchronously process HTTP Web requests for a syndication feed.
	/// </summary>
	/// <remarks>
	/// This handler can generate syndication feeds in a variety of formats and filtering 
	/// options based on the query string parmaeters provided.
	/// </remarks>
	/// <seealso cref="IHttpHandler"/>
	/// <seealso cref="SyndicationGenerator"/>
	public class SyndicationHandler : IHttpHandler
	{

		#region IHttpHandler Members

		/// <summary>
		/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
		public bool IsReusable
		{
			get { return false; }
		}

		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements 
		/// the <see cref="T:System.Web.IHttpHandler"></see> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references 
		/// to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.
		/// </param>
		public void ProcessRequest(HttpContext context)
		{
			string title = RetrieveTitle(context);
			SyndicationFormat format = RetrieveFormat(context);
			List<IPublishable> list = GenerateItemList(context);

			if (context.Request.QueryString["post"] == null)
			{
				// Shorten the list to the number of posts stated in the settings, except for the comment feed.
				int max = Math.Min(BlogSettings.Instance.PostsPerFeed, list.Count);
				list = list.GetRange(0, max);
			}

			SetHeaderInformation(context, list, format);
			SyndicationGenerator generator = new SyndicationGenerator(BlogSettings.Instance, Category.Categories);
			generator.WriteFeed(format, context.Response.OutputStream, list, title);			
		}

		#endregion

		#region Retrieve feed items

		/// <summary>
		/// Generates the list of feed items based on the URL.
		/// </summary>
		private static List<IPublishable> GenerateItemList(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.QueryString["category"]))
			{
				// All posts in the specified category
				Guid categoryId = new Guid(context.Request.QueryString["category"]);
				return Post.GetPostsByCategory(categoryId).ConvertAll(new Converter<Post, IPublishable>(ConvertToIPublishable));
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["author"]))
			{
				// All posts by the specified author
				string author = context.Request.QueryString["author"];
				return Post.GetPostsByAuthor(author).ConvertAll(new Converter<Post, IPublishable>(ConvertToIPublishable));
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["post"]))
			{
				// All comments of the specified post
				Post post = Post.GetPost(new Guid(context.Request.QueryString["post"]));
				return post.Comments.ConvertAll(new Converter<Comment, IPublishable>(ConvertToIPublishable));
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["comments"]))
			{
				// The recent comments added to any post.
				return RecentComments();
			}

			// The latest posts
			return Post.Posts.ConvertAll(new Converter<Post, IPublishable>(ConvertToIPublishable));
		}

		/// <summary>
		/// Creates a list of the most recent comments
		/// </summary>
		private static List<IPublishable> RecentComments()
		{
			List<Comment> temp = new List<Comment>();

			foreach (Post post in Post.Posts)
			{
				foreach (Comment comment in post.ApprovedComments)
				{
					temp.Add(comment);
				}
			}

			temp.Sort();
			temp.Reverse();
			List<Comment> list = new List<Comment>();

			foreach (Comment comment in temp)
			{
				list.Add(comment);
			}

			return list.ConvertAll(new Converter<Comment, IPublishable>(ConvertToIPublishable));
		}

		#endregion

		#region Helper methods

		/// <summary>
		/// A converter delegate used for converting Results to Posts.
		/// </summary>
		private static IPublishable ConvertToIPublishable(IPublishable item)
		{
			return item;
		}

		/// <summary>
		/// Retrieves the syndication format from the urL parameters.
		/// </summary>
		private static SyndicationFormat RetrieveFormat(HttpContext context)
		{
			string query = context.Request.QueryString["format"];
			string format = BlogSettings.Instance.SyndicationFormat;
			if (!string.IsNullOrEmpty(query))
			{
				format = context.Request.QueryString["format"];
			}

			return (SyndicationFormat)Enum.Parse(typeof(SyndicationFormat), format, true);
		}

		private static string RetrieveTitle(HttpContext context)
		{
			string title = BlogSettings.Instance.Name;
			string subTitle = null;

			if (!string.IsNullOrEmpty(context.Request.QueryString["category"]))
			{
				Guid categoryId = new Guid(context.Request.QueryString["category"]);
				subTitle = Category.GetCategory(categoryId).Title;
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["author"]))
			{
				subTitle = context.Request.QueryString["author"];
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["post"]))
			{
				Post post = Post.GetPost(new Guid(context.Request.QueryString["post"]));
				if (post == null)
				{
					context.Response.StatusCode = 404;
					context.Response.Clear();
					context.Response.End();
				}
				subTitle = post.Title;
			}

			if (!string.IsNullOrEmpty(context.Request.QueryString["comments"]))
			{
				subTitle = "Comments";
			}

			if (subTitle != null)
				return title + " - " + subTitle;

			return title;
		}

		/// <summary>
		/// Sets the response header information.
		/// </summary>
		/// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, <b>Request</b>, <b>Response</b>, <b>Session</b>, and <b>Server</b>) used to service HTTP requests.</param>
		/// <param name="items">The collection of <see cref="IPublishable"/> instances used when setting the response header details.</param>
		/// <param name="format">The format of the syndication feed being generated.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="context"/> is a null reference (Nothing in Visual Basic) -or- the <paramref name="posts"/> is a null reference (Nothing in Visual Basic).</exception>
		private static void SetHeaderInformation(HttpContext context, List<IPublishable> items, SyndicationFormat format)
		{
			DateTime lastModified = DateTime.Now;
			bool notModified = false;
			string eTag = context.Request.Headers["If-None-Match"];
			string ifModifiedSince = context.Request.Headers["if-modified-since"];

			if (items.Count > 0)
			{
				lastModified = items[0].DateModified;
			}

			if (!String.IsNullOrEmpty(eTag))
			{
				notModified = eTag.Equals(lastModified.Ticks.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				if (!String.IsNullOrEmpty(ifModifiedSince))
				{
					// ifModifiedSince can have a length param in there
					// If-Modified-Since: Wed, 29 Dec 2004 18:34:27 GMT; length=126275
					if (ifModifiedSince.IndexOf(";", StringComparison.Ordinal) > -1)
					{
						ifModifiedSince = ifModifiedSince.Split(';').GetValue(0).ToString();
					}

					DateTime ifModifiedDate;
					if (DateTime.TryParse(ifModifiedSince, out ifModifiedDate))
					{
						notModified = (lastModified <= ifModifiedDate);
					}
				}
			}

			if (notModified)
			{
				context.Response.StatusCode = 304;
				context.Response.SuppressContent = true;
				context.Response.End();
			}
			else
			{
				context.Response.Cache.SetCacheability(HttpCacheability.Public);
				context.Response.Cache.SetLastModified(DateTime.Now);
				context.Response.Cache.SetETag(lastModified.Ticks.ToString(CultureInfo.InvariantCulture));

				switch (format)
				{
					case SyndicationFormat.Atom:
						context.Response.ContentType = "application/atom+xml";
						context.Response.AppendHeader("Content-Disposition", "inline; filename=atom.xml");
						break;

					case SyndicationFormat.Rss:
						context.Response.ContentType = "application/rss+xml";
						context.Response.AppendHeader("Content-Disposition", "inline; filename=rss.xml");
						break;
				}
			}
		}

		#endregion

	}
}
