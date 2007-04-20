<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Pages.aspx.cs" ValidateRequest="false" Inherits="admin_Pages_pages" Title="Add page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

  <label for="<%=txtTitle.ClientID %>">Title</label>
  <asp:TextBox runat="server" ID="txtTitle" Width="500px" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ErrorMessage="Please enter a title" /><br /><br />

  <script type="text/javascript" src="../tiny_mce/tiny_mce.js"></script>
  <script language="javascript" type="text/javascript">
	tinyMCE.init({
		mode : "exact",
    elements : "<%=txtContent.ClientID %>",
		theme : "advanced",
		//plugins : "style,layer,table,save,advhr,advimage,advlink,emotions,iespell,media,searchreplace,contextmenu,paste,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras",
		plugins : "fullscreen,contextmenu,cleanup,emotions,media",
		theme_advanced_buttons1_add_before : "fullscreen,code,separator,cut,copy,paste,separator,undo,redo,separator",
		theme_advanced_buttons1_add : "separator,bullist,numlist,outdent,indent,separator,link,unlink,image,media,sub,sup,removeformat,cleanup,charmap,emotions,separator,formatselect,fontselect,fontsizeselect,separator,help",
		theme_advanced_buttons2_add : "",
		//theme_advanced_buttons2_add_before: "",
		theme_advanced_disable : "styleselect,code,hr,charmap,sub,sup,visualaid,separator,removeformat,bullist,numlist,outdent,indent,undo,redo,link,unlink,anchor,help,cleanup,image,formatselect",
		//theme_advanced_buttons3_add_before : "tablecontrols,separator",
		//theme_advanced_buttons3_add : "emotions,iespell,media,advhr",
		//theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_path_location : "bottom",
		//content_css : "example_full.css",
	    plugin_insertdate_dateFormat : "%Y-%m-%d",
	    plugin_insertdate_timeFormat : "%H:%M:%S",
		extended_valid_elements : "hr[class|width|size|noshade],font[face|size|color|style],span[class|align|style]",
		external_link_list_url : "example_link_list.js",
		external_image_list_url : "example_image_list.js",
		flash_external_list_url : "example_flash_list.js",
		media_external_list_url : "example_media_list.js",
		//template_external_list_url : "example_template_list.js",
		file_browser_callback : "fileBrowserCallBack",
		theme_advanced_resize_horizontal : false,
		theme_advanced_resizing : true,
		nonbreaking_force_tab : true,
		apply_source_formatting : true,
		relative_urls : false,
		template_replace_values : {
			username : "Jack Black",
			staffid : "991234"
		}
	});
	</script>
  <asp:TextBox runat="Server" ID="txtContent" Width="100%" TextMode="MultiLine" />
  
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