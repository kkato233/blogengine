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
                <label><%=Resources.labels.blogMLDescription %></label>
            </div>

            <h2><%=Resources.labels.import %></h2>
            <ul class="fl leftaligned">
                <li>
                    <input type="button" class="btn rounded" value="<%=Resources.labels.import %>" onclick="location.href='http://dotnetblogengine.net/clickonce/blogimporter/blog.importer.application?url=<%=Utils.AbsoluteWebRoot %>&username=<%=Page.User.Identity.Name %>'" />
                    <label>Run click-once application to import content saved as BlogML or RSS 2.0 into your blog. Internet Explorer only.</label>
                </li>
                <li>OR</li>
                <li>
                    <asp:FileUpload runat="server" CssClass="btn rounded" ID="txtUploadFile" Width="300" />
                </li>
                <li>
                    <asp:Button ID="btnBlogMLImport" runat="server" CssClass="btn rounded" Text="<%$Resources:labels,import %>" OnClick="BtnBlogMlImportClick" />
                    <label>Select saved BlogML file and import it into your blog. Any browser, BlogML only.</label>
                </li>
            </ul>
            
            <h2><%=Resources.labels.export %></h2>
            <ul class="fl leftaligned">
                <li>
                    <input type="button" class="btn rounded" value="<%=Resources.labels.export %>" onclick="location.href='blogml.axd'" />
                    <label>Export your blog's content into XML file (BlogML)</label>
                </li>
            </ul>

        </div>
    </div>
</asp:Content>
