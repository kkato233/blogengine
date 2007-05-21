using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

public partial class archive : BlogEngine.Core.Web.Controls.BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack && !IsCallback)
    {
      CreateAdminMenu();
      CreateArchive();
    }
  }

  private void CreateAdminMenu()
  {
    SortedDictionary<string, Guid> dic = SortCategories(CategoryDictionary.Instance);

    foreach (string cat in dic.Keys)
    {
      HtmlAnchor a = new HtmlAnchor();
      a.InnerHtml = cat;
      a.HRef = "#" + Utils.RemoveIlegalCharacters(cat);

      HtmlGenericControl li = new HtmlGenericControl("li");
      li.Controls.Add(a);
      ulMenu.Controls.Add(li);
    }
  }

  private SortedDictionary<string, Guid> SortCategories(Dictionary<Guid, string> categories)
  {
    SortedDictionary<string, Guid> dic = new SortedDictionary<string, Guid>();
    foreach (Guid key in categories.Keys)
    {
        dic.Add(categories[key], key);
    }

    return dic;
  }

  private void CreateArchive()
  {
    SortedDictionary<string, Guid> dic = SortCategories(CategoryDictionary.Instance);
    foreach (Guid key in dic.Values)
    {
      string name = CategoryDictionary.Instance[key];
      List<Post> list = Post.GetPostsByCategory(key);      

      HtmlAnchor feed = new HtmlAnchor();
      feed.HRef = "~/category/syndication.axd?category=" + key.ToString();

      HtmlImage img = new HtmlImage();
      img.Src = "~/pics/rssButton.gif";
      img.Alt = "";

      feed.Controls.Add(img);

      HtmlGenericControl h2 = new HtmlGenericControl("h2");
      h2.Attributes["id"] = Utils.RemoveIlegalCharacters(name);
      h2.Controls.Add(feed);

      Control header = new LiteralControl(name + " (" + list.Count + ")");
      h2.Controls.Add(header);

      phArchive.Controls.Add(h2);

      foreach (Post post in list)
      {
        HtmlGenericControl span = new HtmlGenericControl("span");
        Control date = new LiteralControl(post.DateCreated.ToString("yyyy-MM-dd"));
        HtmlAnchor a = new HtmlAnchor();
        a.InnerHtml = Server.HtmlEncode( post.Title);
        a.HRef = post.RelativeLink.ToString();

        span.Controls.Add(date);
        span.Controls.Add(a);
        phArchive.Controls.Add(span);
      }
    }
  }
}
