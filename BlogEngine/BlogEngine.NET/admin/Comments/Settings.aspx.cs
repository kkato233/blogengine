using System;
using System.Web.UI.WebControls;
using BlogEngine.Core;

public partial class admin_Comments_Settings : System.Web.UI.Page
{
    static protected ExtensionSettings _filters;
    static protected ExtensionSettings _customFilters;

    public string RadioChecked(int mtype)
    {
        if (BlogSettings.Instance.ModerationType == mtype)
            return "checked";
        return string.Empty;
    }

    public bool CustomFilterEnabled(string filter)
    {
        return ExtensionManager.ExtensionEnabled(filter);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _filters = ExtensionManager.GetSettings("MetaExtension", "BeCommentFilters");
        _customFilters = ExtensionManager.GetSettings("MetaExtension", "BeCustomFilters");

        if (!IsPostBack)
        {
            BindSettings();
            BindFilters();
            BindCustomFilters();           

            string js = "<script type='text/javascript'>ToggleEnableComments();</script>";
            ClientScript.RegisterStartupScript(GetType(), "ClientScript1", js);
        }

        Page.MaintainScrollPositionOnPostBack = true;
        Page.Title = Resources.labels.comments;

        btnSave.Click += btnSave_Click;
        btnSave.Text = Resources.labels.saveSettings;
        
        //btnSaveTop.Click += new EventHandler(btnSave_Click);
        //btnSaveTop.Text = Resources.labels.saveSettings;
    }

    private void BindSettings()
    {
        //-----------------------------------------------------------------------
        // Bind Comments settings
        //-----------------------------------------------------------------------
        cbEnableComments.Checked = BlogSettings.Instance.IsCommentsEnabled;
        cbEnableCommentNesting.Checked = BlogSettings.Instance.IsCommentNestingEnabled;
        cbEnableCountryInComments.Checked = BlogSettings.Instance.EnableCountryInComments;
        cbEnableCoComment.Checked = BlogSettings.Instance.IsCoCommentEnabled;
        cbShowLivePreview.Checked = BlogSettings.Instance.ShowLivePreview;
        ddlCloseComments.SelectedValue = BlogSettings.Instance.DaysCommentsAreEnabled.ToString();
        cbEnableCommentsModeration.Checked = BlogSettings.Instance.EnableCommentsModeration;
        rblAvatar.SelectedValue = BlogSettings.Instance.Avatar;
        ddlCommentsPerPage.SelectedValue = BlogSettings.Instance.CommentsPerPage.ToString();
        // rules
        cbTrustAuthenticated.Checked = BlogSettings.Instance.TrustAuthenticatedUsers;
        ddWhiteListCount.SelectedValue = BlogSettings.Instance.CommentWhiteListCount.ToString();
        ddBlackListCount.SelectedValue = BlogSettings.Instance.CommentBlackListCount.ToString();
    }

    protected void BindFilters()
    {
        gridFilters.DataKeyNames = new string[] { _filters.KeyField };
        gridFilters.DataSource = _filters.GetDataTable();
        gridFilters.DataBind();
    }

    protected void BindCustomFilters()
    {
        gridCustomFilters.DataKeyNames = new string[] { _customFilters.KeyField };
        gridCustomFilters.DataSource = _customFilters.GetDataTable();
        gridCustomFilters.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //-----------------------------------------------------------------------
        // Set Comments settings
        //-----------------------------------------------------------------------
        BlogSettings.Instance.IsCommentsEnabled = cbEnableComments.Checked;
        BlogSettings.Instance.IsCommentNestingEnabled = cbEnableCommentNesting.Checked;
        BlogSettings.Instance.EnableCountryInComments = cbEnableCountryInComments.Checked;
        BlogSettings.Instance.IsCoCommentEnabled = cbEnableCoComment.Checked;
        BlogSettings.Instance.ShowLivePreview = cbShowLivePreview.Checked;
        BlogSettings.Instance.DaysCommentsAreEnabled = int.Parse(ddlCloseComments.SelectedValue);
        BlogSettings.Instance.EnableCommentsModeration = cbEnableCommentsModeration.Checked;
        BlogSettings.Instance.Avatar = rblAvatar.SelectedValue;
        BlogSettings.Instance.CommentsPerPage = int.Parse(ddlCommentsPerPage.SelectedValue);
        BlogSettings.Instance.ModerationType = int.Parse(Request.Form["RadioGroup1"]);
        // rules
        BlogSettings.Instance.TrustAuthenticatedUsers = cbTrustAuthenticated.Checked;
        BlogSettings.Instance.CommentWhiteListCount = int.Parse(ddWhiteListCount.SelectedValue);
        BlogSettings.Instance.CommentBlackListCount = int.Parse(ddBlackListCount.SelectedValue);

        //-----------------------------------------------------------------------
        //  Persist settings
        //-----------------------------------------------------------------------
        BlogSettings.Instance.Save();

        Response.Redirect(Request.RawUrl, true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

        foreach (ExtensionParameter par in _filters.Parameters)
        {
            par.DeleteValue(grdRow.RowIndex);
        }
        ExtensionManager.SaveSettings("MetaExtension", _filters);
        Response.Redirect(Request.RawUrl);
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridFilters.PageIndex = e.NewPageIndex;
        gridFilters.DataBind();
    }

    protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            string id = Guid.NewGuid().ToString();
            string[] f = new string[] { id, 
                    ddAction.SelectedValue, 
                    ddSubject.SelectedValue, 
                    ddOperator.SelectedValue, 
                    txtFilter.Text };

            _filters.AddValues(f);
            ExtensionManager.SaveSettings("MetaExtension", _filters);
            Response.Redirect(Request.RawUrl);
        }
    }

    protected bool ValidateForm()
    {
        if (string.IsNullOrEmpty(txtFilter.Text))
        {
            FilterValidation.InnerHtml = "Filter is a required field";
            return false;
        }

        return true;
    }

    public static string ApprovedCnt(object total, object cought)
    {
        try
        {
            int t = int.Parse(total.ToString());
            int c = int.Parse(cought.ToString());

            int a = t - c;

            return a.ToString();
        }
        catch (Exception)
        {
            return "";
        }
        
    }

    public static string Accuracy(object total, object mistakes)
    {
        try
        {
            int t = int.Parse(total.ToString());
            int m = int.Parse(mistakes.ToString());

            if (m == 0) return "100";

            int a = m / t * 100;

            return a.ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }
}