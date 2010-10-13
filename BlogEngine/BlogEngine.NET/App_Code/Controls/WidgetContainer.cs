using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Threading;
using BlogEngine.Core;
using Resources;

namespace Controls
{

    /// <summary>
    /// Themeable class for displaying WidgetBase derived controls.
    /// </summary>
    /// <remarks>
    /// WidgetContainer is meant to be the themeable parent class of any control that derives from WidgetBase. This way a theme can automatically
    /// apply some basic styling to the way widgets are displayed without having to edit each one or edit the WidgetBase class to change the
    /// rendered output.
    /// 
    /// Inherited WidgetContainers must contain a control named phWidgetBody. This is the control that the WidgetContainer's child Widget is 
    /// injected inside of. phWidgetBody just needs to derive from Control to work, leaving flexibility for anyone creating a theme.
    /// 
    /// If phWidgetBody isn't found, an exception isn't thrown, but a warning label is applied to the page. 
    /// 
    /// </remarks>
    public abstract class WidgetContainer : System.Web.UI.UserControl
    {

        #region "Properties"

        /// <summary>
        /// Gets or sets the Widget this WidgetContainer holds.
        /// </summary>
        public WidgetBase Widget
        {
            get;
            set;
        }


        /// <summary>
        /// Gets a string representing the rendered html for administrative control of this WidgetContainer's child Widget.
        /// </summary>
        protected String AdminLinks
        {
            get
            {
                if (Thread.CurrentPrincipal.IsInRole(BlogSettings.Instance.AdministratorRole))
                {
                    if (this.Widget != null)
                    {
                        var sb = new System.Text.StringBuilder();

                        var widgetId = this.Widget.WidgetId;

                        sb.AppendFormat("<a class=\"delete\" href=\"#\" onclick=\"BlogEngine.widgetAdmin.removeWidget('{0}');return false\" title=\"{1} widget\">X</a>", widgetId, labels.delete);

                        sb.AppendFormat("<a class=\"edit\" href=\"#\" onclick=\"BlogEngine.widgetAdmin.editWidget('{0}', '{1}');return false\" title=\"{2} widget\">{3}</a>", this.Widget.Name, widgetId, labels.edit, labels.edit);
                        sb.AppendFormat("<a class=\"move\" href=\"#\" onclick=\"BlogEngine.widgetAdmin.initiateMoveWidget('{0}');return false\" title=\"{1} widget\">{2}</a>", widgetId, labels.move, labels.move);

                        return sb.ToString();
                    }
                }


                return String.Empty;
            }
        }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // phWidgetBody is the control that the Widget control
            // gets added to.
            Control phWidgetBody = this.FindControl("phWidgetBody");

            if (phWidgetBody != null)
            {
                phWidgetBody.Controls.Add(this.Widget);
            }
            else
            {
                var warn = new LiteralControl();
                warn.Text = "Unable to find control with id \"phWidgetBody\" in theme's WidgetContainer.";
                this.Controls.Add(warn);
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Hide the container if the Widget is null or also not visible.
            this.Visible = ((this.Widget != null) && (this.Widget.Visible));

        }
        
    }

    /// <summary>
    /// Default implementation of WidgetContainer that provides backwards compatibility with themes that do not have
    /// their own WidgetContainer user control.
    /// </summary>
    internal sealed class DefaultWidgetContainer : WidgetContainer
    {

        /// <summary>
        /// The phWidgetBody instance needed by all WidgetContainers.
        /// </summary>
        private System.Web.UI.WebControls.PlaceHolder phWidgetBody = new System.Web.UI.WebControls.PlaceHolder(){ID = "phWidgetBody"};

        internal DefaultWidgetContainer() : base()
        {
            this.Controls.Add(this.phWidgetBody);
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (this.Widget == null)
            {
                throw new NullReferenceException("WidgetContainer requires its Widget property be set to a valid WidgetBase derived control");
            }

            String wName = this.Widget.Name;
            Guid widgetId = this.Widget.WidgetId;

            if (string.IsNullOrEmpty(this.Widget.Name))
            {
                throw new NullReferenceException("Name must be set on a widget");
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"widget " + wName.Replace(" ", string.Empty).ToLowerInvariant() + "\" id=\"widget" + widgetId + "\">");
            sb.Append(this.AdminLinks);
            if (this.Widget.ShowTitle)
            {
                sb.Append("<h4>" + this.Widget.Title + "</h4>");
            }
            else
            {
                sb.Append("<br />");
            }

            sb.Append("<div class=\"content\">");

            writer.Write(sb.ToString());
            base.Render(writer);
            writer.Write("</div>");
            writer.Write("</div>");

        }

    }

}