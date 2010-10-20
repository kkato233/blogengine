<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="HeadTrack.aspx.cs" Inherits="admin.Settings.HeadTrack" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">  
    <script type="text/javascript">
        function SaveSettings() {
            $('.loader').show();
            var hdr = $("[id$='_txtHtmlHeader']").val();
            var ftr = $("[id$='_txtTrackingScript']").val();
            var dto = { "hdr": hdr, "ftr": ftr };

            $.ajax({
                url: "HeadTrack.aspx/Save",
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

	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.settings %></h1>
            <fieldset class="hide">
                <legend>Header and Tracking Script</legend>

                <table class="tblForm">
                    <tr>
                        <td width="250" valign="top"><label><%=Resources.labels.addCustomCodeToHeader %></label></td>
                        <td><asp:TextBox runat="server" ID="txtHtmlHeader" TextMode="multiLine" Rows="9" Columns="30" Width="500" /></td>
                    </tr>
                    <tr>
                        <td><label><%=Resources.labels.trackingScriptDescription %></label></td>
                        <td><asp:TextBox runat="server" ID="txtTrackingScript" TextMode="multiLine" Rows="9" Columns="30" Width="500" /></td>
                    </tr>
                </table>
            </fieldset>
             <div class="action_buttons">
                <input type="submit" class="primarybtn rounded" value="Save" onclick="return SaveSettings();" />
            </div>
        </div>
    </div>
</asp:Content>