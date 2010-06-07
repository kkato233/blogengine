<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Account.master" AutoEventWireup="true" CodeFile="PasswordRetrieval.aspx.cs" Inherits="Account_PasswordRetrieval" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
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
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail"
                    CssClass="failureNotification" ErrorMessage="Email is required." ToolTip="Email is required."
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
            <p class="submitButton">
                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Send" ValidationGroup="LoginUserValidationGroup" OnClick="LoginButton_Click" />
                <span>or <a href="javascript:history.back()">Cancel</a></span>
            </p>
        </div>
    </div>
</asp:Content>