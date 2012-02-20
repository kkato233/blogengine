<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BlogEngine.Core" %>
<%@ Import Namespace="App_Code.Controls" %>
<%@ Import Namespace="Microsoft.Web.Optimization" %>

<script RunAt="server">
  
  /// <summary>
  /// Application Error handler
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
    void Application_Error(object sender, EventArgs e)
    {
        HttpContext context = ((HttpApplication)sender).Context;
        Exception ex = context.Server.GetLastError();
        
        if (ex == null || !(ex is HttpException) || (ex as HttpException).GetHttpCode() == 404)
        {
            return;
        }
        var sb = new StringBuilder();

        try
        {
            sb.AppendLine("Url : " + context.Request.Url);
            sb.AppendLine("Raw Url : " + context.Request.RawUrl);

            while (ex != null)
            {
                sb.AppendLine("Message : " + ex.Message);
                sb.AppendLine("Source : " + ex.Source);
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine("TargetSite : " + ex.TargetSite);
                ex = ex.InnerException;
            }
        }
        catch (Exception ex2)
        {
            sb.AppendLine("Error logging error : " + ex2.Message);
        }

        if (BlogSettings.Instance.EnableErrorLogging)
        {
            Utils.Log(sb.ToString());
        }
        context.Items["LastErrorDetails"] = sb.ToString();
        context.Response.StatusCode = 500;
        
        // Custom errors section defined in the Web.config, will rewrite (not redirect)
        // this 500 error request to error.aspx.
    }

    void Application_Start(object sender, EventArgs e)
    {
        // intentionally not using Application_Start.  instead application
        // start code is below in FirstRequestInitialization.  this is to
        // workaround IIS7 integrated mode issue where HttpContext.Request
        // is not available during Application_Start.
    }

    void Application_BeginRequest(object source, EventArgs e)
    {
        HttpApplication app = (HttpApplication)source;
        HttpContext context = app.Context;
        
        // Attempt to perform first request initialization
        FirstRequestInitialization.Initialize(context);
    }
        
    /// <summary>
    /// Sets the culture based on the language selection in the settings.
    /// </summary>
    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        var culture = BlogSettings.Instance.Culture;
        if (!string.IsNullOrEmpty(culture) && !culture.Equals("Auto"))
        {
            CultureInfo defaultCulture = Utils.GetDefaultCulture();
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
            Thread.CurrentThread.CurrentCulture = defaultCulture;
        }
    }

    private class FirstRequestInitialization
    {
        private static bool _initializedAlready = false;
        private readonly static object _SyncRoot = new Object();

        // Initialize only on the first request
        public static void Initialize(HttpContext context)
        {
            if (_initializedAlready) { return; }

            lock (_SyncRoot)
            {
                if (_initializedAlready) { return; }

                WidgetZone.PreloadWidgetsAsync("be_WIDGET_ZONE"); 
                Utils.LoadExtensions();

                Init_BundleTable();
                
                _initializedAlready = true;
            }
        }

        static void Init_BundleTable()
        {
            BundleTable.Bundles.EnableDefaultBundles();
            BundleTable.Bundles.Clear();

            // for anonymous users
            var css = new Bundle("~/Styles/css", typeof(CssMinify));
            css.AddDirectory("~/Styles", "*.css", false);
            BundleTable.Bundles.Add(css);

            var js = new Bundle("~/Scripts/js", typeof(JsMinify));
            js.AddDirectory("~/Scripts", "*.js", false);
            BundleTable.Bundles.Add(js);

            // for authenticated users
            var cssauth = new Bundle("~/Styles/cssauth", typeof(CssMinify));
            cssauth.AddDirectory("~/Styles", "*.css", false);
            cssauth.AddFile("~/Modules/QuickNotes/Qnotes.css");
            BundleTable.Bundles.Add(cssauth);

            var jsauth = new Bundle("~/Scripts/jsauth", typeof(JsMinify));
            jsauth.AddFile("~/Scripts/jQuery/01-jquery.js");
            jsauth.AddFile("~/Scripts/jQuery/02-jquery.cookie.js");
            jsauth.AddFile("~/Scripts/jQuery/04-jquery-jtemplates.js");
            jsauth.AddDirectory("~/Scripts", "*.js", false); 
            jsauth.AddFile("~/admin/widget.js");
            jsauth.AddFile("~/Modules/QuickNotes/Qnotes.js");   
            BundleTable.Bundles.Add(jsauth);
            
            // administration
            var cssadmin = new Bundle("~/admin/css", typeof(CssMinify));
            cssadmin.AddFile("~/admin/style.css");
            cssadmin.AddFile("~/admin/colorbox.css");
            cssadmin.AddFile("~/admin/tipsy.css");
            BundleTable.Bundles.Add(cssadmin);
            
            var jsadmin = new Bundle("~/admin/js", typeof(JsMinify));
            jsadmin.AddDirectory("~/Scripts/jQuery", "*.js", false);
            jsadmin.AddFile("~/admin/admin.js");
            BundleTable.Bundles.Add(jsadmin);
        }
    }
    
</script>