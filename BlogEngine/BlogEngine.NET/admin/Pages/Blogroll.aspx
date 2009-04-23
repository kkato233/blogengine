<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Blogroll.aspx.cs" Inherits="admin_Pages_blogroll" Title="Blogroll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">

<br />

<div class="settings">
  <h1 style="margin: 0 0 5px 0"><%=Resources.labels.settings %></h1>
  
 <label for="<%=ddlVisiblePosts.ClientID %>" class="wide"><%=Resources.labels.numberOfDisplayedItems %></label>
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
  
  <label for="<%=txtMaxLength.ClientID %>" class="wide"><%=Resources.labels.maxLengthOfItems %></label>
  <asp:TextBox runat="server" ID="txtMaxLength" MaxLength="3" Width="50" />
  <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtMaxLength" Operator="dataTypeCheck" Type="integer" ValidationGroup="settings" ErrorMessage="Not a valid number" /><br />
  
  <label for="<%=txtUpdateFrequency.ClientID %>" class="wide"><%=Resources.labels.updateFrequenzy %></label>
  <asp:TextBox runat="server" ID="txtUpdateFrequency" MaxLength="3" Width="50" />
  <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtUpdateFrequency" Operator="dataTypeCheck" Type="integer" ValidationGroup="settings" ErrorMessage="Not a valid number" />
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSaveSettings" ValidationGroup="settings" />
  </div>
  
 </div>
 
<div class="settings">
  
  <h1 style="margin: 0 0 5px 0"><%=Resources.labels.add %> blog</h1>

  <label for="<%=txtTitle.ClientID %>" class="wide"><%=Resources.labels.title %></label>
  <asp:TextBox runat="server" ID="txtTitle" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtTitle" ErrorMessage="required" ValidationGroup="addNew" /><br />
  
  <label for="<%=txtDescription.ClientID %>" class="wide"><%=Resources.labels.description %></label>
  <asp:TextBox runat="server" ID="txtDescription" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtDescription" ErrorMessage="required" ValidationGroup="addNew" /><br />
  
  <label for="<%=txtWebUrl.ClientID %>" class="wide"><%=Resources.labels.website %></label>
  <asp:TextBox runat="server" ID="txtWebUrl" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtWebUrl" ErrorMessage="required" Display="Dynamic" ValidationGroup="addNew" />
  <asp:CustomValidator runat="server" ControlToValidate="txtWebUrl" ErrorMessage="Invalid" EnableClientScript="false" OnServerValidate="validateWebUrl" ValidationGroup="addnew"></asp:CustomValidator><br />
  
  <label for="<%=txtFeedUrl.ClientID %>" class="wide">RSS url</label>
  <asp:TextBox runat="server" ID="txtFeedUrl" Width="600px" />
  <asp:RequiredFieldValidator runat="Server" ControlToValidate="txtFeedUrl" ErrorMessage="required" Display="Dynamic" ValidationGroup="addNew" />
  <asp:CustomValidator runat="server" ControlToValidate="txtFeedUrl" ErrorMessage="Invalid" EnableClientScript="false" OnServerValidate="validateFeedUrl" ValidationGroup="addnew"></asp:CustomValidator><br /><br />
  
  <label for="<%=cblXfn.ClientID %>" class="wide">XFN tag</label>
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
  
  <div style="text-align:right">
    <asp:Button runat="server" ID="btnSave" ValidationGroup="addNew" />
  </div>
  
</div>
    <asp:GridView runat="server" ID="grid" CssClass="category" 
    GridLines="None"
    AutoGenerateColumns="False" 
    AlternatingRowStyle-CssClass="alt" onrowdeleting="grid_RowDeleting"
    onrowcommand="grid_RowCommand">
        <Columns>
            <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="feedLink" runat="server" ImageUrl="~/pics/rssButton.gif" 
        NavigateUrl='<%# Eval("FeedUrl").ToString() %>' Text="<%# string.Empty %>"></asp:HyperLink>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ControlStyle-BackColor="Transparent">
               <ItemTemplate>
                   <asp:ImageButton ID="ibMoveUp" ImageUrl="~/pics/up_arrow_small.gif" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="moveUp" Width="16" Height="8" />
                   <asp:ImageButton ID="ibMoveDown" ImageUrl="~/pics/down_arrow_small.gif" runat="server" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="moveDown" Width="16" Height="8" />
               </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" 
                        NavigateUrl='<%# Eval("BlogUrl").ToString() %>' Text='<%# Eval("Title") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Description") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
    </asp:GridView>
</asp:Content>
