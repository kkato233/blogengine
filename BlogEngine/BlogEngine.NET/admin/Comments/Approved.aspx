﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Approved.aspx.cs" Inherits="Admin.Comments.Approved" %>
<%@ Import Namespace="BlogEngine.Core" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script src="<%= Utils.AbsoluteWebRoot %>Scripts/jquery.js" type="text/javascript"></script>
    <script src="<%= Utils.AbsoluteWebRoot %>Scripts/jquery-jtemplates.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>
    <script src="<%= Utils.AbsoluteWebRoot %>admin/admin.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        LoadComments(1, 'Approved.aspx');
    </script>
    
    <div class="content-box-hdr">
        <span class="SectionHeader">Approved Comments</span>
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
            <input type="submit" class="btn rounded" value="Reject Selected" onclick="return ProcessSelected('Reject', 'Approved');" />&nbsp;
            <input type="submit" class="btn rounded" value="Delete Selected" onclick="return ProcessSelected('Delete', 'Approved');" />&nbsp;
            <span class="loader">&nbsp;</span>
        </div>
	</div>      
</asp:Content>