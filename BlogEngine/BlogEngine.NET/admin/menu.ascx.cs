using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class admin_menu : System.Web.UI.UserControl
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsCallback)
      BindMenu();
  }

  private void BindMenu()
  {
    string folder = Server.MapPath("~/admin/pages/");
    foreach (string file in Directory.GetFiles(folder, "*.aspx", SearchOption.TopDirectoryOnly))
    {
      FileInfo info = new FileInfo(file);
      HtmlAnchor a = new HtmlAnchor();
      a.HRef = "~/admin/pages/" + info.Name;
      a.InnerHtml = info.Name.Replace(".aspx", string.Empty).Replace("_", " ");

      if (Request.RawUrl.EndsWith(info.Name, StringComparison.OrdinalIgnoreCase))
        a.Attributes["class"] = "current";

      HtmlGenericControl li = new HtmlGenericControl("li");
      li.Controls.Add(a);
      ulMenu.Controls.Add(li);
    }
  }

  public void AddItem(string text, string url)
  {
    HtmlAnchor a = new HtmlAnchor();
    a.InnerHtml = text;
    a.HRef = url;

    HtmlGenericControl li = new HtmlGenericControl("li");
    li.Controls.Add(a);
    ulMenu.Controls.Add(li);
  }
}
