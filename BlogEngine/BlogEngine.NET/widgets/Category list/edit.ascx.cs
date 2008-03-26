#region Using

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using BlogEngine.Core;

#endregion

public partial class widgets_Categories_edit : WidgetEditBase
{
	protected void Page_Load(object sender, EventArgs e)
	{
		StringDictionary settings = (StringDictionary)GetSettings(ObjectType.StringDictionary);
		bool showRssIcon = true;
		bool showPostCount = true;
		if (settings.ContainsKey("showrssicon"))
		{
			bool.TryParse(settings["showrssicon"], out showRssIcon);
			bool.TryParse(settings["showpostcount"], out showPostCount);
		}

		cbShowRssIcon.Checked = showRssIcon;
		cbShowPostCount.Checked = showPostCount;
	}

	public override void Save()
	{
		StringDictionary settings = (StringDictionary)GetSettings(ObjectType.StringDictionary);
		settings["showrssicon"] = cbShowRssIcon.Checked.ToString();
		settings["showpostcount"] = cbShowPostCount.Checked.ToString();
		SaveSettings(settings);
	}
}
