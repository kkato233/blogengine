<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="Rights.aspx.cs" Inherits="Admin.Users.Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        var rights = <%=this.GetRightsJson()%>;
        var role = "<%=this.RoleName %>";

        var rightsControls = {};

        $(document).ready(function() {

            var tempIdCount = 0;

            for (var key in rights) {

                tempIdCount += 1;
                var checkBoxId = "rightCheck" + tempIdCount;

                var row = $("<div class=\"rightRow\">");
                var checkBox = $("<input type=\"checkbox\" />");
                checkBox.attr("id", checkBoxId);

                if (rights[key] === true) {
                    checkBox.attr("checked", "checked");
                }

                row.append(checkBox);
                row.append($("<label>").attr("for", checkBoxId).text(key));

                $("#rightsHolder").append(row);

                rightsControls[key] = {
                    row : row,
                    checkBox : checkBox
                };
            }
        });

        function SaveRights() {

            var rightsToSave = {};
            for (var key in rights) {
                if (rightsControls[key].checkBox.attr("checked") === true) {
                    rightsToSave[key] = true;
                }
            }

            var dto = { 
                roleName: role,
                rightsCollection: rightsToSave
            };

            $.ajax({
                url: "../../api/RoleService.asmx/SaveRights",
                data: JSON.stringify(dto),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var rt = result.d;
                    if(rt.Success) {
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
    <div class="content-box-outer">
        <div class="content-box-right">
            <ul>
                <li><a href="Users.aspx">
                    <%=Resources.labels.users %></a></li>
                <li><a href="Roles.aspx" class="selected">
                    <%=Resources.labels.roles %></a></li>
                <li class="content-box-selected"><a href="Rights.aspx">Rights</a></li>
            </ul>
        </div>
        <div class="content-box-left">
            <h1>Editing Rights for Role <%=Server.HtmlEncode(this.RoleName) %></h1>
            <div id="rightsHolder"></div>
            <input type="submit" class="btn primary rounded" value="save" onclick="return SaveRights();" />
            or <a href="#" onclick="closeOverlay();">cancel</a>
        </div>
    </div>
</asp:Content>
