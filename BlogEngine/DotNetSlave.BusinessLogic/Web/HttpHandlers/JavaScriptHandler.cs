namespace BlogEngine.Core.Web.HttpHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;

    using BlogEngine.Core.Web.HttpModules;

    /// <summary>
    /// Removes whitespace in all stylesheets added to the 
    ///     header of the HTML document in site.master.
    /// </summary>
    public class JavaScriptHandler : IHttpHandler
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref = "T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref = "T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom 
        ///     HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpContext"></see> object that provides 
        ///     references to the intrinsic server objects 
        ///     (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var path = request.QueryString["path"];

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string cacheKey = request.RawUrl;
            string script = (string)context.Cache[cacheKey];

            if (String.IsNullOrEmpty(script))
            {
                if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    script = RetrieveRemoteScript(path, cacheKey);
                }
                else
                {
                    script = RetrieveLocalScript(path, cacheKey);
                }
            }


            if (string.IsNullOrEmpty(script))
            {
                return;
            }

            SetHeaders(script.GetHashCode(), context);
            context.Response.Write(script);

            if (BlogSettings.Instance.EnableHttpCompression)
            {
                CompressionModule.CompressResponse(context); // Compress(context);
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether to hard minify output.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>Whether to hard minify output.</returns>
        private static bool HardMinify(string file)
        {
            var lookfor = ConfigurationManager.AppSettings.Get("BlogEngine.HardMinify").Split(
                new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return lookfor.Any(file.Contains);
        }

        /// <summary>
        /// Call this method for any extra processing that needs to be done on a script resource before
        /// being written to the response.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string ProcessScript(string script, string fileName)
        {
            return StripWhitespace(script, HardMinify(fileName));
        }

        /// <summary>
        /// Retrieves the local script from the disk
        /// </summary>
        /// <param name="file">
        /// The file name.
        /// </param>
        /// <param name="cacheKey">The key used to insert this script into the cache.</param>
        /// <returns>
        /// The retrieve local script.
        /// </returns>
        private static string RetrieveLocalScript(string file, string cacheKey)
        {

            if (StringComparer.InvariantCultureIgnoreCase.Compare(Path.GetExtension(file), ".js") != 0)
            {
                throw new SecurityException("No access");
            }

            var path = HttpContext.Current.Server.MapPath(file);

            if (File.Exists(path))
            {
                string script;
                using (var reader = new StreamReader(path))
                {
                    script = reader.ReadToEnd();
                }

                script = ProcessScript(script, file);
                HttpContext.Current.Cache.Insert(cacheKey, script, new CacheDependency(path));
                return script;
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieves and cached the specified remote script.
        /// </summary>
        /// <param name="file">
        /// The remote URL
        /// </param>
        /// <param name="cacheKey">The key used to insert this script into the cache.</param>
        /// <returns>
        /// The retrieve remote script.
        /// </returns>
        private static string RetrieveRemoteScript(string file, string cacheKey)
        {

            Uri url;

            if (Uri.TryCreate(file, UriKind.Absolute, out url))
            {
                try
                {

                    var remoteFile = new RemoteFile(url, false);
                    string script = ProcessScript(remoteFile.GetFileAsString(), file);
                   HttpContext.Current.Cache.Insert(cacheKey, script, null, Cache.NoAbsoluteExpiration, new TimeSpan(3, 0, 0, 0));
                    return script;
                }
                catch (SocketException)
                {
                    // The remote site is currently down. Try again next time.
                }
            }

            return String.Empty;
        }



        /// <summary>
        /// This will make the browser and server keep the output
        ///     in its cache and thereby improve performance.
        /// </summary>
        /// <param name="hash">
        /// The hash number.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private static void SetHeaders(int hash, HttpContext context)
        {

            var response = context.Response;

            response.ContentType = "text/javascript";

            var cache = response.Cache;

            cache.VaryByHeaders["Accept-Encoding"] = true;
            cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(7));
            cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            var etag = string.Format("\"{0}\"", hash);
            var incomingEtag = context.Request.Headers["If-None-Match"];

            cache.SetETag(etag);
            cache.SetCacheability(HttpCacheability.Public);

            if (String.Compare(incomingEtag, etag) != 0)
            {
                return;
            }

            response.Clear();
            response.StatusCode = (int)HttpStatusCode.NotModified;
            response.SuppressContent = true;
        }

        /// <summary>
        /// Strips the whitespace from any .css file.
        /// </summary>
        /// <param name="body">
        /// The body string.
        /// </param>
        /// <param name="blogEngineScript">
        /// The is Blog Engine Script.
        /// </param>
        /// <returns>
        /// A string contained the whitespace-removed copy of the script.
        /// </returns>
        private static string StripWhitespace(string body, bool blogEngineScript)
        {

            var lines = body.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var emptyLines = new StringBuilder();

            // Don't use linq for this. The previous method was using Trim() in a linq query, but since
            // it couldn't be cached, the resulting loop had to Trim it again.
            foreach (var s in lines)
            {
                var line = s.Trim();

                if ((line.Length > 0) && !line.StartsWith("//"))
                {
                    // Use AppendLine over Append, otherwise the Regex doesn't remove everything correctly.
                    emptyLines.AppendLine(line);
                }
            }

            body = emptyLines.ToString();

            if (blogEngineScript)
            {
                // mark strings and regular expressions
                var re =
                    new Regex(
                        "\"(([^\"\\r\\n])|(\\\"))*\"|'[^'\\r\\n]*'|/[^/\\*](?<![/\\S]/.)([^/\\\\\\r\\n]|\\\\.)*/(?=[ig]{0,2}[^\\S])",
                        RegexOptions.Compiled | RegexOptions.Multiline);

                var m = re.Matches(body);
                var strs = new List<string>(m.Count);
                for (var i = 0; i < m.Count; i++)
                {
                    strs.Add(m[i].Value);

                    // replace string and regular expression with marker
                    // This will be inlined, so there's no real reason to use a StringBuilder here.
                    string strRegex = "_____STRINGREGEX_" + i.ToString() + "_STRINGREGEX_____";
                    body = re.Replace(body, strRegex, 1);
                }

                // remove line comments
                body = Regex.Replace(body, "//.*[\r\n]", String.Empty, RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove C styles comments
                body = Regex.Replace(body, "/\\*.*?\\*/", String.Empty, RegexOptions.Compiled | RegexOptions.Singleline);

                // trim left
                body = Regex.Replace(body, "^\\s*", String.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

                // trim right
                body = Regex.Replace(body, "\\s*[\\r\\n]", "\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove whitespace beside of left curly braced
                body = Regex.Replace(body, "\\s*{\\s*", "{", RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove whitespace beside of right curly braced
                body = Regex.Replace(body, "\\s*}\\s*", "}", RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove whitespace beside of coma
                body = Regex.Replace(body, "\\s*,\\s*", ",", RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove whitespace beside of semicolon
                body = Regex.Replace(body, "\\s*;\\s*", ";", RegexOptions.Compiled | RegexOptions.ECMAScript);

                // remove newline after keywords
                body = Regex.Replace(
                    body,
                    "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)",
                    " ",
                    RegexOptions.Compiled | RegexOptions.ECMAScript);

                // restore marked strings and regular expressions
                for (var i = 0; i < strs.Count; i++)
                {
                    string strReg = "_____STRINGREGEX_" + i.ToString() + "_STRINGREGEX_____";
                    body = Regex.Replace(body, strReg, strs[i]);

                }
            }
            else
            {
                
                body = Regex.Replace(body, @"^[\s]+|[ \f\r\t\v]+$", String.Empty);
                body = Regex.Replace(body, @"([+-])\n\1", "$1 $1");
                body = Regex.Replace(body, @"([^+-][+-])\n", "$1");
                body = Regex.Replace(body, @"([^+]) ?(\+)", "$1$2");
                body = Regex.Replace(body, @"(\+) ?([^+])", "$1$2");
                body = Regex.Replace(body, @"([^-]) ?(\-)", "$1$2");
                body = Regex.Replace(body, @"(\-) ?([^-])", "$1$2");
                body = Regex.Replace(body, @"\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1");
                body = Regex.Replace(body, @"(\W(if|while|for)\([^{]*?\))\n", "$1");
                body = Regex.Replace(body, @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3");
                body = Regex.Replace(body, @"([;}]else)\n", "$1 ");
                body = Regex.Replace(
                    body, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);
            }

            return body;
        }



        #endregion

    }
}