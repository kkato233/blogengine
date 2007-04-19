<%@ Page Language="C#" AutoEventWireup="true" CodeFile="archive.aspx.cs" Inherits="archive" Title="Archive" EnableViewState="false" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="archive">
    <h1>Archive</h1>
    <ul runat="server" id="ulMenu" />
    <asp:placeholder runat="server" id="phArchive" />
  </div>
</asp:Content>