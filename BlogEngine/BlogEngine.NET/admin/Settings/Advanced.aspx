<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Advanced.aspx.cs" Inherits="admin.Settings.Advanced" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
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
        function SaveSettings() {
            $('.loader').show();
			
            var dto = { 
				"enableCompression": $("[id$='_cbEnableCompression']").attr('checked'),
				"removeWhitespaceInStyleSheets": $("[id$='_cbRemoveWhitespaceInStyleSheets']").attr('checked'),
				"compressWebResource": $("[id$='_cbCompressWebResource']").attr('checked'),
				"enableOpenSearch": $("[id$='_cbEnableOpenSearch']").attr('checked'),
				"requireSslForMetaWeblogApi": $("[id$='_cbRequireSslForMetaWeblogApi']").attr('checked'),
				"wwwSubdomain": $('.rblSubdomain input:radio:checked').val(),
				"enableTrackBackSend": $("[id$='_cbEnableTrackBackSend']").attr('checked'),
				"enableTrackBackReceive": $("[id$='_cbEnableTrackBackReceive']").attr('checked'),
				"enablePingBackSend": $("[id$='_cbEnablePingBackSend']").attr('checked'),
				"enablePingBackReceive": $("[id$='_cbEnablePingBackReceive']").attr('checked'),
				"enableErrorLogging": $("[id$='_cbEnableErrorLogging']").attr('checked'),
				"allowRemoteFileDownloads": $("[id$='_cbAllowRemoteFileDownloads']").attr('checked'),
				"remoteTimeout": $("[id$='_txtRemoteTimeout']").attr('value'),
				"remoteMaxFileSize": $("[id$='_txtRemoteMaxFileSize']").attr('value')                
			};
			
            $.ajax({
                url: "Advanced.aspx/Save",
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
                <legend><%=Resources.labels.advancedSettings %></legend>

                <table class="tblForm">
                    <tr>
                        <td style="width:250px;"><label for="<%=cbEnableCompression.ClientID %>"><%=Resources.labels.enableHttpCompression %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableCompression" /><label><%=Resources.labels.enableHttpCompressionDescription %></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbRemoveWhitespaceInStyleSheets.ClientID %>"><%=Resources.labels.trimStylesheet %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbRemoveWhitespaceInStyleSheets" /><label><%=Resources.labels.trimStylesheetDescription %></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbCompressWebResource.ClientID %>"><%=Resources.labels.compressWebResource %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbCompressWebResource" /><label><%=Resources.labels.compressWebResourceDescription%></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbEnableOpenSearch.ClientID %>"><%=Resources.labels.enableOpenSearch %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableOpenSearch" /><label><%=Resources.labels.enableOpenSearchDescription %></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbRequireSslForMetaWeblogApi.ClientID %>"><%=Resources.labels.requireSslForMetaWeblogApi %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbRequireSslForMetaWeblogApi" /><label><%=Resources.labels.requireSslForMetaWeblogApiDescription%></label></td>
                    </tr>
                    <tr>
                        <td><label for=""><%=Resources.labels.enableTrackbacks %></label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbEnableTrackBackSend" /><label><%=Resources.labels.send %></label>
                            &nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="cbEnableTrackBackReceive" /><label><%=Resources.labels.receive %></label>
                        </td>
                    </tr>
                    <tr>
                        <td><label for=""><%=Resources.labels.enablePingbacks %></label></td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbEnablePingBackSend" /><label><%=Resources.labels.send %></label>
                            &nbsp;&nbsp;
                            <asp:CheckBox runat="server" ID="cbEnablePingBackReceive" /><label><%=Resources.labels.receive %></label>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=rblWwwSubdomain.ClientID %>"><%=Resources.labels.handleWwwSubdomain %></label></td>
                        <td>
                            <asp:RadioButtonList runat="server" CssClass="rblSubdomain" ID="rblWwwSubdomain" RepeatLayout="flow" RepeatDirection="horizontal">
                                <asp:ListItem Text="<%$ Resources:labels, remove %>" Value="remove" />
                                <asp:ListItem Text="<%$ Resources:labels, enforce %>" Value="add" />
                                <asp:ListItem Text="<%$ Resources:labels, ignore %>" Value="" Selected="true" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbEnableErrorLogging.ClientID %>"><%=Resources.labels.enableErrorLogging %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableErrorLogging" /><label><%=Resources.labels.enableErrorLoggingDescription%></label></td>
                    </tr>
                 </table>
            </fieldset>

            <fieldset class="rounded">
                <legend><%=Resources.labels.securitySettings %></legend>

                <table class="tblForm">
                    <tr>
                        <td style="width:250px;"><label for="<%=cbAllowRemoteFileDownloads.ClientID %>"><%=Resources.labels.allowRemoteFileDownloads %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbAllowRemoteFileDownloads" /><label><%=Resources.labels.allowRemoteFileDownloadsDescription%></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtRemoteTimeout.ClientID %>"><%=Resources.labels.remoteTimeout %></label></td>
                        <td><asp:TextBox runat="server" ID="txtRemoteTimeout" /><label><%=Resources.labels.remoteTimeoutDescription %></label></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtRemoteMaxFileSize.ClientID %>"><%=Resources.labels.maximumRemoteFileSize%></label></td>
                        <td><asp:TextBox runat="server" ID="txtRemoteMaxFileSize" /><label><%=Resources.labels.maximumRemoteFileSizeDescription%></label></td>
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

