using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class widgets_BlogRoll_widget : WidgetBase
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Name = "Blogroll";
	}
}
