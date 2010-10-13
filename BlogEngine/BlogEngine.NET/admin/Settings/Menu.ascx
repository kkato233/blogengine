<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin.Settings.Menu" %>
<div <%=Current("Main.aspx")%>><a href="Main.aspx"><%=Resources.labels.basic %></a></div>
<div <%=Current("Advanced.aspx")%>><a href="Advanced.aspx"><%=Resources.labels.advanced %></a></div>
<div <%=Current("Feed.aspx")%>><a href="Feed.aspx">Feed</a></div>
<div <%=Current("Email.aspx")%>><a href="Email.aspx"><%=Resources.labels.email %></a></div>
<div <%=Current("HeadTrack.aspx")%>><a href="HeadTrack.aspx">Header & Tracking</a></div>
<div <%=Current("Import.aspx")%>><a href="Import.aspx"><%=Resources.labels.import %> & <%=Resources.labels.export %></a></div>
