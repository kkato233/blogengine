namespace BlogEngine.Core.Ping
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;

    /// <summary>
    /// The trackback.
    /// </summary>
    public static class Trackback
    {
        #region Events

        /// <summary>
        ///     Occurs just before a trackback is sent.
        /// </summary>
        public static event EventHandler<EventArgs> Sending;

        /// <summary>
        ///     Occurs when a trackback has been sent
        /// </summary>
        public static event EventHandler<EventArgs> Sent;

        #endregion

        #region Public Methods

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <returns>
        /// The send.
        /// </returns>
        public static bool Send(TrackbackMessage message)
        {
            if (!BlogSettings.Instance.EnableTrackBackSend)
            {
                return false;
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            OnSending(message.UrlToNotifyTrackback);

            // Warning:next line if for local debugging porpuse please donot remove it until you need to
            // tMessage.PostURL = new Uri("http://www.artinsoft.com/webmaster/trackback.html");
            var request = (HttpWebRequest)WebRequest.Create(message.UrlToNotifyTrackback);
                
                // HttpHelper.CreateRequest(trackBackItem);
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Method = "POST";
            request.ContentLength = message.ToString().Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;
            request.Timeout = 10000;

            using (var myWriter = new StreamWriter(request.GetRequestStream()))
            {
                myWriter.Write(message.ToString());
            }

            var result = false;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                OnSent(message.UrlToNotifyTrackback);
                string answer;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    answer = sr.ReadToEnd();
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // todo:This could be a strict XML parsing if necesary/maybe logging activity here too
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
            catch
            {
                // (WebException wex)
                result = false;
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on sending.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        private static void OnSending(Uri url)
        {
            if (Sending != null)
            {
                Sending(url, new EventArgs());
            }
        }

        /// <summary>
        /// The on sent.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        private static void OnSent(Uri url)
        {
            if (Sent != null)
            {
                Sent(url, new EventArgs());
            }
        }

        #endregion
    }

    /// <summary>
    /// The trackback message.
    /// </summary>
    public class TrackbackMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackbackMessage"/> class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="urlToNotifyTrackback">
        /// The URL to notify trackback.
        /// </param>
        /// <param name="itemUrl">
        /// The item Url.
        /// </param>
        public TrackbackMessage(IPublishable item, Uri urlToNotifyTrackback, Uri itemUrl)
        {
            if (item == null)
            {
                throw new ArgumentNullException("post");
            }

            this.Title = item.Title;
            this.PostUrl = itemUrl;
            this.Excerpt = item.Title;
            this.BlogName = BlogSettings.Instance.Name;
            this.UrlToNotifyTrackback = urlToNotifyTrackback;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the blog.
        /// </summary>
        /// <value>The name of the blog.</value>
        public string BlogName { get; set; }

        /// <summary>
        ///     Gets or sets the excerpt.
        /// </summary>
        /// <value>The excerpt.</value>
        public string Excerpt { get; set; }

        /// <summary>
        ///     Gets or sets the post URL.
        /// </summary>
        /// <value>The post URL.</value>
        public Uri PostUrl { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the URL to notify trackback.
        /// </summary>
        /// <value>The URL to notify trackback.</value>
        public Uri UrlToNotifyTrackback { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "title={0}&url={1}&excerpt={2}&blog_name={3}", 
                this.Title, 
                this.PostUrl, 
                this.Excerpt, 
                this.BlogName);
        }

        #endregion
    }
}