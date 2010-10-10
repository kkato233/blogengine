<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="admin.Settings.Main" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var frm = document.forms.aspnetForm;
            $(frm).validate({
                onsubmit: false
            });

            $("#btnSave").click(function (evt) {
                if ($(frm).valid())
                    SaveSettings();

                evt.preventDefault();
            });
        });
        function PreviewTheme() {
            var theme = document.getElementById('<%=ddlTheme.ClientID %>').value;
            var path = '../../?theme=' + theme;
            window.open(path);
        }
        function SaveSettings() {
            $('.loader').show();
            var dto = { 
				"name": $("[id$='_txtName']").val(),
				"desc": $("[id$='_txtDescription']").val(),
				"postsPerPage": $("[id$='_txtPostsPerPage']").val(),
				"theme": $("[id$='_ddlTheme']").val(),
				"mobileTheme": $("[id$='_ddlMobileTheme']").val(),
				"themeCookieName": $("[id$='_txtThemeCookieName']").val(),
				"useBlogNameInPageTitles": $("[id$='_cbUseBlogNameInPageTitles']").attr('checked'),
				"enableRelatedPosts": $("[id$='_cbShowRelatedPosts']").attr('checked'),
				"enableRating": $("[id$='_cbEnableRating']").attr('checked'),
				"showDescriptionInPostList": $("[id$='_cbShowDescriptionInPostList']").attr('checked'),
				"descriptionCharacters": $("[id$='_txtDescriptionCharacters']").val(),
				"showDescriptionInPostListForPostsByTagOrCategory": $("[id$='_cbShowDescriptionInPostListForPostsByTagOrCategory']").attr('checked'),
				"descriptionCharactersForPostsByTagOrCategory": $("[id$='_txtDescriptionCharactersForPostsByTagOrCategory']").val(),
				"timeStampPostLinks": $("[id$='_cbTimeStampPostLinks']").attr('checked'),
				"showPostNavigation": $("[id$='_cbShowPostNavigation']").attr('checked'),
				"culture": $("[id$='_ddlCulture']").val(),
				"timezone": $("[id$='_txtTimeZone']").val(),
				"enableSelfRegistration": $("[id$='_cbEnableSelfRegistration']").attr('checked'),
				"requireLoginToViewPosts": $("[id$='_cbRequireLoginToViewPosts']").attr('checked')
			};
			
            $.ajax({
                url: "Main.aspx/Save",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dto),
                success: function (result) {
                    var rt = result.d;
                    if (rt.Success)
                        ShowStatus("success", rt.Message);
                    else
                        ShowStatus("warning", rt.Message);
                }
            });
            $('.loader').hide();
            return false;
        }  
    </script>
     
    <div class="content-box-hdr">
        <span class="SectionHeader"><%=Resources.labels.settings %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">

            <fieldset class="rounded">
                <legend><%=Resources.labels.basic %> <%=Resources.labels.settings %></legend>

                <table class="tblForm">
                    <tr>
                        <td width="250"><label for="<%=txtName.ClientID %>"><%=Resources.labels.name %></label></td>
                        <td>
                            <asp:TextBox width="300" runat="server" ID="txtName" CssClass="required" />
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtDescription.ClientID %>"><%=Resources.labels.description %></label></td>
                        <td><asp:TextBox width="300" runat="server" ID="txtDescription" /></td>
                    </tr>
                    <tr>
                        <td><label class="lbl200" for="<%=txtPostsPerPage.ClientID %>"><%=Resources.labels.postPerPage %></label></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostsPerPage" Width="50" MaxLength="4" CssClass="required number" />
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=ddlTheme.ClientID %>"><%=Resources.labels.theme %></label></td>
                        <td>
                            <asp:DropDownList CssClass="txt" Width="212" runat="server" ID="ddlTheme" />
                            <a href="javascript:void(PreviewTheme());"><%=Resources.labels.preview %></a> | <a href="http://www.dotnetblogengine.net/page/themes.aspx" target="_blank"><%=Resources.labels.download %></a>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=ddlMobileTheme.ClientID %>"><%=Resources.labels.mobileTheme %></label></td>
                        <td><asp:DropDownList CssClass="txt" Width="212" runat="server" ID="ddlMobileTheme" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtThemeCookieName.ClientID %>"><%=Resources.labels.themeCookieName %></label></td>
                        <td><asp:TextBox CssClass="w300" runat="server" ID="txtThemeCookieName" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbUseBlogNameInPageTitles.ClientID %>"><%=Resources.labels.useBlogNameInPageTitles%></label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbUseBlogNameInPageTitles" />
                            <label><%=Resources.labels.useBlogNameInPageTitlesDescription%></label>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbShowRelatedPosts.ClientID %>"><%=Resources.labels.showRelatedPosts %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbShowRelatedPosts" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbEnableRating.ClientID %>"><%=Resources.labels.enableRating %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableRating" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbShowDescriptionInPostList.ClientID %>"><%=Resources.labels.showDescriptionInPostList %></label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbShowDescriptionInPostList" />
                            <label style="width: 200px" for="<%=txtDescriptionCharacters.ClientID %>"><%=Resources.labels.numberOfCharacters %></label>
                            <asp:TextBox runat="server" ID="txtDescriptionCharacters" Width="40" CssClass="number" />      
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbShowDescriptionInPostListForPostsByTagOrCategory.ClientID %>"><%=Resources.labels.showDescriptionInPostListForPostsByTagOrCategory %></label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbShowDescriptionInPostListForPostsByTagOrCategory" />
                            <label for="<%=txtDescriptionCharactersForPostsByTagOrCategory.ClientID %>" style="float: none; position: relative; top: -2px;"><%=Resources.labels.numberOfCharacters %></label>
                            <asp:TextBox runat="server" ID="txtDescriptionCharactersForPostsByTagOrCategory" Width="40" CssClass="number" />
                         </td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbTimeStampPostLinks.ClientID %>"><%=Resources.labels.timeStampPostLinks %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbTimeStampPostLinks" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbShowPostNavigation.ClientID %>"><%=Resources.labels.showPostNavigation %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbShowPostNavigation" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=ddlCulture.ClientID %>"><%=Resources.labels.language %></label></td>
                        <td>
                            <asp:DropDownList runat="Server" ID="ddlCulture" Style="text-transform: capitalize">
                                <asp:ListItem Text="Auto" />
                                <asp:ListItem Text="english" Value="en" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtTimeZone.ClientID %>"><%=Resources.labels.timezone %></label></td>
                        <td>
                            <asp:TextBox runat="Server" ID="txtTimeZone" Width="30" CssClass="number" />
                            <label>Server time: <%=DateTime.Now.ToShortTimeString() %></label>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbEnableSelfRegistration.ClientID %>"><%=Resources.labels.enableSelfRegistration %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableSelfRegistration" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbRequireLoginToViewPosts.ClientID %>"><%=Resources.labels.onlyLoggedInCanView %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbRequireLoginToViewPosts" /></td>
                    </tr>
                </table>
            </fieldset>

		</div>
        <div class="action_buttons">
            <input type="submit" id="btnSave" class="btn rounded" value="Save" />&nbsp;
            <span class="loader">&nbsp;</span>
        </div>
	</div>    
</asp:Content>