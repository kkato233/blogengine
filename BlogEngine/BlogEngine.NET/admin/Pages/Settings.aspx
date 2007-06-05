<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="admin_Pages_configuration" Title="Settings" %>
<%@ Import Namespace="BlogEngine.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
<br />

<div style="text-align:right">
  <asp:Button runat="server" ID="btnSaveTop" />
</div><br />

<div class="settings">

  <h1><%=Resources.labels.basic %> <%=Resources.labels.settings %></h1>
  <label for="<%=txtName.ClientID %>"><%=Resources.labels.name %></label>
  <asp:TextBox runat="server" ID="txtName" Width="300" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="Required" /><br />

  <label for="<%=txtDescription.ClientID %>"><%=Resources.labels.description %></label>
  <asp:TextBox runat="server" ID="txtDescription" Width="300" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" ErrorMessage="Required" /><br />
    
  <label for="<%=txtPostsPerPage.ClientID %>"><%=Resources.labels.postPerPage %></label>
  <asp:TextBox runat="server" ID="txtPostsPerPage" Width="50" MaxLength="4" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPostsPerPage" ErrorMessage="Required" />
  <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtPostsPerPage" Operator="DataTypeCheck" Type="integer" ErrorMessage="Please enter a valid number" /><br />
  
  <label for="<%=ddlTheme.ClientID %>"><%=Resources.labels.theme %></label>
  <asp:DropDownList runat="server" ID="ddlTheme" /><br />
    
  <label for="<%=cbShowRelatedPosts.ClientID %>"><%=Resources.labels.showRelatedPosts %></label>
  <asp:CheckBox runat="server" ID="cbShowRelatedPosts" /><br />
  
  <label for="<%=ddlCulture.ClientID %>"><%=Resources.labels.language %></label>
  <asp:DropDownList runat="Server" id="ddlCulture">
    <asp:ListItem Text="Auto" />
    <asp:ListItem Text="english" Value="en" />
  </asp:DropDownList>
  
</div>

<div class="settings">

  <h1><%=Resources.labels.advanced %> <%=Resources.labels.settings %></h1>
  <label for="<%=cbEnableCompression.ClientID %>"><%=Resources.labels.enableHttpCompression %></label>
  <asp:CheckBox runat="server" ID="cbEnableCompression" /> Make the pages load faster (recommended).<br />
  
  <label for="<%=cbRemoveWhitespaceInStyleSheets.ClientID %>"><%=Resources.labels.trimStylesheet %></label>
  <asp:CheckBox runat="server" ID="cbRemoveWhitespaceInStyleSheets" /> Makes all stylesheets of any theme smaller by removing all whitespace at runtime.<br />
  
  <label for="<%=cbEnableOpenSearch.ClientID %>"><%=Resources.labels.enableOpenSearch %></label>
  <asp:CheckBox runat="server" ID="cbEnableOpenSearch" /> Adds the search feature to all new browsers (recommended).<br />
  
  <label for="<%=cbEnableSearchHighlight.ClientID %>"><%=Resources.labels.enableSearchHighlight %></label>
  <asp:CheckBox runat="server" ID="cbEnableSearchHighlight" /> Colors the search words on the page.<br />
  
</div>

<div class="settings">

  <h1><%=Resources.labels.comments %></h1>
  <label for="<%=cbEnableComments.ClientID %>"><%=Resources.labels.enableComments %></label>
  <asp:CheckBox runat="server" ID="cbEnableComments" /> If comments aren't enabled, nobody can write comments to any post.<br />
  
  <label for="<%=cbEnableCountryInComments.ClientID %>"><%=Resources.labels.showCountryChooser %></label>
  <asp:CheckBox runat="server" ID="cbEnableCountryInComments" /> If the country chooser isn't shown, no flag can be shown on each comment.<br />
  
  <label for="<%=cbEnableCoComment.ClientID %>"><%=Resources.labels.enableCoComments %></label>
  <asp:CheckBox runat="server" ID="cbEnableCoComment" /><br />
  
  <label for="<%=cbShowLivePreview.ClientID %>"><%=Resources.labels.showLivePreview %></label>
  <asp:CheckBox runat="server" ID="cbShowLivePreview" /><br />
  
  <label for="<%=ddlCloseComments.ClientID %>" style="position:relative;top:4px"><%=Resources.labels.closeCommetsAfter %></label>
  <asp:DropDownList runat="server" ID="ddlCloseComments">
  <asp:ListItem Text="Never" Value="0" />
  <asp:ListItem Text="1" />
  <asp:ListItem Text="2" />
  <asp:ListItem Text="3" />
  <asp:ListItem Text="7" />
  <asp:ListItem Text="10" />
  <asp:ListItem Text="14" />
  <asp:ListItem Text="21" />
  <asp:ListItem Text="30" />
  <asp:ListItem Text="60" />
  <asp:ListItem Text="90" />
  </asp:DropDownList> days.
</div>

<div class="settings">

  <h1>E-mail</h1>
  <label for="<%=txtEmail.ClientID %>"><%=Resources.labels.emailAddress %></label>
  <asp:TextBox runat="server" ID="txtEmail" Width="300" /><br />
  
  <label for="<%=txtSmtpServer.ClientID %>">SMTP server</label>
  <asp:TextBox runat="server" ID="txtSmtpServer" Width="300" /><br />
  
  <label for="<%=txtSmtpServerPort.ClientID %>"><%=Resources.labels.postNumber %></label>
  <asp:TextBox runat="server" ID="txtSmtpServerPort" Width="35" /> Port 25 is the standard
  <asp:CompareValidator ID="CompareValidator2" runat="Server" ControlToValidate="txtSmtpServerPort" Operator="datatypecheck" Type="integer" ErrorMessage="Not a valid number" /><br />
  
  <label for="<%=txtSmtpUsername.ClientID %>"><%=Resources.labels.userName %></label>
  <asp:TextBox runat="server" ID="txtSmtpUsername" Width="300" /><br />
  
  <label for="<%=txtSmtpPassword.ClientID %>"><%=Resources.labels.password %></label>
  <asp:TextBox runat="server" ID="txtSmtpPassword" Width="300" /><br />
  
  <label for="<%=cbComments.ClientID %>"><%=Resources.labels.sendCommentEmail %></label>
  <asp:CheckBox runat="Server" ID="cbComments" /><br /><br />
  
  <asp:Button runat="server" CausesValidation="False" ID="btnTestSmtp" Text="Test mail settings" />
  <asp:Label runat="Server" ID="lbSmtpStatus" />
</div>

<div class="settings">

  <h1>Feed <%=Resources.labels.settings.ToLowerInvariant() %></h1>
  
  <label for="<%=ddlSyndicationFormat.ClientID %>" style="position:relative;top:4px"><%=Resources.labels.defaultFeedOutput %></label>
  <asp:DropDownList runat="server" ID="ddlSyndicationFormat">
    <asp:ListItem Text="RSS 2.0" Value="Rss" Selected="True" />
    <asp:ListItem Text="Atom 1.0" Value="Atom" />
  </asp:DropDownList> format.<br />
  
  <br />
  
  <label for="<%=txtDublinCoreCreator.ClientID %>"><%=Resources.labels.author %></label>
  <asp:TextBox runat="server" ID="txtDublinCoreCreator" Width="300" /><br />
  
  <label for="<%=txtDublinCoreLanguage.ClientID %>"><%=Resources.labels.languageCode %></label>
  <asp:TextBox runat="server" ID="txtDublinCoreLanguage" MaxLength="5" Width="50" /><br />
  
  <br />
  
  <label for="<%=txtGeocodingLatitude.ClientID %>"><%=Resources.labels.latitude %></label>
  <asp:TextBox runat="server" ID="txtGeocodingLatitude" Width="300" /><br />
  <label for="<%=txtGeocodingLongitude.ClientID %>"><%=Resources.labels.longtitude %></label>
  <asp:TextBox runat="server" ID="txtGeocodingLongitude" Width="300" /><br />
  
  <br />
  
  <label for="<%=txtBlogChannelBLink.ClientID %>"><%=Resources.labels.endorsment %></label>
  <asp:TextBox runat="server" ID="txtBlogChannelBLink" MaxLength="255" Width="400" /><br />
  
  <label for="<%=txtBlogChannelBLink.ClientID %>">Feedburner <%=Resources.labels.userName.ToLowerInvariant() %></label>
  <asp:TextBox runat="server" ID="txtFeedburnerUserName" MaxLength="255" Width="120" />
  
</div>

<div class="settings">

  <h1>Tracking script</h1>
  <label for="<%=txtTrackingScript.ClientID %>">Visitor tracking script<br /><br />The JavaScript code from i.e. Google Analytics.<br /><br />Will be added in the bottom of each page regardless of the theme.<br /><br />(remember to add the &lt;script&gt; tags)</label>
  <asp:TextBox runat="server" ID="txtTrackingScript" TextMode="multiLine" Rows="9" Columns="30" Width="500" />

</div>

<div class="settings">

  <h1><%=Resources.labels.htmlHeadSection %></h1>
  <label for="<%=txtHtmlHeader.ClientID %>"><%=Resources.labels.addCustomCodeToHeader %></label>
  <asp:TextBox runat="server" ID="txtHtmlHeader" TextMode="multiLine" Rows="9" Columns="30" Width="500" />

</div>

<div align="right"><asp:Button runat="server" ID="btnSave" /></div>
<br />

</asp:Content>



<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="rightAdmin">

<div class="settings">

  <h1>Import (beta)</h1>
  <p>This is an experimental import tool for importing posts, comments, files and images from RSS feeds.<br /><br />
     Nothing bad will happen if you try the beta tool. It import posts and files and add them to the App_Data folder.<br />
     If you don't like the result, just delete the posts and files from App_Data/posts and App_Data/files.<br /><br />
     <strong>NB:</strong> The tool has only been tested with dasBlog. If you use any other blog software, you should be aware
     when you import comments or download files automatically.
  </p>
  <p>
    <a href="http://www.madskristensen.dk/clickonce/blogconverter/BlogConverter.application?url=<%=Utils.AbsoluteWebRoot %>&username=<%=System.Threading.Thread.CurrentPrincipal.Identity.Name %>">Launch import tool</a>
  </p>
</div>

</asp:Content>

