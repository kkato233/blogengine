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
			BindText();

		base.Name = "TextBox";
	}

	private void BindText()
	{
		if (Cache["textbox_" + base.WidgetID] == null)
		{
			XmlNode node = base.Xml.SelectSingleNode("content");
			string content = "<content></content>";
			if (node != null)
			{
				content = node.InnerText.Replace("[root]", Utils.RelativeWebRoot);				
			}

			Cache["textbox_" + base.WidgetID] = content;
		}

		LiteralControl text = new LiteralControl((string)Cache["textbox_" + base.WidgetID]);
		this.Controls.Add(text);
	}
}
