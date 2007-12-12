using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using BlogEngine.Core;

public partial class User_controls_xmanager_Parameters : System.Web.UI.UserControl
{
    static protected string _extensionName = string.Empty;
    static protected string _settingsHelp = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        _extensionName = Request.QueryString["ext"];

        if (!Page.IsPostBack)
        {
            BindGrid();
        }

        grid.RowEditing += new GridViewEditEventHandler(grid_RowEditing);
        grid.RowUpdating += new GridViewUpdateEventHandler(grid_RowUpdating);
        grid.RowCancelingEdit += delegate { Response.Redirect(Request.RawUrl); };
        grid.RowDeleting += new GridViewDeleteEventHandler(grid_RowDeleting);
        btnAdd.Click += new EventHandler(btnAdd_Click);
        btnAdd.Text = Resources.labels.add;
        valExist.ServerValidate += new ServerValidateEventHandler(valExist_ServerValidate);
    }

    private void valExist_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        ExtensionSettings settings = ExtensionManager.GetSettings(_extensionName);

        foreach (ExtensionParameter p in settings.Parameters)
        {
            if (p.Name.Equals(txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                args.IsValid = false;
            }
        }
    }

    void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ExtensionSettings settings = ExtensionManager.GetSettings(_extensionName);
            settings.AddParameter(txtName.Text, txtVal.Text);
            ExtensionManager.Save();
            BindGrid();
        }
    }

    void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string key = grid.DataKeys[e.RowIndex].Value.ToString();
        ExtensionSettings settings = ExtensionManager.GetSettings(_extensionName);
        settings.RemoveParameter(key);
        ExtensionManager.Save();
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox textboxName = (TextBox)grid.Rows[e.RowIndex].FindControl("txtName");
        TextBox textboxValue = (TextBox)grid.Rows[e.RowIndex].FindControl("txtValue");
        ExtensionSettings settings = ExtensionManager.GetSettings(_extensionName);
        settings.UpdateParameter(textboxName.Text, textboxValue.Text);
        ExtensionManager.Save();
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    private void BindGrid()
    {
        ExtensionSettings settings = ExtensionManager.GetSettings(_extensionName);
        grid.DataKeyNames = new string[] { "Name" };
        _settingsHelp = settings.SettingsHelp;
        grid.DataSource = settings.Parameters;
        grid.DataBind();
    }
}
