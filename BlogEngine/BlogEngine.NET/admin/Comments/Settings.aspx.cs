using System;
using System.IO;
using System.Web;
using BlogEngine.Core;

public partial class admin_Comments_Settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSettings();
        }

        Page.MaintainScrollPositionOnPostBack = true;
        Page.Title = Resources.labels.comments;

        btnSave.Click += new EventHandler(btnSave_Click);
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


        //-----------------------------------------------------------------------
        //  Persist settings
        //-----------------------------------------------------------------------
        BlogSettings.Instance.Save();

        Response.Redirect(Request.RawUrl, true);

    }

}
