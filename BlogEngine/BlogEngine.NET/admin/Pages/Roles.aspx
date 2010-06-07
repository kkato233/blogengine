<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="admin_Account_Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

    <div class="settings" id="newrole" runat="server"> 
        <h1>Create Role</h1>
        <div style="margin-bottom:3px">
            <label for="<%=txtCreateRole.ClientID %>">New Role</label>
            <asp:textbox id="txtCreateRole" runat="server" Width="300"></asp:textbox>
        </div>
        <div>
            <label>&nbsp;</label>
            <asp:button id="btnCreateRole" runat="server" text="Create Role" onclick="btnCreateRole_Click" OnClientClick="return ValidateEmptyField('txtCreateRole', 'Role Name is required');" />
        </div>
    </div>

    <div class="settings" id="roles" runat="server"> 
        <h1>Delete Role</h1>
        <div style="margin-bottom:3px">
            <label for="<%=lbxAvailableRoles.ClientID %>">Roles</label>
            <asp:listbox id="lbxAvailableRoles" runat="server" Width="300"></asp:listbox>
        </div>
        <div>
            <label>&nbsp;</label>
            <asp:button id="btnDeleteRole" runat="server" text="Delete Selected Role" onclick="btnDeleteRole_Click" OnClientClick="return ValidateNotSelectedField('lbxAvailableRoles', 'Could not delete the role: nothing selected');" />
        </div>
    </div>
</asp:Content>

