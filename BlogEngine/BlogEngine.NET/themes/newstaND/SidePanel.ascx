<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SidePanel.ascx.cs" EnableViewState="false" Inherits="User_controls_SidePanel" %>
<%@ Register Src="~/admin/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<%@ Import Namespace="BlogEngine.Core" %>

<blog:WidgetZone runat="server" ID="rightzone" />

<div class="box">
  <p><asp:LoginStatus ID="LoginStatus1" runat="Server" LoginText="Sign in" LogoutText="Sign out" EnableViewState="false" />
</p>
</div>