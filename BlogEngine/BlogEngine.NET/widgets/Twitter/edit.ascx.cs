#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

#endregion

public partial class widgets_Twitter_edit : WidgetEditBase
{
    private const string CACHE_KEY = "twits";  // same key used in widget.ascx.cs.

	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        { 
		    StringDictionary settings = GetSettings();
		    if (settings.ContainsKey("feedurl"))
		    {
			    txtUrl.Text = settings["feedurl"];
			    txtAccountUrl.Text = settings["accounturl"];
			    txtTwits.Text = settings["maxitems"];
                txtPolling.Text = settings["pollinginterval"];
                txtFollowMe.Text = settings["followmetext"];
		    }
        }
	}

	public override void Save()
	{
		StringDictionary settings = GetSettings();		
		settings["feedurl"] = txtUrl.Text;
		settings["accounturl"] = txtAccountUrl.Text;
		settings["maxitems"] = txtTwits.Text;
        settings["pollinginterval"] = txtPolling.Text;
        settings["followmetext"] = txtFollowMe.Text;
		SaveSettings(settings);

        HttpRuntime.Cache.Remove(CACHE_KEY);
	}
}
