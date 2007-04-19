<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Blogroll.aspx.cs" Inherits="admin_Pages_blogroll" Title="Blogroll" %>
<%@ Reference Control="~/User controls/blogroll.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">

  <h1 style="margin: 0 0 5px 0">Settings</h1>
  
  <label for="<%=ddlVisiblePosts.ClientID %>" class="wide"># of displayed items</label>
  <asp:DropDownList runat="server" id="ddlVisiblePosts">
    <asp:ListItem Text="0" />
    <asp:ListItem Text="1" />
    <asp:ListItem Text="2" />
    <asp:ListItem Text="3" />
    <asp:ListItem Text="4" />
    <asp:ListItem Text="5" />
    <asp:ListItem Text="6" />
    <asp:ListItem Text="7" />
    <asp:ListItem Text="8" />
    <asp:ListItem Text="9" />
    <asp:ListItem Text="10" />
  </asp:DropDownList><br />
  
  <label for="<%=txtMaxLength.ClientID %>" class="wide">Max lenght of items</label>
  <asp:TextBox runat="server" ID="txtMaxLength" MaxLength="3" Width="50" />
  <asp:CompareValidator runat="server" ControlToValidate="txtMaxLength" Operator="dataTypeCheck" Type="integer" ValidationGroup="settings" ErrorMessage="Not a valid number" />
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSaveSettings" Text="Save settings" ValidationGroup="settings" />
  </div>
  
  <hr />
  
  <h1 style="margin: 0 0 5px 0">Add blog</h1>

  <label for="<%=txtTitle.ClientID %>" class="wide">Title</label>
  <asp:TextBox runat="server" ID="txtTitle" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtTitle" ErrorMessage="required" /><br />
  
  <label for="<%=txtDescription.ClientID %>" class="wide">Description</label>
  <asp:TextBox runat="server" ID="txtDescription" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtDescription" ErrorMessage="required" /><br />
  
  <label for="<%=txtWebUrl.ClientID %>" class="wide">Website</label>
  <asp:TextBox runat="server" ID="txtWebUrl" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtWebUrl" ErrorMessage="required" /><br />
  
  <label for="<%=txtFeedUrl.ClientID %>" class="wide">RSS url</label>
  <asp:TextBox runat="server" ID="txtFeedUrl" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtFeedUrl" ErrorMessage="required" /><br />
  
  <label for="<%=cblXfn.ClientID %>" class="wide">XFN tag</label>
  <asp:CheckBoxList runat="server" ID="cblXfn" RepeatColumns="8">
    <asp:ListItem Text="contact" />
    <asp:ListItem Text="acquaintance " />
    <asp:ListItem Text="friend " />
    <asp:ListItem Text="met" />
    <asp:ListItem Text="co-worker" />
    <asp:ListItem Text="colleague " />
    <asp:ListItem Text="co-resident" />
    <asp:ListItem Text="neighbor " />
    <asp:ListItem Text="child" />
    <asp:ListItem Text="parent" />
    <asp:ListItem Text="sibling" />
    <asp:ListItem Text="spouse" />
    <asp:ListItem Text="kin" />
    <asp:ListItem Text="muse" />
    <asp:ListItem Text="crush" />
    <asp:ListItem Text="date" />
    <asp:ListItem Text="sweetheart" />
    <asp:ListItem Text="me" />
  </asp:CheckBoxList>
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSave" Text="Add blog" />
  </div>
  
  <hr />
  
  <asp:Repeater runat="Server" ID="rep">
    <HeaderTemplate>
      <table style="width:100%;background-color:White" cellspacing="0" cellpadding="3" summary="Blogroll">
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td>
          <a href="<%#((System.Xml.XmlNode)Container.DataItem).Attributes["xmlUrl"].Value %>"><img src="../../pics/rssButton.gif" alt="RSS feed" /></a>
          <a href="<%#((System.Xml.XmlNode)Container.DataItem).Attributes["htmlUrl"].Value %>"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["title"].Value %></a>
          &nbsp;<%#((System.Xml.XmlNode)Container.DataItem).Attributes["description"].Value %>
          &nbsp;(<%#((System.Xml.XmlNode)Container.DataItem).Attributes["xfn"].Value.Replace(";", " ")%>)
        </td>
        <td style="width:50px">
          <a href="?delete=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["title"].Value %>" onclick="return confirm('Are you sure?')">Delete</a>
        </td>
      </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
      <tr class="alt">
        <td>
          <a href="<%#((System.Xml.XmlNode)Container.DataItem).Attributes["xmlUrl"].Value %>"><img src="../../pics/rssButton.gif" alt="RSS feed" /></a>
          <a href="<%#((System.Xml.XmlNode)Container.DataItem).Attributes["htmlUrl"].Value %>"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["title"].Value %></a>
          &nbsp;<%#((System.Xml.XmlNode)Container.DataItem).Attributes["description"].Value %>
          &nbsp;(<%#((System.Xml.XmlNode)Container.DataItem).Attributes["xfn"].Value.Replace(";", " ") %>)
        </td>
        <td style="width:50px">
          <a href="?delete=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["title"].Value %>" onclick="return confirm('Are you sure?')">Delete</a>
        </td>
      </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
      </Table>
    </FooterTemplate>
  </asp:Repeater>
</asp:Content>
