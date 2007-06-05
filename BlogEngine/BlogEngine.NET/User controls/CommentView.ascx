<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommentView.ascx.cs" Inherits="User_controls_CommentView" %>
<%@ Import Namespace="BlogEngine.Core" %>

<% if (Post.Comments.Count > 0){ %>
<h1 id="comment"><%=Resources.labels.comments %></h1>
<%} %>

<div id="commentlist">
  <asp:PlaceHolder runat="server" ID="phComments" />
</div>

<asp:PlaceHolder runat="Server" ID="phAddComment">

<img src="<%=Utils.RelativeWebRoot %>pics/ajax-loader.gif" alt="Saving the comment" style="display:none" id="ajaxLoader" />  
<span id="status"></span>

<div class="commentForm">
  <h1 id="addcomment"><%=Resources.labels.addComment %></h1>

  <label for="<%=txtName.ClientID %>"><%=Resources.labels.name %>*</label>
  <asp:TextBox runat="Server" ID="txtName" TabIndex="1" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Required" Display="dynamic" /><br />

  <label for="<%=txtEmail.ClientID %>"><%=Resources.labels.email %>*</label>
  <asp:TextBox runat="Server" ID="txtEmail" TabIndex="2" />
  (<%=string.Format(Resources.labels.willShowGravatar, "<a href=\"http://www.gravatar.com\" target=\"_blank\">Gravatar</a>") %>)
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Required" Display="dynamic" />
  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" /><br />

  <label for="<%=txtWebsite.ClientID %>"><%=Resources.labels.website %></label>
  <asp:TextBox runat="Server" ID="txtWebsite" TabIndex="3" />
  <asp:RegularExpressionValidator runat="Server" ControlToValidate="txtWebsite" ValidationExpression="(http://|https://|)([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" ErrorMessage="Please enter a valid URL" Display="Dynamic" /><br />
  
  <% if(BlogSettings.Instance.EnableCountryInComments){ %>
  <label for="<%=ddlCountry.ClientID %>"><%=Resources.labels.country %></label>
  <asp:DropDownList runat="server" ID="ddlCountry" onchange="SetFlag(this.value)" TabIndex="4" />&nbsp;
  <asp:Image runat="server" ImageUrl="~/pics/pixel.gif" ID="imgFlag" AlternateText="Country flag" Width="16" Height="11" /><br /><br />
  <%} %>

  <label for="<%=txtContent.ClientID %>">Comment*</label>
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContent" ErrorMessage="Required" Display="dynamic" /><br />
  <asp:TextBox runat="server" ID="txtContent" TextMode="multiLine" Columns="50" Rows="10" TabIndex="5" onkeyup="ShowCommentPreview('livepreview', this)" /><br /><br />  
 
  <input type="button" id="btnSave" value="<%=Resources.labels.saveComment %>" onclick="if(Page_ClientValidate()){AddComment()}" tabindex="6" />  
  <asp:Button runat="server" ID="btnSave" style="display:none" Text="<%$Resources:labels, addComment %>" UseSubmitBehavior="false" TabIndex="6" />
  
  <% if (BlogSettings.Instance.ShowLivePreview) { %>  
  <h2>Live preview</h2> 
  <div id="livepreview">
    <asp:PlaceHolder runat="Server" ID="phLivePreview" />
  </div>
  <%} %>
</div>

<script type="text/javascript">
<!--//
var isAjaxSupported = (window.ActiveXObject != "undefined" || window.XMLHttpRequest != "undefined");
if (!isAjaxSupported)
{
  document.getElementById('<%=btnSave.ClientID %>').style.display = "inline";
  document.getElementById('btnSave').style.display = "none";
}

function AddComment()
{
  document.getElementById("btnSave").disabled = true;
  document.getElementById("ajaxLoader").style.display = "inline";
  document.getElementById("status").className = "";
  document.getElementById("status").innerHTML = "Saving the comment...";
  
  var author = document.getElementById("<%=txtName.ClientID %>").value;
  var email = document.getElementById("<%=txtEmail.ClientID %>").value;
  var website = document.getElementById("<%=txtWebsite.ClientID %>").value;
  var country = "";
  if (document.getElementById("<%=ddlCountry.ClientID %>"))
    country = document.getElementById("<%=ddlCountry.ClientID %>").value;
  var content = document.getElementById("<%=txtContent.ClientID %>").value;
  var argument = author + "¤" + email + "¤" + website + "¤" + country + "¤" + content;
  <%=Page.ClientScript.GetCallbackEventReference(this, "argument", "AppendComment", "'comment'") %>
  
  if (typeof OnComment != "undefined")
    OnComment(author, email, website, country, content);
}

function AppendComment(args, context)
{
  if (context == "comment")
  {
    if (document.getElementById("commentlist").innerHTML == "")
      document.getElementById("commentlist").innerHTML = "<h1 id='comment'>Comments</h1>"
    document.getElementById("commentlist").innerHTML += args;
    document.getElementById("<%=txtContent.ClientID %>").value = "";
    document.getElementById("ajaxLoader").style.display = "none";
    document.getElementById("status").className = "success";
    document.getElementById("status").innerHTML = "The comment was saved. Thank you for the feedback";
  }
  
  document.getElementById("btnSave").disabled = false;
}

var flagImage = document.getElementById("<%= imgFlag.ClientID %>");

function SetFlag(iso)
{  
  if (iso.length > 0)
    flagImage.src = "<%=VirtualPathUtility.ToAbsolute("~/") %>pics/flags/" + iso + ".png";
  else
    flagImage.src = "<%=VirtualPathUtility.ToAbsolute("~/") %>pics/pixel.gif";
}
//-->
</script>

<% if (BlogSettings.Instance.IsCoCommentEnabled){ %>
<script type="text/javascript">
// this ensures coComment gets the correct values
coco =
{
     tool          : "BlogEngine",
     siteurl       : "<%=Utils.AbsoluteWebRoot %>",
     sitetitle     : "<%=BlogSettings.Instance.Name %>",
     pageurl       : "<%=Request.Url %>",
     pagetitle     : "<%=this.Post.Title %>",
     author        : "<%=this.Post.Title %>",
     formID        : "<%=Page.Form.ClientID %>",
     textareaID    : "<%=txtContent.UniqueID %>",
     buttonID      : "<%=btnSave.ClientID %>"
}
</script>
<script id="cocomment-fetchlet" src="http://www.cocomment.com/js/enabler.js" type="text/javascript">
// this activates coComment
</script>
<%} %>
</asp:PlaceHolder>

<asp:label runat="server" id="lbCommentsDisabled" visible="false">Comments are closed</asp:label>