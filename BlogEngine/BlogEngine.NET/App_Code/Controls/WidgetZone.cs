#region Using

using System;
using System.Web.UI.WebControls;
using System.Threading;
using System.Xml;
using System.Web;
using System.Web.Hosting;
using BlogEngine.Core;
using System.IO;

#endregion

namespace Controls
{
	public class WidgetZone : PlaceHolder
	{

		static WidgetZone()
		{
			if (XML_DOCUMENT == null)
				XML_DOCUMENT = RetrieveXml();

			WidgetEditBase.Saved += delegate { XML_DOCUMENT = RetrieveXml(); };
		}

		private static XmlDocument XML_DOCUMENT = RetrieveXml();

		private static XmlDocument RetrieveXml()
		{
			XmlDocument doc = new XmlDocument();
            doc.Load(HostingEnvironment.MapPath(BlogSettings.Instance.StorageLocation + "widgetzone.xml"));
			return doc;
		}

		/// <summary>
		/// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see>
		/// object and stores tracing information about the control if tracing is enabled.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HTmlTextWriter"></see> object that receives the control content.</param>
		public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
		{
			XmlNodeList zone = XML_DOCUMENT.SelectNodes("//widget");
			foreach (XmlNode widget in zone)
			{
				string fileName = Utils.RelativeWebRoot + "widgets/" + widget.InnerText + "/widget.ascx";
				WidgetBase control = (WidgetBase)Page.LoadControl(fileName);
				control.WidgetID = new Guid(widget.Attributes["id"].InnerText);
				control.Title = widget.Attributes["title"].InnerText;
				control.ShowTitle = bool.Parse(widget.Attributes["showTitle"].InnerText);
				control.LoadWidget();
				this.Controls.Add(control);
			}

			base.RenderControl(writer);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<div id=\"widgetzone\">");
			
			base.Render(writer);

			writer.Write("</div>");

			if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
				writer.Write("<select id=\"widgetselector\">");
				DirectoryInfo di = new DirectoryInfo(Page.Server.MapPath(Utils.RelativeWebRoot + "widgets"));
				foreach (DirectoryInfo dir in di.GetDirectories())
				{
					writer.Write("<option value=\"" + dir.Name + "\">" + dir.Name + "</option>");
				}

				writer.Write("</select>&nbsp;&nbsp;");
				writer.Write("<input type=\"button\" value=\"Add\" onclick=\"addWidget($('widgetselector').value)\" />");
				writer.Write("<div class=\"clear\" id=\"clear\">&nbsp;</div>");
			}
		}

	}
}
