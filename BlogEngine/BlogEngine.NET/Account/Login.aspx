<%@ Page Title="Log In" Language="C#" MasterPageFile="Account.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <div class="login">
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                         <div class="boxRound">
                            <asp:TextBox ID="UserName" runat="server" AutoCompleteType="None" CssClass="textEntry"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                        <div class="boxRound">
                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        </div>
                        
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                    </p>
                    <p class="submitButton">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" ValidationGroup="LoginUserValidationGroup"/>
                        <span><a href="PasswordRetrieval.aspx">Forgot your password?</a></span>
                    </p>
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