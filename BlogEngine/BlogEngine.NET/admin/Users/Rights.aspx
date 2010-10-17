<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Rights.aspx.cs" Inherits="Admin.Users.Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
	<div class="content-box-outer">
		<div class="content-box-right">
            <ul>
			    <li><a href="Users.aspx"><%=Resources.labels.users %></a></li>
			    <li><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></li>
			    <li class="content-box-selected"><a href="Rights.aspx">Rights</a></li>
            </ul>
		</div>
		<div class="content-box-left">
            <h1>User Rights</h1>
            placeholder
		</div>
	</div>
</asp:Content>
