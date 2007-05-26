using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using BlogEngine.Core;

public partial class admin_Pages_referrers : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      if (BlogSettings.Instance.EnableReferrerTracking)
      {
        BindDays();
        BindReferrers(DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture));
      }
      else
      {
        ddlDays.Enabled = false;
      }

      cbEnableReferrers.Checked = BlogSettings.Instance.EnableReferrerTracking;
    }

    ddlDays.SelectedIndexChanged += new EventHandler(ddlDays_SelectedIndexChanged);
    cbEnableReferrers.CheckedChanged += new EventHandler(cbEnableReferrers_CheckedChanged);
  }

  private void cbEnableReferrers_CheckedChanged(object sender, EventArgs e)
  {
    if (cbEnableReferrers.Checked)
    {
      BindDays();
      BindReferrers(DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture));
    }
    else
    {
      ddlDays.Enabled = false;
    }

    BlogSettings.Instance.EnableReferrerTracking = cbEnableReferrers.Checked;
    BlogSettings.Instance.Save();
  }

  void ddlDays_SelectedIndexChanged(object sender, EventArgs e)
  {
    BindReferrers(ddlDays.SelectedValue);
  }

  private void BindDays()
  {
    //string folder = Server.MapPath("~/app_data/log/");
    //if (Directory.Exists(folder))
    //{
    //  foreach (string file in Directory.GetFiles(folder, "*.xml"))
    //  {
    //    string name = new FileInfo(file).Name;
    //    name = name.Substring(0, name.Length - 4);
    //    ddlDays.Items.Add(name);
    //  }

    //  ddlDays.Enabled = true;
    //  ddlDays.ClearSelection();

      foreach (ListItem item in ddlDays.Items)
      {
        if (item.Text == DateTime.Now.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture))
        {
          item.Selected = true;
          break;
        }
      }
    //}
  }

  private void BindReferrers(string day)
  {
    string filename = Server.MapPath("~/app_data/log/" + day + ".xml");
    if (File.Exists(filename))
    {
      DataSet ds = new DataSet();
      ds.ReadXml(filename);

      DataTable table = new DataTable();
      table.Columns.Add("url", typeof(string));
      table.Columns.Add("shortUrl", typeof(string));
      table.Columns.Add("hits", typeof(int));
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        DataRow newRow = table.NewRow();
        newRow["url"] = Server.HtmlEncode(row["address"].ToString());
        newRow["shortUrl"] = MakeShortUrl(row["address"].ToString());
        newRow["hits"] = int.Parse(row["url_text"].ToString());
        table.Rows.Add(newRow);
      }

      DataView view = new DataView(table);
      view.Sort = "hits desc";

      grid.DataSource = view;
      grid.DataBind();
      PaintRows(3);
    }
  }

  private string MakeShortUrl(string url)
  {
    if (url.Length > 150)
      return url.Substring(0, 150) + "...";

    return Server.HtmlEncode( url.Replace("http://", string.Empty).Replace("www.", string.Empty));
  }

  /// <summary>
  /// Paints the background color of the alternate rows
  /// in the gridview.
  /// </summary>
  private void PaintRows(int alternateRows)
  {
    int count = 0;
    for (int i = 0; i < grid.Controls[0].Controls.Count - 1; i++)
    {
      if (count > alternateRows)
        (grid.Controls[0].Controls[i] as WebControl).CssClass = "alt";

      count++;

      if (count == alternateRows + alternateRows + 1)
        count = 1;
    }
  }

}
