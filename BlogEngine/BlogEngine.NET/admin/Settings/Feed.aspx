<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Feed.aspx.cs" Inherits="admin.Settings.Feed" %>
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
        function geodeAsk() {
            if (navigator.geolocation)
                navigator.geolocation.getCurrentPosition(geoFound, geoNotFound);
        }
        function geoFound(pos) {
            document.getElementById('<%=txtGeocodingLatitude.ClientID %>').value = pos.latitude;
            document.getElementById('<%=txtGeocodingLongitude.ClientID %>').value = pos.longitude;
        }

        function geoNotFound() {
            alert('You must be on a wifi network for us to determine your location');
        } 
		function SaveSettings() {
            $('.loader').show();
            var dto = { 
				"syndicationFormat": $("[id$='_ddlSyndicationFormat']").val(),
				"postsPerFeed": $("[id$='_txtPostsPerFeed']").val(),
				"dublinCoreCreator": $("[id$='_txtDublinCoreCreator']").val(),
				"dublinCoreLanguage": $("[id$='_txtDublinCoreLanguage']").val(),
				"geocodingLatitude": $("[id$='_txtGeocodingLatitude']").val(),
				"geocodingLongitude": $("[id$='_txtGeocodingLongitude']").val(),
				"blogChannelBLink": $("[id$='_txtBlogChannelBLink']").val(),
				"alternateFeedUrl": $("[id$='_txtAlternateFeedUrl']").val(),
				"enableEnclosures": $("[id$='_cbEnableEnclosures']").attr('checked')
			};
			
            $.ajax({
                url: "Feed.aspx/Save",
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
                <legend>Feed Settings</legend>

                <table class="tblForm">
                    <tr>
                        <td width="250"><label for="<%=ddlSyndicationFormat.ClientID %>" style="position: relative; top: 4px"><%=Resources.labels.defaultFeedOutput %></label></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSyndicationFormat">
                                <asp:ListItem Text="RSS 2.0" Value="Rss" Selected="True" />
                                <asp:ListItem Text="Atom 1.0" Value="Atom" />
                            </asp:DropDownList>
                            format.
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtPostsPerFeed.ClientID %>"><%=Resources.labels.postsPerFeed %></label></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostsPerFeed" Width="50" MaxLength="4" CssClass="required number" />
                         </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtDublinCoreCreator.ClientID %>"><%=Resources.labels.author %></label></td>
                        <td><asp:TextBox runat="server" ID="txtDublinCoreCreator" Width="300" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtDublinCoreLanguage.ClientID %>"><%=Resources.labels.languageCode %></label></td>
                        <td><asp:TextBox runat="server" ID="txtDublinCoreLanguage" Width="60" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtGeocodingLatitude.ClientID %>"><%=Resources.labels.latitude %></label></td>
                        <td><asp:TextBox runat="server" ID="txtGeocodingLatitude" Width="300" CssClass="number" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtGeocodingLongitude.ClientID %>"><%=Resources.labels.longtitude %></label></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtGeocodingLongitude" Width="300" CssClass="number" />&nbsp;
                            <input type="button" class="btn rounded" id="findPosition" onclick="geodeAsk()" value="<%=Resources.labels.findPosition %>" style="display: none" />
                            <script type="text/javascript">
                                if (navigator.geolocation) document.getElementById('findPosition').style.display = 'inline';
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtBlogChannelBLink.ClientID %>"><%=Resources.labels.endorsment %></label></td>
                        <td><asp:TextBox runat="server" ID="txtBlogChannelBLink" MaxLength="255" Width="300" /></td>
                    </tr>
                    <tr>
                        <td><label for="<%=txtAlternateFeedUrl.ClientID %>"><%=Resources.labels.alternateFeedUrl %></label></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAlternateFeedUrl" Width="300" CssClass="url" />
                            <em>(http://feeds.feedburner.com/username)</em>
                        </td>
                    </tr>
                     <tr>
                        <td><label for="<%=cbEnableEnclosures.ClientID %>"><%=Resources.labels.enableEnclosures %></label></td>
                        <td><asp:CheckBox runat="server" ID="cbEnableEnclosures" /></td>
                    </tr>
                </table>
            </fieldset>
            <div class="action_buttons">
                <input type="submit" id="btnSave" class="primarybtn rounded" value="Save" />
            </div>
       </div>
    </div>
</asp:Content>