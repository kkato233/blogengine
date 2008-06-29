using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class widgets_Search_widget : WidgetBase
{

	public override string Name
	{
		get { return "Search"; }
	}

	public override bool IsEditable
	{
		get { return false; }
	}

	public override void LoadWidget()
	{
		// Nothing to load
	}
}
