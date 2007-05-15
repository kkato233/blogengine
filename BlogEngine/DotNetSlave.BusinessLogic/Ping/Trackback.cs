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
        /// SendTrackbackMessage
        /// </summary>
        /// <param name="urlToNotifiedTrackback"></param>
        /// <param name="tMessage"></param>
        /// <returns>The Response of the trackbackHandler in the other End(urltoNotified)</returns>
        public static bool Send(string urlToNotifiedTrackback, TrackbackMessage tMessage)
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

            //request.BeginGetResponse(EndGetResponse, request);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //todo:we need to parse the XML response for errors in the trackback XML response
                    return true;
                }
                else
                {
                    return false;
                }
                //----------------------------------------------------
            }
            catch //(WebException wex)
            {
                return false;
            }

            return false;
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
        /// <param name="title"></param>
        /// <param name="postUrl"></param>
        /// <param name="excerpt"></param>
        /// <param name="blogName"></param>
        public TrackbackMessage(string title, string postUrl, string excerpt, string blogName)
        {
            Title = title;
            PostURL = new Uri(postUrl);
            Excerpt = excerpt;
            BlogName = blogName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("title={0}&url={1}&excerpt={2}&blog_name={3}", Title, PostURL, Excerpt, BlogName);
        }
    }

}
