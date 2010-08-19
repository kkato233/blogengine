<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="admin_Account_Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
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