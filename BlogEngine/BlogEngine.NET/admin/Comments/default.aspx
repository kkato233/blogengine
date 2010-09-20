<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="admin_Comments_Default" %>
<%@ Register src="DataGrid.ascx" tagname="DataGrid" tagprefix="uc1" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=Resources.labels.roles %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <uc1:DataGrid ID="DataGridComments" runat="server" />
		</div>
	</div> 
</asp:Content>
