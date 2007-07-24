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

  protected override void OnInit(EventArgs e)
  {
    if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
      Response.Redirect("~/");

  

    base.OnInit(e);
  }

}
