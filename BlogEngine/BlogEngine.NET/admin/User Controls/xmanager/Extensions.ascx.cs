using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;

public partial class User_controls_xmanager_ExtensionsList : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		lblErrorMsg.InnerHtml = string.Empty;
		lblErrorMsg.Visible = false;
		lblExtensions.Text = GetExtensions();

		object act = Request.QueryString["act"];
		object ext = Request.QueryString["ext"];

		if (act != null && ext != null)
		{
			ChangeStatus(act.ToString(), ext.ToString());
		}

		btnRestart.Click += new EventHandler(btnRestart_Click);
	}

	void btnRestart_Click(object sender, EventArgs e)
	{
		// This short cercuits the IIS process. Need to find a better way to restart the app.
		ThreadStart threadStart = delegate { ForceRestart(); };
		Thread thread = new Thread(threadStart);
		thread.IsBackground = true;
		thread.Start();
		Response.Redirect(Request.RawUrl, true);
	}

	public void ForceRestart()
	{
		throw new ApplicationException();
	}

	private string GetExtensions()
	{
		List<Extension> extList = ExtensionManager.Extensions;
		StringBuilder sb = new StringBuilder("<table style='background:#F1F1F1;border:2px solid #fff;width:100%'>");
		sb.Append("<tr style='background:#f9f9f9'>");
		sb.Append("<th>Name</th>");
		sb.Append("<th>Version</th>");
		sb.Append("<th>Description</th>");
		sb.Append("<th>Status</th>");
		sb.Append("<th>Source</th>");
		sb.Append("<th>Settings</th>");
		sb.Append("</tr>");

		if (extList != null)
		{
			int alt = 0;
			foreach (Extension x in extList)
			{
				if (alt % 2 == 0)
					sb.Append("<tr style='background:#f7f7f7'>");
				else
					sb.Append("<tr>");
				sb.Append("<td>" + x.Name + "</td>");
				sb.Append("<td>" + x.Version + "</td>");
				sb.Append("<td>" + x.Description + "</td>");

				if (x.Enabled)
					sb.Append("<td align='center' style='background:#ccffcc'><a href='?act=dis&ext=" + x.Name + "' title='Click to disable " + x.Name + "'>Enabled</a></td>");
				else
					sb.Append("<td align='center' style='background:#ffcc66'><a href='?act=enb&ext=" + x.Name + "' title='Click to enable " + x.Name + "'>Disabled</a></td>");

				sb.Append("<td align='center'><a href='?ctrl=editor&ext=" + x.Name + "'>" + Resources.labels.edit + "</a></td>");
				sb.Append("<td align='center'><a href='?ctrl=params&ext=" + x.Name + "'>" + Resources.labels.edit + "</a></td>");
				sb.Append("</tr>");
				alt++;
			}
		}
		sb.Append("</table>");
		return sb.ToString();
	}

	void ChangeStatus(string act, string ext)
	{
		if (act == "dis")
			ExtensionManager.ChangeStatus(ext, false);
		else
			ExtensionManager.ChangeStatus(ext, true);

		// UnloadAppDomain() requires full trust - touch web.config to reload app
		try
		{
			string ConfigPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\web.config";
			System.IO.File.SetLastWriteTimeUtc(ConfigPath, DateTime.UtcNow);
		}
		catch (Exception e)
		{
			lblErrorMsg.Visible = true;
			lblErrorMsg.InnerHtml = e.Message;
		}
		Response.Redirect("~/admin/User controls/xmanager/default.aspx");
	}
}
