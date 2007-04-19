<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="admin_Pages_configuration" Title="Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

<div class="settings">

  <h1>Basic settings</h1>
  <label for="<%=txtName.ClientID %>">Name</label>
  <asp:TextBox runat="server" ID="txtName" Width="300" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Required" /><br />

  <label for="<%=txtDescription.ClientID %>">Description</label>
  <asp:TextBox runat="server" ID="txtDescription" Width="300" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription" ErrorMessage="Required" /><br />
    
  <label for="<%=txtPostsPerPage.ClientID %>">Posts per page</label>
  <asp:TextBox runat="server" ID="txtPostsPerPage" Width="50" MaxLength="4" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPostsPerPage" ErrorMessage="Required" />
  <asp:CompareValidator runat="server" ControlToValidate="txtPostsPerPage" Operator="DataTypeCheck" Type="integer" ErrorMessage="Please enter a valid number" /><br />
  
  <label for="<%=ddlTheme.ClientID %>">Theme</label>
  <asp:DropDownList runat="server" ID="ddlTheme" /><br />
    
  <label for="<%=cbShowRelatedPosts.ClientID %>">Show related posts</label>
  <asp:CheckBox runat="server" ID="cbShowRelatedPosts" />
</div>

<div class="settings">

  <h1>Comments</h1>
  <label for="<%=cbEnableComments.ClientID %>">Enable comments</label>
  <asp:CheckBox runat="server" ID="cbEnableComments" /><br />
  
  <label for="<%=cbEnableCountryInComments.ClientID %>">Show country chooser</label>
  <asp:CheckBox runat="server" ID="cbEnableCountryInComments" />
</div>

<div class="settings">

  <h1>Email</h1>
  <label for="<%=txtEmail.ClientID %>">Email address</label>
  <asp:TextBox runat="server" ID="txtEmail" Width="300" /><br />
  
  <label for="<%=txtSmtpServer.ClientID %>">SMTP server</label>
  <asp:TextBox runat="server" ID="txtSmtpServer" Width="300" /><br />
  
  <label for="<%=txtSmtpUsername.ClientID %>">User name</label>
  <asp:TextBox runat="server" ID="txtSmtpUsername" Width="300" /><br />
  
  <label for="<%=txtSmtpPassword.ClientID %>">Password</label>
  <asp:TextBox runat="server" ID="txtSmtpPassword" Width="300" /><br />
  
  <label for="<%=cbComments.ClientID %>">Send comment mail</label>
  <asp:CheckBox runat="Server" ID="cbComments" /><br /><br />
  
  <asp:Button runat="server" CausesValidation="False" ID="btnTestSmtp" Text="Test mail settings" />
  <asp:Label runat="Server" ID="lbSmtpStatus" />
</div>

<div class="settings">

  <h1>Import (beta)</h1>
  <p>This is an experimental import tool for importing posts, comments, files and images from RSS feeds.<br /><br />
     Nothing bad will happen if you try the beta tool. It import posts and files and add them to the App_Data folder.<br />
     If you don't like the result, just delete the posts and files from App_Data/posts and App_Data/files.<br /><br />
     <strong>NB:</strong> The tool has only been tested with dasBlog. If you use any other blog software, you should be aware
     when you import comments or download files automatically.
  </p>
  <p>
    <a href="http://www.madskristensen.dk/clickonce/blogconverter/BlogConverter.application?url=<%=DotNetSlave.BlogEngine.BusinessLogic.Utils.AbsoluteWebRoot %>&username=<%=System.Threading.Thread.CurrentPrincipal.Identity.Name %>">Launch import tool</a>
  </p>
</div>

<br />
<asp:Button runat="server" ID="btnSave" Text="Save settings" />
<br /><br /><br />

</asp:Content>

