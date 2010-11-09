<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
	<div class="content-box-outer">
		<div class="content-box-full">
            <div class="dashboardWidget rounded">
                <h2>Statistics</h2>
                    <ul>
                        <li>
                            <%=PostsPublished%> Posts <a href="Posts/Posts.aspx">View all</a>
                        </li>
                        <li>
                            <%=PagesCount%> Pages <a href="Pages/Pages.aspx">View all</a>
                        </li>
                        <li>
                            <%=CommentsAll%> Comments <a href="Comments/Approved.aspx">View all</a>
                        </li>
                        <li>
                            <%=CategoriesCount%> Categories <a href="Posts/Categories.aspx">View all</a>
                        </li>
                        <li>
                            <%=TagsCount%> Tags <a href="#">View all</a>
                        </li>
                        <li>
                            <%=UsersCount%> Users <a href="Users/Users.aspx">View all</a>
                        </li>
                    </ul>
            </div>
            <div class="dashboardWidget rounded">
                <h2>Draft posts</h2>
                <ul id="DraftPosts" runat="server" visible="false"></ul>
                <a href="Posts/Add_entry.aspx">Add new post</a>
            </div>
            <div class="dashboardWidget rounded">
                <h2>Draft pages</h2>
                <ul id="DraftPages" runat="server" visible="false"></ul>
                <a href="Pages/EditPage.aspx">Add new page</a>
            </div>
            <div class="dashboardWidget rounded">
                <h2>Trash</h2>
                <a href="Trash.aspx">View All</a> &nbsp;&nbsp;
                <a href="#" onclick="return ProcessTrash('Purge', 'All');">Clear</a>
            </div>
            <div class="dashboardWidget rounded">
                <h2>Recent comments</h2>
                <p>Recent comments if moderation is off, or pending if it is on</p>
                <a href="Comments/Approved.aspx">View all comments</a>
            </div>

            <div style="clear:both">&nbsp;</div>
            
        </div>
        
    </div>



</asp:Content>

