<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Blogroll.aspx.cs" Inherits="admin_Pages_blogroll" Title="Blogroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <div class="settings">
        <h1 style="margin: 0 0 5px 0">
            <%=Resources.labels.settings %></h1>
        <div>
            <asp:Label runat="server" AssociatedControlID="ddlVisiblePosts" CssClass="wide" Text='<%$ Code: Resources.labels.numberOfDisplayedItems %>' />
            <asp:DropDownList runat="server" ID="ddlVisiblePosts">
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
            </asp:DropDownList>
        </div>
        <div style="padding-bottom: 5px">
            <asp:Label runat="server" AssociatedControlID="txtMaxLength" CssClass="wide" Text='<%$ Code: Resources.labels.maxLengthOfItems %>' />
            <asp:TextBox runat="server" ID="txtMaxLength" MaxLength="3" Width="50" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtMaxLength"
                Operator="dataTypeCheck" Type="integer" ValidationGroup="settings" ErrorMessage="<%$Resources:labels,noValidNumber %>" />
        </div>
        <div>
            <asp:Label runat="server" AssociatedControlID="txtUpdateFrequency" CssClass="wide"
                Text='<%$ Code: Resources.labels.updateFrequenzy %>' />
            <asp:TextBox runat="server" ID="txtUpdateFrequency" MaxLength="3" Width="50" />
            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtUpdateFrequency"
                Operator="dataTypeCheck" Type="integer" ValidationGroup="settings" ErrorMessage="<%$Resources:labels,noValidNumber %>" />
        </div>
        <div style="text-align: right">
            <asp:Button runat="server" ID="btnSaveSettings" ValidationGroup="settings" Width="120" />
        </div>
    </div>
    <div class="settings">
        <h1 style="margin: 0 0 5px 0">
            <%=Resources.labels.add %>
            blog</h1>
        <div style="margin-bottom: 3px">
            <asp:Label runat="server" AssociatedControlID="txtTitle" CssClass="wide" Text='<%$ Code: Resources.labels.title %>' />
            <asp:TextBox runat="server" ID="txtTitle" Width="600px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtTitle"
                ErrorMessage="<%$Resources:labels,required %>" ValidationGroup="addNew" />
        </div>
        <div style="margin-bottom: 3px">
            <asp:Label runat="server" AssociatedControlID="txtDescription" CssClass="wide" Text='<%$ Code: Resources.labels.description %>' />
            <asp:TextBox runat="server" ID="txtDescription" Width="600px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtDescription"
                ErrorMessage="<%$Resources:labels,required %>" ValidationGroup="addNew" />
        </div>
        <div style="margin-bottom: 3px">
            <asp:Label runat="server" AssociatedControlID="txtWebUrl" CssClass="wide" Text='<%$ Code: Resources.labels.website %>' />
            <asp:TextBox runat="server" ID="txtWebUrl" Width="600px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtWebUrl"
                ErrorMessage="<%$Resources:labels,required %>" Display="Dynamic" ValidationGroup="addNew" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtWebUrl"
                ErrorMessage="<%$Resources:labels,invalid %>" EnableClientScript="false" OnServerValidate="validateWebUrl"
                ValidationGroup="addnew"></asp:CustomValidator>
        </div>
        <div style="margin-bottom: 3px">
            <asp:Label runat="server" AssociatedControlID="txtFeedUrl" CssClass="wide" Text="RSS url" />
            <asp:TextBox runat="server" ID="txtFeedUrl" Width="600px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtFeedUrl"
                ErrorMessage="<%$Resources:labels,required %>" Display="Dynamic" ValidationGroup="addNew" />
            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtFeedUrl"
                ErrorMessage="<%$Resources:labels,invalid %>" EnableClientScript="false" OnServerValidate="validateFeedUrl"
                ValidationGroup="addnew"></asp:CustomValidator>
        </div>
        <asp:Label runat="server" AssociatedControlID="cblXfn" CssClass="wide" Text="XFN tag" />
        <asp:CheckBoxList runat="server" ID="cblXfn" CssClass="nowidth" RepeatColumns="8">
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
        <div style="padding: 3px; text-align: right; margin-bottom: 5px">
            <asp:Button runat="server" ID="btnSave" ValidationGroup="addNew" Width="120" />
        </div>
        <asp:GridView runat="server" ID="grid" BorderColor="#f8f8f8" BorderStyle="solid"
            BorderWidth="1px" RowStyle-BorderWidth="0" RowStyle-BorderStyle="None" GridLines="None"
            Width="100%" AlternatingRowStyle-BackColor="#f8f8f8" AlternatingRowStyle-BorderColor="#f8f8f8"
            HeaderStyle-BackColor="#F1F1F1" CellPadding="3" AutoGenerateColumns="False" OnRowDeleting="grid_RowDeleting"
            OnRowCommand="grid_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="feedLink" runat="server" ImageUrl="~/pics/rssButton.gif" NavigateUrl='<%# Eval("FeedUrl").ToString() %>'
                            Text="<%# string.Empty %>"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-BackColor="Transparent">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibMoveUp" ImageUrl="~/pics/up_arrow_small.gif" runat="server"
                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="moveUp"
                            Width="16" Height="8" />
                        <asp:ImageButton ID="ibMoveDown" ImageUrl="~/pics/down_arrow_small.gif" runat="server"
                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="moveDown"
                            Width="16" Height="8" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("BlogUrl").ToString() %>'
                            Text='<%# Eval("Title") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Description") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
