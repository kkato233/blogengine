<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor.ascx.cs" Inherits="User_controls_xmanager_SourceEditor" %>
<h1>Source Editor</h1>
<div>
    <asp:TextBox ID="txtEditor" runat="server" TextMode="multiLine" Width="100%" Height="350"></asp:TextBox>
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
</div>