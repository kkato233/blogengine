<%@ Page Language="C#" AutoEventWireup="true" CodeFile="month.aspx.cs" Inherits="month" %>
<%@ Register Src="User controls/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="Server">
  <uc1:PostList ID="PostList1" runat="server" />
  <blog:PostCalendar runat="server" ID="calendar" 
    ShowPostTitles="true" 
    BorderWidth="0"
    NextPrevStyle-CssClass="header"
    CssClass="calendar" 
    WeekendDayStyle-CssClass="weekend" 
    OtherMonthDayStyle-CssClass="other" 
    UseAccessibleHeader="true" 
    Visible="false" 
    Width="100%" />
</asp:content>