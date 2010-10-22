<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="referrers.aspx.cs" Inherits="Admin.Tracking.Referrers" Title="Referrers" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
        <div class="content-box-left">
        <div style="float: right;">
            <asp:Button CssClass="btn rounded" runat="server" ID="btnSaveTop" Text="<%$ Resources:labels, saveSettings %>" />
        </div>
        <h1 style="border:none;">Referrers</h1>
        <fieldset style="margin:0;">
            <legend>Settings</legend>
            <table class="tblForm" style="margin:0;">
                <tr>
                <td>
                    <asp:CheckBox runat="Server" ID="cbEnableReferrers" AutoPostBack="true" />
                    <label for=""><%=Resources.labels.enableReferrerTracking %></label>
                </td>
                <td></td>
                </tr>
                <tr>
                <td>
                    <label class="lbl" for="<%= txtNumberOfDays.ClientID %>"><%= Resources.labels.numberOfDaysToKeep %></label>
                    <asp:TextBox ID="txtNumberOfDays" runat="server"></asp:TextBox><br />
                </td>
                <td>
                    <label class="lbl" for="<%=ddlDays.ClientID %>"><%=Resources.labels.selectDay %></label>
                    <asp:DropDownList runat="server" ID="ddlDays" AutoPostBack="true" Style="text-transform: capitalize"
                        DataTextFormatString="{0:d}">
                    </asp:DropDownList>
                </td>
                </tr>
            </table>
        </fieldset>
        <div>
            <h2><%=Resources.labels.referrers%></h2>
            <asp:GridView runat="server" ID="grid" BorderColor="#f8f8f8" BorderStyle="solid"
                BorderWidth="1px" RowStyle-BorderWidth="0" RowStyle-BorderStyle="None" GridLines="None"
                Width="100%" AlternatingRowStyle-BackColor="#f8f8f8" AlternatingRowStyle-BorderColor="#f8f8f8"
                HeaderStyle-BackColor="#F1F1F1" CellPadding="3" ShowFooter="true" AutoGenerateColumns="False"
                EnableViewState="false">
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
        </div>
        <div>
            <h2><%=Resources.labels.possibleSpam%></h2>
            <asp:GridView runat="server" ID="spamGrid" BorderColor="#f8f8f8" BorderStyle="solid"
                BorderWidth="1px" RowStyle-BorderWidth="0" RowStyle-BorderStyle="None" GridLines="None"
                Width="100%" AlternatingRowStyle-BackColor="#f8f8f8" AlternatingRowStyle-BorderColor="#f8f8f8"
                HeaderStyle-BackColor="#F1F1F1" CellPadding="3" ShowFooter="true" AutoGenerateColumns="False"
                EnableViewState="false">
                <Columns>
                    <asp:HyperLinkField HeaderText="<%$ Resources:labels, referrer %>" FooterStyle-HorizontalAlign="left"
                        DataNavigateUrlFields="url" Target="_blank" DataTextField="shortUrl" HeaderStyle-HorizontalAlign="left" />
                    <asp:BoundField HeaderText="Hits" DataField="hits" HeaderStyle-HorizontalAlign="center"
                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="40" />
                </Columns>
                <FooterStyle Font-Bold="true" HorizontalAlign="center" />
            </asp:GridView>
        </div>
        <div class="action_buttons">
            <asp:Button runat="server" CssClass="btn rounded" ID="btnSave" Text="<%$ Resources:labels, saveSettings %>" />
        </div>
        </div>
    </div>
</asp:Content>
