<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin.Users.Users" Title="Create new user" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        function AddUser(obj) {
            var txtUser = $('#txtUserName').val();
            var txtPwd = $('#txtPassword').val();
            var txtPwd2 = $('#txtPassword2').val();
            var txtEmail = $('#txtEmail').val();

            var rowCnt = $('#UserService tr').length;
            var bg = (rowCnt % 2 == 0) ? 'bgcolor="#F8F8F8"' : 'bgcolor="#F0F0F0"';
            var row = '<tr id="' + txtUser + '" ' + bg + '><td><input type="checkbox" name="chk"' + txtUser + ' class="chk"/></td>';
            row += '<td>' + txtUser + '</td><td class="editable">' + txtEmail + '</td>';
            row += '<td align="center"><a href="Profile.aspx?id=' + txtUser + '">Profile</a></td>';
            row += '<td align="center"><a href="#" class="editButton">Edit</a></td>';
            row += '<td align="center"><a href="#" class="deleteButton">Delete</a></td></tr>';

            $('#txtUserNameReq').addClass('hidden');
            $('#txtPasswordReq').addClass('hidden');
            $('#txtPasswordMatch').addClass('hidden');
            $('#txtEmailReq').addClass('hidden');

            if (txtUser.length == 0) {
                $('#txtUserNameReq').removeClass('hidden');
                $('#txtUserName').focus().select();
                return false;
            }
            else if (txtPwd.length == 0 || txtPwd2.length == 0) {
                $('#txtPasswordReq').removeClass('hidden');
                $('#txtPassword').focus().select();
                return false;
            }
            else if (txtPwd != txtPwd2) {
                $('#txtPasswordMatch').removeClass('hidden');
                $('#txtPassword').focus().select();
                return false;
            }
            else if (txtEmail.length == 0) {
                $('#txtEmailReq').removeClass('hidden');
                $('#txtEmail').focus().select();
                return false;
            }
            else {
                var dto = { "user": txtUser, "pwd": txtPwd, "email": txtEmail };
                $.ajax({
                    url: "../../api/UserService.asmx/Add",
                    data: JSON.stringify(dto),
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var rt = result.d;
                        if (rt.Success) {
                            $('#UserService').append(row);
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

	<div class="content-box-outer">
		<div class="content-box-right">
            <ul>
			    <li class="content-box-selected"><a href="Users.aspx"><%=Resources.labels.users %></a></li>
			    <li><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></li>
			    <li><a href="Rights.aspx">Rights</a></li>
            </ul>
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.users %><a href="#" onclick="Show('frmAddNew');" class="addNew">Add new user</a></h1>
            <div id="frmAddNew" class="rounded" style="display:none;">
                <table class="tblForm">
                    <tr>
                        <td>
				            <label for="txtUserName" class="lbl">Name</label>
				            <input type="text" id="txtUserName" class="txt200"/>
				            <span id="txtUserNameReq" class="req hidden">*</span>
				        </td>
				        <td>
				            <label for="txtEmail" class="lbl">Email</label>
				            <input type="text" id="txtEmail" class="txt200"/>
				            <span id= "txtEmailReq" class="req hidden">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
				                <label for="txtPassword" class="lbl">Password</label>
				                <input type="password" id="txtPassword" class="txt200"/>
                        </td>
                        <td>
                                <label for="txtPassword2" class="lbl">Confirm password</label>
				                <input type="password" id="txtPassword2" class="txt200"/>
				                <span  id= "txtPasswordReq" class="req hidden">*</span>
				                <span  id= "txtPasswordMatch" class="req hidden">Passwords should match</span>
                        </td>
                    </tr>
                </table>
				<input type="submit" class="btnAddNew btn rounded" value="save" onclick="return AddUser(this);" id="btnNewUser" />
				or <a href="#" onclick="Hide('frmAddNew');">cancel</a>
			</div>
            <div id="Container"></div>
		</div>
	</div>
</asp:Content>
