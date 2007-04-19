<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Pages.aspx.cs" ValidateRequest="false" Inherits="admin_Pages_pages" Title="Add page" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

  <label for="<%=txtTitle.ClientID %>">Title</label>
  <asp:TextBox runat="server" ID="txtTitle" Width="500px" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="Please enter a title" /><br /><br />

  <FTB:FreeTextBox runat="server" ID="txtContent" Width="100%" SupportFolder="../freetextbox/" /><br />
  
  <table id="entrySettings">
    <tr>
      <td class="label">Upload image</td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" />
        <asp:Button runat="server" ID="btnUploadImage" Text="Upload" CausesValidation="False" />
      </td>
    </tr>
    <tr>
      <td class="label">Upload file</td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" />        
        <asp:Button runat="server" ID="btnUploadFile" Text="Upload" CausesValidation="False" ValidationGroup="fileUpload" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUploadFile" ErrorMessage="Specify a file name" ValidationGroup="fileUpload" />
      </td>
    </tr>    
    <tr>
      <td class="label">Description</td>
      <td><asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50" Rows="5" /></td>
    </tr>
    <tr>
      <td class="label">Keywords</td>
      <td><asp:TextBox runat="server" ID="txtKeyword" Width="400" /></td>
    </tr>
  </table>  
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSave" Text="Save page" />
  </div>
  <br /><br />
</asp:Content>