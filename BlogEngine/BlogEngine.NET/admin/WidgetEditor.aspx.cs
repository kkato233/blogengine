#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Hosting;
using System.Xml;
using System.IO;
using BlogEngine.Core;

#endregion

public partial class User_controls_WidgetEditor : System.Web.UI.Page
{

	#region Event handlers

	/// <summary>
	/// Handles the Load event of the Page control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!User.IsInRole(BlogSettings.Instance.AdministratorRole))
		{
			Response.StatusCode = 403;
			Response.Clear();
			Response.End();
		}

		string widget = Request.QueryString["widget"];
		string id = Request.QueryString["id"];
		string move = Request.QueryString["move"];
		string add = Request.QueryString["add"];
		string remove = Request.QueryString["remove"];

		if (!string.IsNullOrEmpty(widget) && !string.IsNullOrEmpty(id))
			InitEditor(widget, id);

		if (!string.IsNullOrEmpty(move))
			MoveWidgets(move);

		if (!string.IsNullOrEmpty(add))
			AddWidget(add);

		if (!string.IsNullOrEmpty(remove) && remove.Length == 36)
			RemoveWidget(remove);
	}

	/// <summary>
	/// Handles the Click event of the btnSave control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	private void btnSave_Click(object sender, EventArgs e)
	{
		WidgetEditBase widget = (WidgetEditBase)FindControl("widget");
		if (widget != null)
			widget.Save();

		XmlDocument doc = GetXmlDocument();
		XmlNode node = doc.SelectSingleNode("//widget[@id=\"" + Request.QueryString["id"] + "\"]");
		bool isChanged = false;

		if (node.Attributes["title"].InnerText != txtTitle.Text.Trim())
		{
			node.Attributes["title"].InnerText = txtTitle.Text.Trim();
			isChanged = true;
		}

		if (node.Attributes["showTitle"].InnerText != cbShowTitle.Checked.ToString())
		{
			node.Attributes["showTitle"].InnerText = cbShowTitle.Checked.ToString();
			isChanged = true;
		}

		if (isChanged)
			SaveXmlDocument(doc);

		WidgetEditBase.OnSaved();
		Cache.Remove("widget_" + Request.QueryString["id"]);

		string script = "top.location.href = top.location.href";
		Page.ClientScript.RegisterStartupScript(this.GetType(), "closeWindow", script, true);
	}

	#endregion

	/// <summary>
	/// Removes the widget from the XML file.
	/// </summary>
	/// <param name="id">The id of the widget to remove.</param>
	private void RemoveWidget(string id)
	{
		XmlDocument doc = GetXmlDocument();

		XmlNode node = doc.SelectSingleNode("//widget[@id=\"" + id + "\"]");
		if (node != null)
		{
			node.ParentNode.RemoveChild(node);
			SaveXmlDocument(doc);

            string fileName = Server.MapPath(BlogSettings.Instance.StorageLocation + "widgets/" + id + ".xml");
			if (File.Exists(fileName))
				File.Delete(fileName);

			WidgetEditBase.OnSaved();
		}
	}

	/// <summary>
	/// Adds a widget of the specified type.
	/// </summary>
	/// <param name="type">The type of widget.</param>
	private void AddWidget(string type)
	{
		WidgetBase widget = (WidgetBase)LoadControl(Utils.RelativeWebRoot + "widgets/" + type + "/widget.ascx");
		widget.WidgetID = Guid.NewGuid();
		widget.Name = type;
		widget.Title = "New widget";
		widget.ShowTitle = cbShowTitle.Checked;

		SaveNewWidget(widget);

		Response.Clear();
		using (StringWriter sw = new StringWriter())
		{
			widget.RenderControl(new HtmlTextWriter(sw));
			Response.Write(sw);
		}

		WidgetEditBase.OnSaved();
		Response.End();
	}

	/// <summary>
	/// Saves the new widget to the XML file.
	/// </summary>
	/// <param name="widget">The widget to add.</param>
	private void SaveNewWidget(WidgetBase widget)
	{
		XmlDocument doc = GetXmlDocument();
		XmlNode node = doc.CreateElement("widget");
		node.InnerText = widget.Name;

		XmlAttribute id = doc.CreateAttribute("id");
		id.InnerText = widget.WidgetID.ToString();
		node.Attributes.Append(id);

		XmlAttribute title = doc.CreateAttribute("title");
		title.InnerText = widget.Title;
		node.Attributes.Append(title);

		XmlAttribute show = doc.CreateAttribute("showTitle");
		show.InnerText = "True";
		node.Attributes.Append(show);

		doc.SelectSingleNode("widgets").AppendChild(node);
		SaveXmlDocument(doc);
	}

	/// <summary>
	/// Moves the widgets as specified while dragging and dropping.
	/// </summary>
	/// <param name="move">The move string.</param>
	private void MoveWidgets(string move)
	{
		XmlDocument doc = GetXmlDocument();
		string[] ids = move.Split(';');

		for (int i = 0; i < ids.Length; i++)
		{
			string id = ids[i];
			XmlNode node = doc.SelectSingleNode("//widget[@id=\"" + id + "\"]");
			XmlNode parent = node.ParentNode;

			parent.RemoveChild(node);
			parent.AppendChild(node);
		}

		SaveXmlDocument(doc);
		WidgetEditBase.OnSaved();
	}

	#region Helper methods

    private static readonly string FILE_NAME = HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "widgetzone.xml");

	/// <summary>
	/// Gets the XML document.
	/// </summary>
	/// <returns></returns>
	private XmlDocument GetXmlDocument()
	{
		XmlDocument doc = new XmlDocument();
		doc.Load(FILE_NAME);
		return doc;
	}

	/// <summary>
	/// Saves the XML document.
	/// </summary>
	/// <param name="doc">The doc.</param>
	private void SaveXmlDocument(XmlDocument doc)
	{
        //string fileName = Server.MapPath(Utils.RelativeWebRoot + "App_Data/widgetzone.xml");
        string fileName = Server.MapPath(BlogSettings.Instance.StorageLocation + "widgetzone.xml");
		doc.Save(FILE_NAME);
	}

	/// <summary>
	/// Inititiates the editor for widget editing.
	/// </summary>
	/// <param name="type">The type of widget to edit.</param>
	/// <param name="id">The id of the particular widget to edit.</param>
	private void InitEditor(string type, string id)
	{
		XmlDocument doc = GetXmlDocument();
		XmlNode node = doc.SelectSingleNode("//widget[@id=\"" + id + "\"]");
		string fileName = Utils.RelativeWebRoot + "widgets/" + type + "/edit.ascx";

		if (File.Exists(Server.MapPath(fileName)))
		{
			WidgetEditBase edit = (WidgetEditBase)LoadControl(fileName);
			edit.WidgetID = new Guid(node.Attributes["id"].InnerText);
			edit.Title = node.Attributes["title"].InnerText;
			edit.ID = "widget";
			edit.ShowTitle = bool.Parse(node.Attributes["showTitle"].InnerText);
			phEdit.Controls.Add(edit);
		}

		if (!Page.IsPostBack)
		{
			cbShowTitle.Checked = bool.Parse(node.Attributes["showTitle"].InnerText);
			txtTitle.Text = node.Attributes["title"].InnerText;
			txtTitle.Focus();
			btnSave.Text = Resources.labels.save;
		}

		btnSave.Click += new EventHandler(btnSave_Click);
	}

	#endregion

}
