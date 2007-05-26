<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="referrers.aspx.cs" Inherits="admin_Pages_referrers" Title="Referrers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
  <label for="">Enable referrer tracking</label>
  <asp:CheckBox runat="Server" ID="cbEnableReferrers" AutoPostBack="true" /><br /><hr />

  <label for="<%=ddlDays.ClientID %>">Select day</label>
  <asp:DropDownList runat="server" id="ddlDays" AutoPostBack="true">
    <asp:ListItem Text="Monday" />
    <asp:ListItem Text="Tuesday" />
    <asp:ListItem Text="Wednesday" />
    <asp:ListItem Text="Thursday" />
    <asp:ListItem Text="Friday" />
    <asp:ListItem Text="Saturday" />
    <asp:ListItem Text="Sunday" />
  </asp:DropDownList>
  
  <br /><br />
  
  <asp:GridView runat="server" ID="grid" Width="100%" GridLines="None" AutoGenerateColumns="False" CssClass="referrer" EnableViewState="false">
    <Columns>
      <asp:HyperLinkField HeaderText="Referrer" DataNavigateUrlFields="url" Target="_blank" DataTextField="shortUrl" HeaderStyle-HorizontalAlign="left" />
      <asp:BoundField HeaderText="Hits" DataField="hits" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" />
    </Columns>
  </asp:GridView>
</asp:Content>