<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Account.master" AutoEventWireup="true" CodeFile="PasswordRetrieval.aspx.cs" Inherits="Account_PasswordRetrieval" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:RequiredFieldValidator 
        ID="EmailRequired" 
        runat="server"
        visible="false" 
        ControlToValidate="txtEmail"
        CssClass="warning" 
        ErrorMessage="Email is required." 
        ToolTip="Email is required."
        ValidationGroup="LoginUserValidationGroup">
            Email Required <a href="javascript: return Hide('EmailRequired');">X</a>
    </asp:RequiredFieldValidator>
    
    <h2>Password Retrieval</h2>
    <p>
        Type your email address in the field above and your password will be emailed to you shortly.
    </p>
    <div class="accountInfo">
        <div class="login">
            <div>
                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="txtEmail">Email:</asp:Label>
                <div class="boxRound">
                    <asp:TextBox ID="txtEmail" runat="server" AutoCompleteType="None" CssClass="textEntry"></asp:TextBox>
                </div>
                
            </div>
            <p class="submitButton">
                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Send" ValidationGroup="LoginUserValidationGroup" OnClick="LoginButton_Click" OnClientClick="return ValidatePasswordRetrieval()" />
                <span>or <a runat="server" href="~/Account/Login.aspx">Cancel</a></span>
            </p>
        </div>
    </div>
</asp:Content>