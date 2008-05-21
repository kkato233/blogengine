#region Using

using System;
using System.IO;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Threading;

#endregion

namespace BlogEngine.Core
{
    /// <summary>
    /// Utilities for the entire solution to use.
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Strips all illegal characters from the specified title.
        /// </summary>
        public static string RemoveIllegalCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace(".", string.Empty);
						text = text.Replace(",", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace(" ", "-");
            text = RemoveDiacritics(text);
            text = RemoveExtraHyphen(text);

            return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
        }

        private static string RemoveExtraHyphen(string text)
        {
            if (text.Contains("--"))
            {
                text = text.Replace("--", "-");
                return RemoveExtraHyphen(text);
            }

            return text;
        }

        private static String RemoveDiacritics(string text)
        {
            String normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < normalized.Length; i++)
            {
                Char c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        private static readonly Regex STRIP_HTML = new Regex("<[^>]*>", RegexOptions.Compiled);
        /// <summary>
        /// Strips all HTML tags from the specified string.
        /// </summary>
        /// <param name="html">The string containing HTML</param>
        /// <returns>A string without HTML tags</returns>
        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return STRIP_HTML.Replace(html, string.Empty);
        }

				/// <summary>
				/// Writes ETag and Last-Modified headers and sets the conditional get headers.
				/// </summary>
				/// <param name="date">The date.</param>
				public static void SetConditionalGetHeaders(DateTime date)
				{
					HttpResponse response = HttpContext.Current.Response;
					HttpRequest request = HttpContext.Current.Request;

					string etag = "\"" + date.Ticks + "\"";
					string incomingEtag = request.Headers["If-None-Match"];

					if (String.Compare(incomingEtag, etag) == 0)
					{
						response.Clear();
						response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
						response.End();
					}

					response.AppendHeader("ETag", etag);
				}

        #region URL handling

        /// <summary>
        /// Gets the relative URL of the blog feed. If a Feedburner username
        /// is entered in the admin settings page, it will return the 
        /// absolute Feedburner URL to the feed.
        /// </summary>
        public static string FeedUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(BlogSettings.Instance.AlternateFeedUrl))
                    return BlogSettings.Instance.AlternateFeedUrl;
                else
                    return AbsoluteWebRoot + "syndication.axd";
            }
        }

			private static string _RelativeWebRoot;
        /// <summary>
        /// Gets the relative root of the website.
        /// </summary>
        /// <value>A string that ends with a '/'.</value>
        public static string RelativeWebRoot
        {
            get 
						{
							if (_RelativeWebRoot == null)
							_RelativeWebRoot = VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["BlogEngine.VirtualPath"]);

							return  _RelativeWebRoot;
						}
        }

        //private static Uri _AbsoluteWebRoot;

        /// <summary>
        /// Gets the absolute root of the website.
        /// </summary>
        /// <value>A string that ends with a '/'.</value>
        public static Uri AbsoluteWebRoot
        {
            get
            {
								//if (_AbsoluteWebRoot == null)
								//{
                    HttpContext context = HttpContext.Current;
                    if (context == null)
                        throw new System.Net.WebException("The current HttpContext is null");

											if (context.Items["absoluteurl"] == null)
												context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

											return context.Items["absoluteurl"] as Uri;
											//_AbsoluteWebRoot = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);// new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + RelativeWebRoot);
                //}
                //return _AbsoluteWebRoot;
            }
        }

        /// <summary>
        /// Converts a relative URL to an absolute one.
        /// </summary>
        public static Uri ConvertToAbsolute(Uri relativeUri)
        {
            return ConvertToAbsolute(relativeUri.ToString()); ;
        }

        /// <summary>
        /// Converts a relative URL to an absolute one.
        /// </summary>
        public static Uri ConvertToAbsolute(string relativeUri)
        {
            if (String.IsNullOrEmpty(relativeUri))
                throw new ArgumentNullException("relativeUri");

            string absolute = AbsoluteWebRoot.ToString();
            int index = absolute.LastIndexOf(RelativeWebRoot.ToString());

            return new Uri(absolute.Substring(0, index) + relativeUri);
        }

        /// Retrieves the subdomain from the specified URL.
        /// </summary>
        /// <param name="url">The URL from which to retrieve the subdomain.</param>
        /// <returns>The subdomain if it exist, otherwise null.</returns>
        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }

            return null;
        }

        #endregion

        #region Is mobile device

        private static readonly Regex MOBILE_REGEX = new Regex(ConfigurationManager.AppSettings.Get("BlogEngine.MobileDevices"), RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets a value indicating whether the client is a mobile device.
        /// </summary>
        /// <value><c>true</c> if this instance is mobile; otherwise, <c>false</c>.</value>
        public static bool IsMobile
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    if (request.Browser.IsMobileDevice)
                        return true;

                    if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                        return true;
                }

                return false;
            }
        }

        #endregion

        #region Is Mono/Linux

        private static int mono = 0;
        /// <summary>
        /// Gets a value indicating whether we're running under Mono.
        /// </summary>
        /// <value><c>true</c> if Mono; otherwise, <c>false</c>.</value>
        public static bool IsMono
        {
            get
            {
                if (mono == 0)
                {
                    if (Type.GetType("Mono.Runtime") != null)
                        mono = 1;
                    else
                        mono = 2;
                }

                return mono == 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we're running under Linux or a Unix variant.
        /// </summary>
        /// <value><c>true</c> if Linux/Unix; otherwise, <c>false</c>.</value>
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return ((p == 4) || (p == 128));
            }
        }

        #endregion

        #region Send e-mail

        /// <summary>
        /// Sends a MailMessage object using the SMTP settings.
        /// </summary>
        public static void SendMailMessage(MailMessage message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            try
            {
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                SmtpClient smtp = new SmtpClient(BlogSettings.Instance.SmtpServer);
                smtp.Credentials = new System.Net.NetworkCredential(BlogSettings.Instance.SmtpUserName, BlogSettings.Instance.SmtpPassword);
                smtp.Port = BlogSettings.Instance.SmtpServerPort;
                smtp.EnableSsl = BlogSettings.Instance.EnableSsl;
                smtp.Send(message);
                OnEmailSent(message);
            }
            catch (SmtpException)
            {
                OnEmailFailed(message);
            }
            finally
            {
                // Remove the pointer to the message object so the GC can close the thread.
                message.Dispose();
                message = null;
            }
        }

        /// <summary>
        /// Sends the mail message asynchronously in another thread.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void SendMailMessageAsync(MailMessage message)
        {
            //ThreadStart threadStart = delegate { Utils.SendMailMessage(message); };
            //Thread thread = new Thread(threadStart);
            //thread.IsBackground = true;
            //thread.Start();
            ThreadPool.QueueUserWorkItem(delegate { Utils.SendMailMessage(message); });
        }

        /// <summary>
        /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailSent;
        private static void OnEmailSent(MailMessage message)
        {
            if (EmailSent != null)
            {
                EmailSent(message, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailFailed;
        private static void OnEmailFailed(MailMessage message)
        {
            if (EmailFailed != null)
            {
                EmailFailed(message, new EventArgs());
            }
        }

        #endregion



        #region FileHelpers

        //This was written by Mike Hilden and I found it at http://vbcity.com/forums/topic.asp?tid=19980
         // Usage: 
        // Copy Recursive with Overwrite if exists. 
        // RecursiveDirectoryCopy("C:\Data", "D:\Data", True, True) 
        // Copy Recursive without Overwriting. 
        // RecursiveDirectoryCopy("C:\Data", "D:\Data", True, False) 
        // Copy this directory Only. Overwrite if exists. 
        // RecursiveDirectoryCopy("C:\Data", "D:\Data", False, True) 
        // Copy this directory only without overwriting. 
        // RecursiveDirectoryCopy("C:\Data", "D:\Data", False, False) 
        // Recursively copy all files and subdirectories from the specified source to the specified 
        // destination. 
        public static void RecursiveDirectoryCopy(string sourceDir, string destDir, bool fRecursive, bool overWrite)
        {
           //string sDir;
            string tmp;
            System.IO.DirectoryInfo dDirInfo;
            System.IO.DirectoryInfo sDirInfo;
           // string sFile;
            System.IO.FileInfo sFileInfo;
            System.IO.FileInfo dFileInfo;
            // Add trailing separators to the supplied paths if they don't exist. 
            if (!sourceDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                sourceDir += System.IO.Path.DirectorySeparatorChar; 
            }
            if (!destDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                destDir += System.IO.Path.DirectorySeparatorChar;
            }
            //If destination directory does not exist, create it. 
            dDirInfo = new System.IO.DirectoryInfo(destDir);
            if (dDirInfo.Exists == false)
                dDirInfo.Create();
            dDirInfo = null;
            // Recursive switch to continue drilling down into directory structure. 
            if (fRecursive)
            {
                // Get a list of directories from the current parent. 
                foreach (string sDir in System.IO.Directory.GetDirectories(sourceDir))
                {
                    sDirInfo = new System.IO.DirectoryInfo(sDir);
                    dDirInfo = new System.IO.DirectoryInfo(destDir + sDirInfo.Name);
                    // Create the directory if it does not exist. 
                    if (dDirInfo.Exists == false)
                        dDirInfo.Create();
                    // Since we are in recursive mode, copy the children also 
                    RecursiveDirectoryCopy(sDirInfo.FullName, dDirInfo.FullName, fRecursive, overWrite);
                    sDirInfo = null;
                    dDirInfo = null;
                }
            }
            // Get the files from the current parent. 
            foreach (string sFile in System.IO.Directory.GetFiles(sourceDir))
            {
                sFileInfo = new System.IO.FileInfo(sFile);
                dFileInfo = new System.IO.FileInfo(sFile.Replace(sourceDir, destDir));
                //If File does not exist. Copy. 
                if (dFileInfo.Exists == false)
                {
                    sFileInfo.CopyTo(dFileInfo.FullName, overWrite);
                }
                else
                {
                    //If file exists and is the same length (size). Skip. 
                    //If file exists and is of different Length (size) and overwrite = True. Copy 
                    if (sFileInfo.Length != dFileInfo.Length && overWrite)
                    {
                        sFileInfo.CopyTo(dFileInfo.FullName, overWrite);
                    }
                    //If file exists and is of different Length (size) and overwrite = False. Skip 
                    else if (sFileInfo.Length != dFileInfo.Length && !overWrite)
                    {
                      
                    }
                }
                sFileInfo = null;
                dFileInfo = null;
            }
        }

        #endregion 
    

    }
}
