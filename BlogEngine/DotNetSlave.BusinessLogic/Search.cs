#region Using

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using BlogEngine.Core;
using System.Text.RegularExpressions;

#endregion

namespace BlogEngine.Core
{
  /// <summary>
  /// Searches the post collection and returns a result based on a search term.
  /// <remarks>It is used for related posts and the in-site search feature.</remarks>
  /// </summary>
  public static class Search
  {

    static Search()
    {
      BuildCatalog();
      Post.Saved += new EventHandler<SavedEventArgs>(Post_Saved);
      BlogSettings.Changed += new EventHandler<EventArgs>(BlogSettings_Changed);
    }

    #region Event handlers

    /// <summary>
    /// rebuilds the catalog of entries if the EnableCommentSearch setting have been changed.
    /// </summary>
    private static void BlogSettings_Changed(object sender, EventArgs e)
    {
      bool catalogContainComments = false;

      lock (_SyncRoot)
      {
        foreach (Entry entry in _Catalog)
        {
          if (!string.IsNullOrEmpty(entry.Comments))
          {
            catalogContainComments = true;
            break;
          }
        }
      }

      if (catalogContainComments != BlogSettings.Instance.EnableCommentSearch)
        BuildCatalog();
    }

    /// <summary>
    /// Adds a post to the catalog when it is added.
    /// </summary>
    private static void Post_Saved(object sender, SavedEventArgs e)
    {
      lock (_SyncRoot)
      {
        if (e.Action == SaveAction.Insert)
          AddPost(sender as Post);
      }
    }

    #endregion

    #region Search

    /// <summary>
    /// Searches all the posts and returns a ranked result set.
    /// </summary>
    /// <param name="searchTerm">The term to search for</param>
    /// <param name="includeComments">True to include a post's comments and their authors in search</param>
    public static List<Post> Hits(string searchTerm, bool includeComments)
    {
      lock (_SyncRoot)
      {
        List<Result> results = BuildResultSet(searchTerm, includeComments);
        List<Post> posts = results.ConvertAll(new Converter<Result, Post>(ResultToPost));
        return posts;
      }
    }

    /// <summary>
    /// Returns a list of posts that is related to the specified post.
    /// </summary>
    public static List<Post> FindRelatedPosts(Post post)
    {
      string term = post.Title;
      foreach (string tag in post.Tags)
      {
        term += " " + tag;
      }

      foreach (Category cat in post.Categories)
      {
        term += " " + cat.Title;
      }

      term = CleanContent(term, false);
      return Hits(term, false);
    }

    /// <summary>
    /// Builds the results set and ranks it.
    /// </summary>
    private static List<Result> BuildResultSet(string searchTerm, bool includeComments)
    {
      List<Result> results = new List<Result>();
      string term = CleanContent(searchTerm.ToLowerInvariant().Trim(), false);
      string[] terms = term.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      string regex = string.Format(System.Globalization.CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

      foreach (Entry entry in _Catalog)
      {
        Result result = new Result();
        int titleMatches = Regex.Matches(entry.Title, regex).Count;
        result.Rank = titleMatches * 20;

        int postMatches = Regex.Matches(entry.Content, regex).Count;
        result.Rank += postMatches;

        if (includeComments)
        {
          int commentMatches = Regex.Matches(entry.Comments, regex).Count;
          result.Rank += commentMatches;
        }

        if (result.Rank > 0)
        {
          result.Post = entry.Post;
          results.Add(result);
        }
      }

      results.Sort();
      return results;
    }

    /// <summary>
    /// A converter delegate used for converting Results to Posts.
    /// </summary>
    private static Post ResultToPost(Result result)
    {
      return result.Post;
    }

    #endregion

    #region Properties and private fields

    private readonly static object _SyncRoot = new object();
    private readonly static Regex _StripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);
    private readonly static StringCollection _StopWords = GetStopWords();
    private static Collection<Entry> _Catalog = new Collection<Entry>();

    #endregion

    #region BuildCatalog

    private static void BuildCatalog()
    {
      lock (_SyncRoot)
      {
        _Catalog.Clear();
        foreach (Post post in Post.Posts)
        {
          AddPost(post);
        }
      }
    }

    private static void AddPost(Post post)
    {
      Entry entry = new Entry();
      entry.Post = post;
      entry.Title = CleanContent(post.Title, false);
      entry.Content = HttpUtility.HtmlDecode(CleanContent(post.Content, true));

      if (BlogSettings.Instance.EnableCommentSearch)
        entry.Comments = GetCommentString(post);

      _Catalog.Add(entry);
    }

    /// <summary>
    /// Removes stop words and HTML from the specified string.
    /// </summary>
    private static string CleanContent(string content, bool removeHtml)
    {
      if (removeHtml)
        content = _StripHtml.Replace(content, string.Empty);

      content = content
                      .Replace("\\", string.Empty)
                      .Replace("|", string.Empty);

      string[] words = content.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < words.Length; i++)
      {
        string word = words[i].ToLowerInvariant().Trim();
        if (!_StopWords.Contains(word))
          sb.Append(word + " ");
      }

      return sb.ToString();
    }

    /// <summary>
    /// Retrieves the stop words from the stopwords.txt file located in App_Data.
    /// </summary>
    private static StringCollection GetStopWords()
    {
      using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation) + "stopwords.txt"))
      {
        string file = reader.ReadToEnd();
        string[] words = file.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        StringCollection col = new StringCollection();
        col.AddRange(words);

        return col;
      }
    }

    private static string GetCommentString(Post post)
    {
      StringBuilder sb = new StringBuilder();
      foreach (Comment comment in post.Comments)
      {
        sb.Append(comment.Content);
        sb.Append(" " + comment.Author);
      }

      return sb.ToString();
    }

    #endregion

  }

  #region Entry and Result structs

  /// <summary>
  /// A search optimized post object cleansed from HTML and stop words.
  /// </summary>
  internal struct Entry
  {
    /// <summary>The post object reference</summary>
    internal Post Post;
    /// <summary>The title of the post cleansed for stop words</summary>
    internal string Title;
    /// <summary>The content of the post cleansed for stop words and HTML</summary>
    internal string Content;
    /// <summary>All the comments and their authors in a single string.</summary>
    internal string Comments;
  }

  /// <summary>
  /// A result is a search result which contains a post and its ranking.
  /// </summary>
  internal struct Result : IComparable<Result>
  {
    /// <summary>
    /// The rank of the post based on the search term. The higher the rank, the higher the post is in the result set.
    /// </summary>
    internal int Rank;

    /// <summary>
    /// The post of the result.
    /// </summary>
    internal Post Post;

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value 
    /// has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero 
    /// This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    public int CompareTo(Result other)
    {
      return other.Rank.CompareTo(Rank);
    }
  }

  #endregion

}
