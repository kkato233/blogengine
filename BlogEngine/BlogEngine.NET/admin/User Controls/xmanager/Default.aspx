﻿<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="User_controls_xdashboard_Default" Title="Extensions" %>
<%@ Reference Control = "Extensions.ascx" %>
<%@ Reference Control = "Editor.ascx" %>
<%@ Reference Control = "Settings.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
<br />
<div class="settings">
    <asp:HiddenField ID="args" runat="server" />
    <div>
        <asp:PlaceHolder ID="ucPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
</div>
</asp:Content>
