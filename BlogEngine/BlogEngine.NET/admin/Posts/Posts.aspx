<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" ValidateRequest="False" CodeFile="Posts.aspx.cs" Inherits="admin_Posts_Posts" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        LoadPosts(1, 'All');
    </script>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1>Posts</h1>
            Show | <a href="#" onclick="LoadPosts(1, 'All')">All</a> | <a href="#" onclick="LoadPosts(1, 'Draft')">Drafts</a> | <a href="#" onclick="LoadPosts(1, 'Published')">Published</a>
            <div id="Container"></div>
            <div id="Pager"></div>
        </div>
    </div>
</asp:Content>
