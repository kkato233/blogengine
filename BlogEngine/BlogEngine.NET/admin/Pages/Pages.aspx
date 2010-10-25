<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="Pages.aspx.cs" ValidateRequest="false" Inherits="Admin.Pages.PagesPage"
    Title="Add page" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        LoadPages('All');
    </script>
    <div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.pages %></h1>
            Show | <a href="#" onclick="LoadPages('All')">All</a> | <a href="#" onclick="LoadPages('Draft')">Drafts</a> | <a href="#" onclick="LoadPages('Published')">Published</a>
            <div id="Container"></div>
        </div>
    </div>
</asp:Content>
