<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="referrers.aspx.cs" Inherits="admin_Pages_referrers" Title="Referrers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <label for="">
        <%=Resources.labels.enableReferrerTracking %></label>
    <asp:CheckBox runat="Server" ID="cbEnableReferrers" AutoPostBack="true" /><br />
    <hr />
    <div style="text-align: right">
        <asp:Button runat="server" ID="btnSaveTop" 
            Text="<%$ Resources:labels, saveSettings %>" />
    </div>
    <label for="<%= txtNumberOfDays.ClientID %>">
        <%= Resources.labels.numberOfDaysToKeep %></label>
    <asp:TextBox ID="txtNumberOfDays" runat="server"></asp:TextBox><br />
    <label for="<%=ddlDays.ClientID %>">
        <%=Resources.labels.selectDay %></label>
    <asp:DropDownList runat="server" ID="ddlDays" AutoPostBack="true" Style="text-transform: capitalize"
        DataTextFormatString="{0:d}">
    </asp:DropDownList>
    <br />
    <br />
    <asp:GridView runat="server" ID="grid" Width="100%" ShowFooter="true" GridLines="None"
        AutoGenerateColumns="False" CssClass="referrer" EnableViewState="false">
        <Columns>
            <asp:HyperLinkField HeaderText="<%$ Resources:labels, referrer %>" FooterStyle-HorizontalAlign="left"
                DataNavigateUrlFields="url" Target="_blank" DataTextField="shortUrl" HeaderStyle-HorizontalAlign="left" />
            <asp:HyperLinkField HeaderText="<%$ Resources:labels, link %>" FooterStyle-HorizontalAlign="left"
                DataNavigateUrlFields="target" Target="_blank" DataTextField="shortTarget" HeaderStyle-HorizontalAlign="left" />
            <asp:BoundField HeaderText="Hits" DataField="hits" HeaderStyle-HorizontalAlign="center"
                ItemStyle-HorizontalAlign="center" ItemStyle-Width="40" />
        </Columns>
        <FooterStyle Font-Bold="true" HorizontalAlign="center" />
    </asp:GridView>
    <br />
    <asp:GridView runat="server" CaptionAlign="left" Caption="<%$ Resources:labels, possibleSpam %>"
        ID="spamGrid" Width="100%" ShowFooter="true" GridLines="None" AutoGenerateColumns="False"
        CssClass="referrer" EnableViewState="false">
        <Columns>
            <asp:HyperLinkField HeaderText="<%$ Resources:labels, referrer %>" FooterStyle-HorizontalAlign="left"
                DataNavigateUrlFields="url" Target="_blank" DataTextField="shortUrl" HeaderStyle-HorizontalAlign="left" />
            <asp:BoundField HeaderText="Hits" DataField="hits" HeaderStyle-HorizontalAlign="center"
                ItemStyle-HorizontalAlign="center" ItemStyle-Width="40" />
        </Columns>
        <FooterStyle Font-Bold="true" HorizontalAlign="center" />
    </asp:GridView>
    <div align="right">
        <asp:Button runat="server" ID="btnSave" 
            Text="<%$ Resources:labels, saveSettings %>"/></div>
</asp:Content>