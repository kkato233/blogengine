<%@ Page Title="" Language="C#" MasterPageFile="account.master" AutoEventWireup="true" 
    CodeFile="create-blog.aspx.cs" Inherits="Account.CreateBlog" %>
<%@ MasterType VirtualPath="~/Account/account.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <p>
        <span id="CreateHdr">Create new blog</span> 
    </p>
    <div class="accountInfo">
        <div class="login">
            <p>
                <span><%=BlogEngine.Core.Utils.AbsoluteWebRoot %></span><span id="blogId" style="font-weight:bold"></span>
                <div class="boxRound">
                    <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                </div>
            </p>
            <p class="submitButton">
                <asp:Button ID="CreateUserButton" runat="server" Text="Create"
                    OnClientClick="return ValidateNewBlog()" OnClick="CreateUserButton_Click" />
            </p>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[name$='UserName']").focus();

            $(function () {
                $('#<%= UserName.ClientID %>').keyup(function () {
                    $('#blogId').text($(this).val());
                });
            });
        });
    </script>
</asp:Content>

