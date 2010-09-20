<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Spam.aspx.cs" Inherits="admin_Comments_Spam" %>
<%@ Register src="DataGrid.ascx" tagname="DataGrid" tagprefix="uc1" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">  
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=Resources.labels.spam %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <uc1:DataGrid ID="DataGridComments" runat="server" />
		</div>
        <div style="text-align:center">
            <input type="submit" class="btnAddNew btn rounded" value="Restore Selected" onclick="return AddRole();" />&nbsp;
		    <input type="submit" class="btnAddNew btn rounded" value="Delete Selected" onclick="return AddRole();" />
        </div>
	</div>       
</asp:Content>