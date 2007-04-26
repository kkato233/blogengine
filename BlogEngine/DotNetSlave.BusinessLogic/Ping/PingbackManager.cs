using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BlogEngine.Core.Ping
{
    public class PingbackManager
    {
        public PingbackManager()
        { }

        public void Send(string postTitle, string postContent, string postURL, string excerpt, string blogName)
        {
            foreach (string url in GetUrlsFromContent(postContent))
            {
                TrackbackMessage tMessage = new TrackbackMessage(postTitle, postURL, excerpt, blogName);
                //Go to external site and get TrackBack RDF code/def
                string rdfContents = ReadFromWeb(url);
                string urlToNotifiedTrackback = GetTrackBackLinkFromText(rdfContents);
                if (!string.IsNullOrEmpty(urlToNotifiedTrackback.Trim()))
                {
                    SendTrackbackMessage(urlToNotifiedTrackback, tMessage);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlToNotifiedTrackback"></param>
        /// <param name="tMessage"></param>
        /// <returns>The Response of the trackbackHandler in the other End(urltoNotified)</returns>
        private void SendTrackbackMessage(string urlToNotifiedTrackback, TrackbackMessage tMessage)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(urlToNotifiedTrackback); //HttpHelper.CreateRequest(trackBackItem);
            request.Method = "POST";
            request.ContentLength = tMessage.ToString().Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;
            request.Timeout = 3000;

            using (StreamWriter myWriter = new StreamWriter(request.GetRequestStream()))
            {
                myWriter.Write(tMessage.ToString());
            }

            request.BeginGetResponse(EndGetResponse, request);
        }

        /// <summary>
        /// Receives the response.
        /// </summary>
        private void EndGetResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            
            HttpWebResponse response = null;

            //done: if the URL mispelled,error the request.GetResponse raise and WebException - 404 not found
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                string answer;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    answer = sr.ReadToEnd();
                    sr.Close();
                }

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    //strResult = "OK";
                //}
                //else
                //{
                //    //strResult = "ERROR";
                //}
                //----------------------------------------------------
            }
            catch (WebException wex)
            {
                //todo: Log 404 errors?
                //throw;
            }   
        }

        /// <summary>
        /// Returns the HTML code of a given URL.
        /// </summary>
        /// <param name="sourceUrl">The URL you want to extract the html code.</param>
        /// <returns></returns>
        private string ReadFromWeb(string sourceUrl)
        {
            string html;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(sourceUrl);
            }
            return html;
        }


        #region "RegEx Methods"
   
        private static readonly Regex urlsRegex = new Regex(@"\<a\s+href=""(http://.*?)"".*\>.+\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
   
        private static readonly Regex trackbackLinkRegex = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      
        private List<string> GetUrlsFromContent(string content)
        {
            List<string> urlsList = new List<string>();
            foreach (Match myMatch in urlsRegex.Matches(content))
            {
                urlsList.Add(myMatch.Groups[1].ToString().Trim());
            }

            return urlsList;
        }

        private string GetTrackBackLinkFromText(string input)
        {
            return trackbackLinkRegex.Match(input).Groups[1].ToString();
        }

        #endregion
    }

    struct TrackbackMessage
    {
        public string Title;
        public Uri PostURL;
        public string Excerpt;
        public string BlogName;

        public TrackbackMessage(string title, string postUrl, string excerpt, string blogName)
        {
            Title = title;
            PostURL = new Uri(postUrl);
            Excerpt = excerpt;
            BlogName = blogName;
        }

        public override string ToString()
        {
            return String.Format("title={0}&url={1}&excerpt={2}&blog_name={3}", Title, PostURL, Excerpt, BlogName);
        }
    }

}
