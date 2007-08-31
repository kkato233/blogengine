<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Users.aspx.cs" Inherits="admin_newuser" Title="Create new user" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <br />
    <div class="settings">
        <h1>
            <%=Resources.labels.createNewUser %>
        </h1>
        <br />
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" LoginCreatedUser="false">
            <WizardSteps>
                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" />
                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server" />
            </WizardSteps>
        </asp:CreateUserWizard>
    </div>
    <br />
    <div class="settings">
        <asp:GridView runat="server" ID="gridUsers" AutoGenerateColumns="false" UseAccessibleHeader="true"
            Width="100%" HeaderStyle-HorizontalAlign="left" DataKeyNames="username">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="<%$ Resources:labels, userName %>" />
                <asp:BoundField DataField="email" HeaderText="E-mail" />
                <asp:TemplateField HeaderText="<%$ Resources:labels, delete %>">
                    <ItemTemplate>
                        <a href="?delete=<%# Eval("username") %>" onclick="return confirm('<%# string.Format(Resources.labels.areYouSure, Resources.labels.delete.ToLower(), Eval("username")) %>')">
                            <%=Resources.labels.delete %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roles">
                    <ItemTemplate>
                        <asp:CheckBoxList ID="cblRoles" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" OnPreRender="cblRoles_PreRender" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="cblRoles_SelectedIndexChanged"  >
                            <asp:ListItem Value="Administrators">Administrator</asp:ListItem>
                            <asp:ListItem Value="Editors">Editor</asp:ListItem>
                        </asp:CheckBoxList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left" />
        </asp:GridView>
    </div>
</asp:Content>
