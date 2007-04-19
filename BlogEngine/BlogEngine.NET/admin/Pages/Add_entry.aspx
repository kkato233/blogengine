<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Add_entry.aspx.cs" Inherits="admin_entry" Title="Add entry" ValidateRequest="False" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">


  <label for="<%=txtTitle.ClientID %>">Title</label>
  <asp:TextBox runat="server" ID="txtTitle" Width="500px" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=ddlAuthor.ClientID %>">Author</label>
  <asp:DropDownList runat="Server" ID="ddlAuthor" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=txtDate.ClientID %>">Date</label>
  <asp:TextBox runat="server" ID="txtDate" Width="100px" />
  
  <asp:CompareValidator runat="server" ControlToValidate="txtDate" Operator="dataTypeCheck" Type="date" ErrorMessage="Please enter a valid date (yyyy-mm-dd)" Display="dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" ErrorMessage="Please enter a date (yyyy-mm-dd)" Display="Dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="Please enter an author" Display="Dynamic" />
  <br /><br />

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
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUploadFile" ErrorMessage="Specify a file name" ValidationGroup="fileUpload" />
      </td>
    </tr>    
    <tr>
      <td class="label">Description</td>
      <td><asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50" Rows="5" /></td>
    </tr>
    <tr>
      <td class="label">Category</td>
      <td>
        <asp:TextBox runat="server" ID="txtCategory" ValidationGroup="category" />
        <asp:Button runat="server" ID="btnCategory" Text="Add" ValidationGroup="category" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategory" ErrorMessage="Required" ValidationGroup="category" /><br />
        
        <asp:CheckBoxList runat="server" Width="400" ID="cblCategories" CssClass="cblCategories" RepeatLayout="flow" RepeatDirection="Horizontal" />
      </td>
    </tr>
    <tr>
      <td class="label">Tags</td>
      <td>
        <asp:TextBox runat="server" ID="txtTags" Width="400" />
        <span>Separate each tag with a comment</span>
      </td>
    </tr>
    <tr>
      <td class="label">Settings</td>
      <td>
        <asp:CheckBox runat="server" ID="cbEnableComments" Text="Enable comments" Checked="true" />
        <asp:CheckBox runat="server" ID="cbPublish" Text="Publish" Checked="true" />
      </td>
    </tr>
  </table>  
  
  <div style="text-align:right">  
    <asp:Button runat="server" ID="btnSave" Text="Save post" />
  </div>
  <br /><br />
</asp:Content>

