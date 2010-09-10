﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="admin_Pages_Profile" %>
<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <script type="text/javascript">
        function SaveProfile() {
            var vals = new Array();
            var roles = new Array();
            var cnt = 0;

            $.each($('.chkRole:checked'), function (i, v) {
                roles[cnt] = v.id;
                cnt++;
            });

            var displayName = $('#txtDispalayName').val();
            var firstName = $('#txtFirstName').val();
            var middleName = $('#txtMiddleName').val();
            var lastName = $('#txtLastName').val();
            var email = $('#txtEmail').val();
            var birthday = $('#txtBirthday').val();
            var photoURL = $('#txtPhotoURL').val();
            var isPrivate = false;
            if ($('#chkPrivate').attr('checked')) {
                isPrivate = true;
            }
            var mobile = $('#txtMobile').val();
            var phone = $('#txtMainPhone').val();
            var fax = $('#txtFax').val();

            var city = $('#txtCity').val();
            var state = $('#txtState').val();
            var country = $('#txtCountry').val();
            var biography = $('#biography').val();

            if (displayName.length == 0) {
                $('#txtDispalayNameReq').removeClass('hidden');
                $('#txtDispalayName').focus().select();
                return false;
            }

            $('#txtDispalayNameReq').addClass('hidden');

            vals[0] = displayName;
            vals[1] = firstName;
            vals[2] = middleName;
            vals[3] = lastName;
            vals[4] = email;
            vals[5] = birthday;
            vals[6] = photoURL;
            vals[7] = isPrivate;
            vals[8] = mobile;
            vals[9] = phone;
            vals[10] = fax;

            vals[11] = city;
            vals[12] = state;
            vals[13] = country;
            vals[14] = biography;

            var dto = { "id": Querystring('id'), "vals" : vals, "roles" : roles };

            $.ajax({
                url: "../../api/Profile.asmx/Save",
                data: JSON.stringify(dto),
                success: function (result) {
                    var rt = result.d;
                    if (rt.Success) {
                        ShowStatus("success", rt.Message);
                    }
                    else {
                        ShowStatus("warning", rt.Message);
                    }
                }
            });
            return false;
        }
    </script>
    <table width="100%" border="0">
        <tr>
            <td style="width:auto; vertical-align: top;">
                <div class="beTableHdr">
		            <span class="SectionHeader"><%=Resources.labels.profile %> : <%=Request.QueryString["id"] %></span>
	            </div>
                <div id="Container"></div>
                <fieldset class="rounded">
                    <legend>Roles</legend>
                    <div><%=RolesList%></div>
                </fieldset>
                <div id="Container2"></div>
                <div>
                    <input type="submit" class="btn rounded" value="<%=Resources.labels.save %>" onclick="return SaveProfile()" />
		            or <a href="Users.aspx"><%=Resources.labels.cancel %></a>
                </div>
            </td>
            <td style="width:180px; vertical-align: top; padding-left: 10px;">
                <ul id="RightList">
                    <li><a href="Users.aspx" class="selected"><%=Resources.labels.users %></a></li>
                    <li><a href="Roles.aspx"><%=Resources.labels.roles %></a></li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>