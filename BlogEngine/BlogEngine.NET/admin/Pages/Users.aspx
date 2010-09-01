<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="admin_newuser" Title="Create new user" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td style="width:auto; vertical-align: top;">
                <div class="beTableHdr">
		            <span class="SectionHeader"><%=Resources.labels.users %></span>
		            <a href="#" onclick="Show('frmAddNew');" class="addNew">add new</a>
	            </div>
                <div id="frmAddNew" class="rounded" style="display:none">
					<span class="lbl200">Name</span>
					<input type="text" id="txtUserName" class="txt200"/>
					<span id="txtUserNameReq" class="req hidden">*</span>
					<br/>
					<span class="lbl200">Password</span>
					<input type="password" id="txtPassword" class="txt200"/>
					<input type="password" id="txtPassword2" class="txt200"/>
					<span  id= "txtPasswordReq" class="req hidden">*</span>
                    <span  id= "txtPasswordMatch" class="req hidden">Passwords should match</span>
					<br/>
					<span class="lbl200">Email</span>
					<input type="text" id="txtEmail" class="txt200"/>
					<span id= "txtEmailReq" class="req hidden">*</span>
					<br/>
					<span class="lbl200">&nbsp;</span>
					<input type="submit" class="btnAddNew btn rounded" value="save" onclick="return AddUser(this);" id="btnNewUser" />
					or <a href="#" onclick="Hide('frmAddNew');">cancel</a>
				</div>
                <div id="Container"></div>
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