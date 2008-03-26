<%@ Page Language="C#" MasterPageFile="~/admin/admin1.master" AutoEventWireup="true"
    CodeFile="Profiles.aspx.cs" Inherits="admin_profiles" Title="Modify Profiles" %>

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

    <label for="<%=tbGender.ClientID %>"><%=Resources.labels.gender %></label>
    <asp:TextBox ID="tbGender" runat="server"></asp:TextBox><br />

    <label for="<%=tbCityTown.ClientID %>"><%=Resources.labels.cityTown %></label>
    <asp:TextBox ID="tbCityTown" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbRegionState.ClientID %>"><%=Resources.labels.regionState %></label>
    <asp:TextBox ID="tbRegionState" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbCountry.ClientID %>"><%=Resources.labels.country %></label>
    <asp:TextBox ID="tbCountry" runat="server"></asp:TextBox><br />          
    
    <label for="<%=tbInterests.ClientID %>"><%=Resources.labels.interests %></label>
    <asp:TextBox ID="tbInterests" runat="server" TextMode="multiLine" Rows="5" Columns="50"></asp:TextBox><br /> 
    
    <label for="<%=tbAboutMe.ClientID %>"><%=Resources.labels.aboutMe %></label>
    <asp:TextBox ID="tbAboutMe" runat="server" TextMode="multiLine" Rows="5" Columns="50"></asp:TextBox><br />       
    
        <p>
            <asp:LinkButton ID="lbSaveProfile" runat="server" OnClick="lbSaveProfile_Click"><%=Resources.labels.saveProfile %></asp:LinkButton>
        </p>
    </div>
</asp:Content>
