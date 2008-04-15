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
            <asp:DropDownList ID="ddlUserList" runat="server" 
                onselectedindexchanged="ddlUserList_SelectedIndexChanged">
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
     
    <label for="<%=tbDisplayName.ClientID %>"> <%=Resources.labels.displayName %></label>
    <asp:TextBox ID="tbDisplayName" runat="server"></asp:TextBox><br />

    <label for="<%=tbtitle.ClientID %>"> <%=Resources.labels.title %></label>
    <asp:TextBox ID="tbtitle" runat="server"></asp:TextBox><br />

    <label for="<%=tbFirstName.ClientID %>"> <%=Resources.labels.firstName %></label>
    <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbMiddleName.ClientID %>"><%=Resources.labels.middleName %></label>
    <asp:TextBox ID="tbMiddleName" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbLastName.ClientID %>"><%=Resources.labels.lastName %></label>
    <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbBirthdate.ClientID %>"><%=Resources.labels.birthday %></label>
    <asp:TextBox ID="tbBirthdate" runat="server"></asp:TextBox><br />

    <label for="<%=tbPhotoUrl.ClientID %>"><%=Resources.labels.photoURL %></label>
    <asp:TextBox ID="tbPhotoUrl" runat="server" Columns="50"></asp:TextBox><br />    
    <label for="<%=tbPhoneMain.ClientID %>"><%=Resources.labels.phoneMain %></label>
    <asp:TextBox ID="tbPhoneMain" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbPhoneMobile.ClientID %>"><%=Resources.labels.phoneMobile %></label>   <asp:TextBox ID="tbPhoneMobile" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbPhoneFax.ClientID %>"><%=Resources.labels.phoneFax %></label>
    <asp:TextBox ID="tbPhoneFax" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbEmailAddress.ClientID %>"><%=Resources.labels.emailAddress %></label>
    <asp:TextBox ID="tbEmailAddress" runat="server"></asp:TextBox><br />

    <label for="<%=tbCityTown.ClientID %>"><%=Resources.labels.cityTown %></label>
    <asp:TextBox ID="tbCityTown" runat="server"></asp:TextBox><br />
    
    <label for="<%=tbRegionState.ClientID %>"><%=Resources.labels.regionState %></label>
    <asp:TextBox ID="tbRegionState" runat="server"></asp:TextBox><br />
    
    <label for="<%=ddlCountry.ClientID %>"><%=Resources.labels.country %></label>
    <asp:DropDownList ID="ddlCountry" runat="server" onchange="SetFlag(this.value)" 
            AutoPostBack="True" ></asp:DropDownList>
    <asp:Image runat="server" ID="imgFlag" AlternateText="Country flag" Width="16" Height="11" EnableViewState="false" />
    <br />          
    
    <label for="<%=tbCompany.ClientID %>"><%=Resources.labels.company %></label>
    <asp:TextBox runat="server" id="tbCompany" />
    <br /> 
    
    <label for="<%=tbAboutMe.ClientID %>"><%=Resources.labels.aboutMe %></label>
  <asp:TextBox runat="server" id="tbAboutMe" Rows="5" Width="50%" TextMode="MultiLine"  /><br /> 
        
        <p>
            <asp:LinkButton ID="lbSaveProfile" runat="server" OnClick="lbSaveProfile_Click"><%=Resources.labels.saveProfile %></asp:LinkButton>
        </p>
    </div>
</asp:Content>
