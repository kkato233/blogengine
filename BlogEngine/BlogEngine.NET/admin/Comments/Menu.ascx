<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin.Comments.Menu" %>
<%@ Import Namespace="BlogEngine.Core" %>
<ul>
    <% if(BlogSettings.Instance.EnableCommentsModeration){ %>
    <li <%=Current("Pending.aspx")%>><a href="Pending.aspx">Pending (<span id="pending_counter"><%=PendingCount%></span>)</a></li>
    <% } %>
    <li <%=Current("Approved.aspx")%>><a href="Approved.aspx"><%=Resources.labels.comments %> (<span id="comment_counter"><%=CommentCount%></span>)</a></li>
    <li <%=Current("Pingbacks.aspx")%>><a href="Pingbacks.aspx">Pingbacks & Trackbacks (<span id="pingback_counter"><%=PingbackCount%></span>)</a></li>
    <li <%=Current("Spam.aspx")%>><a href="Spam.aspx"><%=Resources.labels.spam %> (<span id="spam_counter"><%=SpamCount%></span>)</a></li>
    <li <%=Current("Settings.aspx")%>><a href="Settings.aspx"><%=Resources.labels.configuration %></a></li>
    <li <%=Current("Rules.aspx")%>><a href="Rules.aspx"><%=Resources.labels.rules %> & <%=Resources.labels.filters %></a></li>
</ul>