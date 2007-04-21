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

public partial class admin_Pages_Controls : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
      BindSettings();

    btnSave.Click += new EventHandler(btnSave_Click);
  }

  void btnSave_Click(object sender, EventArgs e)
  {
    BlogSettings.Instance.NumberOfRecentPosts = int.Parse(txtNumberOfPosts.Text);
    BlogSettings.Instance.SearchButtonText = txtSearchButtonText.Text;
    BlogSettings.Instance.SearchCommentLabelText = txtCommentLabelText.Text;
    BlogSettings.Instance.SearchDefaultText = txtDefaultSearchText.Text;
    BlogSettings.Instance.EnableCommentSearch = cbEnableCommentSearch.Checked;

    BlogSettings.Instance.Save();
  }

  private void BindSettings()
  {
    txtNumberOfPosts.Text = BlogSettings.Instance.NumberOfRecentPosts.ToString();
    txtSearchButtonText.Text = BlogSettings.Instance.SearchButtonText;
    txtCommentLabelText.Text = BlogSettings.Instance.SearchCommentLabelText;
    txtDefaultSearchText.Text = BlogSettings.Instance.SearchDefaultText;
    cbEnableCommentSearch.Checked = BlogSettings.Instance.EnableCommentSearch;
  }
}
