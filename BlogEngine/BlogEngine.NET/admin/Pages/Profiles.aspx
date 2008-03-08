<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Profiles.aspx.cs" Inherits="admin_profiles" Title="Modify Profiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <br />
    <div class="settings">
        <h1>
            <%=Resources.labels.userProfiles %>
        </h1>
        <asp:Panel ID="pnlAdmin" runat="server" Visible='<%# User.IsInRole("Administrator") %>'>
            <asp:DropDownList ID="ddlUserList" runat="server" >
            </asp:DropDownList>
            <asp:LinkButton ID="lbChangeUserProfile" runat="server" 
                Text='<%# Resources.labels.view %>' onclick="lbChangeUserProfile_Click"></asp:LinkButton>
        </asp:Panel>
        <br />
    </div>
    <br />
    <div >
        <asp:Label ID="lbFirstName" runat="server" AssociatedControlID="tbFirstName"><%= Resources.labels.firstName %>&nbsp;</asp:Label><asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox><br />
        <asp:Label ID="lbLastName" runat="server" AssociatedControlID="tbLastName"><%= Resources.labels.lastName %>&nbsp;</asp:Label><asp:TextBox ID="tbLastName" runat="server"></asp:TextBox><br />
        <p>
        <asp:LinkButton ID="lbSaveProfile" runat="server" onclick="lbSaveProfile_Click" ><%=Resources.labels.saveProfile %></asp:LinkButton>
        </p>
    </div>
</asp:Content>
