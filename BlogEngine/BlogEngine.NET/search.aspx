<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="search.aspx.cs" Inherits="search" %>
<%@ Import Namespace="BlogEngine.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div class="searchpage post">
    <h1 runat="server" id="h1Headline" />
    <br />
    <blog:SearchBox runat="Server" /><br /><br />
  
    <asp:repeater runat="server" id="rep">
      <ItemTemplate>
        <div class="searchresult">
          <a href="<%# Eval("RelativeLink") %>"><%# Eval("Title") %></a>
          <span class="text"><%# GetContent((string)Eval("Description"), (string)Eval("Content")) %></span>
         <span class="type" runat="server" id="type" />
          <span class="url"><%# ShortenUrl((Uri)Eval("RelativeLink")) %></span>
        </div>
      </ItemTemplate>
    </asp:repeater>
    
    <ul id="paging" runat="Server" class="paging" />
  </div>
</asp:Content>