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

    Page.Title = Resources.labels.archive;
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
      img.Src = "~/pics/rssbutton.gif";
      img.Alt = "RSS";

      feed.Controls.Add(img);

      HtmlGenericControl h2 = new HtmlGenericControl("h2");
      h2.Attributes["id"] = Utils.RemoveIlegalCharacters(name);
      h2.Controls.Add(feed);

      Control header = new LiteralControl(name + " (" + list.Count + ")");
      h2.Controls.Add(header);

      phArchive.Controls.Add(h2);

      HtmlTable table = CreateTable(name);     

      
      foreach (Post post in list)
      {
        HtmlTableRow row = new HtmlTableRow();
        
        HtmlTableCell date = new HtmlTableCell();
        date.InnerHtml = post.DateCreated.ToString("yyyy-MM-dd");
        date.Attributes.Add("class", "date");
        row.Cells.Add(date);

        HtmlTableCell title = new HtmlTableCell();
        title.InnerHtml = string.Format("<a href=\"{0}\">{1}</a>", post.RelativeLink, post.Title);
        title.Attributes.Add("class", "title");
        row.Cells.Add(title);

        if (BlogSettings.Instance.IsCommentsEnabled)
        {
          HtmlTableCell comments = new HtmlTableCell();
          comments.InnerHtml = post.Comments.Count.ToString();
          comments.Attributes.Add("class", "comments");
          row.Cells.Add(comments);
        }

        if (BlogSettings.Instance.EnableRating)
        {
          HtmlTableCell rating = new HtmlTableCell();
          rating.InnerHtml = post.Raters == 0 ? "None" : Math.Round(post.Rating, 1).ToString();
          rating.Attributes.Add("class", "rating");
          row.Cells.Add(rating);
        }

        table.Rows.Add(row);
      }

      phArchive.Controls.Add(table);
    }
  }

  private HtmlTable CreateTable(string name)
  {
    HtmlTable table = new HtmlTable();
    table.Attributes.Add("summary", name);

    HtmlTableRow header = new HtmlTableRow();

    HtmlTableCell date = new HtmlTableCell("th");
    date.InnerHtml = base.Translate("date");
    header.Cells.Add(date);

    HtmlTableCell title = new HtmlTableCell("th");
    title.InnerHtml = base.Translate("title");
    header.Cells.Add(title);

    if (BlogSettings.Instance.IsCommentsEnabled)
    {
      HtmlTableCell comments = new HtmlTableCell("th");
      comments.InnerHtml = base.Translate("comments");
      comments.Attributes.Add("class", "comments");
      header.Cells.Add(comments);
    }

    if (BlogSettings.Instance.EnableRating)
    {
      HtmlTableCell rating = new HtmlTableCell("th");
      rating.InnerHtml = base.Translate("rating");
      rating.Attributes.Add("class", "rating");
      header.Cells.Add(rating);
    }

    table.Rows.Add(header);

    return table;
  }
}
