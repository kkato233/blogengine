<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostList.ascx.cs" EnableViewState="false" Inherits="User_controls_PostList" %>
<div runat="server" id="posts" class="posts" />

<div id="postPaging">
  <asp:HyperLink runat="server" ID="hlPrev" Text="<< Previous posts" SkinID="pagingPrev" style="float:left" />
  <asp:HyperLink runat="server" ID="hlNext" Text="Next posts >>" SkinID="pagingNext" style="float:right" />
</div>