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
            foreach (string url in GetUrlsFromContent(post.Content))
            {
                TrackbackMessage tMessage = new TrackbackMessage(post);
                //Go to external site and get TrackBack RDF code/def
                string rdfContents = ReadFromWeb(url);
                string urlToNotifiedTrackback = GetTrackBackLinkFromText(rdfContents);
                if (!string.IsNullOrEmpty(urlToNotifiedTrackback.Trim()))
                {
                    //old:code:SendTrackbackMessage(urlToNotifiedTrackback, tMessage);
                    bool isTrackbackSent = Trackback.Send(urlToNotifiedTrackback, tMessage);
                    if (!isTrackbackSent)
                    {
                      Pingback.Send(post.AbsoluteLink.ToString(), url);
                    }
                }
            }
        }

        #region "RegEx Methods"

        private static readonly Regex urlsRegex = new Regex(@"\<a\s+href=""(http://.*?)"".*\>.+\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex trackbackLinkRegex = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static List<string> GetUrlsFromContent(string content)
        {
            List<string> urlsList = new List<string>();
            foreach (Match myMatch in urlsRegex.Matches(content))
            {
                urlsList.Add(myMatch.Groups[1].ToString().Trim());
            }

            return urlsList;
        }

        private static string GetTrackBackLinkFromText(string input)
        {
            return trackbackLinkRegex.Match(input).Groups[1].ToString();
        }

        /// <summary>
        /// Returns the HTML code of a given URL.
        /// </summary>
        /// <param name="sourceUrl">The URL you want to extract the html code.</param>
        /// <returns></returns>
        private static string ReadFromWeb(string sourceUrl)
        {
            string html;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(sourceUrl);
            }
            return html;
        }

        #endregion


    }
}
