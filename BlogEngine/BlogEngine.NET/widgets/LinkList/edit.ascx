﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="edit.ascx.cs" Inherits="Widgets.LinkList.Edit" %>
<fieldset>
    <legend><%=Resources.labels.addNewLink %></legend>
    <label for="<%=txtTitle.ClientID %>">
        <%=Resources.labels.title %></label>
    <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtTitle" ErrorMessage="<%$Resources:labels, enterTitle %>"
        ValidationGroup="add" /><br />
    <asp:TextBox runat="server" ID="txtTitle" Width="250px" />
    <asp:CheckBox runat="server" ID="cbNewWindow" Text="<%$Resources:labels, openLinkInNewWindow %>" />
    <br />
    <br />
    <label for="<%=txtUrl.ClientID %>">
        <%=Resources.labels.websiteUrl %></label>
    <asp:RegularExpressionValidator runat="Server" ControlToValidate="txtUrl" ValidationExpression="(http://|https://|)([\w-]+\.)+[\w-]+(/[\w- ./?%&=;~]*)?"
        ErrorMessage="<%$Resources:labels, enterValidUrl %>" ValidationGroup="add" /><br />
    <asp:TextBox runat="server" ID="txtUrl" Width="250px" Text="http://" />
    <asp:Button runat="server" ID="btnAdd" Text="<%$Resources:labels, addLink %>" ValidationGroup="add" />
</fieldset>
<br />
<asp:GridView runat="server" ID="grid" CellPadding="2" Width="100%" AutoGenerateColumns="False"
    AlternatingRowStyle-CssClass="alt" AlternatingRowStyle-BackColor="#F1F1F1" AutoGenerateDeleteButton="True"
    AutoGenerateEditButton="True">
    <HeaderStyle HorizontalAlign="left" />
    <Columns>
        <asp:TemplateField HeaderText="<%$ Resources:labels, title %>">
            <ItemTemplate>
                <%# Eval("title") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtTitle" Width="90%" Text='<%# Eval("title") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$Resources:labels, websiteUrl %>">
            <ItemTemplate>
                <%# Eval("url")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtUrl" Width="90%" Text='<%# Eval("url") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$Resources:labels, newWindow %>">
            <ItemTemplate>
                <asp:CheckBox runat="server" Enabled="False" ID="cbNewWindow" Checked='<%# bool.Parse((string)Eval("newwindow")) %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:CheckBox runat="server" ID="cbNewWindow" Checked='<%# bool.Parse((string)Eval("newwindow")) %>' />
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
