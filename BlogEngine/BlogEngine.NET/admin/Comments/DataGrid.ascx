<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataGrid.ascx.cs" Inherits="admin_Comments_DataGrid" %>
<%@ Import Namespace="Resources"%>
   
<table id="UserService" class="beTable rounded">
    <thead>
        <tr>
	        <th width="20"><input type="checkbox" id="selectall" onclick="toggleAllChecks(this)" /></th>
            <th width="30">&nbsp;</th>
	        <th width="120"><%=Resources.labels.author %></th>
	        <th width="120"><%=Resources.labels.ip %></th>
	        <th width="120"><%=Resources.labels.email %></th>
	        <th width="120"><%=Resources.labels.website %></th>
	        <th width="auto"><%=Resources.labels.comment %></th>
	        <th width="150"><%=Resources.labels.date %></th>
            <th width="80">&nbsp;</th>
	        <th width="80">&nbsp;</th>
        </tr>
    </thead>
    <tbody>
        <%=BindComments()%>
    </tbody>
</table>