﻿<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="Add_entry.aspx.cs" Inherits="Admin.Posts.AddEntry" ValidateRequest="False"
    EnableSessionState="True" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
	<div class="content-box-outer">
		<div class="content-box-full">
            <h1>Add/edit post</h1>
           <%--<div id="divDrafts" runat="server" visible="False" enableviewstate="False" style="margin-bottom: 10px">
                <a id="aDrafts" runat="server" href="javascript:void(ToggleVisibility());" />
                <ul id="ulDrafts" runat="server" style="display: none; list-style-type: circle" />
            </div>
                                                                                                                                                                                                                                                                                                                                                                                                   <script type="text/javascript">
                function ToggleVisibility() {
                    var element = document.getElementById('<%=ulDrafts.ClientID%>');
                    if (element.style.display == "none")
                        element.style.display = "block";
                    else
                        element.style.display = "none";
                }--%>
        <script type="text/javascript">
                function GetSlug() {
                    var title = document.getElementById('<%=txtTitle.ClientID %>').value;
                    WebForm_DoCallback('__Page', title, ApplySlug, 'slug', null, false)
                }

                function ApplySlug(arg, context) {
                    var slug = document.getElementById('<%=txtSlug.ClientID %>');
                    slug.value = arg;
                }

                function AutoSave() {
                    var content = document.getElementById('<%=txtRawContent.ClientID %>') != null ? document.getElementById('<%=txtRawContent.ClientID %>').value : tinyMCE.activeEditor.getContent();
                    var title = document.getElementById('<%=txtTitle.ClientID %>').value;
                    var desc = document.getElementById('<%=txtDescription.ClientID %>').value;
                    var slug = document.getElementById('<%=txtSlug.ClientID %>').value;
                    var tags = document.getElementById('<%=txtTags.ClientID %>').value;
                    var s = ';|;';
                    var post = content + s + title + s + desc + s + slug + s + tags;

                    if (content.length > 10) {
                        WebForm_DoCallback('__Page', '_autosave' + post, null, 'autosave', null, false);
                    }

                    setTimeout("AutoSave()", 5000);

                    var currentDate = new Date()
                    document.getElementById('autoSaveLabel').innerHTML = "Autosaved on " + currentDate;
                }

                document.body.onkeypress = ESCclose;

                function ESCclose(evt) {
                    if (!evt)
                        evt = window.event;

                    if (evt.keyCode == 27)
                        document.getElementById('tagselector').style.display = 'none';
                }

                function AddTag(element) {
                    var input = document.getElementById('<%=txtTags.ClientID %>');
                    input.value += element.innerHTML + ', ';
                }

                function ToggleTagSelector() {
                    var element = document.getElementById('tagselector');
                    if (element.style.display == "none")
                        element.style.display = "block";
                    else
                        element.style.display = "none";
                }
                function toggleAutomaticDate() {
                    var element = document.getElementById('rbtManual');
                    var panel = document.getElementById('datePanel');
                    if (element.checked) {
                        panel.style.display = "block";
                    }
                    else {
                        panel.style.display = "none";
                    }
                }
                function SavePost() {
                    $('.loader').show();

                    var content = document.getElementById('<%=txtRawContent.ClientID %>') != null ? document.getElementById('<%=txtRawContent.ClientID %>').value : tinyMCE.activeEditor.getContent();

                    var title = document.getElementById('<%=txtTitle.ClientID %>').value;
                    var desc = document.getElementById('<%=txtDescription.ClientID %>').value;
                    var slug = document.getElementById('<%=txtSlug.ClientID %>').value;
                    var tags = document.getElementById('<%=txtTags.ClientID %>').value;
                    
                    var author = $("[id$='ddlAuthor'] option:selected").val();
                    var isPublished = $("[id$='cbPublish']").is(':checked');
                    var hasCommentsEnabled = $("[id$='cbEnableComments']").is(':checked');

                    var cats = "";
                    var checkedCats = $('.cblCategories input[@type=checkbox]:checked');
                    if (checkedCats.length > 0) {
                        checkedCats.each(function () {
                            var jThis = $(this);
                            cats += jThis.attr("id") + ",";
                        });
                    }

                    var date = document.getElementById('<%=txtDate.ClientID %>').value;
                    var time = document.getElementById('<%=txtTime.ClientID %>').value;

                    var dto = {
                        "id": Querystring('id'),
                        "content": content,
                        "title": title,
                        "desc": desc,
                        "slug": slug,
                        "tags": tags,
                        "author": author,
                        "isPublished": isPublished,
                        "hasCommentsEnabled": hasCommentsEnabled,
                        "cats": cats,
                        "date": date,
                        "time": time
                    };

                    //alert(JSON.stringify(dto));

                    $.ajax({
                        url: "../AjaxHelper.aspx/SavePost",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(dto),
                        success: function (result) {
                            var rt = result.d;
                            if (rt.Success)
                                ShowStatus("success", rt.Message);
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
                    return false;
                }

                function colorboxDialogSubmitClicked(validationGroup, panelId) {

                    // For file/image uploads, colorbox moves the file upload and submit buttons
                    // outside the form tag.  This prevents submitting from working.  Before
                    // a submit can work, need to move the dialog box containing the controls
                    // back inside the form tag.
                    // First check to make sure validation passes before closing colorbox.

                    if (typeof Page_ClientValidate !== 'undefined') {
                        if (!Page_ClientValidate(validationGroup)) {
                            return true;
                        }
                    }

                    $.colorbox.close();
                    $("form").append($("#" + panelId));
                    return true;
                }
            </script>

            <div runat="server" style="visibility:hidden;height:1px">
                <div id="uploadImagePanel" class="overlaypanel">
                    <h2><%=Resources.labels.uploadImage %></h2>
                    <ul class="fl" style="margin:0;">
                        <li>
                            <asp:Label ID="lblFileUpload" CssClass="lbl" AssociatedControlID="txtUploadImage" runat="server" Text='Choose image you wish to upload' />
                            <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" size="50" ValidationGroup="imageupload" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtUploadImage" ErrorMessage="<%$ Resources:labels, required %>"
                                ValidationGroup="imageupload" />
                        </li>
                        <li style="margin:0;">
                            <asp:Button runat="server" ID="btnUploadImage" Text="<%$Resources:labels,upload %>"
                                ValidationGroup="imageupload" CssClass="btn primary" OnClientClick="colorboxDialogSubmitClicked('imageupload', 'uploadImagePanel');" /> or <a href="#" onclick="return closeOverlay();">Cancel</a>
                        </li>
                    </ul>
                </div>
                <div id="uploadFilePanel" class="overlaypanel">
                    <h2><%=Resources.labels.uploadFile%></h2>
                    <ul class="fl" style="margin:0;">
                        <li>
                            <asp:Label ID="Label1" CssClass="lbl" AssociatedControlID="txtUploadFile" runat="server" Text='Choose image you wish to upload' />
                            <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" size="50" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUploadFile" ErrorMessage="<%$ Resources:labels, required %>"
                                ValidationGroup="fileUpload" />
                        </li>
                        <li style="margin:0;">
                            <asp:Button runat="server" ID="btnUploadFile" Text="<%$Resources:labels,upload %>"
                                ValidationGroup="fileUpload" CssClass="btn primary" OnClientClick="colorboxDialogSubmitClicked('fileUpload', 'uploadFilePanel');" /> or <a href="#" onclick="return closeOverlay();">Cancel</a>
                        </li>
                    </ul>
                </div>
            </div>

            <table class="tblForm largeForm" style="width:100%; margin:0;">
                <tr>
                    <td style="vertical-align:top; padding:0 40px 0 0;">
                        <ul class="fl">
                            <li>
                                <asp:Label CssClass="lbl" AssociatedControlID="txtTitle" runat="server" Text='<%$ Code: Resources.labels.title %>' />
                                <asp:TextBox runat="server" ID="txtTitle" Width="600px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" 
                                    ErrorMessage="<%$Resources:labels,enterTitle %>" Display="Dynamic" />
                            </li>
                            <li>
                                <div class="editToolbar">
                                    <asp:CheckBox runat="server" ID="cbUseRaw" Text="<%$Resources:labels,useRawHtmlEditor %>"
                                        AutoPostBack="true" />
                                    <a href="#" id="uploadImage" class="image">Insert image</a>
                                    <a href="#" id="uploadFile" class="file">Attach file</a>
                                </div>
                                <Blog:TextEditor runat="server" id="txtContent" />
                                <asp:TextBox runat="server" ID="txtRawContent" Width="98%" TextMode="multiLine"
                                    Height="400px" Visible="false" />
                            </li>
                            <li>
                                <asp:Label ID="Label2" CssClass="lbl" AssociatedControlID="txtSlug" runat="server" Text='<%$Resources:labels,slug %>' />
                                <asp:TextBox runat="server" ID="txtSlug" Width="600" />
                                <a href="javascript:void(GetSlug());">
                                    <%=Resources.labels.extractFromTitle %></a>
                            </li>
                            <li>
                                <asp:Label ID="Label3" CssClass="lbl" AssociatedControlID="txtDescription" runat="server" Text='Summary' />
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="multiLine" Columns="50"
                                    Rows="3" Width="600" Height="80" />
                            </li>
                            <li>
                                <label class="lbl">Options</label>
                                <asp:CheckBox runat="server" ID="cbEnableComments" Text="<%$ Resources:labels, enableComments %>"
                                    Checked="true" />
                            </li>
                            <li>
                                 <asp:CheckBox runat="server" ID="cbPublish" Text="<%$ Resources:labels, publish %>"
                                    Checked="true" />
                           </li>
                        </ul>
                        <div class="action_buttons">
                            <input type="button" id="btnSave" value="<%=Resources.labels.save %>" class="btn primary rounded" onclick="return SavePost()" /> or 
                            <% if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                               { %>
                            <a href="<%=PostUrl %>" title="Go to page">Go to post</a>
                            <%}
                               else
                               {%>
                            <a href="Posts.aspx" title="Cancel"><%=Resources.labels.cancel %></a>
                            <%} %>
                            <span id="autoSaveLabel" style="display:none;"></span>
                        </div>
                    </td>
                    <td class="secondaryForm" style="padding:0; vertical-align:top;">
                        <ul class="fl">
                            <li>
                                <asp:Label CssClass="lbl" AssociatedControlID="ddlAuthor" runat="server" Text='<%$ Code: Resources.labels.author %>' />
                                <asp:DropDownList runat="Server" ID="ddlAuthor" />
                            </li>
                            <li>
                                <asp:Label CssClass="lbl" AssociatedControlID="txtDate" runat="server" Text='Set publish date' />
                                <input type="radio" name="PublishDate" id="rbtAuto" onclick="toggleAutomaticDate()" checked="checked" /><label for="rbtAuto">Automatically</label>
                                <input type="radio" name="PublishDate" id="rbtManual" onclick="toggleAutomaticDate()" /><label for="rbtManual">Manually</label>
                                <div id="datePanel" style="display:none;">
                                    <asp:TextBox runat="server" ID="txtDate" Width="170" />
                                    <asp:TextBox runat="server" ID="txtTime" Width="50" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDate" ValidationExpression="[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]"
                                        ErrorMessage="<%$Resources:labels,enterValidDate %>" Display="dynamic" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDate" ErrorMessage="<%$Resources:labels,enterDate %>"
                                        Display="Dynamic" />
                                </div>
                            </li>
                            <li>
                                <label class="lbl"><%=Resources.labels.categories %></label>
                                <div class="rounded" style="overflow-y:auto; max-height:160px;border:solid 1px #dcdcdc; padding:5px; margin:0 0 5px;">
                                    <span id="cblCategories" runat="server" class="cblCategories"></span>
                                </div>
                                <div style="">
                                    <label for="<%=txtCategory.ClientID %>" style="margin-bottom:5px;">Quick add new category</label>
                                    <asp:TextBox runat="server" ID="txtCategory" ValidationGroup="category" Width="180" />
                                    <asp:Button runat="server" ID="btnCategory" Text="<%$ Resources:labels, add %>" ValidationGroup="category" CssClass="btn" style="min-width:0px; margin:0;" />
                                    <asp:CustomValidator runat="Server" ID="valExist" ValidationGroup="category" ControlToValidate="txtCategory"
                                        ErrorMessage="<%$ Resources:labels, categoryAlreadyExists %>" Display="dynamic" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCategory" ErrorMessage="<%$ Resources:labels, required %>"
                                        ValidationGroup="category" Display="Dynamic" />
                                </div>
                            </li>
                            <li style="position:relative;">
                                <label class="lbl"><%=Resources.labels.tags%></label>
                                <asp:TextBox runat="server" ID="txtTags" TextMode="MultiLine" Rows="3" />
                                <span><%=Resources.labels.separateTagsWitComma %></span> or 
                                
                                <a href="javascript:void(ToggleTagSelector())">Choose from existing tags</a>
                                <div id="tagselector" class="rounded" style="display: none;">
                                    <a class="close" onclick="ToggleTagSelector()"></a>
                                    <span>Click a tag:</span>
                                    <div class="clear"></div>
                                    <div style="max-height:150px; overflow-y:auto;">
                                        <asp:PlaceHolder runat="server" ID="phTags" />
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </table>
            <% if (Request.QueryString["id"] == null)
               { %>
            <script type="text/javascript">
                setTimeout("AutoSave()", 5000);
            </script>
            <% } %>
        </div>
    </div>
</asp:Content>
