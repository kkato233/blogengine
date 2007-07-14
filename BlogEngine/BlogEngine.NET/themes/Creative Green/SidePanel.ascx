<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SidePanel.ascx.cs" EnableViewState="false" Inherits="User_controls_SidePanel" %>
<%@ Register Src="~/admin/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<%@ Import Namespace="BlogEngine.Core" %>

<div class="box vcard">
  <h1>About me</h1>
  <table summary="">
    <tbody><tr>
      <td valign="top"><img src="~/themes/creative green/noavatar.jpg" id="imgAvatar" runat="server" class="photo" alt="A picture of me"></td>
      <td style="vertical-align: top;">
        <span class="fn">My Name</span><br>
        Let the visitor know a little bit more about myself.
        <div style="display: none;" class="adr">
          <span class="nickname">my nickname</span>
          <span class="country-name">Land that saw be born</span>
        </div>
        <br><br>
        <!-- Email -->
        <a href="~/contact.aspx" runat="server" style="float: right; clear: both;" class="email">
          E-mail me <img src="~/pics/mail.gif" runat="server" alt="Send mail" style="width: 16px;">
        </a>
      </td>
    </tr>
  </tbody></table>  
</div>

<div class="box">
  <h1>Tags</h1>
  <blog:TagCloud ID="TagCloud1" runat="server" />
</div>

<div class="box blogroll">
  <h1>Blogroll</h1>   
  <blog:Blogroll ID="Blogroll1" runat="server"  />
  <a href="opml.axd" style="display:block;text-align:right" title="Download OPML file" >Download OPML file <asp:Image ID="Image1" runat="server" ImageUrl="~/pics/opml.png" AlternateText="OPML" /></a>
</div>

<div class="box categories">
  <h1>Categories</h1>
  <blog:CategoryList ID="CategoryList14" runat="Server" ShowRssIcon=true /><br />
  <a id="A1" href="~/archive.aspx" runat="Server">Archive</a>
</div>

<% if (Page.User.Identity.IsAuthenticated){ %>
<div class="box admin">
  <h1>Administration</h1>
  <uc1:menu ID="Menu1" runat="server" />
</div>
<%} %>
<%--
<div class="box">
  <h1>Authors</h1>
  <blog:AuthorList ID="AuthorList1" runat="Server" />
</div>

<div class="box">
  <h1>Calendar</h1>
  <div style="text-align:center">
    <blog:PostCalendar runat="Server" NextMonthText=">>" DayNameFormat="FirstTwoLetters" FirstDayOfWeek="monday" PrevMonthText="<<" CssClass="calendar" BorderWidth="0" WeekendDayStyle-CssClass="weekend" OtherMonthDayStyle-CssClass="other" UseAccessibleHeader="true" EnableViewState="false" />
    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/?calendar=show" Text="View posts in large calendar" EnableViewState="false" />
  </div>
</div>

<div class="box recent">
  <h1>Recent posts</h1>
  <blog:RecentPosts runat="Server" />
</div>

<div class="box recent">
  <h1>Recent comments</h1>
  <blog:RecentComments runat="Server" />
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
  <a href="opml.axd" style="display:block;text-align:right" title="Download OPML file" >Download OPML file <asp:Image ID="Image1" runat="server" ImageUrl="~/pics/opml.png" AlternateText="OPML" /></a>
</div>

<div class="box">
  <h1>Disclaimer</h1>
  <p>
    The opinions expressed herein are my own personal opinions and do not represent my employer's view in anyway.<br /><br />
    &copy; Copyright <%=DateTime.Now.Year %><br /><br />
    <asp:LoginStatus runat="Server" LoginText="Sign in" LogoutText="Sign out" EnableViewState="false" />
</p>
</div>
--%>
<p>&nbsp;</p>