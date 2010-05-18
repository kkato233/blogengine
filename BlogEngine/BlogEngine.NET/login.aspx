<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" Title="Sign in" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:Login ID="Login1" runat="server" class="loginbox" BorderPadding="25">
    <TextBoxStyle Font-Size="1em" width="150" />   
</asp:Login>

<div style="text-align:center">
    <div>
        <asp:changepassword runat="server" id="changepassword1" visible="false" />
    </div>

    <div>
        <asp:loginstatus runat="server" id="lsLogout" visible="false" />
    </div>
</div>

</asp:Content>