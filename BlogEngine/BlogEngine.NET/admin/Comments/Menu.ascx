﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin_Comments_Menu" %>
<table width="100%" style="padding: 0; margin: 0; margin-bottom: 10px; display:none">
    <tr>
        <td>
            <h1 style="margin-bottom: -14px; margin-right: -5px"><span id="hdr" runat="server" /></h1>
        </td>
        <td style="padding: 0; margin: 0;">
            <div class="tabs" style="margin-bottom: 0; padding-bottom: 0;">
                <ul id="UlMenu" runat="server" style="float: right"></ul>
            </div>
        </td>
    </tr>       
</table>

<div <%=Current("Approved.aspx")%>><a href="Approved.aspx"><%=Resources.labels.approved %></a></div>
<div <%=Current("Pending.aspx")%>><a href="Pending.aspx">Pending</a></div>
<div <%=Current("Spam.aspx")%>><a href="Spam.aspx"><%=Resources.labels.spam %></a></div>
<div <%=Current("Settings.aspx")%>><a href="Settings.aspx"><%=Resources.labels.configuration %></a></div>