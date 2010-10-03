﻿<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    ValidateRequest="False" CodeFile="Categories.aspx.cs" Inherits="admin_Pages_Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <div class="settings">
        <h1>
            <%=Resources.labels.categories %></h1>
        <asp:Label ID="lblNewCategory" runat="server" AssociatedControlID="txtNewCategory"
            Text="<%$Resources:labels,title %>" /><br />
        <asp:TextBox runat="Server" ID="txtNewCategory" Width="200" /><br />
        <asp:Label ID="lblNewNewDescription" runat="server" AssociatedControlID="txtNewNewDescription"
            Text="<%$Resources:labels,description %>" /><br />
        <asp:TextBox runat="Server" ID="txtNewNewDescription" Width="400" TextMode="MultiLine"
            Rows="4" /><br />
        <asp:Label ID="lblNewParent" runat="server" AssociatedControlID="ddlNewParent" Text="<%$Resources:labels,parent %>" /><br />
        <asp:DropDownList ID="ddlNewParent" Width="200" runat="server" />
        <br />
        <br />
        <asp:Button runat="server" ID="btnAdd" ValidationGroup="new" />
        <asp:CustomValidator runat="Server" ID="valExist" ValidationGroup="new" ControlToValidate="txtNewCategory"
            ErrorMessage="<%$Resources:labels,categoryAlreadyExists %>" Display="dynamic" />
        <asp:RequiredFieldValidator runat="Server" ValidationGroup="new" ControlToValidate="txtNewCategory"
            ErrorMessage="<%$Resources:labels,enterValidName %>" /><br />
        <hr />
        <asp:GridView runat="server" ID="grid" BorderColor="#f8f8f8" BorderStyle="solid"
            BorderWidth="1px" RowStyle-BorderWidth="0" RowStyle-BorderStyle="None" GridLines="None"
            Width="100%" AlternatingRowStyle-BackColor="#f8f8f8" AlternatingRowStyle-BorderColor="#f8f8f8"
            HeaderStyle-BackColor="#F1F1F1" CellPadding="3" AutoGenerateColumns="False" AutoGenerateDeleteButton="True"
            AutoGenerateEditButton="True">
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:labels, name %>">
                    <ItemTemplate>
                        <%# Server.HtmlEncode(Eval("title").ToString()) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtTitle" Text='<%# Eval("title") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:labels, description %>">
                    <ItemTemplate>
                        <%# Server.HtmlEncode(Eval("description").ToString())%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" MaxLength="255" ID="txtDescription" Text='<%# Eval("description") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Parent">
                    <ItemTemplate>
                        <%# GetParentTitle(Container.DataItem) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlParent" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
