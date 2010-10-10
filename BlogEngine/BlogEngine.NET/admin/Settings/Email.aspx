<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Email.aspx.cs" Inherits="admin.Settings.Email" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script type="text/javascript">
        $(document).ready(function () {
            var	frm	= document.forms.aspnetForm;
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
				"email": $("[id$='_txtEmail']").val(),
				"smtpServer": $("[id$='_txtSmtpServer']").val(),
				"smtpServerPort": $("[id$='_txtSmtpServerPort']").val(),
				"smtpUserName": $("[id$='_txtSmtpUsername']").val(),
				"smtpPassword": $("[id$='_txtSmtpPassword']").val(),
				"sendMailOnComment": $("[id$='_cbComments']").attr('checked'),
				"enableSsl": $("[id$='_cbEnableSsl']").attr('checked'),
				"emailSubjectPrefix": $("[id$='_txtEmailSubjectPrefix']").val()
			};
			
            $.ajax({
                url: "Email.aspx/Save",
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
        function TestEmail() {
            $('.loader').show();
            var dto = {
                "email": $("[id$='_txtEmail']").val(),
                "smtpServer": $("[id$='_txtSmtpServer']").val(),
                "smtpServerPort": $("[id$='_txtSmtpServerPort']").val(),
                "smtpUserName": $("[id$='_txtSmtpUsername']").val(),
                "smtpPassword": $("[id$='_txtSmtpPassword']").val(),
                "sendMailOnComment": $("[id$='_cbComments']").attr('checked'),
                "enableSsl": $("[id$='_cbEnableSsl']").attr('checked'),
                "emailSubjectPrefix": $("[id$='_txtEmailSubjectPrefix']").val()
            };

            $.ajax({
                url: "Email.aspx/TestSmtp",
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
                <legend><%=Resources.labels.email %></legend>

                <table class="tblForm">
                    <tr>
                        <td width="250"><label for="<%=txtEmail.ClientID %>"><%=Resources.labels.emailAddress %></label></td>
                        <td><asp:TextBox CssClass="w300 email" runat="server" ID="txtEmail" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtSmtpServer.ClientID %>">SMTP server</label></td>
                        <td><asp:TextBox runat="server" ID="txtSmtpServer" CssClass="w300" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtSmtpServerPort.ClientID %>"><%=Resources.labels.portNumber %></label></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSmtpServerPort" Width="35" CssClass="number" />
                            <label><%=Resources.labels.portNumberDescription %></label>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtSmtpUsername.ClientID %>"><%=Resources.labels.userName %></label></td>
                        <td><asp:TextBox CssClass="txt" runat="server" ID="txtSmtpUsername" Width="300" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtSmtpPassword.ClientID %>"><%=Resources.labels.password %></label></td>
                        <td><asp:TextBox TextMode="Password"  runat="server" ID="txtSmtpPassword" Width="300" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbEnableSsl.ClientID %>"><%=Resources.labels.enableSsl%></label></td>
                        <td><asp:CheckBox runat="Server" ID="cbEnableSsl" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=cbComments.ClientID %>"><%=Resources.labels.sendCommentEmail %></label></td>
                        <td><asp:CheckBox runat="Server" ID="cbComments" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtEmailSubjectPrefix.ClientID %>"><%=Resources.labels.emailSubjectPrefix %></label></td>
                        <td><asp:TextBox runat="server" ID="txtEmailSubjectPrefix" Width="300" /></td>
                    </tr>
                    <tr>
                        <td><input type="submit" class="btn rounded" value="<%=Resources.labels.testEmailSettings %>" onclick="return TestEmail();" /></td>
                        <td><asp:Label runat="Server" ID="lbSmtpStatus" /></td>
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

