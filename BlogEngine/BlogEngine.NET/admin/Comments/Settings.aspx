<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Admin.Comments.Settings" %>
<%@ Import Namespace="Resources"%>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

    <script type="text/javascript">
        function ConfirmReset() {
            return confirm('<%=labels.confirmResetCounters%>');  
        } 
    </script>
  
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=labels.configuration %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu1" runat="server" />
		</div>
		<div class="content-box-left">

            <fieldset class="rounded">
                <legend>General</legend>
                <div class="subsection">
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableComments.ClientID %>"><%=labels.enableComments %></label>
                        <asp:CheckBox runat="server" ID="cbEnableComments" onclick="ToggleEnableComments();" />
                        <label class="lbl"><%=labels.enableCommentsDescription %></label>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableCommentsModeration.ClientID %>"><%=labels.enableCommentsModeration%></label>
                        <asp:CheckBox runat="server" ID="cbEnableCommentsModeration" onclick="ToggleModeration();" /> 
                        <label class="lbl"><%=labels.pendingApproval%></label>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbRequireLogin.ClientID %>">Require Login To Post Comment</label>
                        <asp:CheckBox runat="server" ID="cbRequireLogin" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableCommentNesting.ClientID %>"><%=labels.enableCommentNesting %></label>
                        <asp:CheckBox runat="server" ID="cbEnableCommentNesting" />
                        <label class="lbl"><%=labels.enableCommentNestingDescription%></label>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableCoComment.ClientID %>"><%=labels.enableCoComments %></label>
                        <asp:CheckBox runat="server" ID="cbEnableCoComment" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=ddlCloseComments.ClientID %>"> <%=labels.closeCommetsAfter %></label>
                        <asp:DropDownList runat="server" ID="ddlCloseComments">
                            <asp:ListItem Text="<%$ Resources:labels, never %>" Value="0" />
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
                        <%=labels.days%>
                    </div>
                </div>
            </fieldset>

            <fieldset class="rounded">
                <legend>Display</legend>
                <div class="subsection">
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableCountryInComments.ClientID %>"><%=labels.showCountryChooser %></label>
                        <asp:CheckBox runat="server" ID="cbEnableCountryInComments" />
                        <label class="lbl"><%=labels.showCountryChooserDescription %></label>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbShowLivePreview.ClientID %>"><%=labels.showLivePreview %></label>
                        <asp:CheckBox runat="server" ID="cbShowLivePreview" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbShowPingBacks.ClientID %>"><%=labels.showPingBacks %></label>
                        <asp:CheckBox runat="server" ID="cbShowPingBacks" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=rblAvatar.ClientID %>"><%=labels.avatars %></label>
                        <asp:RadioButtonList runat="Server" ID="rblAvatar" RepeatLayout="flow" RepeatDirection="horizontal">
                          <asp:ListItem Text="MonsterID" Value="monster" />
                          <asp:ListItem Text="Wavatar" Value="wavatar" />
                          <asp:ListItem Text="Identicon" Value="identicon" />
                          <asp:ListItem Text="<%$ Resources:labels, none %>" Value="none" />
                        </asp:RadioButtonList>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=ddlCommentsPerPage.ClientID %>" style="position: relative; top: 4px">
                            <%=labels.commentsPerPage%>
                        </label>
                        <asp:DropDownList runat="server" ID="ddlCommentsPerPage">
                            <asp:ListItem Text="5" />
                            <asp:ListItem Text="10" />
                            <asp:ListItem Text="15" />
                            <asp:ListItem Text="20" />
                            <asp:ListItem Text="50" />
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>

            <fieldset class="rounded">
                <legend><%=labels.disqusSettings %></legend>       
                <div class="subsection" style="float:left;width:500px">
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbEnableDisqus.ClientID %>">Switch comments to Disqus</label>
                        <asp:CheckBox runat="server" ID="cbEnableDisqus" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=txtDisqusName.ClientID %>"><%=labels.disqusShortName %></label>
                        <asp:TextBox runat="server" ID="txtDisqusName" class="txt200" MaxLength="250" />
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbDisqusDevMode.ClientID %>"><%=labels.developmentMode %></label>
                        <asp:CheckBox runat="server" ID="cbDisqusDevMode" />
                        <label class="lbl"><%=labels.developmentModeCheckboxMessage %></label>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbDisqusAddToPages.ClientID %>"><%=labels.addCommentsToPages %></label>
                        <asp:CheckBox runat="server" ID="cbDisqusAddToPages" />
                        <label class="lbl"><%=labels.addToPages %></label>
                    </div>
                </div>
                <div class="info rounded" style="float:right; position:relative; overflow:visible; width: 350px; top: 0;">
                    <%=labels.disqusSignupMessage %>
                </div>  
            </fieldset>
    
            <div class="action_buttons">
                <asp:Button runat="server" class="btn rounded" ID="btnSave" />&nbsp;
                <span class="loader">&nbsp;</span>
            </div>
            
		</div>
	</div> 

</asp:Content>