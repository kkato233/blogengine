﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true" 
ValidateRequest="false" CodeFile="EditPage.aspx.cs" Inherits="Admin.Pages.EditPage" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        function GetSlug() {
            var title = document.getElementById('<%=txtTitle.ClientID %>').value;
            WebForm_DoCallback('__Page', title, ApplySlug, 'slug', null, false)
        }

        function ApplySlug(arg, context) {
            var slug = document.getElementById('<%=txtSlug.ClientID %>');
            slug.value = arg;
        }

        function SavePage() {
            $('.loader').show();

            var content = tinyMCE.activeEditor.getContent();

            if (content.length == 0) content = '[No text]';

            var title = $("[id$='txtTitle']").val();
            var description = $("[id$='txtDescription']").val();
            var keywords = $("[id$='txtKeyword']").val();
            var slug = $("[id$='txtSlug']").val();

            var isFrontPage = $("[id$='cbFrontPage']").is(':checked');
            var showInList = $("[id$='cbShowInList']").is(':checked');
            var isPublished = $("[id$='cbPublished']").is(':checked');

            var parent = $("[id$='ddlParent'] option:selected").val();

            var dto = {
                "id": Querystring('id'),
                "content": content,
                "title": title,
                "description": description,
                "keywords": keywords,
                "slug": slug,
                "isFrontPage": isFrontPage,
                "showInList": showInList,
                "isPublished": isPublished,
                "parent": parent
            };

            //alert(JSON.stringify(dto));

            $.ajax({
                url: "../AjaxHelper.aspx/SavePage",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dto),
                success: function (result) {
                    var rt = result.d;
                    if (rt.Success) {
                        if (rt.Data) {
                            window.location.href = rt.Data;
                        } else {
                            ShowStatus("success", rt.Message);
                        }
                    }
                    else
                        ShowStatus("warning", rt.Message);
                }
            });

            $('.loader').hide();
            return false;
        }
    </script>

    <script type="text/javascript" src="../jquery.colorbox.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#uploadImage").colorbox({ width: "550px", inline: true, href: "#uploadImagePanel" });
            $("#uploadFile").colorbox({ width: "550px", inline: true, href: "#uploadFilePanel" });
        });

        function closeOverlay() {
            $.colorbox.close();
        }
    </script>

    <div style="display:none;">
        <div id="uploadImagePanel" class="overlaypanel">
            <h2><%=Resources.labels.uploadImage %></h2>
            <ul class="fl" style="margin:0;">
                <li>
                    <label class="lbl"><%=Resources.labels.uploadImage %></label>
                    <asp:FileUpload runat="server" ID="txtUploadImage" size="50" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtUploadImage" ErrorMessage="<%$ Resources:labels, required %>"
                        ValidationGroup="imageupload" />
                </li>
                <li style="margin:0;">
                    <asp:Button CssClass="btn primary" runat="server" ID="btnUploadImage" Text="<%$Resources:labels,upload %>"
                        ValidationGroup="imageupload" OnClientClick="colorboxDialogSubmitClicked('imageupload', 'uploadImagePanel');" />
                        or <a href="#" onclick="closeOverlay();">Cancel</a>
                </li>
            </ul>
        </div>
        <div id="uploadFilePanel" class="overlaypanel">
            <h2><%=Resources.labels.uploadFile%></h2>
            <ul class="fl" style="margin:0;">
                <li>
                    <label class="lbl"><%=Resources.labels.uploadFile %></label>
                    <asp:FileUpload runat="server" ID="txtUploadFile" size="50" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUploadFile" ErrorMessage="<%$ Resources:labels, required %>"
                        ValidationGroup="fileUpload" />
                </li>
                <li style="margin:0;">
                    <asp:Button CssClass="btn primary" runat="server" ID="btnUploadFile" Text="<%$Resources:labels,upload %>"
                        ValidationGroup="fileUpload" OnClientClick="colorboxDialogSubmitClicked('fileUpload', 'uploadFilePanel');" />
                        or <a href="#" onclick="closeOverlay();">Cancel</a>
                </li>
            </ul>
        </div>
    </div>

    <div class="content-box-full">
        <h1>Edit page</h1>
        <table class="tblForm largeForm" style="width:100%; margin:0;">
            <tr>
                <td style="vertical-align:top; padding:0 40px 0 0;">
                    <ul class="fl">
                        <li>
                            <label class="lbl" for="<%=txtTitle.ClientID %>">
                                <%=Resources.labels.title %></label>
                            <asp:TextBox runat="server" ID="txtTitle" Width="600" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" Display="Dynamic"
                                ErrorMessage="<%$Resources:labels,enterTitle %>" />
                        </li>
                        <li>
                            <div class="editToolbar">
                                <a href="#" id="uploadImage" class="image">Insert image</a>
                                <a href="#" id="uploadFile" class="file">Attach file</a>
                            </div>
                            <Blog:TextEditor runat="server" id="txtContent" TabIndex="4" />
                        </li>
                        <li>
                            <label class="lbl" for="<%=txtSlug.ClientID %>"><%=Resources.labels.slug %></label>
                            <asp:TextBox runat="server" ID="txtSlug" TabIndex="9" Width="600" />
                            <a href="javascript:void(GetSlug());">
                                <%=Resources.labels.extractFromTitle %></a>
                        </li>
                        <li>
                            <label class="lbl" for="<%=txtDescription.ClientID %>"><%=Resources.labels.description %></label>
                            <asp:TextBox runat="server" ID="txtDescription" Width="600" TextMode="multiLine"
                                Columns="50" Rows="4" />
                        </li>
                        <li>
                            <asp:CheckBox runat="Server" ID="cbPublished" Checked="true" Text="<%$ Resources:labels, publish %>" />
                        </li>
                    </ul>
                </td>
                <td class="secondaryForm" style="padding:0; vertical-align:top;">
                    <ul class="fl">
                        <li>
                            <label class="lbl" for="<%=ddlParent.ClientID %>"><%=Resources.labels.selectParent %></label>
                            <asp:DropDownList runat="server" ID="ddlParent" Width="250" />
                        </li>
                        <li>
                            <label class="lbl" for="<%=txtKeyword.ClientID %>"><%=Resources.labels.keywords %></label>
                            <asp:TextBox runat="server" ID="txtKeyword" TextMode="MultiLine" Rows="5"  />
                        </li>
                        <li>
                            <label class="lbl">Options</label>
                            <asp:CheckBox runat="Server" ID="cbFrontPage" Text="<%$ Resources:labels, isFrontPage %>" /><br />
                        </li>
                        <li>
                             <asp:CheckBox runat="Server" ID="cbShowInList" Text="<%$ Resources:labels, showInList %>" Checked="true" />
                        </li>
                    </ul>
                </td>
            </tr>
        </table>

        <div class="action_buttons">
            <input type="button" id="btnSave" value="<%=Resources.labels.save %>" class="btn primary rounded" onclick="return SavePage()" /> or 
            <% if (!string.IsNullOrEmpty(Request.QueryString["id"]))
               { %>
            <a href="<%=PageUrl %>" title="Go to page">Go to page</a>
            <%}
               else
               {%>
            or <a href="Pages.aspx" title="Cancel"><%=Resources.labels.cancel %></a>
            <%} %>
        </div>
    </div>
</asp:Content>