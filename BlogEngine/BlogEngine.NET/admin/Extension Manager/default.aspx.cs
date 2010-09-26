using System;
using System.Linq;
using System.Web;
using System.Web.UI;

using BlogEngine.Core.Web.Extensions;

public partial class User_controls_xdashboard_Default : System.Web.UI.Page
{
    /// <summary>
    /// Handles page load, loading control
    /// based on query string parameter
    /// </summary>
    /// <param name="sender">Page</param>
    /// <param name="e">Event args</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string ctrlToLoad = string.Empty;
        UserControl uc;

        switch (Request.QueryString["ctrl"])
        {
          case "params":
            string xName = Request.QueryString["ext"].ToString();

            foreach (ExtensionSettings setting in from x in ExtensionManager.Extensions
                                                  where x.Name == xName
                                                  from setting in x.Settings
                                                  where !string.IsNullOrEmpty(setting.Name) && !setting.Hidden
                                                  select setting)
            {
                uc = (UserControl)this.Page.LoadControl("Settings.ascx");
                uc.ID = setting.Name;
                this.ucPlaceHolder.Controls.Add(uc);
            }
            break;
          case "editor":
              uc = (UserControl)Page.LoadControl("Editor.ascx");
              ucPlaceHolder.Controls.Add(uc);
              break;
          default:
              uc = (UserControl)Page.LoadControl("Extensions.ascx");
              ucPlaceHolder.Controls.Add(uc);
              break;
        }
    }
}