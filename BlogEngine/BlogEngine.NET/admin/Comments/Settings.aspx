<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="admin_Comments_Settings" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

    <div class="settings">   
        <menu:TabMenu ID="TabMenu" runat="server" />
        
        <div id="ErrorMsg" runat="server" style="color: Red; display: block;"></div>
        <div id="InfoMsg" runat="server" style="color: Green; display: block;"></div>
            
        <label for="<%=cbEnableComments.ClientID %>"><%=Resources.labels.enableComments %></label>
        <asp:CheckBox runat="server" ID="cbEnableComments" /><%=Resources.labels.enableCommentsDescription %><br />

        <label for="<%=cbEnableCommentsModeration.ClientID %>"><%=Resources.labels.enableCommentsModeration%></label>
        <asp:CheckBox runat="server" ID="cbEnableCommentsModeration" /><br />
        
        <label for="<%=cbEnableCommentNesting.ClientID %>"><%=Resources.labels.enableCommentNesting %></label>
        <asp:CheckBox runat="server" ID="cbEnableCommentNesting" /><%=Resources.labels.enableCommentNestingDescription%><br />
        
        <label for="<%=cbEnableCoComment.ClientID %>"><%=Resources.labels.enableCoComments %></label>
        <asp:CheckBox runat="server" ID="cbEnableCoComment" /><br />
        
        <label for="<%=cbEnableCountryInComments.ClientID %>"><%=Resources.labels.showCountryChooser %></label>
        <asp:CheckBox runat="server" ID="cbEnableCountryInComments" /><%=Resources.labels.showCountryChooserDescription %><br />
        
        <label for="<%=cbShowLivePreview.ClientID %>"><%=Resources.labels.showLivePreview %></label>
        <asp:CheckBox runat="server" ID="cbShowLivePreview" /><br />
        
        <label for="<%=rblAvatar.ClientID %>"><%=Resources.labels.avatars %></label>
        <asp:RadioButtonList runat="Server" ID="rblAvatar" RepeatLayout="flow" RepeatDirection="horizontal">
          <asp:ListItem Text="MonsterID" Value="monster" />
          <asp:ListItem Text="Wavatar" Value="wavatar" />
          <asp:ListItem Text="Identicon" Value="identicon" />
          <asp:ListItem Text="<%$ Resources:labels, none %>" Value="none" />
        </asp:RadioButtonList><br />

        <label for="<%=ddlCloseComments.ClientID %>" style="position: relative; top: 4px">
            <%=Resources.labels.closeCommetsAfter %>
        </label>
        <asp:DropDownList runat="server" ID="ddlCloseComments">
            <asp:ListItem Text="Never" Value="0" />
            <asp:ListItem Text="1" />
            <asp:ListItem Text="2" />
            <asp:ListItem Text="3" />
            <asp:ListItem Text="7" />
            <asp:ListItem Text="10" />
            <asp:ListItem Text="14" />
            <asp:ListItem Text="21" />
            <asp:ListItem Text="30" />
            <asp:ListItem Text="60" />
            <asp:ListItem Text="90" />
            <asp:ListItem Text="180" />
            <asp:ListItem Text="365" />
        </asp:DropDownList>
        days.<br />
        
        <label for="<%=ddlCommentsPerPage.ClientID %>" style="position: relative; top: 4px">
            Comments per page
        </label>
        <asp:DropDownList runat="server" ID="ddlCommentsPerPage">
            <asp:ListItem Text="5" />
            <asp:ListItem Text="10" />
            <asp:ListItem Text="15" />
            <asp:ListItem Text="20" />
            <asp:ListItem Text="50" />
        </asp:DropDownList>
    </div>
    
    <div class="settings">
        <h1>Rules</h1>
        
        <label for="<%=cbTrustAuthenticated.ClientID %>">Trust authenticated authors</label>
        <asp:CheckBox runat="server" ID="cbTrustAuthenticated" />If user authenticated, always trust<br />
 
        <label for="<%=ddWhiteListCount.ClientID %>">Add to white list if at least</label>
            <asp:DropDownList runat="server" ID="ddWhiteListCount">
                <asp:ListItem Text="0" />
                <asp:ListItem Text="1" />
                <asp:ListItem Text="2" />
                <asp:ListItem Text="3" />
                <asp:ListItem Text="5" />
            </asp:DropDownList> comments of this author have been approved   
        <br />

        <label for="<%=ddBlackListCount.ClientID %>">Add to black list if at least</label>
        <asp:DropDownList runat="server" ID="ddBlackListCount">
                <asp:ListItem Text="0" />
                <asp:ListItem Text="1" />
                <asp:ListItem Text="2" />
                <asp:ListItem Text="3" />
                <asp:ListItem Text="5" />
            </asp:DropDownList> comments of this author have been rejected<br />

       <label for="<%=cbBlockOnDelete.ClientID %>">Block on delete</label>
        <asp:CheckBox runat="server" ID="cbBlockOnDelete" />If comment was deleted, always block author<br />

    </div>
    
    <div class="settings">
        <h1>Filters</h1>
        
        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="ddAction" runat="server">
                        <asp:ListItem Text="Block" Value="Block" Selected=true></asp:ListItem>
                        <asp:ListItem Text="Allow" Value="Allow" Selected=false></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddSubject" runat="server">
                        <asp:ListItem Text="IP" Value="IP" Selected=true></asp:ListItem>
                        <asp:ListItem Text="Author" Value="Author" Selected=false></asp:ListItem>
                        <asp:ListItem Text="Website" Value="Website" Selected=false></asp:ListItem>
                        <asp:ListItem Text="Email" Value="Email" Selected=false></asp:ListItem>
                        <asp:ListItem Text="Comment" Value="Comment" Selected=false></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddOperator" runat="server">
                        <asp:ListItem Text="Equals" Value="Equals" Selected=true></asp:ListItem>
                        <asp:ListItem Text="Contains" Value="Contains" Selected=false></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td><asp:TextBox ID="txtFilter" runat="server" MaxLength="250" Width="300px"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnAddFilter" runat="server" Text="Add Filter" OnClick="btnAddFilter_Click"/>
                </td>
                <td><span runat="Server" ID="FilterValidation" style="color:Red"></span></td>
            </tr>
        </table>
        
        <asp:GridView ID="gridFilters" 
                PageSize="20" 
                BorderColor="Silver" 
                BorderStyle="solid" 
                BorderWidth="1px"
                cellpadding="2"
                runat="server"  
                width="100%"
                AutoGenerateColumns="False"
                AllowPaging="True"
                OnPageIndexChanging="gridView_PageIndexChanging"
                AllowSorting="True">
              <Columns>
                <asp:BoundField DataField = "ID" Visible="false" />
                <asp:TemplateField HeaderText="Filter">
                    <ItemTemplate>
                        <%# Eval("Action") %> comments where <%# Eval("Subject") %> <%# Eval("Operator") %> <%# Eval("Filter") %> 
                    </ItemTemplate>
                </asp:TemplateField>
                        
                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" ImageAlign="middle" CausesValidation="false" ImageUrl="~/admin/images/del.png" OnClick="btnDelete_Click" CommandName="btnDelete" AlternateText="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
              </Columns>
              <pagersettings mode="Numeric" position="Bottom" pagebuttoncount="10"/>
          </asp:GridView>
    </div>
    
    <div class="settings">
        <h1>Custom Filters</h1>       
        <asp:GridView ID="gridCustomFilters" 
                BorderColor="Silver" 
                BorderStyle="solid" 
                BorderWidth="1px"
                cellpadding="2"
                runat="server"  
                width="100%" 
                AutoGenerateColumns="False">
              <Columns>
                <asp:BoundField DataField = "FullName" Visible="false" />
                <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkEnabled" Checked="true" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>  
                <asp:BoundField DataField = "Name" HeaderText="Filter Name" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField = "SiteUrl" HeaderText="Site URL" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField = "ApiKey" HeaderText="API Key" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField = "Checked" HeaderText="Checked" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField = "Cought" HeaderText="Cought" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField = "Reported" HeaderText="Reported" HeaderStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="Accuracy" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        100% 
                    </ItemTemplate>
                </asp:TemplateField>
              </Columns>
        </asp:GridView>
    </div>

    <div style="text-align: center; margin-bottom: 10px">
        <asp:Button runat="server" ID="btnSave" />
    </div>
   
</asp:Content>