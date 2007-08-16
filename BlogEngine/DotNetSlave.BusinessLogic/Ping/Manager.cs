using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace BlogEngine.Core.Ping
{
  /// <summary>
  /// 
  /// </summary>
  public static class Manager
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="post"></param>
    public static void Send(Post post)
    {
      foreach (Uri url in GetUrlsFromContent(post.Content))
      {
        string rdfContents = ReadFromWeb(url);
        Uri urlToNotifyTrackback = GetTrackBackLinkFromText(rdfContents);

        if (urlToNotifyTrackback != null)
        {
          TrackbackMessage tMessage = new TrackbackMessage(post, urlToNotifyTrackback);
          bool isTrackbackSent = Trackback.Send(tMessage);
          if (!isTrackbackSent)
          {
            Pingback.Send(post.AbsoluteLink, url);
          }
        }
      }
    }

    #region "RegEx Methods"

    private static readonly Regex urlsRegex = new Regex(@"\<a\s+href=""(http://.*?)"".*\>.+\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex trackbackLinkRegex = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static List<Uri> GetUrlsFromContent(string content)
    {
      List<Uri> urlsList = new List<Uri>();
      foreach (Match myMatch in urlsRegex.Matches(content))
      {
        string url = myMatch.Groups[1].ToString().Trim();
        Uri uri;
        if (Uri.TryCreate(url, UriKind.Absolute, out uri))
          urlsList.Add(uri);
      }

      return urlsList;
    }

    private static Uri GetTrackBackLinkFromText(string input)
    {
      string url = trackbackLinkRegex.Match(input).Groups[1].ToString().Trim();
      Uri uri;

      if (Uri.TryCreate(url, UriKind.Absolute, out uri))
        return uri;
      else
        return null;
    }
    #endregion
    /// <summary>
    /// Returns the HTML code of a given URL.
    /// </summary>
    /// <param name="sourceUrl">The URL you want to extract the html code.</param>
    /// <returns></returns>
    private static string ReadFromWeb(Uri sourceUrl)
    {
      string html;
      using (WebClient client = new WebClient())
      {
        html = client.DownloadString(sourceUrl);
      }
      return html;
    }
  }
}
