<%@ Page Title="Log In" Language="C#" MasterPageFile="Account.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"> 
    
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
        <LayoutTemplate>
            <div class="accountInfo">
                
                <div class="login">
                    <h2>Login</h2>  
                    <div class="field">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                         <div class="boxRound">
                            <asp:TextBox ID="UserName" runat="server" AutoCompleteType="None" CssClass="textEntry"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                        <div class="boxRound">
                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                    </div>
                    <div class="submitButton">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" OnClientClick="return ValidateLogin();"/>
                        <span><a href="PasswordRetrieval.aspx">Forgot your password?</a></span>
                    </div>
                </div>
            </div>
        </LayoutTemplate>
    </asp:Login>

    <% if (BlogEngine.Core.BlogSettings.Instance.EnableSelfRegistration){ %>
    <div id="LoginRegister">
        Don't have account yet? 
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Create now!</asp:HyperLink>
    </div>
    <% } %>
</asp:Content>