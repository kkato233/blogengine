<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Rights.aspx.cs" Inherits="admin_Users_Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <div class="content-box-hdr">
        <span class="SectionHeader">User Rights</span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<div><a href="Users.aspx"><%=Resources.labels.users %></a></div>
			<div><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></div>
			<div class="content-box-selected"><a href="Rights.aspx">Rights</a></div>
		</div>
		<div class="content-box-left">
            placeholder
		</div>
	</div>
</asp:Content>
