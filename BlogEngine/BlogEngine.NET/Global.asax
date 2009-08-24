<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="BlogEngine.Core" %>
<%@ Import Namespace="BlogEngine.Core.Web.Controls" %>
<%@ Import Namespace="BlogEngine.Core.Web" %>

<script RunAt="server">
  /// <summary>
  /// Hooks up the available extensions located in the App_Code folder.
  /// An extension must be decorated with the ExtensionAttribute to work.
  /// <example>
  ///  <code>
  /// [Extension("Description of the SomeExtension class")]
  /// public class SomeExtension
  /// {
  ///   //There must be a parameterless default constructor.
  ///   public SomeExtension()
  ///   {
  ///     //Hook up to the BlogEngine.NET events.
  ///   }
  /// }
  /// </code>
  /// </example>
  /// </summary>
  /// 
    void Application_Error(object sender, EventArgs e)
    {
        HttpContext context = ((HttpApplication)sender).Context;
        Exception ex = context.Server.GetLastError();
        if (ex == null || !(ex is HttpException) || (ex as HttpException).GetHttpCode() == 404)
            return;

        StringBuilder sb = new StringBuilder();

        try
        {
            sb.Append("Url : " + context.Request.Url.ToString());
            sb.Append(Environment.NewLine);
            sb.Append("Raw Url : " + context.Request.RawUrl);
            sb.Append(Environment.NewLine);

            while (ex != null)
            {
                sb.Append("Message : " + ex.Message);
                sb.Append(Environment.NewLine);
                sb.Append("Source : " + ex.Source);
                sb.Append(Environment.NewLine);
                sb.Append("StackTrace : " + ex.StackTrace);
                sb.Append(Environment.NewLine);
                sb.Append("TargetSite : " + ex.TargetSite);
                sb.Append(Environment.NewLine);
                sb.Append("Environment.StackTrace : " + Environment.StackTrace);
                sb.Append(Environment.NewLine);
                ex = ex.InnerException;
            }
        }
        catch (Exception ex2)
        {
            sb.Append("Error logging error : " + ex2.Message);
        }

        if (BlogSettings.Instance.EnableErrorLogging)
            Utils.Log(sb.ToString());

        context.Items["LastErrorDetails"] = sb.ToString();
        context.Response.StatusCode = 500;
        Server.ClearError();
        context.Server.Transfer("~/error.aspx");
    }
    
  void Application_Start(object sender, EventArgs e)
  {
    ArrayList codeAssemblies = Utils.CodeAssemblies();
    List<SortedExtension> sortedExtensions = new List<SortedExtension>();

    // initialize comment rules and filters
    CommentHandlers.Listen();

    foreach (Assembly a in codeAssemblies)
    {
      Type[] types = a.GetTypes();
      foreach (Type type in types)
      {
        object[] attributes = type.GetCustomAttributes(typeof(ExtensionAttribute), false);
        foreach (object attribute in attributes)
        {
          if (attribute.GetType().Name == "ExtensionAttribute")
          {
            ExtensionAttribute ext = (ExtensionAttribute)attribute;
            sortedExtensions.Add(new SortedExtension(ext.Priority, type.Name, type.FullName));
          }
        }
      }

      sortedExtensions.Sort(delegate(SortedExtension e1, SortedExtension e2)
      {
        if (e1.Priority == e2.Priority) 
          return string.CompareOrdinal(e1.Name, e2.Name); 
        return e1.Priority.CompareTo(e2.Priority);
      }); 
        
      foreach (SortedExtension x in sortedExtensions)
      {
        if (ExtensionManager.ExtensionEnabled(x.Name))
        {
          a.CreateInstance(x.Type);
        }
      }
    }
  }

  /// <summary>
  /// Sets the culture based on the language selection in the settings.
  /// </summary>
  void Application_PreRequestHandlerExecute(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(BlogSettings.Instance.Culture))
    {
      if (!BlogSettings.Instance.Culture.Equals("Auto"))
      {
        CultureInfo culture = CultureInfo.CreateSpecificCulture(BlogSettings.Instance.Culture);
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
      }
    }
  }
 
</script>