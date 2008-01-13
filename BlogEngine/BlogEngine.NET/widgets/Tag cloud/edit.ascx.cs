#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

#endregion

public partial class widgets_Tag_cloud_edit : WidgetEditBase
{

	protected override void OnLoad(EventArgs e)
	{
		XmlNode node = Xml.SelectSingleNode("//minimumPosts");
		if (node != null)
			ddlNumber.SelectedValue = node.InnerText;
	}

	public override void Save()
	{
		XmlNode node = Xml.SelectSingleNode("//minimumPosts");
		if (node == null)
		{
			XmlNode top = Xml.CreateElement("tagcloud");
			Xml.AppendChild(top);

			XmlNode posts = Xml.CreateElement("minimumPosts");
			posts.InnerText = ddlNumber.SelectedValue;
			top.AppendChild(posts);
		}
		else
		{
			node.InnerText = ddlNumber.SelectedValue;
		}

		SaveXml();
		widgets_Tag_cloud_widget.Reload();
	}
}
