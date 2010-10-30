﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Spam.aspx.cs" Inherits="Admin.Comments.Spam" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>

    <script type="text/javascript">
        LoadComments(1, 'Spam.aspx');
    </script>

	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.spam %></h1>
            <div class="tableToolBox">
                <div class="Pager"></div>
            </div>
            <div id="Container"></div>
            <div class="Pager"></div>
            <%if (CommentCounter > 0)
              {%>
            <div class="action_buttons">
                <input type="submit" class="btn" value="Restore Selected" onclick="return ProcessSelected('Approve','Spam');" />&nbsp;
		        <input type="submit" class="btn" value="Delete Selected" onclick="return ProcessSelected('Delete', 'Spam');" />&nbsp;
                <input type="submit" class="btn" value="Delete All" onclick="return DeleteAllSpam();" />&nbsp;
                <span class="loader">&nbsp;</span>
            </div>
            <%
              }%>
		</div>
	</div>       
</asp:Content>