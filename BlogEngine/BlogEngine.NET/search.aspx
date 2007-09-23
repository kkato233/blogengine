<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="search.aspx.cs" Inherits="search" %>
<%@ Import Namespace="BlogEngine.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div class="searchpage post">
    <h1 runat="server" id="h1Headline" />
    <br />
    
    <div id="searchpage">
      <input type="text" name="q" id="q" value="<%=Request.QueryString["q"] %>" onkeypress="if(event.keyCode==13) SearchPage()" />
      <input type="button" value="<%=Resources.labels.search %>" onclick="SearchPage()" onkeypress="SearchPage()" />
      <% if (BlogSettings.Instance.EnableCommentSearch){ %>
      <input type="checkbox" name="comment" id="comment" /><label for="comment"><%=BlogSettings.Instance.SearchCommentLabelText %></label>
      <%} %>
    </div>

    <script type="text/javascript">      
      var check = document.getElementById('comment');
      
      function SearchPage()
      {        
        var searchTerm = encodeURIComponent(document.getElementById('q').value);
        var include = check.checked;
        var comment = '&comment=true';
        
        if (!include)
        {
          comment = ''
        }
        
        location.href = 'search.aspx?q=' + searchTerm + comment;
      }
      
      check.checked = <%=(Request.QueryString["comment"] != null).ToString().ToLowerInvariant() %>
    </script>
  
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