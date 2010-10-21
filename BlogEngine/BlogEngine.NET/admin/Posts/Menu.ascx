<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin.Settings.Menu" %>
<ul>
    <li <%=Current("Posts.aspx")%>><a href="Posts.aspx">Posts</a></li>
    <li <%=Current("Categories.aspx")%>><a href="Categories.aspx">Categories</a></li>
</ul>