<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="admin.Users.admin_Account_Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        function AddRole() {
            var txtUser = $('#txtUserName').val();
            var rowCnt = $('#RoleService tr').length;
            var bg = (rowCnt % 2 == 0) ? 'bgcolor="#F8F8F8"' : 'bgcolor="#F0F0F0"';
            var row = '<tr id="' + txtUser + '" ' + bg + '><td><input type="checkbox" name="chk"' + txtUser + ' class="chk"/></td>';
            row += '<td class="editable">' + txtUser + '</td>';
            row += '<td align="center"><a href="#" class="editButton">edit</a></td>';
            row += '<td align="center"><a href="#" class="deleteButton">delete</a></td></tr>';

            if (txtUser.length == 0) {
                $('#txtUserNameReq').removeClass('hidden');
                $('#txtUserName').focus().select();
                return false;
            }
            else {
                $('#txtUserNameReq').addClass('hidden');
                var dto = { "roleName": txtUser };

                $.ajax({
                    url: "../../api/RoleService.asmx/Add",
                    data: JSON.stringify(dto),
                    success: function (result) {
                        var rt = result.d;
                        if (rt.Success) {
                            $('#RoleService').append(row);
                            ShowStatus("success", rt.Message);
                        }
                        else {
                            ShowStatus("warning", rt.Message);
                        }
                    }
                });
            }
            return false;
        }
    </script>
    <table width="100%" border="0">
        <tr>
            <td style="width:auto; vertical-align: top;">
                <div class="beTableHdr">
		            <span class="SectionHeader"><%=Resources.labels.roles %></span>
		            <a href="#" onclick="Show('frmAddNew');" class="addNew">add new</a>
	            </div>
                <div id="frmAddNew" class="rounded" style="display:none">
		            <span class="lbl200">Name</span>
		            <input type="text" id="txtUserName" class="txt200" />
		            <span id="txtUserNameReq" class="req hidden">*</span>
		            <br/>
                    <span class="lbl200">&nbsp;</span>
		            <input type="submit" class="btnAddNew btn rounded" value="save" onclick="return AddRole();" />
		            or <a href="#" onclick="Hide('frmAddNew');">cancel</a>
	            </div>
                <div id="Container"></div>
            </td>
            <td style="width:180px; vertical-align: top; padding-left: 10px;">
                <ul id="RightList">
                    <li><a href="Users.aspx"><%=Resources.labels.users %></a></li>
                    <li><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>