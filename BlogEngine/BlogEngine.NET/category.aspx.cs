#region Using

using System;
using System.Collections.Generic;
using DotNetSlave.BlogEngine.BusinessLogic;

#endregion

public partial class category : BlogBasePage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!String.IsNullOrEmpty(Request.QueryString["id"]))
    {
      Guid categoryId = new Guid(Request.QueryString["id"]); 
      PostList1.Posts = Post.GetPostsByCategory(categoryId);
      Page.Title = BlogSettings.Instance.Name + " - " + CategoryDictionary.Instance[categoryId];
    }

    base.AddMetaTag("description", BlogSettings.Instance.Description);    
  }
}
