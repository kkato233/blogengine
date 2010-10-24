<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Pingbacks.aspx.cs" Inherits="Admin.Comments.Pingbacks" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>

    <script type="text/javascript">
        LoadComments(1, 'Pingbacks.aspx');
    </script>
     
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1>Pingbacks &amp; Trackbacks</h1>
            <div id="Container"></div>
            <div id="Pager"></div>
            <div class="action_buttons">
		        <input type="submit" class="btnAddNew btn" value="Delete Selected" onclick="return ProcessSelected('Delete', 'Pingback');" />&nbsp;
                <span class="loader">&nbsp;</span>
            </div>
		</div>
	</div>      
</asp:Content>