<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SidePanel.ascx.cs" EnableViewState="false" Inherits="User_controls_SidePanel" %>
<%@ Register Src="~/admin/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<%@ Import Namespace="BlogEngine.Core" %>

<div class="box vcard" style="float:left">
  <h1>About me</h1>
  <table summary="">
    <tbody><tr>
      <td valign="top"><img src="~/themes/creative green/noavatar.jpg" id="imgAvatar" runat="server" class="photo" alt="A picture of me"></td>
      <td style="vertical-align: top;">
        <span class="fn">My Name</span><br />
        Let the visitor know a little bit more about myself.
        <div style="display: none;" class="adr">
          <span class="nickname">my nickname</span>
          <span class="country-name">Land that saw be born</span>
        </div>
        <br /><br />
        <!-- Email -->
        <a href="~/contact.aspx" runat="server" style="float: right; clear: both;" class="email">
          E-mail me <img src="~/pics/mail.gif" runat="server" alt="Send mail" style="width: 16px;">
        </a>
      </td>
    </tr>
  </tbody></table>  
</div>

<div class="box categories" style="float:left;margin-left: 20px">
  <h1>Categories</h1>
  <blog:CategoryList runat="server" />
</div>

<% if (Page.User.Identity.IsAuthenticated){ %>
<div class="box admin" style="margin:10px 0;clear:both">
  <h1>Administration</h1>
  <uc1:menu ID="Menu1" runat="server" />
</div>
<%} %>