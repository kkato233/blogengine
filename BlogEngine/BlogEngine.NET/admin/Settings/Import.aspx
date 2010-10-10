<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Import.aspx.cs" Inherits="admin.Settings.Import" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">    
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=Resources.labels.settings %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <fieldset class="rounded">
                <legend><%=Resources.labels.email %></legend>

                <table class="tblForm">
                    <tr>
                        <td>
                            <br/>
                            <label><%=Resources.labels.blogMLDescription %> (<a href="http://blogml.org/" target="_blank">blogml.org</a>)</label>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" class="btn rounded" value="<%=Resources.labels.import %>" onclick="location.href='http://dotnetblogengine.net/clickonce/blogimporter/blog.importer.application?url=<%=Utils.AbsoluteWebRoot %>&username=<%=Page.User.Identity.Name %>'" />&nbsp;&nbsp;
                            <input type="button" class="btn rounded" value="<%=Resources.labels.export %>" onclick="location.href='blogml.axd'" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>
