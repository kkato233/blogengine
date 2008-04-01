<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Profiles.aspx.cs" Inherits="admin_profiles" Title="Modify Profiles" %>
<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <br />
    <div class="settings">
        <h1>
            <%=Resources.labels.userProfiles %>
        </h1>
        <asp:Panel ID="pnlAdmin" runat="server" Visible='<%# User.IsInRole("Administrator") %>'>
            <asp:DropDownList ID="ddlUserList" runat="server">
            </asp:DropDownList>
            <asp:LinkButton ID="lbChangeUserProfile" runat="server" 
                OnClick="lbChangeUserProfile_Click"><%= Resources.labels.switchUserProfile %></asp:LinkButton>
        </asp:Panel>
        <br />
    </div>
    <br />
    <div class="settings">
       <label for="<%=cbIsPublic.ClientID %>"><%=Resources.labels.isPrivate %></label>
    <asp:CheckBox ID="cbIsPublic" runat="server" />       <br />
     
    <label for="<%=tbFirstName.ClientID %>"> <%=Resources.labels.firstName %></label>
    <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbLastName.ClientID %>"><%=Resources.labels.lastName %></label>
    <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox><br />

    <label for="<%=cblGender.ClientID %>"><%=Resources.labels.gender %></label>
    <asp:CheckBoxList ID="cblGender" runat="server" EnableViewState="false" RepeatDirection="Horizontal" TextAlign="Right" CellPadding="0" CellSpacing="0">
    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
    </asp:CheckBoxList><br />

    <label for="<%=tbPhotoUrl.ClientID %>"><%=Resources.labels.photoURL %></label>
    <asp:TextBox ID="tbPhotoUrl" runat="server" Columns="50"></asp:TextBox><br />

    <label for="<%=tbCityTown.ClientID %>"><%=Resources.labels.cityTown %></label>
    <asp:TextBox ID="tbCityTown" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbRegionState.ClientID %>"><%=Resources.labels.regionState %></label>
    <asp:TextBox ID="tbRegionState" runat="server"></asp:TextBox><br />
    
    <label for="<%=ddlCountry.ClientID %>"><%=Resources.labels.country %></label>
    <asp:DropDownList ID="ddlCountry" runat="server" onchange="SetFlag(this.value)" ></asp:DropDownList>
    <asp:Image runat="server" ID="imgFlag" AlternateText="Country flag" Width="16" Height="11" EnableViewState="false" />
    <br />          
    
    <label for="<%=tbInterests.ClientID %>"><%=Resources.labels.interests %></label>
    <Blog:TextEditor runat="server" id="tbInterests"      /><br /> 
    
    <label for="<%=tbAboutMe.ClientID %>"><%=Resources.labels.aboutMe %></label>
    <Blog:TextEditor runat="server" id="tbAboutMe" TabIndex="4" /><br /> 
        
        <p>
            <asp:LinkButton ID="lbSaveProfile" runat="server" OnClick="lbSaveProfile_Click"><%=Resources.labels.saveProfile %></asp:LinkButton>
        </p>
    </div>
</asp:Content>
