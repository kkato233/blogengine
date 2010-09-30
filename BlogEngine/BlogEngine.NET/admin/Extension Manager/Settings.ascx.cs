using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlogEngine.Core.Web.Extensions;

using Resources;

/// <summary>
/// The user_controls_xmanager_ parameters.
/// </summary>
public partial class User_controls_xmanager_Parameters : UserControl
{
    #region Private members

    /// <summary>
    /// The _extension name.
    /// </summary>
    private string _extensionName = string.Empty;

    /// <summary>
    /// The _settings.
    /// </summary>
    protected ExtensionSettings _settings;

    #endregion

    /// <summary>
    /// Gets or sets SettingName.
    /// </summary>
    public string SettingName
    {
        get
        {
            return this._extensionName;
        }

        set
        {
            this._extensionName = value;
        }
    }

    /// <summary>
    /// The generate delete button.
    /// </summary>
    public bool GenerateDeleteButton = true;

    /// <summary>
    /// The generate edit button.
    /// </summary>
    public bool GenerateEditButton = true;

    /// <summary>
    /// Dynamically loads form controls or
    ///     data grid and binds data to controls
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="e">
    /// </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this._extensionName = this.ID;
        this._settings = ExtensionManager.GetSettings(this._extensionName);

        this.GenerateDeleteButton = this._settings.ShowDelete;
        this.GenerateEditButton = this._settings.ShowEdit;

        if (this._settings.ShowAdd)
        {
            this.CreateFormFields();
        }

        if (!this.Page.IsPostBack)
        {
            if (this._settings.Scalar)
            {
                this.BindScalar();
            }
            else
            {
                this.CreateTemplatedGridView();
                this.BindGrid();
            }
        }

        if (this._settings.Scalar)
        {
            this.btnAdd.Text = labels.save;
        }
        else
        {
            if (this._settings.ShowAdd)
            {
                this.grid.RowEditing += this.grid_RowEditing;
                this.grid.RowUpdating += this.grid_RowUpdating;
                this.grid.RowCancelingEdit += delegate { this.Response.Redirect(this.Request.RawUrl); };
                this.grid.RowDeleting += this.grid_RowDeleting;
                this.btnAdd.Text = labels.add;
            }
            else
            {
                this.btnAdd.Visible = false;
            }
        }

        this.btnAdd.Click += this.btnAdd_Click;
    }

    /// <summary>
    /// Handels adding a new value(s)
    /// </summary>
    /// <param name="sender">
    /// Button
    /// </param>
    /// <param name="e">
    /// Arguments
    /// </param>
    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (this.IsValidForm())
        {
            foreach (Control ctl in this.phAddForm.Controls)
            {
                if (ctl.GetType().Name == "TextBox")
                {
                    var txt = (TextBox)ctl;

                    if (this._settings.Scalar)
                    {
                        this._settings.UpdateScalarValue(txt.ID, txt.Text);
                    }
                    else
                    {
                        this._settings.AddValue(txt.ID, txt.Text);
                    }
                }
                else if (ctl.GetType().Name == "CheckBox")
                {
                    var cbx = (CheckBox)ctl;
                    this._settings.UpdateScalarValue(cbx.ID, cbx.Checked.ToString());
                }
                else if (ctl.GetType().Name == "DropDownList")
                {
                    var dd = (DropDownList)ctl;
                    this._settings.UpdateSelectedValue(dd.ID, dd.SelectedValue);
                }
                else if (ctl.GetType().Name == "ListBox")
                {
                    var lb = (ListBox)ctl;
                    this._settings.UpdateSelectedValue(lb.ID, lb.SelectedValue);
                }
                else if (ctl.GetType().Name == "RadioButtonList")
                {
                    var rbl = (RadioButtonList)ctl;
                    this._settings.UpdateSelectedValue(rbl.ID, rbl.SelectedValue);
                }
            }

            ExtensionManager.SaveSettings(this._extensionName, this._settings);
            if (this._settings.Scalar)
            {
                this.InfoMsg.InnerHtml = labels.theValuesSaved;
                this.InfoMsg.Visible = true;
            }
            else
            {
                this.BindGrid();
            }
        }
    }

    /// <summary>
    /// Deliting row in the data grid
    /// </summary>
    /// <param name="sender">
    /// Grid View
    /// </param>
    /// <param name="e">
    /// Arguments
    /// </param>
    private void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var paramIndex = ParameterValueIndex(sender, e.RowIndex);

        foreach (var par in this._settings.Parameters)
        {
            par.DeleteValue(paramIndex);
        }

        ExtensionManager.SaveSettings(this._extensionName, this._settings);
        this.Response.Redirect(this.Request.RawUrl);
    }

    /// <summary>
    /// Updating row in the grid
    /// </summary>
    /// <param name="sender">
    /// Grid View
    /// </param>
    /// <param name="e">
    /// Event args
    /// </param>
    private void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // extract and store input values in the collection
        var updateValues = new StringCollection();
        foreach (DataControlFieldCell cel in this.grid.Rows[e.RowIndex].Controls)
        {
            foreach (Control ctl in cel.Controls)
            {
                if (ctl.GetType().Name == "TextBox")
                {
                    var txt = (TextBox)ctl;
                    updateValues.Add(txt.Text);
                }
            }
        }

        var paramIndex = ParameterValueIndex(sender, e.RowIndex);

        for (var i = 0; i < this._settings.Parameters.Count; i++)
        {
            var parName = this._settings.Parameters[i].Name;
            if (this._settings.IsRequiredParameter(parName) && string.IsNullOrEmpty(updateValues[i]))
            {
                // throw error if required field is empty
                this.ErrorMsg.InnerHtml = "\"" + this._settings.GetLabel(parName) + "\" " + labels.isRequiredField;
                this.ErrorMsg.Visible = true;
                e.Cancel = true;
                return;
            }
            else if (parName == this._settings.KeyField && this._settings.IsKeyValueExists(updateValues[i]))
            {
                // check if key value was changed; if not, it's ok to update
                if (!this._settings.IsOldValue(parName, updateValues[i], paramIndex))
                {
                    // trying to update key field with value that already exists
                    this.ErrorMsg.InnerHtml = "\"" + updateValues[i] + "\" " + labels.isAlreadyExists;
                    this.ErrorMsg.Visible = true;
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                this._settings.Parameters[i].Values[paramIndex] = updateValues[i];
            }
        }

        ExtensionManager.SaveSettings(this._extensionName, this._settings);
        this.Response.Redirect(this.Request.RawUrl);
    }

    /// <summary>
    /// Returns index of the parameter calculated 
    ///     based on the page number and size
    /// </summary>
    /// <param name="sender">
    /// GridView object
    /// </param>
    /// <param name="rowindex">
    /// Index of the row in the grid
    /// </param>
    /// <returns>
    /// Index of the parameter
    /// </returns>
    private static int ParameterValueIndex(object sender, int rowindex)
    {
        var paramIndex = rowindex;
        var gv = (GridView)sender;
        if (gv.PageIndex > 0)
        {
            paramIndex = gv.PageIndex * gv.PageSize + rowindex;
        }

        return paramIndex;
    }

    /// <summary>
    /// Editing data in the data grid
    /// </summary>
    /// <param name="sender">
    /// Grid View
    /// </param>
    /// <param name="e">
    /// Event args
    /// </param>
    private void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.grid.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }

    /// <summary>
    /// Handles page changing event in the data grid
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="e">
    /// </param>
    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.grid.PageIndex = e.NewPageIndex;
        this.grid.DataSource = this._settings.GetDataTable();
        this.grid.DataBind();
    }

    /// <summary>
    /// Binds settings values formatted as
    ///     data table to grid view
    /// </summary>
    private void BindGrid()
    {
        if (this.GenerateEditButton)
        {
            this.grid.AutoGenerateEditButton = true;
        }

        if (this.GenerateDeleteButton)
        {
            this.grid.AutoGenerateDeleteButton = true;
        }

        this.grid.DataKeyNames = new[] { this._settings.KeyField };
        this.grid.DataSource = this._settings.GetDataTable();
        this.grid.DataBind();
    }

    /// <summary>
    /// Binds single value parameters
    ///     to text boxes
    /// </summary>
    private void BindScalar()
    {
        foreach (var par in this._settings.Parameters)
        {
            foreach (Control ctl in this.phAddForm.Controls)
            {
                if (ctl.GetType().Name == "CheckBox")
                {
                    var cbx = (CheckBox)ctl;
                    if (cbx.ID.ToLower() == par.Name.ToLower())
                    {
                        if (par.Values != null && par.Values.Count > 0)
                        {
                            cbx.Checked = bool.Parse(par.Values[0]);
                        }
                    }
                }

                if (ctl.GetType().Name == "TextBox")
                {
                    var txt = (TextBox)ctl;
                    if (txt.ID.ToLower() == par.Name.ToLower())
                    {
                        if (par.Values != null)
                        {
                            if (par.Values.Count == 0)
                            {
                                txt.Text = string.Empty;
                            }
                            else
                            {
                                txt.Text = par.Values[0];
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates template for data grid view
    /// </summary>
    private void CreateTemplatedGridView()
    {
        foreach (var par in this._settings.Parameters)
        {
            var col = new BoundField();
            col.DataField = par.Name;
            col.HeaderText = par.Name;
            this.grid.Columns.Add(col);
        }
    }

    /// <summary>
    /// Dynamically add controls to the form
    /// </summary>
    private void CreateFormFields()
    {
        foreach (var par in this._settings.Parameters)
        {
            this.ErrorMsg.InnerHtml = string.Empty;
            this.ErrorMsg.Visible = false;
            this.InfoMsg.InnerHtml = string.Empty;
            this.InfoMsg.Visible = false;

            // add label
            if (par.ParamType != ParameterType.Boolean)
            {
                this.AddLabel(par.Label, string.Empty);
            }

            if (par.ParamType == ParameterType.Boolean)
            {
                // add checkbox
                var cb = new CheckBox();
                cb.Checked = false;
                cb.ID = par.Name;
                cb.CssClass = "mgrCheck";
                this.phAddForm.Controls.Add(cb);
                this.AddLabel(par.Label, "mgrCheckLbl");
            }
            else if (par.ParamType == ParameterType.DropDown)
            {
                // add dropdown
                var dd = new DropDownList();
                foreach (var item in par.Values)
                {
                    dd.Items.Add(item);
                }

                dd.SelectedValue = par.SelectedValue;
                dd.ID = par.Name;
                dd.Width = 250;
                this.phAddForm.Controls.Add(dd);
            }
            else if (par.ParamType == ParameterType.ListBox)
            {
                var lb = new ListBox();
                lb.Rows = par.Values.Count;
                foreach (var item in par.Values)
                {
                    lb.Items.Add(item);
                }

                lb.SelectedValue = par.SelectedValue;
                lb.ID = par.Name;
                lb.Width = 250;
                this.phAddForm.Controls.Add(lb);
            }
            else if (par.ParamType == ParameterType.RadioGroup)
            {
                var rbl = new RadioButtonList();
                foreach (var item in par.Values)
                {
                    rbl.Items.Add(item);
                }

                rbl.SelectedValue = par.SelectedValue;
                rbl.ID = par.Name;
                rbl.RepeatDirection = RepeatDirection.Horizontal;
                rbl.CssClass = "mgrRadioList";
                this.phAddForm.Controls.Add(rbl);
            }
            else
            {
                // add textbox
                var bx = new TextBox();
                bx.Text = string.Empty;
                bx.ID = par.Name;
                bx.Width = new Unit(250);
                bx.MaxLength = par.MaxLength;
                this.phAddForm.Controls.Add(bx);
            }

            using (var br2 = new Literal { Text = @"<br />" })
            {
                this.phAddForm.Controls.Add(br2);
            }
        }
    }

    /// <summary>
    /// Adds the label.
    /// </summary>
    /// <param name="txt">The text.</param>
    /// <param name="cls">The css class.</param>
    private void AddLabel(string txt, string cls)
    {
        using (var lbl = new Label())
        {
            lbl.Width = new Unit("250");
            lbl.Text = txt;
            if (!string.IsNullOrEmpty(cls))
            {
                lbl.CssClass = cls;
            }

            this.phAddForm.Controls.Add(lbl);
        }

        using (var br = new Literal { Text = @"<br />" })
        {
            this.phAddForm.Controls.Add(br);
        }
    }

    /// <summary>
    /// Validate the form
    /// </summary>
    /// <returns>
    /// True if valid
    /// </returns>
    private bool IsValidForm()
    {
        var rval = true;
        this.ErrorMsg.InnerHtml = string.Empty;
        foreach (Control ctl in this.phAddForm.Controls)
        {
            if (ctl.GetType().Name == "TextBox")
            {
                var txt = (TextBox)ctl;

                // check if required
                if (this._settings.IsRequiredParameter(txt.ID) && string.IsNullOrEmpty(txt.Text.Trim()))
                {
                    this.ErrorMsg.InnerHtml = string.Format("\"{0}\" {1}", this._settings.GetLabel(txt.ID), labels.isRequiredField);
                    this.ErrorMsg.Visible = true;
                    rval = false;
                    break;
                }

                // check data type
                if (!string.IsNullOrEmpty(txt.Text) && !this.ValidateType(txt.ID, txt.Text))
                {
                    this.ErrorMsg.InnerHtml = string.Format("\"{0}\" must be a {1}", this._settings.GetLabel(txt.ID), this._settings.GetParameterType(txt.ID));
                    this.ErrorMsg.Visible = true;
                    rval = false;
                    break;
                }

                if (!this._settings.Scalar)
                {
                    if (this._settings.KeyField == txt.ID && this._settings.IsKeyValueExists(txt.Text.Trim()))
                    {
                        this.ErrorMsg.InnerHtml = string.Format("\"{0}\" {1}", txt.Text, labels.isAlreadyExists);
                        this.ErrorMsg.Visible = true;
                        rval = false;
                        break;
                    }
                }
            }
        }

        return rval;
    }

    /// <summary>
    /// The validate type.
    /// </summary>
    /// <param name="parameterName">
    /// The parameter name.
    /// </param>
    /// <param name="val">
    /// The val.
    /// </param>
    /// <returns>
    /// The validate type.
    /// </returns>
    protected bool ValidateType(string parameterName, object val)
    {
        var retVal = true;
        try
        {
            switch (this._settings.GetParameterType(parameterName))
            {
                case ParameterType.Boolean:
                    bool.Parse(val.ToString());
                    break;
                case ParameterType.Integer:
                    int.Parse(val.ToString());
                    break;
                case ParameterType.Long:
                    long.Parse(val.ToString());
                    break;
                case ParameterType.Float:
                    float.Parse(val.ToString());
                    break;
                case ParameterType.Double:
                    double.Parse(val.ToString());
                    break;
                case ParameterType.Decimal:
                    decimal.Parse(val.ToString());
                    break;
            }
        }
        catch (Exception)
        {
            retVal = false;
        }

        return retVal;
    }

    /// <summary>
    /// Gets a handle on grid data just before
    ///     bound them to grid view
    /// </summary>
    /// <param name="sender">
    /// Grid view
    /// </param>
    /// <param name="e">
    /// Event args
    /// </param>
    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        AddConfirmDelete((GridView)sender, e);
    }

    /// <summary>
    /// Adds confirmation box to delete buttons
    ///     in the data grid
    /// </summary>
    /// <param name="gv">
    /// Data grid view
    /// </param>
    /// <param name="e">
    /// Event args
    /// </param>
    protected static void AddConfirmDelete(GridView gv, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }

        foreach (var dcf in
            e.Row.Cells.Cast<DataControlFieldCell>().Where(dcf => string.IsNullOrEmpty(dcf.Text.Trim())))
        {
            foreach (var deleteButton in
                dcf.Controls.Cast<Control>().Select(ctrl => ctrl as LinkButton).Where(deleteButton => deleteButton != null && deleteButton.Text == labels.delete))
            {
                deleteButton.Attributes.Add("onClick", string.Format("return confirm('{0}');", labels.areYouSureDeleteRow));
                break;
            }

            break;
        }
    }
}