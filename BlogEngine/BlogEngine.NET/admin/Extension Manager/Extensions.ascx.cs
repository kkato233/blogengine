using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlogEngine.Core;
using BlogEngine.Core.Web.Extensions;

using Resources;

/// <summary>
/// The user controls xmanager extensions list.
/// </summary>
public partial class UserControlsXmanagerExtensionsList : UserControl
{
    // const string confirm = "The website will be unavailable for a few seconds. Are you sure you wish to continue?";
    // const string jsOnClick = "onclick=\"if (confirm('" + confirm + "')) { window.location.href = this.href } return false;\"";
    // const string clickToEnable = "Click to enable ";
    // const string clickToDisable = "Click to disable ";
    // const string enabled = "Enabled";
    // const string disabled = "Disabled";
    #region Public Methods

    /// <summary>
    /// The settings link.
    /// </summary>
    /// <param name="extensionName">
    /// The extension name.
    /// </param>
    /// <returns>
    /// The settings link.
    /// </returns>
    public static string SettingsLink(string extensionName)
    {
        var x = ExtensionManager.GetExtension(extensionName);
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(x.AdminPage))
        {
            var url = Utils.AbsoluteWebRoot.AbsoluteUri;
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            if (x.AdminPage.StartsWith("~/"))
            {
                url += x.AdminPage.Substring(2);
            }
            else if (x.AdminPage.StartsWith("/"))
            {
                url += x.AdminPage.Substring(1);
            }
            else
            {
                url += x.AdminPage;
            }

            sb.AppendFormat("<a href='{0}'>{1}</a>", url, labels.edit);
        }
        else
        {
            if (x.Settings == null)
            {
                sb.Append("&nbsp;");
            }
            else
            {
                if (x.Settings.Count == 0 || (x.Settings.Count == 1 && x.Settings[0] == null) || x.ShowSettings == false)
                {
                    sb.Append("&nbsp;");
                }
                else
                {
                    sb.AppendFormat("<a href='?ctrl=params&ext={0}'>{1}</a>", x.Name, labels.edit);
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// The status link.
    /// </summary>
    /// <param name="extensionName">
    /// The extension name.
    /// </param>
    /// <returns>
    /// The status link.
    /// </returns>
    public static string StatusLink(string extensionName)
    {
        var jsOnClick = string.Format("onclick=\"if (confirm('{0}')) {{ window.location.href = this.href }} return false;\"", labels.siteUnavailableConfirm);
        var x = ExtensionManager.GetExtension(extensionName);
        var sb = new StringBuilder();
        if (x.Enabled)
        {
            sb.AppendFormat("<span style='background:#ccffcc'><a href='?act=dis&ext={0}' title='{1}{2}' {3}>{4}</a></span>", x.Name, labels.clickToDisable, x.Name, jsOnClick, labels.enabled);
        }
        else
        {
            sb.AppendFormat("<span style='background:#ffcc66'><a href='?act=enb&ext={0}' title='{1}{2}' {3}>{4}</a></span>", x.Name, labels.clickToEnable, x.Name, jsOnClick, labels.disabled);
        }

        return sb.ToString();
    }

    /// <summary>
    /// The force restart.
    /// </summary>
    /// <exception cref="ApplicationException">
    /// </exception>
    public void ForceRestart()
    {
        throw new ApplicationException();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The change priority.
    /// </summary>
    /// <param name="filterName">
    /// The filter name.
    /// </param>
    /// <param name="up">
    /// The up.
    /// </param>
    protected void ChangePriority(string filterName, bool up)
    {
        var x = ExtensionManager.GetExtension(filterName);

        if (x != null)
        {
            if (up && x.Priority > 1)
            {
                x.Priority--;
            }
            else
            {
                x.Priority++;
            }

            ExtensionManager.SaveToStorage(x);
        }

        this.Response.Redirect(this.Request.RawUrl);
    }

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblErrorMsg.InnerHtml = string.Empty;
        this.lblErrorMsg.Visible = false;
        this.btnRestart.Visible = false;
        this.btnRestart.OnClientClick = "return confirm('" + labels.siteUnavailableConfirm + "');";

        object act = this.Request.QueryString["act"];
        object ext = this.Request.QueryString["ext"];

        if (act != null && ext != null)
        {
            this.ChangeStatus(act.ToString(), ext.ToString());
        }

        if (!this.Page.IsPostBack)
        {
            var extensions = new List<ManagedExtension>();

            foreach (var x in ExtensionManager.Extensions)
            {
                if (x.Name != "MetaExtension")
                {
                    extensions.Add(x);
                }
            }

            // remove system meta extension from the list
            // extensions.Remove(extensions.Find(delegate(ManagedExtension x) { return x.Name == "MetaExtension"; }));
            extensions.Sort(
                delegate(ManagedExtension e1, ManagedExtension e2)
                    {
                        if (e1.Priority == e2.Priority)
                        {
                            return string.CompareOrdinal(e1.Name, e2.Name);
                        }

                        return e1.Priority.CompareTo(e2.Priority);
                    });

            this.gridExtensionsList.DataSource = extensions;
            this.gridExtensionsList.DataBind();
        }

        this.btnRestart.Click += this.BtnRestartClick;
    }

    /// <summary>
    /// Handles the click event of the btnPriorityDwn control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnPriorityDwn_click(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var grdRow = (GridViewRow)btn.Parent.Parent;

        var s = this.gridExtensionsList.DataKeys[grdRow.RowIndex].Value.ToString();
        this.ChangePriority(s, false);
    }

    /// <summary>
    /// Handles the click event of the btnPriorityUp control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnPriorityUp_click(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var grdRow = (GridViewRow)btn.Parent.Parent;

        var s = this.gridExtensionsList.DataKeys[grdRow.RowIndex].Value.ToString();
        this.ChangePriority(s, true);
    }

    /// <summary>
    /// Test stuff - ignore for now
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="e">
    /// </param>
    private void BtnRestartClick(object sender, EventArgs e)
    {
        // This short cercuits the IIS process. Need to find a better way to restart the app.
        // ThreadPool.QueueUserWorkItem(delegate { ForceRestart(); });
        // ThreadStart threadStart = delegate { ForceRestart(); };
        // Thread thread = new Thread(threadStart);
        // thread.IsBackground = true;
        // thread.Start();
        this.Response.Redirect(this.Request.RawUrl, true);
    }

    /// <summary>
    /// Method to change extension status
    ///     to enable or disable extension and
    ///     then will restart applicaton by
    ///     touching web.config file
    /// </summary>
    /// <param name="act">
    /// Enable or Disable
    /// </param>
    /// <param name="ext">
    /// Extension Name
    /// </param>
    private void ChangeStatus(string act, string ext)
    {
        // UnloadAppDomain() requires full trust - touch web.config to reload app
        try
        {
            ExtensionManager.ChangeStatus(ext, act != "dis");

            if (ExtensionManager.FileAccessException == null)
            {
                // string ConfigPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\web.config";
                // System.IO.File.SetLastWriteTimeUtc(ConfigPath, DateTime.UtcNow);
                this.Response.Redirect("default.aspx");
            }
            else
            {
                this.ShowError(ExtensionManager.FileAccessException);
            }
        }
        catch (Exception e)
        {
            this.ShowError(e);
        }
    }

    /// <summary>
    /// Show error message if something
    ///     goes wrong
    /// </summary>
    /// <param name="e">
    /// Exception
    /// </param>
    private void ShowError(Exception e)
    {
        this.lblErrorMsg.Visible = true;
        this.lblErrorMsg.InnerHtml = string.Format("Changes will not be applied: {0}", e.Message);
    }

    #endregion
}