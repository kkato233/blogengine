using System;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using BlogEngine.Core;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Search
/// </summary>
public static class Search
{
  #region Fields

  // remove all whitespace
  private static Regex _regexWhitespace = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

  //Remove all html tags
  private static Regex _regexAll = new Regex("<(.|\n)*?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

  //Word splitter
  private static char[] _splitter = new char[1] { ' ' };

  //Stopwords (will give a cleaner result) TODO: add more...
  private static string[] _stopWords = GetStopWords(); //new string[6] { "a", "I", "the", "that", "there", "are" };

  //The index collection
  private static Collection<Catalog> _catalogIndex = new Collection<Catalog>();

  //The result collection
  private static List<Result> _res = new List<Result>();

  #endregion

  #region DataStructures

  /// <summary>
  /// The PointerList is used to link between the post and a single word
  /// </summary>
  public struct PointerList
  {
    /// <summary>
    /// Reference to the post (or is it?)
    /// </summary>
    public Post post;

    //Reserved for futher search terms...
  }

  /// <summary>
  /// The Catalog hold word pointers 
  /// </summary>
  public struct Catalog
  {
    public string _word;
    public Collection<PointerList> _pointers;

    public Catalog(string word)
    {
      _word = word;
      _pointers = new Collection<PointerList>();
    }

    public Collection<PointerList> Adding
    {
      get
      {
        return _pointers;
      }
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">Another object to compare to.</param>
    /// <returns>
    /// true if obj and this instance are the same type and represent the same value; otherwise, false.
    /// </returns>
    public override bool Equals(object obj)
    {
      if (_word.GetHashCode() == obj.GetHashCode())
        return true;
      else
        return false;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode()
    {
      return _word.GetHashCode();
    }

    /// <summary>
    /// Returns the fully qualified type name of this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> containing a fully qualified type name.
    /// </returns>
    public override string ToString()
    {
      return _word;
    }
  }

  public struct Result
  {
    public Post _post;
    public int rank;

    public Result(Post post)
    {
      _post = post;
      rank = 0;
    }

    public override bool Equals(object obj)
    {
      if (_post.GetHashCode() == obj.GetHashCode())
        return true;
      else
        return false;
    }

    public static bool operator ==(Result a, Result b)
    {
      return (a == b && b == a);
    }

    public static bool operator !=(Result a, Result b)
    {
      return !(a == b);
    }


    public override int GetHashCode()
    {
      return _post.GetHashCode();
    }

    public override string ToString()
    {
      return _post.ToString();
    }
  }

  #endregion

  private static bool _IsBuilingCatalog;

  public static List<Post> Hits(List<Post> postsToSearch, string searchTerm, bool includeComments)
  {
    //_catalogIndex.Clear();
    _res.Clear();

    //Pass.1 - First we build the full text catalog
    if (_catalogIndex.Count == 0 && !_IsBuilingCatalog)
    {
      _IsBuilingCatalog = true;
      ThreadPool.QueueUserWorkItem(BuildCatalog); ;// BuildCatalog(postsToSearch);    
    }

    if (_catalogIndex.Count == 0)
      return new List<Post>();

    //Pass.2
    BuildResult(searchTerm);

    //Pass.3
    List<Post> _gotit = new List<Post>();
    _res.Sort(CompareByRank);
    _res.Reverse();
    foreach (Result res in _res)
      _gotit.Add(res._post);

    return _gotit;
  }

  /// <summary>
  /// Builds the result.
  /// </summary>
  /// <param name="query">The query.</param>
  private static void BuildResult(string query)
  {
    string[] words = query.Split(_splitter);

    foreach (string word in words)
    {
      foreach (Catalog catalog in _catalogIndex)
      {
        if (catalog.ToString() == word)
        {
          foreach (PointerList pl in catalog._pointers)
          {
            Result r = new Result(pl.post);
            r.rank = 1;

            if (!_res.Contains(r))
            {
              _res.Add(r);
            }
            else
            {
              int sr = _res[_res.IndexOf(r)].rank;
              _res.Remove(r); // THIS IS A SLOW FUCKER
              sr++;
              r.rank = sr;
              _res.Add(r);
            }
          }
        }
      }
    }
  }

  /// <summary>
  /// Builds the catalog.
  /// </summary>
  /// <param name="postsToSearch">The posts to search.</param>
  private static void BuildCatalog(object stateInfo)
  {
    //To build the index we must first loop every post
    foreach (Post post in Post.Posts)
    {
      //Add what you would like to be able to search for
      string tempContent = post.Title + " " + post.Content;

      //1. remove all HTML - TODO: remove javascript here...
      tempContent = _regexAll.Replace(tempContent, String.Empty);

      //2. compress all whitespace to one space
      tempContent = _regexWhitespace.Replace(tempContent, " ");

      //3. ensure no space
      tempContent = tempContent.Trim();

      //split every word
      string[] words = tempContent.Split(_splitter);

      foreach (string word in words)
      {
        if (!IsStopWord(word))
        {
          Catalog catalog = new Catalog(word);
          PointerList pointerlist = new PointerList();
          pointerlist.post = post;

          if (!_catalogIndex.Contains(catalog))
          {
            catalog.Adding.Add(pointerlist);
            _catalogIndex.Add(catalog);
          }
          else
          {
            _catalogIndex[_catalogIndex.IndexOf(catalog)].Adding.Add(pointerlist);
          }
        }
      }
    }

    _IsBuilingCatalog = false;
  }

  /// <summary>
  /// Determines whether [is stop word] [the specified word].
  /// </summary>
  /// <param name="word">The word.</param>
  /// <returns>
  /// 	<c>true</c> if [is stop word] [the specified word]; otherwise, <c>false</c>.
  /// </returns>
  private static bool IsStopWord(string word)
  {
    foreach (string w in _stopWords)
    {
      if (w == word)
        return true;
    }

    return false;
  }

  private static string[] GetStopWords()
  {
    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation) + "stopwords.txt"))
    {
      string words = reader.ReadToEnd();
      return words.Split(new char[] { '\n','\r' }, StringSplitOptions.RemoveEmptyEntries);
    }    
  }

  /// <summary>
  /// Compares the by rank.
  /// </summary>
  /// <param name="x">The x.</param>
  /// <param name="y">The y.</param>
  /// <returns></returns>
  private static int CompareByRank(Result x, Result y)
  {
    if (x == null)
    {
      if (y == null)
      {
        return 0; // If x is null and y is null, they're equal. 
      }
      else
      {
        return -1; // If x is null and y is not null, y is greater. 
      }
    }
    else
    {
      // If x is not null...
      if (y == null) // ...and y is null, x is greater.
      {
        return 1;
      }
      else
      {
        // ...and y is not null, compare the rank of the two results.
        int retval = x.rank.CompareTo(y.rank);

        if (retval != 0)
        {
          // If the rank are not of equal rank, the bigger rank is greater.
          return retval;
        }
        else
        {
          // If the strings are of equal length, sort them with ordinary string comparison.
          return x.rank.CompareTo(y.rank);
        }
      }
    }
  }
}
