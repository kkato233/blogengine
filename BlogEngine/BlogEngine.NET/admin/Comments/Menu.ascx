<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin.Comments.Menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<% if(BlogSettings.Instance.EnableCommentsModeration){ %>
<div <%=Current("Pending.aspx")%>><a href="Pending.aspx">Pending (<span id="pending_counter"><%=PendingCount%></span>)</a></div>
<% } %>
<div <%=Current("Approved.aspx")%>><a href="Approved.aspx"><%=Resources.labels.comments %> (<span id="comment_counter"><%=CommentCount%></span>)</a></div>
<div <%=Current("Pingbacks.aspx")%>><a href="Pingbacks.aspx">Pingbacks & Trackbacks (<span id="pingback_counter"><%=PingbackCount%></span>)</a></div>
<div <%=Current("Spam.aspx")%>><a href="Spam.aspx"><%=Resources.labels.spam %> (<span id="spam_counter"><%=SpamCount%></span>)</a></div>
<div <%=Current("Settings.aspx")%>><a href="Settings.aspx"><%=Resources.labels.configuration %></a></div>
<div <%=Current("Rules.aspx")%>><a href="Rules.aspx"><%=Resources.labels.rules %> & <%=Resources.labels.filters %></a></div>
