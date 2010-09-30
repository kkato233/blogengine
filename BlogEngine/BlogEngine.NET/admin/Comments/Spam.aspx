<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Spam.aspx.cs" Inherits="admin.Comments.Spam" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script src="<%= Utils.AbsoluteWebRoot %>Scripts/jquery.js" type="text/javascript"></script>
    <script src="<%= Utils.AbsoluteWebRoot %>Scripts/jquery-jtemplates.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>
    <script src="<%= Utils.AbsoluteWebRoot %>admin/admin.js" type="text/javascript"></script>

    <script type="text/javascript">
        LoadComments(1, 'Spam.aspx');
    </script>
     
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=Resources.labels.spam %></span>
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
            <input type="submit" class="btn rounded" value="Restore Selected" onclick="return ProcessSelected('Approve','Spam');" />&nbsp;
		    <input type="submit" class="btn rounded" value="Delete Selected" onclick="return ProcessSelected('Delete', 'Spam');" />&nbsp;
            <input type="submit" class="btn rounded" value="Delete All" onclick="return DeleteAllSpam();" />&nbsp;
            <span class="loader">&nbsp;</span>
        </div>
	</div>       
</asp:Content>