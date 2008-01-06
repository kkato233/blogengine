#region Using

using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using BlogEngine.Core;

#endregion

public partial class widgets_LinkList_widget : WidgetBase
{
	/// <summary>
	/// Handles the Load event of the Page control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
			BindLinks();

		base.Name = "LinkList";
	}

	private void BindLinks()
	{
		XmlNodeList links = base.Xml.SelectNodes("//link");

		if (links.Count == 0)
		{
			ulLinks.Visible = false;
		}
		else
		{
			foreach (XmlNode node in links)
			{
				HtmlAnchor a = new HtmlAnchor();
					
				if (node.Attributes["url"] != null)
					a.HRef = node.Attributes["url"].InnerText;

				if (node.Attributes["title"] != null)
					a.InnerText = node.Attributes["title"].InnerText;

				HtmlGenericControl li = new HtmlGenericControl("li");
				li.Controls.Add(a);
				ulLinks.Controls.Add(li);
			}
		}
	}
}
