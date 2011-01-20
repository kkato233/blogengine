<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/admin/Extension Manager/Settings.ascx.cs" Inherits="Admin.ExtensionManager.UserControlsXmanagerParameters" %>
<h1><%=this.SettingName%></h1>
<div id="ErrorMsg" runat="server" style="color:Red; padding:5px 0 5px 0; display:block;"></div>
<div id="InfoMsg" runat="server" style="color:Green; padding:5px 0 5px 0; display:block;"></div>

<table width="100%" border="0">
    <tr>
        <td valign="top" style="width:100%">
            <div class="mgr">
                <asp:PlaceHolder ID="phAddForm" runat="server"></asp:PlaceHolder>
            </div>

            <div style="margin: 10px 0; padding-bottom: 10px; display:block">
                <asp:Button CssClass="btn primary" runat="server" ID="btnAdd" ValidationGroup="new" /> <%=Resources.labels.or %> <a href="default.cshtml"><%=Resources.labels.cancel %></a>
            </div>

            <asp:GridView ID="grid"  
                    runat="server"  
                    GridLines="None"
                    AutoGenerateColumns="False"
                    HeaderStyle-BackColor="#f9f9f9"
                    AlternatingRowStyle-BackColor="#f7f7f7"
                    CellPadding="3" 
                    HeaderStyle-HorizontalAlign="Left"
                    BorderStyle="Ridge"
                    BorderWidth="1"
                    Width="100%"
                    AllowPaging="True" 
                    AllowSorting="True"
                    onpageindexchanging="GridPageIndexChanging" 
                    OnRowDataBound="GridRowDataBound" >
             </asp:GridView>
        </td>
        <td valign="top" style="width:350px">
            <% if(!string.IsNullOrEmpty(this.Settings.Help)) { %>
            <div class="info" style="float:right; width: 350px; display:block; overflow:visible;"><%=this.Settings.Help%></div>
            <%} %>
            <div class="clear"></div>
        </td>
    </tr>
</table>



