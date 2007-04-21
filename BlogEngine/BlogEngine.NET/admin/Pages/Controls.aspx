<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Controls.aspx.cs" Inherits="admin_Pages_Controls" Title="Control settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

<div class="settings">

  <h1>Recent posts</h1>
  <label for="<%=txtNumberOfPosts.ClientID %>">Number of posts</label>
  <asp:TextBox runat="server" ID="txtNumberOfPosts" Width="30" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumberOfPosts" ErrorMessage="Required" />
  <asp:CompareValidator runat="Server" ControlToValidate="txtNumberOfPosts" Operator="dataTypeCheck" Type="integer" ErrorMessage="Please enter a valid number" /><br />

</div>

<div class="settings">

  <h1>Search field</h1>
  <label for="<%=txtSearchButtonText.ClientID %>">Button text</label>
  <asp:TextBox runat="server" ID="txtSearchButtonText" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSearchButtonText" ErrorMessage="Required" /><br />
  
  <label for="<%=txtDefaultSearchText.ClientID %>">Search field text</label>
  <asp:TextBox runat="server" ID="txtDefaultSearchText" /> The default text shown in the search textbox
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDefaultSearchText" ErrorMessage="Required" /><br />
  
  <label for="<%=cbEnableCommentSearch.ClientID %>">Enable comment search</label>
  <asp:CheckBox runat="Server" ID="cbEnableCommentSearch" /><br />
  
  <label for="<%=txtCommentLabelText.ClientID %>">Comment label text</label>
  <asp:TextBox runat="server" ID="txtCommentLabelText" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCommentLabelText" ErrorMessage="Required" /><br />

</div>

<br />
<asp:Button runat="server" ID="btnSave" Text="Save settings" />
<br /><br /><br />

</asp:Content>

