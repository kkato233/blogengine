﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Trash.aspx.cs" Inherits="Admin.Trash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
	<div class="content-box-outer">
		<div class="content-box-full">

            <h1><%=Resources.labels.trash %></h1>
            <% if (BlogEngine.Core.Json.JsonTrashList.IsTrashEmpty() == false){ %>
            <div class="tableToolBox"> 
                <%=Resources.labels.show %> : <a id="All" class="current" href="#" onclick="LoadTrash(this)"><%=Resources.labels.all %></a> | 
                <a id="Post" href="#" onclick="LoadTrash(this)"><%=Resources.labels.posts %></a> | 
                <a id="Page" href="#" onclick="LoadTrash(this)"><%=Resources.labels.pages %></a> |
                <a id="Comment" href="#" onclick="LoadTrash(this)"><%=Resources.labels.comments %></a>
                <div class="Pager"></div>
            </div>
            <%} %>
            <div id="Container"></div>
            <div class="Pager"></div>

        </div>
    </div>

    <script type="text/javascript">
        LoadTrash(null);
    </script>

</asp:Content>
