namespace Admin.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.UI;
    using BlogEngine.Core;
    using BlogEngine.Core.Web.Extensions;

    public partial class Settings : System.Web.UI.Page
    {
        protected List<ManagedExtension> ExtensionList()
        {
            var extensions = ExtensionManager.Extensions.Where(x => x.Key != "MetaExtension").ToList();

            extensions.Sort(
                (e1, e2) => e1.Value.Priority == e2.Value.Priority ? string.CompareOrdinal(e1.Key, e2.Key) : e1.Value.Priority.CompareTo(e2.Value.Priority));

            List<ManagedExtension> manExtensions = new List<ManagedExtension>();

            foreach (KeyValuePair<string, ManagedExtension> ext in extensions)
            {
                var oExt = ExtensionManager.GetExtension(@ext.Key);
                manExtensions.Add(oExt);
            }
            return manExtensions;
        }
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);
            UserControl uc;

            var extname = this.Request.QueryString["ext"];

            foreach (var setting in from x in ExtensionManager.Extensions
                                    where x.Key == extname
                                    from setting in x.Value.Settings
                                    where !string.IsNullOrEmpty(setting.Name) && !setting.Hidden
                                    select setting)
            {
                uc = (UserControl)this.Page.LoadControl("Settings.ascx");
                uc.ID = setting.Name;
                this.ucPlaceHolder.Controls.Add(uc);
            }

            base.OnInit(e);
        }
    }
}