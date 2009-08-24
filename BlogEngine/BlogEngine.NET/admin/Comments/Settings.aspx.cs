﻿using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using BlogEngine.Core;

public partial class admin_Comments_Settings : System.Web.UI.Page
{
    static protected ExtensionSettings _filters;

    protected void Page_Load(object sender, EventArgs e)
    {
        _filters = ExtensionManager.GetSettings("CommentFilters", "Filters");

        if (!IsPostBack)
        {
            BindSettings();
            BindFilters();
        }

        Page.MaintainScrollPositionOnPostBack = true;
        Page.Title = Resources.labels.comments;

        btnSave.Click += btnSave_Click;
        //btnSaveTop.Click += new EventHandler(btnSave_Click);

        //btnSaveTop.Text = Resources.labels.saveSettings;
        btnSave.Text = Resources.labels.saveSettings;
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

    private void btnSave_Click(object sender, EventArgs e)
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
        ExtensionManager.SaveSettings("CommentFilters", _filters);
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
            ExtensionManager.SaveSettings("CommentFilters", _filters);
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

    protected void RadioNone_CheckedChanged(object sender, EventArgs e)
    {
        ShowHide();
    }
    protected void RadioWaegis_CheckedChanged(object sender, EventArgs e)
    {
        ShowHide();
    }
    protected void RadioAkismet_CheckedChanged(object sender, EventArgs e)
    {
        ShowHide();
    }

    protected void ShowHide()
    {
        if (RadioNone.Checked)
        {
            SpamExtension.Visible = false;
            ReportMistakes.Visible = false;
        }
        else
        {
            SpamExtension.Visible = true;
            ReportMistakes.Visible = true;
        }
    }
}