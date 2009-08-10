<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="admin_Comments_Settings" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    
        
    <div class="settings">
        
        <menu:TabMenu ID="TabMenu" runat="server" />
        
        <div id="ErrorMsg" runat="server" style="color: Red; display: block;"></div>
        <div id="InfoMsg" runat="server" style="color: Green; display: block;"></div>
        
        
             
        <label for="<%=cbEnableComments.ClientID %>"><%=Resources.labels.enableComments %></label>
        <asp:CheckBox runat="server" ID="cbEnableComments" /><%=Resources.labels.enableCommentsDescription %><br />

        <label for="<%=cbEnableCommentNesting.ClientID %>"><%=Resources.labels.enableCommentNesting %></label>
        <asp:CheckBox runat="server" ID="cbEnableCommentNesting" /><%=Resources.labels.enableCommentNestingDescription%><br />
        
        <label for="<%=cbEnableCountryInComments.ClientID %>"><%=Resources.labels.showCountryChooser %></label>
        <asp:CheckBox runat="server" ID="cbEnableCountryInComments" /><%=Resources.labels.showCountryChooserDescription %><br />
        
        <label for="<%=cbEnableCoComment.ClientID %>"><%=Resources.labels.enableCoComments %></label>
        <asp:CheckBox runat="server" ID="cbEnableCoComment" /><br />
        
        <label for="<%=cbShowLivePreview.ClientID %>"><%=Resources.labels.showLivePreview %></label>
        <asp:CheckBox runat="server" ID="cbShowLivePreview" /><br />
        
        <label for="<%=rblAvatar.ClientID %>"><%=Resources.labels.avatars %></label>
        <asp:RadioButtonList runat="Server" ID="rblAvatar" RepeatLayout="flow" RepeatDirection="horizontal">
          <asp:ListItem Text="MonsterID" Value="monster" />
          <asp:ListItem Text="Wavatar" Value="wavatar" />
          <asp:ListItem Text="Identicon" Value="identicon" />
          <asp:ListItem Text="<%$ Resources:labels, none %>" Value="none" />
        </asp:RadioButtonList><br />
        
        <label for="<%=cbEnableCommentsModeration.ClientID %>"><%=Resources.labels.enableCommentsModeration%></label>
        <asp:CheckBox runat="server" ID="cbEnableCommentsModeration" /><br />
        
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
        days.
        
        <div style="text-align: center">
            <asp:Button runat="server" ID="btnSave" />
        </div>
    </div>
   
</asp:Content>