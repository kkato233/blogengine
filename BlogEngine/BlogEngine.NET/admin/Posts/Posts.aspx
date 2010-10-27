﻿<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" ValidateRequest="False" CodeFile="Posts.aspx.cs" Inherits="Admin.Posts.Posts" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        LoadPosts(1, 'All', '');
        $(document).ready(function () {
            $(".tableToolBox a").click(function () {
                $(".tableToolBox a").removeClass("current");
                $(this).addClass("current");
            });
        });
    </script>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1>Posts</h1>
            <div class="tableToolBox">
                Show : <a class="current" href="#" onclick="LoadPosts(1, 'All', '')">All</a> | <a href="#" onclick="LoadPosts(1, 'Draft', '')">Drafts</a> | <a href="#" onclick="LoadPosts(1, 'Published', '')">Published</a>
                <div class="Pager"></div>
            </div>
            <div id="Container"></div>
            <div class="Pager"></div>
        </div>
    </div>
</asp:Content>
