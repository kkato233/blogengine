#region Using

using System;
using System.Web.UI.WebControls;
using System.Threading;
using System.Xml;
using System.Web;
using System.Web.Hosting;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;
using System.IO;
using System.Collections.Generic;

#endregion

namespace Controls
{
	public class WidgetZone : PlaceHolder
	{
		static WidgetZone()
		{
            WidgetEditBase.Saved += delegate { OnZonesUpdated(); };
		}

        private static Dictionary<string, XmlDocument> _xmlDocumentByZone = new Dictionary<string, XmlDocument>();

        private static void OnZonesUpdated()
        {
            _xmlDocumentByZone.Clear();
        }

        private XmlDocument XML_DOCUMENT
        {
            get
            {
                // look up the document by zone name
                if (_xmlDocumentByZone.ContainsKey(ZoneName))
                {
                    return _xmlDocumentByZone[ZoneName];
                }

                return null;
            }
        }

        // For backwards compatibility or if a ZoneName is omitted, provide a default ZoneName.
        private string _ZoneName = "be_WIDGET_ZONE";
        /// <summary>
        /// Gets or sets the name of the data-container used by this instance
        /// </summary>
        public string ZoneName
        {
            get { return _ZoneName; }
            set { _ZoneName = Utils.RemoveIllegalCharacters(value); }
        }

        protected override void OnInit(EventArgs e)
        {
            if (XML_DOCUMENT == null)
            {
                // if there's no document for this zone name yet, load it
                XmlDocument doc = RetrieveXml(ZoneName);
                if (doc != null)
                {
                    _xmlDocumentByZone[ZoneName] = doc;
                }
            }

            base.OnInit(e);
        }

        private static XmlDocument RetrieveXml(string zoneName)
        {
            WidgetSettings ws = new WidgetSettings(zoneName);
            ws.SettingsBehavior = new XMLDocumentBehavior();
            XmlDocument doc = (XmlDocument)ws.GetSettings();
            return doc;
        }

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			XmlNodeList zone = XML_DOCUMENT.SelectNodes("//widget");
			foreach (XmlNode widget in zone)
			{
				string fileName = Utils.RelativeWebRoot + "widgets/" + widget.InnerText + "/widget.ascx";
				try
				{
					WidgetBase control = (WidgetBase)Page.LoadControl(fileName);
					control.WidgetID = new Guid(widget.Attributes["id"].InnerText);
					control.ID = control.WidgetID.ToString().Replace("-", string.Empty);
					control.Title = widget.Attributes["title"].InnerText;
                    control.Zone = _ZoneName;
					
					if (control.IsEditable)
						control.ShowTitle = bool.Parse(widget.Attributes["showTitle"].InnerText);
					else
						control.ShowTitle = control.DisplayHeader;

					control.LoadWidget();
					this.Controls.Add(control);
				}
				catch (Exception ex)
				{
					Literal lit = new Literal();
					lit.Text = "<p style=\"color:red\">Widget " + widget.InnerText + " not found.<p>";
					lit.Text += ex.Message;
                    lit.Text += "<a class=\"delete\" href=\"javascript:void(0)\" onclick=\"BlogEngine.widgetAdmin.removeWidget('" + widget.Attributes["id"].InnerText + "');return false\" title=\"" + Resources.labels.delete + " widget\">X</a>";

					this.Controls.Add(lit);
				}
			}
		}

		/// <summary>
		/// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> 
		/// object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="writer">
		/// The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object 
		/// that receives the server control content.
		/// </param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<div id=\"widgetzone_" + _ZoneName + "\" class=\"widgetzone\">");

			base.Render(writer);

			writer.Write("</div>");

			if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
			{
                string selectorId = "widgetselector_" + _ZoneName;
				writer.Write("<select id=\"" + selectorId + "\" class=\"widgetselector\">");
				DirectoryInfo di = new DirectoryInfo(Page.Server.MapPath(Utils.RelativeWebRoot + "widgets"));
				foreach (DirectoryInfo dir in di.GetDirectories())
				{
					if (File.Exists(Path.Combine(dir.FullName, "widget.ascx")))
						writer.Write("<option value=\"" + dir.Name + "\">" + dir.Name + "</option>");
				}

				writer.Write("</select>&nbsp;&nbsp;");
				writer.Write("<input type=\"button\" value=\"Add\" onclick=\"BlogEngine.widgetAdmin.addWidget(BlogEngine.$('" + selectorId + "').value, '" + _ZoneName + "')\" />");
				writer.Write("<div class=\"clear\" id=\"clear\">&nbsp;</div>");
			}
		}

	}
}
