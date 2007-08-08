<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Pages.aspx.cs" ValidateRequest="false" Inherits="admin_Pages_pages" Title="Add page" %>
<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

  <label for="<%=txtTitle.ClientID %>"><%=Resources.labels.title %></label>
  <asp:TextBox runat="server" ID="txtTitle" Width="400px" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Please enter a title" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=ddlParent.ClientID %>"><%=Resources.labels.selectParent %></label>
  <asp:DropDownList runat="server" id="ddlParent" />&nbsp;&nbsp;&nbsp;
  
  <asp:CheckBox runat="Server" ID="cbIsFrontPage" Text="<%$ Resources:labels, isFrontPage %>" />
  <br /><br />

  <blog:TextEditor runat="server" id="txtContent" TabIndex="4" />
  
  <table id="entrySettings">
    <tr>
      <td class="label"><%=Resources.labels.uploadImage %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" />
        <asp:Button runat="server" ID="btnUploadImage" Text="Upload" CausesValidation="False" />
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.uploadFile %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" />        
        <asp:Button runat="server" ID="btnUploadFile" Text="Upload" CausesValidation="False" ValidationGroup="fileUpload" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUploadFile" ErrorMessage="Specify a file name" ValidationGroup="fileUpload" />
      </td>
    </tr>    
    <tr>
      <td class="label"><%=Resources.labels.description %></td>
      <td><asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50" Rows="5" /></td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.keywords %></td>
      <td><asp:TextBox runat="server" ID="txtKeyword" Width="400" /></td>
    </tr>
  </table>  
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSave" Text=" <%$ Resources:labels, savePage %> " />
  </div>
  <br />
</asp:Content>