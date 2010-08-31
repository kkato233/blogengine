<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Add_entry.aspx.cs" Inherits="admin_entry" ValidateRequest="False" EnableSessionState="True" %>
<%@ Register Src="../htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

<div id="tagselector" style="display: none">
    <a href="javascript:void(ToggleTagSelector())" style="color:Black;float:right"><%=Resources.labels.close %></a>
    <div style="clear:both"></div>
    <asp:PlaceHolder runat="server" ID="phTags" />
    <div style="clear:both"></div>
  </div>
  
<script type="text/javascript">
function ToggleVisibility()
{
  var element = document.getElementById('<%=ulDrafts.ClientID%>');
  if (element.style.display == "none")
    element.style.display = "block";
  else
    element.style.display = "none";
}

function GetSlug()
{
  var title = document.getElementById('<%=txtTitle.ClientID %>').value;
  WebForm_DoCallback('__Page', title, ApplySlug, 'slug', null, false) 
}

function ApplySlug(arg, context)
{
  var slug = document.getElementById('<%=txtSlug.ClientID %>');
  slug.value = arg;
}

function AutoSave()
{
	var content = document.getElementById('<%=txtRawContent.ClientID %>') != null ? document.getElementById('<%=txtRawContent.ClientID %>').value : tinyMCE.activeEditor.getContent();
  var title = document.getElementById('<%=txtTitle.ClientID %>').value;
  var desc = document.getElementById('<%=txtDescription.ClientID %>').value;
  var slug = document.getElementById('<%=txtSlug.ClientID %>').value;
  var tags = document.getElementById('<%=txtTags.ClientID %>').value;
  var s = ';|;';
  var post = content + s + title + s + desc + s + slug + s + tags;
  
  if (content.length > 10)
  {
    WebForm_DoCallback('__Page', '_autosave' + post, null, 'autosave', null, false);
  }
  
  setTimeout("AutoSave()", 5000);
}

document.body.onkeypress = ESCclose;

function ESCclose(evt) 
{
  if (!evt)
    evt = window.event; 
    
  if (evt.keyCode == 27) 
    document.getElementById('tagselector').style.display = 'none';  
 }

function AddTag(element)
{
  var input = document.getElementById('<%=txtTags.ClientID %>');  
  input.value += element.innerHTML + ', ';
}

function ToggleTagSelector()
{
  var element = document.getElementById('tagselector');
  if (element.style.display == "none")
    element.style.display = "block";
  else
    element.style.display = "none";
}
</script>

  <div id="divDrafts" runat="server" visible="False" enableviewstate="False" style="margin-bottom: 10px">
    <a id="aDrafts" runat="server" href="javascript:void(ToggleVisibility());" />
    <ul id="ulDrafts" runat="server" style="display:none;list-style-type:circle" />
  </div>

  <label for="<%=txtTitle.ClientID %>"><%=Resources.labels.title %></label>
  <asp:TextBox runat="server" ID="txtTitle" Width="450px" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=ddlAuthor.ClientID %>"><%=Resources.labels.author %></label>
  <asp:DropDownList runat="Server" ID="ddlAuthor" />&nbsp;&nbsp;&nbsp;
  
  <label for="<%=txtDate.ClientID %>"><%=Resources.labels.date %></label>
  <asp:TextBox runat="server" ID="txtDate" Width="110px" />
  
  
  <asp:CheckBox runat="server" ID="cbUseRaw" Text="<%$Resources:labels,useRawHtmlEditor %>" AutoPostBack="true" />
  
  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDate" ValidationExpression="[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9] [0-9][0-9]:[0-9][0-9]" ErrorMessage="<%$Resources:labels,enterValidDate %>" Display="dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" ErrorMessage="<%$Resources:labels,enterDate %>" Display="Dynamic" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="<%$Resources:labels,enterTitle %>" Display="Dynamic" />
  <br /><br />
  
  <Blog:TextEditor runat="server" id="txtContent" />
  <asp:TextBox runat="server" ID="txtRawContent" Width="100%" TextMode="multiLine" Height="300px" Visible="false" />
  <br />
  
  <table id="entrySettings">
    <tr>
      <td class="label"><%=Resources.labels.uploadImage %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" size="50" ValidationGroup="imageupload" />
        <asp:Button runat="server" ID="btnUploadImage" Text="<%$Resources:labels,upload %>" ValidationGroup="imageupload" />
        <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtUploadImage" ErrorMessage="<%$ Resources:labels, required %>" ValidationGroup="imageupload" />
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.uploadFile %></td>
      <td>
        <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" size="50" />
        <asp:Button runat="server" ID="btnUploadFile" Text="<%$Resources:labels,upload %>" ValidationGroup="fileUpload" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUploadFile" ErrorMessage="<%$ Resources:labels, required %>" ValidationGroup="fileUpload" />
      </td>
    </tr>    
    <tr>
      <td class="label"><%=Resources.labels.slug %></td>
      <td>
        <asp:TextBox runat="server" ID="txtSlug" Width="400" />
        <a href="javascript:void(GetSlug());"><%=Resources.labels.extractFromTitle %></a>
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.description %></td>
      <td><asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50" Rows="3" Width="400" Height="32px" /></td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.categories %></td>
      <td>
        <asp:TextBox runat="server" ID="txtCategory" ValidationGroup="category" />
        <asp:Button runat="server" ID="btnCategory" Text="<%$ Resources:labels, add %>" ValidationGroup="category" />
        <asp:CustomValidator runat="Server" ID="valExist" ValidationGroup="category" ControlToValidate="txtCategory" ErrorMessage="<%$ Resources:labels, categoryAlreadyExists %>" Display="dynamic" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategory" ErrorMessage="<%$ Resources:labels, required %>" ValidationGroup="category" /><br />
        <div style="width:400px">
        <asp:CheckBoxList runat="server" Width="400" ID="cblCategories" CssClass="cblCategories" RepeatLayout="table" RepeatColumns="4" RepeatDirection="Horizontal" />
        </div>
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.tags %></td>
      <td>
        <asp:TextBox runat="server" ID="txtTags" Width="400" />
        <a href="javascript:void(ToggleTagSelector())"><%=Resources.labels.showSelector %></a>
        <span><%=Resources.labels.separateTagsWitComma %></span>
      </td>
    </tr>
    <tr>
      <td class="label"><%=Resources.labels.settings %></td>
      <td>
        <asp:CheckBox runat="server" ID="cbEnableComments" Text="<%$ Resources:labels, enableComments %>" Checked="true" />
        <asp:CheckBox runat="server" ID="cbPublish" Text="<%$ Resources:labels, publish %>" Checked="true" />
      </td>
    </tr>
  </table>  
  
  <div style="text-align:right">  
    <asp:Button runat="server" ID="btnSave" />
  </div>
  <br />
<% if (Request.QueryString["id"] == null){ %>  
  <script type="text/javascript">
    setTimeout("AutoSave()", 5000);
  </script>
<%} %>
</asp:Content>

