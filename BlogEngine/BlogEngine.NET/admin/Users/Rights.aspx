<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="Rights.aspx.cs" Inherits="Admin.Users.Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        var rights = <%=this.GetRightsJson()%>;
        var role = "<%=this.RoleName %>";

        var rightsControls = {};

        $(document).ready(function() {

            var tempIdCount = 0;

            for (var category in rights) {
            
                var catDiv = $("<div class=\"dashboardWidget rounded\">");
                var header = $("<h2 style='border:none;'>");
                header.html(category);

                var catUl = $("<ul class='fl'>");
                catDiv.append(header);
                catDiv.append(catUl);

                for (var key in rights[category]) {

                    tempIdCount += 1;
                    var checkBoxId = "rightCheck" + tempIdCount;

                    var li = $("<li>");
                    var checkBox = $("<input type=\"checkbox\" />");
                    checkBox.attr("id", checkBoxId);
                    if (role.toLowerCase() === "administrators") {
                        checkBox.click(function() {
                            if (!$(this).is(":checked")) {
                                alert("Rights cannot be removed from the Administrators role.");
                                return false;
                            }
                        });
                    }

                    if (rights[category][key] === true) {
                        checkBox.attr("checked", "checked");
                    }

                    li.append(checkBox);
                    li.append($("<label>").attr("for", checkBoxId).text(key));

                    catUl.append(li);

                    rightsControls[key] = {
                        li : li,
                        checkBox : checkBox
                    };
                }

                $("#rightsHolder").append(catDiv);
            }
        });

        function SaveRights() {

            var rightsToSave = {};
            for (var category in rights) {
                for (var key in rights[category]) {
                    if (rightsControls[key].checkBox.attr("checked") === true) {
                        rightsToSave[key] = true;
                    }
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
                <li><a href="Users.aspx"><%=Resources.labels.users %></a></li>
                <li class="content-box-selected"><a href="Roles.aspx" class="selected"><%=Resources.labels.roles %></a></li>
            </ul>
        </div>
        <div class="content-box-left">
            <h1>Editing Rights for Role <%=Server.HtmlEncode(this.RoleName) %></h1>
            <div id="rightsHolder"></div>
            <div style="clear:both">&nbsp;</div>
            <input type="submit" class="btn primary rounded" value="save" onclick="return SaveRights();" />
            or <a href="Roles.aspx"><%=Resources.labels.cancel %></a>
        </div>
    </div>
</asp:Content>
