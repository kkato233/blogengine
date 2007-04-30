<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SidePanel.ascx.cs" EnableViewState="false" Inherits="User_controls_SidePanel" %>
<%@ Register Src="~/admin/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<%@ Import Namespace="BlogEngine.Core" %>

<div class="box">
  <h1>About the author</h1>
  <table summary="About the author">
    <tr>
      <td><img src="~/themes/standard/madskristensen.png" runat="server" alt="Mads Kristensen" /></td>
      <td style="vertical-align:top">
        Mads Kristensen<br />
        Lead Developer at <a href="http://www.traceworks.com">Traceworks</a> and long time .NET slave. 
        <asp:HyperLink runat="server" NavigateUrl="~/page/about-mads-kristensen.aspx">More...</asp:HyperLink>
        <br /><br />
        
        <!-- Skype -->
        <a href="callto://dotnetslave" style="float:right">
          Skype me <img src="http://mystatus.skype.com/smallicon/dotnetslave" alt="Skype Me" />
        </a>
        <!-- Email -->
        <a href="<%=Macro.SafeMail("post@madskristensen.dk", BlogSettings.Instance.Name) %>" style="float:right;clear:both">
          E-mail me <img src="~/pics/mail.gif" alt="Send mail" runat="server" style="width:16px" />
        </a>
      </td>
    </tr>
  </table>  
</div>

<% if (Page.User.Identity.IsAuthenticated){ %>
<div class="box">
  <h1>Administration</h1>
  <uc1:menu ID="Menu1" runat="server" />
</div>
<%} %>

<div class="box">
  <h1>Download BlogEngine.NET</h1>
  <p>
    <a style="font:bold 13px verdana" href="http://www.codeplex.com/blogengine/SourceControl/ListDownloadableCommits.aspx">Download at CodePlex</a><br />
  </p>
</div>

<div class="box">
  <h1>Calendar</h1>
  <div style="text-align:center">
    <blog:PostCalendar runat="Server" NextMonthText=">>" DayNameFormat="FirstTwoLetters" FirstDayOfWeek="monday" PrevMonthText="<<" CssClass="calendar" BorderWidth="0" WeekendDayStyle-CssClass="weekend" OtherMonthDayStyle-CssClass="other" UseAccessibleHeader="true" />
    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/?calendar=show" Text="View posts in large calendar" EnableViewState="false" />
  </div>
</div>

<div class="box recent">
  <h1>Recent posts</h1>
  <blog:RecentPosts runat="Server" />
</div>

<div class="box">
  <h1>Archive</h1>
  <blog:MonthList runat="server" />
</div>

<div class="box">
  <h1>Authors</h1>
  <blog:AuthorList runat="Server" />
</div>

<div class="box">
  <h1>Tags</h1>
  <blog:TagCloud runat="server" />
</div>

<div class="box">
  <h1>Categories</h1>
  <blog:CategoryList runat="Server" /><br />
  <a href="~/archive.aspx" runat="Server">Archive</a>
  <blog:SearchBox runat="server" />
</div>

<div class="box">
  <h1>Blogroll</h1>
  <blog:Blogroll runat="server" />
</div>

<div class="box">
  <h1>Disclaimer</h1>
  <p>
    The opinions expressed herein are my own personal opinions and do not represent my employer's view in anyway.<br /><br />
    &copy; Copyright <%=DateTime.Now.Year %><br /><br />
    <asp:LoginStatus runat="Server" LoginText="Sign in" LogoutText="Sign out" EnableViewState="false" />
</p>
</div>