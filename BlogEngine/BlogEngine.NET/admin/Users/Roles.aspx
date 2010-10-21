﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Admin.Users.Roles" %>
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
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
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

	<div class="content-box-outer">
		<div class="content-box-right">
            <ul>
			    <li><a href="Users.aspx"><%=Resources.labels.users %></a></li>
			    <li class="content-box-selected"><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></li>
			    <li><a href="Rights.aspx">Rights</a></li>
            </ul>
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.roles %><a href="#" onclick="Show('frmAddNew');" class="addNew">Add new role</a></h1>
           <div id="frmAddNew" class="rounded" style="display:none">
		        <label for="txtUserName" class="lbl">Name</label>
		        <input type="text" id="txtUserName" class="txt200" />
		        <span id="txtUserNameReq" class="req hidden">*</span>
		        <br/><br/>
		        <input type="submit" class="btnAddNew btn rounded" value="save" onclick="return AddRole();" />
		        or <a href="#" onclick="Hide('frmAddNew');">cancel</a>
	        </div>
            <div id="Container"></div>
		</div>
	</div>
</asp:Content>
