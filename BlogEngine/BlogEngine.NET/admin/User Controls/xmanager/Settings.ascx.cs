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
    static string _fileName = string.Empty;
    static protected string _extensionName = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        _fileName = Server.MapPath(BlogSettings.Instance.StorageLocation) + "xmanager.xml";
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
        //foreach (Category category in Category.Categories)
        //{
        //    if (category.Title.Equals(txtNewCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase))
        //        args.IsValid = false;
        //}
    }

    void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Dictionary<string, string[]> settings = ExtensionManager.Settings(_extensionName);
            settings.Add(txtName.Text, txtVal.Text.Split(','));
            ExtensionManager.SaveSettings(_extensionName, settings);
            BindGrid();
        }
    }

    void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string key = grid.DataKeys[e.RowIndex].Value.ToString();
        Dictionary<string, string[]> settings = ExtensionManager.Settings(_extensionName);
        settings.Remove(key);
        ExtensionManager.SaveSettings(_extensionName, settings);
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox textboxName = (TextBox)grid.Rows[e.RowIndex].FindControl("txtName");
        TextBox textboxValue = (TextBox)grid.Rows[e.RowIndex].FindControl("txtValue");
        Dictionary<string, string[]> settings = ExtensionManager.Settings(_extensionName);
        settings[textboxName.Text] = textboxValue.Text.Split(',');
        ExtensionManager.SaveSettings(_extensionName, settings);
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    private void BindGrid()
    {
        Dictionary<string, string[]> settings = ExtensionManager.Settings(_extensionName);
        Dictionary<string, string> gridValues = new Dictionary<string, string>();
        foreach (string key in settings.Keys)
        {
            gridValues.Add(key, string.Join(",", settings[key]));
        }
        grid.DataKeyNames = new string[] { "key" };
        grid.DataSource = gridValues;
        grid.DataBind();
    }
}
