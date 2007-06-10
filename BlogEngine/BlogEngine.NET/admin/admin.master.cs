using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using System.Threading;
using System.Globalization;

public partial class admin_admin : System.Web.UI.MasterPage
{
  protected void Page_Init(object sender, EventArgs e)
  {
    if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
      Response.Redirect("~/");

    if (!string.IsNullOrEmpty(BlogSettings.Instance.Culture))
    {
      if (BlogSettings.Instance.Culture.Equals("Auto"))
      {
        Page.UICulture = "auto";
        Page.Culture = "auto";
      }
      else
      {
        CultureInfo culture = CultureInfo.CreateSpecificCulture(BlogSettings.Instance.Culture);
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Page.UICulture = culture.Name;
        Page.Culture = culture.Name;
      }
    }
  }
}
