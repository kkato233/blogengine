using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Reflection;
using BlogEngine.Core;

public partial class User_controls_xmanager_Parameters : System.Web.UI.UserControl
{
    static protected string _extensionName = string.Empty;
    static protected ExtensionSettings _settings = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        _extensionName = Request.QueryString["ext"];
        _settings = ExtensionManager.GetSettings(_extensionName);

        CreateFormFields();

        if (!Page.IsPostBack)
        {
            CreateTemplatedGridView();
            BindGrid();
        }

        grid.RowEditing += new GridViewEditEventHandler(grid_RowEditing);
        grid.RowUpdating += new GridViewUpdateEventHandler(grid_RowUpdating);
        grid.RowCancelingEdit += delegate { Response.Redirect(Request.RawUrl); };
        grid.RowDeleting += new GridViewDeleteEventHandler(grid_RowDeleting);
        btnAdd.Click += new EventHandler(btnAdd_Click);
        btnAdd.Text = Resources.labels.add;

    }

    void btnAdd_Click(object sender, EventArgs e)
    {
        if (IsValidForm())
        {
            foreach (Control ctl in phAddForm.Controls)
            {
                if (ctl.GetType().Name == "TextBox")
                {
                    TextBox txt = (TextBox)ctl;
                    _settings.AddValue(txt.ID, txt.Text);
                }
            }

            bool focusSet = false;
            foreach (Control ctl in phAddForm.Controls)
            {
                if (ctl.GetType().Name == "TextBox")
                {
                    TextBox txt = (TextBox)ctl;
                    txt.Text = string.Empty;
                    if (!focusSet)
                    {
                        txt.Focus();
                        focusSet = true;
                    }
                }
            }
            ExtensionManager.SaveSettings(_extensionName, _settings);
            BindGrid();
        }
    }

    void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        foreach (ExtensionParameter par in _settings.Parameters)
        {
            par.DeleteValue(e.RowIndex);
        }
        ExtensionManager.SaveSettings(_extensionName, _settings);
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        StringCollection updateValues = new StringCollection();
        foreach (DataControlFieldCell cel in grid.Rows[e.RowIndex].Controls)
        {
            foreach (Control ctl in cel.Controls)
            {
                if (ctl.GetType().Name == "TextBox")
                {
                    TextBox txt = (TextBox)ctl;
                    updateValues.Add(txt.Text);
                }
            }
        }

        for (int i = 0; i < _settings.Parameters.Count; i++)
        {
            _settings.Parameters[i].Values[e.RowIndex] = updateValues[i];
        }

        ExtensionManager.SaveSettings(_extensionName, _settings);
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    private void BindGrid()
    {
        grid.DataKeyNames = new string[] { _settings.KeyField };
        grid.DataSource = _settings.GetDataTable();
        grid.DataBind();
    }
    void CreateTemplatedGridView()
    {
        foreach (ExtensionParameter par in _settings.Parameters)
        {
            BoundField col = new BoundField();
            col.DataField = par.Name;
            col.HeaderText = par.Label;
            grid.Columns.Add(col);
        }
    }

    /// <summary>
    /// Dynamically add controls to the form
    /// </summary>
    void CreateFormFields()
    {
        foreach (ExtensionParameter par in _settings.Parameters)
        {
            ErrorMsg.InnerHtml = string.Empty;
            ErrorMsg.Visible = false;

            // add label
            Label lbl = new Label();
            lbl.Width = new Unit(100);
            lbl.Text = par.Label;
            phAddForm.Controls.Add(lbl);

            Literal br = new Literal();
            br.Text = "<br />";
            phAddForm.Controls.Add(br);

            // add textbox
            TextBox bx = new TextBox();
            bx.Text = string.Empty;
            bx.ID = par.Name;
            bx.Width = new Unit(250);
            bx.MaxLength = par.MaxLength;
            phAddForm.Controls.Add(bx);

            Literal br2 = new Literal();
            br2.Text = "<br />";
            phAddForm.Controls.Add(br2);
        }
    }

    private bool IsValidForm()
    {
        bool rval = true;
        ErrorMsg.InnerHtml = string.Empty;
        foreach (Control ctl in phAddForm.Controls)
        {
            if (ctl.GetType().Name == "TextBox")
            {
                TextBox txt = (TextBox)ctl;
                if (_settings.IsRequiredParameter(txt.ID) && string.IsNullOrEmpty(txt.Text.Trim()))
                {
                    ErrorMsg.InnerHtml = txt.ID + " is a required field";
                    ErrorMsg.Visible = true;
                    rval = false;
                    break;
                }
                if (_settings.KeyField == (txt.ID) && _settings.IsKeyValueExists(txt.Text.Trim()))
                {
                    ErrorMsg.InnerHtml = txt.Text + " is already exists";
                    ErrorMsg.Visible = true;
                    rval = false;
                    break;
                }
            }
        }
        return rval;
    }
}
