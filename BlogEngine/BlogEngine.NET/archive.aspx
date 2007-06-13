<%@ Page Language="C#" AutoEventWireup="true" CodeFile="archive.aspx.cs" Inherits="archive" Title="Archive" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="archive">
    <h1><%=Resources.labels.archive %></h1>
    <ul runat="server" id="ulMenu" />
    <asp:placeholder runat="server" id="phArchive" />
  </div>
</asp:Content>