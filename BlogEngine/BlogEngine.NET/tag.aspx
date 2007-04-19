<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tag.aspx.cs" Inherits="tag" %>
<%@ Register Src="User controls/PostList.ascx" TagName="PostList" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="cphBody" Runat="Server">
    <uc1:PostList ID="PostList1" runat="server" />
</asp:Content>