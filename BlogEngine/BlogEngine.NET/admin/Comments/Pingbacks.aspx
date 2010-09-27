<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Pingbacks.aspx.cs" Inherits="admin.Comments.Pingbacks" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 

    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="../admin.js" type="text/javascript"></script>
    <script type="text/javascript">
        LoadComments(1, 'Pingbacks.aspx');
    </script>
     
    <div class="content-box-hdr">
        <span class="SectionHeader">Pingbacks & Trackbacks</span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <div id="Container"></div>
            <div id="Pager"></div>
		</div>
        <div class="action_buttons">
		    <input type="submit" class="btnAddNew btn rounded" value="Delete Selected" onclick="return ProcessSelected('Delete', 'Pingback');" />&nbsp;
            <span class="loader">&nbsp;</span>
        </div>
	</div>      
</asp:Content>