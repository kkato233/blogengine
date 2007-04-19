<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default" Title="Untitled Page" %>
<%@ Register Src="User controls/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
    <uc1:PostList ID="PostList1" runat="server" />
</asp:Content>