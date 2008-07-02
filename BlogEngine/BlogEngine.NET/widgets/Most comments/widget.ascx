<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_Most_comments_widget" %>
<asp:Repeater runat="server" ID="rep">
  <HeaderTemplate>
    <table summary="Most active commenters" style="border-collapse:collapse">
  </HeaderTemplate>
  
  <ItemTemplate>
    <tr>
      <td style="padding-bottom: 7px">
        <asp:Image runat="server" ID="imgAvatar" />
      </td>
      <td style="vertical-align :top; padding-left: 10px; line-height:17px">
        <strong><asp:Literal runat="Server" ID="litName" /></strong><br />
        <asp:Literal runat="server" ID="litNumber" />
        <asp:Image runat="server" ID="imgCountry" />
        <asp:Literal runat="server" ID="litCountry" />
      </td>
    </tr>
  </ItemTemplate>
  
  <FooterTemplate>
    </table>
  </FooterTemplate>
</asp:Repeater>