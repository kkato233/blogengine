<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="admin_Pages_configuration" Title="Settings" %>
<%@ Import Namespace="BlogEngine.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
<br />

<div style="text-align:right">
  <asp:Button runat="server" ID="btnSaveTop" Text="Save settings" />
</div><br />

<div class="settings">

  <h1>Basic settings</h1>
  <label for="<%=txtName.ClientID %>">Name</label>
  <asp:TextBox runat="server" ID="txtName" Width="300" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="Required" /><br />

  <label for="<%=txtDescription.ClientID %>">Description</label>
  <asp:TextBox runat="server" ID="txtDescription" Width="300" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" ErrorMessage="Required" /><br />
    
  <label for="<%=txtPostsPerPage.ClientID %>">Posts per page</label>
  <asp:TextBox runat="server" ID="txtPostsPerPage" Width="50" MaxLength="4" />
  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPostsPerPage" ErrorMessage="Required" />
  <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtPostsPerPage" Operator="DataTypeCheck" Type="integer" ErrorMessage="Please enter a valid number" /><br />
  
  <label for="<%=ddlTheme.ClientID %>">Theme</label>
  <asp:DropDownList runat="server" ID="ddlTheme" /><br />
    
  <label for="<%=cbShowRelatedPosts.ClientID %>">Show related posts</label>
  <asp:CheckBox runat="server" ID="cbShowRelatedPosts" /> 
  
</div>

<div class="settings">

  <h1>Advanced settings</h1>
  <label for="<%=cbEnableCompression.ClientID %>">Enable HTTP compression</label>
  <asp:CheckBox runat="server" ID="cbEnableCompression" /> Make the pages load faster (recommended).<br />
  
  <label for="<%=cbRemoveWhitespaceInStyleSheets.ClientID %>">Trim stylesheets</label>
  <asp:CheckBox runat="server" ID="cbRemoveWhitespaceInStyleSheets" /> Makes all stylesheets of any theme smaller by removing all whitespace at runtime.<br />
  
  <label for="<%=cbEnableOpenSearch.ClientID %>">Enable <a href="http://www.opensearch.org/Home" target="_blank">OpenSearch</a></label>
  <asp:CheckBox runat="server" ID="cbEnableOpenSearch" /> Adds the search feature to all new browsers (recommended).<br />
  
  <label for="<%=cbEnableSearchHighlight.ClientID %>">Enable search hightlight</label>
  <asp:CheckBox runat="server" ID="cbEnableSearchHighlight" /> Colors the search words on the page.<br />
  
</div>

<div class="settings">

  <h1>Comments</h1>
  <label for="<%=cbEnableComments.ClientID %>">Enable comments</label>
  <asp:CheckBox runat="server" ID="cbEnableComments" /> If comments aren't enabled, nobody can write comments to any post.<br />
  
  <label for="<%=cbEnableCountryInComments.ClientID %>">Show country chooser</label>
  <asp:CheckBox runat="server" ID="cbEnableCountryInComments" /> If the country chooser isn't shown, no flag can be shown on each comment.<br />
  
  <label for="<%=cbEnableCoComment.ClientID %>">Enable <a href="http://www.cocomment.com/" target="_blank">coComment</a></label>
  <asp:CheckBox runat="server" ID="cbEnableCoComment" /><br />
  
  <label for="<%=cbShowLivePreview.ClientID %>">Show live preview</label>
  <asp:CheckBox runat="server" ID="cbShowLivePreview" /><br />
  
  <label for="<%=ddlCloseComments.ClientID %>" style="position:relative;top:4px">Close comments after</label>
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

  <h1>Email</h1>
  <label for="<%=txtEmail.ClientID %>">Email address</label>
  <asp:TextBox runat="server" ID="txtEmail" Width="300" /><br />
  
  <label for="<%=txtSmtpServer.ClientID %>">SMTP server</label>
  <asp:TextBox runat="server" ID="txtSmtpServer" Width="300" /><br />
  
  <label for="<%=txtSmtpServerPort.ClientID %>">Port number</label>
  <asp:TextBox runat="server" ID="txtSmtpServerPort" Width="35" /> Port 25 is the standard
  <asp:CompareValidator ID="CompareValidator2" runat="Server" ControlToValidate="txtSmtpServerPort" Operator="datatypecheck" Type="integer" ErrorMessage="Not a valid number" /><br />
  
  <label for="<%=txtSmtpUsername.ClientID %>">User name</label>
  <asp:TextBox runat="server" ID="txtSmtpUsername" Width="300" /><br />
  
  <label for="<%=txtSmtpPassword.ClientID %>">Password</label>
  <asp:TextBox runat="server" ID="txtSmtpPassword" Width="300" /><br />
  
  <label for="<%=cbComments.ClientID %>">Send comment mail</label>
  <asp:CheckBox runat="Server" ID="cbComments" /><br /><br />
  
  <asp:Button runat="server" CausesValidation="False" ID="btnTestSmtp" Text="Test mail settings" />
  <asp:Label runat="Server" ID="lbSmtpStatus" />
</div>

<div class="settings">

  <h1>Feed settings</h1>
  
  <label for="<%=ddlSyndicationFormat.ClientID %>" style="position:relative;top:4px">Default feed output</label>
  <asp:DropDownList runat="server" ID="ddlSyndicationFormat">
    <asp:ListItem Text="RSS 2.0" Value="Rss" Selected="True" />
    <asp:ListItem Text="Atom 1.0" Value="Atom" />
  </asp:DropDownList> format.<br />
  
  <br />
  
  <label for="<%=txtDublinCoreCreator.ClientID %>">Author name</label>
  <asp:TextBox runat="server" ID="txtDublinCoreCreator" Width="300" /><br />
  
  <label for="<%=txtDublinCoreLanguage.ClientID %>">Language code</label>
  <asp:TextBox runat="server" ID="txtDublinCoreLanguage" MaxLength="5" Width="50" /><br />
  
  <br />
  
  <label for="<%=txtGeocodingLatitude.ClientID %>">Latitude</label>
  <asp:TextBox runat="server" ID="txtGeocodingLatitude" Width="300" /><br />
  <label for="<%=txtGeocodingLongitude.ClientID %>">Longitude</label>
  <asp:TextBox runat="server" ID="txtGeocodingLongitude" Width="300" /><br />
  
  <br />
  
  <label for="<%=txtBlogChannelBLink.ClientID %>">Endorsement (bLink)</label>
  <asp:TextBox runat="server" ID="txtBlogChannelBLink" MaxLength="255" Width="400" /><br />
  
</div>

<div class="settings">

  <h1>Tracking script</h1>
  <label for="<%=txtTrackingScript.ClientID %>">Visitor tracking script<br /><br />The JavaScript code from i.e. Google Analytics.<br /><br />Will be added in the bottom of each page regardless of the theme.<br /><br />(remember to add the &lt;script&gt; tags)</label>
  <asp:TextBox runat="server" ID="txtTrackingScript" TextMode="multiLine" Rows="9" Columns="30" Width="500" />

</div>

<div align="right"><asp:Button runat="server" ID="btnSave" Text="Save settings" /></div>
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

