<%@ Page Title="Log In" Language="C#" MasterPageFile="Account.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"> 
    
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
        <LayoutTemplate>
            <div class="accountInfo">
                
                <div class="login">
                    <h2>Login</h2>  
                    <div class="field">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"><%=Resources.labels.userName %>:</asp:Label>
                         <div class="boxRound">
                            <asp:TextBox ID="UserName" runat="server" AutoCompleteType="None" CssClass="textEntry"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"><%=Resources.labels.password %>:</asp:Label>
                        <div class="boxRound">
                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline"><%=Resources.labels.rememberMe %></asp:Label>
                    </div>
                    <div class="submitButton">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" OnClientClick="return ValidateLogin();"/>
                        <span><asp:HyperLink runat=server ID=linkForgotPassword NavigateUrl="~/Account/PasswordRetrieval.aspx" Text="<%$ Resources:labels,forgotPassword %>" />
                        </span>
                    </div>
                </div>
            </div>
        </LayoutTemplate>
    </asp:Login>

    <% if (BlogEngine.Core.BlogSettings.Instance.EnableSelfRegistration){ %>
    <div id="LoginRegister">
        <%=Resources.labels.dontHaveAccount %> 
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false"/>
    </div>
    <% } %>
</asp:Content>