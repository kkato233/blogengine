#region Using

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// Summary description for Calendar
  /// </summary>
  public class PostCalendar : Calendar, ICallbackEventHandler
  {

    protected override void OnPreRender(EventArgs e)
    {
      if (!Page.IsCallback && !Page.IsPostBack)
        base.VisibleDate = DateTime.Now;

      if (!Page.IsPostBack && Context.Request.QueryString["date"] != null)
      {
        DateTime date = DateTime.Now;
        if (DateTime.TryParse(Context.Request.QueryString["date"], out date))
          base.VisibleDate = date;
      }

      //Page.MaintainScrollPositionOnPostBack = true;
      base.OnPreRender(e);
      if (!ShowPostTitles)
        ShowTitle = false;
    }

    public bool ShowPostTitles
    {
      get { return (bool)(ViewState["ShowPostTitles"] ?? default(bool)); }
      set { ViewState["ShowPostTitles"] = value; }
    }

    protected override void OnDayRender(TableCell cell, CalendarDay day)
    {
      List<Post> list = Post.GetPostsByDate(day.Date, day.Date);
      if (list.Count > 0)
      {
        cell.Controls.Clear();
        if (ShowPostTitles)
        {
          cell.Controls.Add(new LiteralControl(day.DayNumberText));
          foreach (Post post in list)
          {
            HtmlAnchor a = new HtmlAnchor();
            a.InnerHtml = "<br /><br />" + post.Title;
            a.HRef = post.RelativeLink.ToString();
            cell.Controls.Add(a);
          }
        }
        else
        {
          HtmlAnchor a = new HtmlAnchor();
          a.InnerHtml = day.DayNumberText;
          a.HRef = Utils.RelativeWebRoot + "?date=" + day.Date.ToString("yyyy-MM-dd");
          a.Attributes["class"] = "exist";
          cell.Controls.Add(a);
        }
      }
      else
      {
        cell.Controls.Clear();
        cell.Text = day.DayNumberText;
        //cell.Controls.Add(new LiteralControl(day.DayNumberText));
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (!ShowPostTitles)
      {
        if (Page.IsPostBack && !Page.IsCallback)
          VisibleDate = DateTime.Now;

        writer.Write("<div id=\"calendarContainer\">");
        writer.Write("<table class=\"calendar\" summary=\"Post calendar navigation\" style=\";border-collapse:collapse;\">");
        writer.Write("<tr><td>");

        DateTime oldest = GetOldestPostDate();

        if (VisibleDate.Year != oldest.Year || VisibleDate.Month != oldest.Month)
          writer.Write("<a href=\"javascript:CalNav('" + this.VisibleDate.AddMonths(-1).ToString("yyyy-MM-dd") + "')\">" + HttpUtility.HtmlEncode(PrevMonthText) + "</a>&nbsp;&nbsp;");
        else
          writer.Write(HttpUtility.HtmlEncode(PrevMonthText) + "&nbsp;&nbsp;");

        writer.Write("</td><td style=\"text-align:center;width:100px\">" + VisibleDate.ToString("MMMM yyyy") + "</td><td align=\"right\">");

        if (VisibleDate.Year != DateTime.Now.Year || VisibleDate.Month != DateTime.Now.Month)
          writer.Write("&nbsp;&nbsp;<a href=\"javascript:CalNav('" + this.VisibleDate.AddMonths(1).ToString("yyyy-MM-dd") + "')\">" + HttpUtility.HtmlEncode(NextMonthText) + "</a>");
        else
          writer.Write("&nbsp;&nbsp;" + HttpUtility.HtmlEncode(NextMonthText));

        writer.Write("</td></tr>");
        writer.Write("</table>");

        base.Attributes.Add("summary", "Post calendar");
        base.Render(writer);
        
        writer.Write("</div>");

        if (!Page.IsCallback)
          writer.Write(Script());
      }
      else if (!Page.IsCallback)
      {
        base.Render(writer);
      }
    }

    private DateTime GetOldestPostDate()
    {
      if (Post.Posts.Count > 0)
      {
        return Post.Posts[Post.Posts.Count - 1].DateCreated;
      }

      return DateTime.Now;
    }

    private string Script()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("<script type=\"text/javascript\">");
      sb.Append("var months = new Object();");
      sb.Append("function CalNav(date){");
      sb.Append("if (months[date] == null || months[date] == 'undefined')");
      sb.Append("{" + Page.ClientScript.GetCallbackEventReference(this, "date", "UpdateCalendar", "date") + "}");
      sb.Append("else {UpdateCalendar(months[date], date)}");
      sb.Append("}");

      //sb.AppendLine("function UpdateCalendar(args, context){");
      //sb.AppendLine("var cal = document.getElementById('calendarContainer');");
      //sb.AppendLine("cal.innerHTML = args;");
      //sb.AppendLine("months[context] = args;");
      //sb.AppendLine("}");
      sb.Append("</script>");
      return sb.ToString();
    }

    #region ICallbackEventHandler Members

    private string _Callback;

    public string GetCallbackResult()
    {
      return _Callback;
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
      DateTime date = DateTime.Now;
      if (DateTime.TryParse(eventArgument, out date))
      {
        base.VisibleDate = date;
        base.ShowTitle = false;
        using (System.IO.StringWriter sw = new System.IO.StringWriter())
        {
          this.RenderControl(new HtmlTextWriter(sw));
          _Callback = sw.ToString();
        }
      }
    }

    #endregion
  }
}