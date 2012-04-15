﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="edit.ascx.cs" Inherits="Widgets.Newsletter.Edit" %>
<style type="text/css">
    .beTable { border-collapse:collapse; border: 1px solid #dcdcdc; border-top:solid 5px #c8c8c8; margin: 10px 0; width: 100%; position:static;}
    .beTable th { padding: 10px 5px; background-color:#e5e5e5; text-align: left; }
    .beTable td { padding: 8px; margin: 0; min-height:30px; }
    .beTable tr.alt td { background-color:#f5f5f5; }
    .beTable a { color: #CE3900; text-decoration: none; }
</style>

<label style="display: block; margin: 10px 0 5px 0" for="<%=txtPrefix %>">Email subject prefix</label>
<asp:TextBox runat="server" ID="txtPrefix" Width="300" /><br />

<asp:GridView 
  runat="server"
  id="gridEmails"
  CssClass="beTable"
  BorderStyle="solid"
  RowStyle-BorderWidth="0" 
  RowStyle-BorderStyle="None" 
  GridLines="None"
  Width="100%" 
  AlternatingRowStyle-BackColor="#f8f8f8" 
  AlternatingRowStyle-BorderColor="#f8f8f8"
  HeaderStyle-BackColor="#F1F1F1" 
  CellPadding="3"
  AutoGenerateColumns="False"
  AutoGenerateDeleteButton="true">
 
  <Columns>
    <asp:BoundField HeaderText="Email" DataField="innertext" />
  </Columns> 
</asp:GridView>