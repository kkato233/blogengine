using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BlogEngine.Core.Ping
{
    /// <summary>
    /// 
    /// </summary>
    public class Trackback
    {
        /// <summary>
        /// 
        /// </summary>
        public Trackback()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tMessage"></param>
        /// <returns></returns>    
        public static bool Send(TrackbackMessage tMessage)
        {

            //Warning:next line if for local debugging porpuse please donot remove it until you need to
            //tMessage.PostURL = new Uri("http://www.artinsoft.com/webmaster/trackback.html");
            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(tMessage.URLToNotifyTrackback); //HttpHelper.CreateRequest(trackBackItem);
            request.Method = "POST";
            request.ContentLength = tMessage.ToString().Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;
            request.Timeout = 10000;

            using (StreamWriter myWriter = new StreamWriter(request.GetRequestStream()))
            {
                myWriter.Write(tMessage.ToString());
            }

            bool result = false;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                string answer;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    answer = sr.ReadToEnd();
                    sr.Close();
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //todo:This could be a strict XML parsing if necesary/maybe logging activity here too
                    if (answer.Contains("<error>0</error>"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;    
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch //(WebException wex)
            {
                result = false;
            }
            return result;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct TrackbackMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title;

        /// <summary>
        /// 
        /// </summary>
        public Uri PostURL;

        /// <summary>
        /// 
        /// </summary>
        public string Excerpt;

        /// <summary>
        /// 
        /// </summary>
        public string BlogName;

        /// <summary>
        /// 
        /// </summary>
        public Uri URLToNotifyTrackback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="urlToNotifyTrackback"></param>
        public TrackbackMessage(Post post, string urlToNotifyTrackback)
        {
            Title = post.Title;
            PostURL = post.AbsoluteLink;
            Excerpt = post.Title;
            BlogName = BlogSettings.Instance.Name;
            URLToNotifyTrackback = new Uri(urlToNotifyTrackback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(URLToNotifyTrackback.Query))
            {
                return String.Format("?title={0}&url={1}&excerpt={2}&blog_name={3}", Title, PostURL, Excerpt, BlogName);
            }
            else
            {
                return String.Format("&title={0}&url={1}&excerpt={2}&blog_name={3}", Title, PostURL, Excerpt, BlogName);
            }
        }
    }
}