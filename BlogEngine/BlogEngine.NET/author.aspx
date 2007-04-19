<%@ Page Language="C#" AutoEventWireup="true" CodeFile="author.aspx.cs" Inherits="author" %>
<%@ Register Src="User controls/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="cphBody" Runat="Server">
    <uc1:PostList ID="PostList1" runat="server" />
</asp:Content>