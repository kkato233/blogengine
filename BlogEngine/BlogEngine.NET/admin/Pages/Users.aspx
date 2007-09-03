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
             HeaderStyle-HorizontalAlign="left" DataKeyNames="username" OnLoad="gridUsers_Load">
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
                <asp:TemplateField HeaderText="Roles" ItemStyle-Wrap="false" ItemStyle-Width="100%" >
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left" />
        </asp:GridView>
    </div>
</asp:Content>
