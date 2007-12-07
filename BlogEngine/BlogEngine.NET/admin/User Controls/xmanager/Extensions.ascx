<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Extensions.ascx.cs" Inherits="User_controls_xmanager_ExtensionsList" %>
<asp:Literal runat="server" Text="<h1>Extensions</h1>" />
<div id="lblErrorMsg" style="background:#ffcc66;padding:5px;border:1px solid #ccc;font-weight:600;" runat="server"></div>
<asp:Label ID="lblExtensions" runat="server" Text="Not found"></asp:Label>

<br /><br />
<div style="text-align:right">
  <asp:Button runat="Server" ID="btnRestart" 
    Text="Apply changes" 
    OnClientClick="return confirm('The website will be unavailable for a few seconds.\nAre you sure you wish to continue?')" 
  />
</div>