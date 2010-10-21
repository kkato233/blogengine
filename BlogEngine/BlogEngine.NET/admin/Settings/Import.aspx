<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Import.aspx.cs" Inherits="admin.Settings.Import" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">    
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.import %> &amp; <%=Resources.labels.export %></h1>
            <div style="padding: 10px 0">
                <label><%=Resources.labels.blogMLDescription %> (<a href="http://blogml.codeplex.com/" target="_blank">BlogML</a>)</label>
            </div>

            <fieldset class="rounded">
                <legend><%=Resources.labels.import %></legend>
                <table class="tblForm">
                    <tr>
                        <td>
                            <input type="button" class="btn rounded" value="<%=Resources.labels.import %>" onclick="location.href='http://dotnetblogengine.net/clickonce/blogimporter/blog.importer.application?url=<%=Utils.AbsoluteWebRoot %>&username=<%=Page.User.Identity.Name %>'" />
                            <label>Run click-once application to import content saved as BlogML or RSS 2.0 into your blog. Internet Explorer only.</label>
                        </td>
                    </tr>
                    <tr><td><div style="margin-top: 20px">OR</div></td></tr>
                    <tr>
                        <td>
                            <asp:FileUpload runat="server" CssClass="btn rounded" ID="txtUploadFile" Width="300" />&nbsp;&nbsp;
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnBlogMLImport" runat="server" CssClass="btn rounded" Text="<%$Resources:labels,import %>" OnClick="BtnBlogMlImportClick" />
                            <label>Select saved BlogML file and import it into your blog. Any browser, BlogML only.</label>
                        </td>
                    </tr>
                 </table>
            </fieldset>

            <fieldset class="rounded">
                <legend><%=Resources.labels.export %></legend>
                <table class="tblForm">
                    <tr>
                        <td>
                            <input type="button" class="btn rounded" value="<%=Resources.labels.export %>" onclick="location.href='blogml.axd'" />
                            <label>Export your blog's content into XML file (BlogML)</label>
                        </td>
                    </tr>    
                </table>
            </fieldset>

        </div>
    </div>
</asp:Content>
