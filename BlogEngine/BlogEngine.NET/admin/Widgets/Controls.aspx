<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" ValidateRequest="False"
    AutoEventWireup="true" CodeFile="Controls.aspx.cs" Inherits="Admin.Pages.Controls"
    Title="Control settings" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1>
                <%=Resources.labels.recentPosts %></h1>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtNumberOfPosts" Text='<%$ Code: Resources.labels.numberOfPosts %>' />
                <asp:TextBox runat="server" ID="txtNumberOfPosts" Width="30" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumberOfPosts" ErrorMessage="Required" />
                <asp:CompareValidator runat="Server" ControlToValidate="txtNumberOfPosts" Operator="dataTypeCheck"
                    Type="integer" ErrorMessage="<%$Resources:labels,enterValidNumber %>" /><br />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="cbDisplayComments" Text='<%$ Code: Resources.labels.displayCommentsOnRecentPosts %>' />
                <asp:CheckBox runat="Server" ID="cbDisplayComments" /><br />
                <asp:Label runat="server" AssociatedControlID="cbDisplayRating" Text='<%$ Code: Resources.labels.displayRatingsOnRecentPosts %>' />
                <asp:CheckBox runat="Server" ID="cbDisplayRating" />
            </div>

            <h1>
                <%=Resources.labels.recentComments %></h1>
            <asp:Label runat="server" AssociatedControlID="txtNumberOfPosts" Text='<%$ Code: Resources.labels.numberOfComments %>' />
            <asp:TextBox runat="server" ID="txtNumberOfComments" Width="30" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumberOfComments"
                ErrorMessage="<%$Resources:labels,required %>" />
            <asp:CompareValidator runat="Server" ControlToValidate="txtNumberOfComments" Operator="dataTypeCheck"
                Type="integer" ErrorMessage="<%$Resources:labels,enterValidNumber %>" /><br />

            <h1>
                <%=Resources.labels.searchField %></h1>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtSearchButtonText" Text='<%$ Code: Resources.labels.buttonText %>' />
                <asp:TextBox runat="server" ID="txtSearchButtonText" Width="320" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSearchButtonText"
                    ErrorMessage="<%$Resources:labels,required %>" />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtDefaultSearchText" Text='<%$ Code: Resources.labels.searchFieldText %>' />
                <asp:TextBox runat="server" ID="txtDefaultSearchText" Width="320" />
                <%=Resources.labels.defaultTextShownInSearchField %>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDefaultSearchText"
                    ErrorMessage="<%$Resources:labels,required %>" />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="cbEnableCommentSearch" Text='<%$ Code: Resources.labels.enableCommentSearch %>' />
                <asp:CheckBox runat="Server" ID="cbEnableCommentSearch" />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtCommentLabelText" Text='<%$ Code: Resources.labels.commentLabelText %>' />
                <asp:TextBox runat="server" ID="txtCommentLabelText" Width="320" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCommentLabelText"
                    ErrorMessage="<%$Resources:labels,required %>" />
            </div>

            <h1>
                <%=Resources.labels.contactForm %></h1>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtFormMessage" Text='<%$ Code: Resources.labels.formMessage %>' />
                <asp:TextBox runat="server" ID="txtFormMessage" TextMode="multiLine" Rows="5" Columns="40" /><br />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="txtThankMessage" Text='<%$ Code: Resources.labels.thankYouMessage %>' />
                <asp:TextBox runat="server" ID="txtThankMessage" TextMode="multiLine" Rows="5" Columns="40" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtThankMessage" ErrorMessage="<%$Resources:labels,required %>" /><br />
            </div>
            <div style="margin-bottom: 3px">
                <asp:Label runat="server" AssociatedControlID="cbEnableAttachments" Text='<%$ Code: Resources.labels.enableAttachments %>' />
                <asp:CheckBox runat="Server" ID="cbEnableAttachments" />
            </div>
            <div style="text-align: right">
                <asp:Button runat="server" ID="btnSave" />
            </div>
        </div>
    </div>
</asp:Content>
