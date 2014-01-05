﻿<%@ Page Title="" Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Admin.Extensions.Settings" %>
<%@ Reference Control = "Settings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <div class="content-box-outer">
		<div class="content-box-left">
            <asp:PlaceHolder ID="ucPlaceHolder" runat="server"></asp:PlaceHolder>
            <asp:HiddenField ID="args" runat="server" />
		</div>
	</div>
</asp:Content>
