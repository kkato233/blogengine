#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using BlogEngine.Core;

#endregion

public partial class widgets_LinkList_edit : WidgetEditBase
{

	/// <summary>
	/// Handles the Load event of the Page control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			XmlNode node = Xml.SelectSingleNode("content");
			if (node!= null)
			{
				txtText.Text = node.InnerText;
			}			
		}
	}

	/// <summary>
	/// Saves this the basic widget settings such as the Title.
	/// </summary>
	public override void Save()
	{
		if (Xml.ChildNodes.Count == 0)
		{
			XmlNode node = Xml.CreateElement("content");
			Xml.AppendChild(node);			
		}

		Xml.SelectSingleNode("content").InnerText = txtText.Text;

		SaveXml();
		Cache.Remove("textbox_" + WidgetID);
	}
}
