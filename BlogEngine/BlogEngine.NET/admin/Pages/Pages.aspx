<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Pages.aspx.cs" ValidateRequest="false" Inherits="Admin.Pages.PagesPage"
    Title="Add page" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        function ToggleVisibility() {
            var element = document.getElementById('<%=ulPages.ClientID%>');
            if (element.style.display == "none")
                element.style.display = "block";
            else
                element.style.display = "none";
        }

        function GetSlug() {
            var title = document.getElementById('<%=txtTitle.ClientID %>').value;
            WebForm_DoCallback('__Page', title, ApplySlug, 'slug', null, false)
        }

        function ApplySlug(arg, context) {
            var slug = document.getElementById('<%=txtSlug.ClientID %>');
            slug.value = arg;
        }
    </script>
    <div class="content-box-full">
        <h1>Edit page</h1>
        <div id="divPages" runat="server" visible="true" enableviewstate="False" style="margin-bottom: 10px">
            <a id="aPages" runat="server" href="javascript:void(ToggleVisibility());" />
            <ul id="ulPages" runat="server" style="display: none; list-style-type: circle" />
        </div>
        <table class="tblForm largeForm" style="width:100%; margin:0;">
            <tr>
                <td style="vertical-align:top; padding:0 40px 0 0;"
                    <ul class="fl">
                        <li>
                            <label class="lbl" for="<%=txtTitle.ClientID %>">
                                <%=Resources.labels.title %></label>
                            <asp:TextBox runat="server" ID="txtTitle" Width="600" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" Display="Dynamic"
                                ErrorMessage="<%$Resources:labels,enterTitle %>" />
                        </li>
                        <li>
                            <Blog:TextEditor runat="server" id="txtContent" TabIndex="4" />
                        </li>
                        <li>
                            <label class="lbl"><%=Resources.labels.uploadImage %></label>
                            <asp:FileUpload runat="server" ID="txtUploadImage" size="50" />
                            <asp:Button CssClass="btn" runat="server" ID="btnUploadImage" Text="<%$Resources:labels,upload %>"
                                ValidationGroup="imageupload" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtUploadImage" ErrorMessage="<%$ Resources:labels, required %>"
                                ValidationGroup="imageupload" />
                        </li>
                        <li>
                            <label class="lbl"><%=Resources.labels.uploadFile %></label>
                            <asp:FileUpload runat="server" ID="txtUploadFile" size="50" />
                            <asp:Button CssClass="btn" runat="server" ID="btnUploadFile" Text="<%$Resources:labels,upload %>"
                                ValidationGroup="fileUpload" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUploadFile" ErrorMessage="<%$ Resources:labels, required %>"
                                ValidationGroup="fileUpload" />
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
            <asp:Button runat="server" ID="btnSave" CssClass="btn primary rounded" />
        </div>
    </div>
</asp:Content>
