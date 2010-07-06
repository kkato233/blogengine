<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="admin_Account_Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td style="width:auto">
                <div class="settings" id="newrole" runat="server">
                    <h1>Roles</h1>
                    <div>
                        <table id="RolesList" class="BETable" width="500">
                            <tr> 
                                <th scope="col" abbr="Roles" style="width:420px">Name</th> 
                                <th scope="col" abbr="" style="width: 80px;">&nbsp;</th> 
                            </tr> 
                            <% foreach (string role in Roles.GetAllRoles()) { %>
                                <tr id="<%= role %>">
                                    <td class="editable"><%= role %></td>
                                    <td><a href="#" class="deleteButton">delete</a></td>
                                </tr>
                            <% } %>
                        </table>
                    </div>
                    <div id="AddRoleForm" style="margin: 10px 0; display: none">
                        <asp:TextBox ID="txtCreateRole" runat="server" Width="200"></asp:TextBox>
                        <asp:Button ID="btnCreateRole" runat="server" Text="Create Role" />
                        or
                        <asp:LinkButton ID="lnkCancelNewRole" OnClientClick="return Hide('AddRoleForm')" runat="server" Text="Cancel" />
                    </div>
                    <div style="clear:both; margin-top: 10px; width:480px">
                        <a href="#" onclick="Show('AddRoleForm');" style="float:right">Add new role</a>
                    </div>
                     <div style="clear:both;">&nbsp;</div>
                </div>
            </td>
            <td style="width:180px; vertical-align: top">
                <ul id="RightList">
                    <li><a href="Users.aspx">Users</a></li>
                    <li><a href="Roles.aspx" class="selected">Roles</a></li>
                    <li><a href="Profiles.aspx">Profiles</a></li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>