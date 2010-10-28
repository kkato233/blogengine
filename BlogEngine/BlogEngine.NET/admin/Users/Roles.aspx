<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Admin.Users.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        function AddRole() {
            var txtUser = $('#txtUserName').val();
            var rowCnt = $('#RoleService tr').length;
            var bg = (rowCnt % 2 == 0) ? 'class=""' : 'class="alt"';
            var row = '<tr id="' + txtUser + '" ' + bg + '><td><input type="checkbox" name="chk"' + txtUser + ' class="chk"/></td>';
            row += '<td class="editable">' + txtUser + '</td>';
            row += '<td align="center"><a href="#" class="editButton">Edit</a></td>';
            row += '<td align="center"><a href="Rights?role=' + txtUser + '">Rights</a></td>';
            row += '<td align="center"><a href="#" class="deleteButton">Delete</a></td></tr>';

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
            closeOverlay();
            return false;
        }
    </script>
    <script type="text/javascript" src="../jquery.colorbox.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".addNew").colorbox({ width: "300px", inline: true, href: "#frmAddNew" });
        });

        function closeOverlay() {
            $.colorbox.close();
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
            <h1><%=Resources.labels.roles %><a href="#" class="addNew">Add new role</a></h1>
            <div  style="display:none;">
            <div id="frmAddNew" class="overlaypanel">
                <h2>Add new role</h2>
		        <label for="txtUserName" class="lbl">Name</label>
		        <input type="text" id="txtUserName" class="txt200" />
		        <span id="txtUserNameReq" class="req hidden">*</span>
		        <br/><br/>
		        <input type="submit" class="btn primary rounded" value="save" onclick="return AddRole();" />
		        or <a href="#" onclick="closeOverlay();">cancel</a>
	        </div>
            </div>
            <div id="Container"></div>
		</div>
	</div>
</asp:Content>
