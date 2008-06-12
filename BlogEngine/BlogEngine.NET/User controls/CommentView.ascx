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
  <asp:TextBox runat="Server" ID="txtName" TabIndex="1" ValidationGroup="AddComment" />
  <asp:CustomValidator runat="server" ControlToValidate="txtName" ErrorMessage=" <%$Resources:labels, chooseOtherName %>" Display="dynamic" ClientValidationFunction="CheckAuthorName" EnableClientScript="true" ValidationGroup="AddComment" />
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" /><br />

  <label for="<%=txtEmail.ClientID %>"><%=Resources.labels.email %>*</label>
  <asp:TextBox runat="Server" ID="txtEmail" TabIndex="2" ValidationGroup="AddComment" />
  <span id="gravatarmsg">
  <%if (BlogSettings.Instance.Avatar != "none" && BlogSettings.Instance.Avatar != "monster"){ %>
  (<%=string.Format(Resources.labels.willShowGravatar, "<a href=\"http://www.gravatar.com\" target=\"_blank\">Gravatar</a>")%>)
  <%} %>
  </span>
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" />
  <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="<%$Resources:labels, enterValidEmail   %>" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AddComment" /><br />

  <label for="<%=txtWebsite.ClientID %>"><%=Resources.labels.website %></label>
  <asp:TextBox runat="Server" ID="txtWebsite" TabIndex="3" ValidationGroup="AddComment" />
  <asp:RegularExpressionValidator runat="Server" ControlToValidate="txtWebsite" ValidationExpression="(http://|https://|)([\w-]+\.)+[\w-]+(/[\w- ./?%&=;~]*)?" ErrorMessage="<%$Resources:labels, enterValidUrl %>" Display="Dynamic" ValidationGroup="AddComment" /><br />
  
  <% if(BlogSettings.Instance.EnableCountryInComments){ %>
  <label for="<%=ddlCountry.ClientID %>"><%=Resources.labels.country %></label>
  <asp:DropDownList runat="server" ID="ddlCountry" onchange="SetFlag(this.value)" TabIndex="4" EnableViewState="false" ValidationGroup="AddComment" />&nbsp;
  <asp:Image runat="server" ID="imgFlag" AlternateText="Country flag" Width="16" Height="11" EnableViewState="false" /><br /><br />
  <%} %>

  <span class="bbcode" title="BBCode tags"><%=BBCodes() %></span>
  <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContent" ErrorMessage="<%$Resources:labels, required %>" Display="dynamic" ValidationGroup="AddComment" /><br />

  <% if (BlogSettings.Instance.ShowLivePreview) { %>  
  <ul id="commentMenu">
    <li id="compose" class="selected" onclick="ToggleCommentMenu(this)"><%=Resources.labels.comment%></li>
    <li id="preview" onclick="ToggleCommentMenu(this)"><%=Resources.labels.livePreview%></li>
  </ul>
  <% } %> 
  <div id="commentCompose">
    <asp:TextBox runat="server" ID="txtContent" TextMode="multiLine" Columns="50" Rows="10" TabIndex="5" ValidationGroup="AddComment" />
  </div>
  <div id="commentPreview">
    <img src="<%=Utils.RelativeWebRoot %>pics/ajax-loader.gif" alt="Loading" />  
  </div>
  
  <br />
  <input type="checkbox" id="cbNotify" style="width: auto" tabindex="6" />
  <label for="cbNotify" style="width:auto;float:none;display:inline"><%=Resources.labels.notifyOnNewComments %></label><br /><br />
 
  <input type="button" id="btnSaveAjax" value="<%=Resources.labels.saveComment %>" onclick="if(Page_ClientValidate('AddComment')){AddComment()}" tabindex="7" />    
  <asp:HiddenField runat="server" ID="hfCaptcha" />
</div>

<script type="text/javascript">
<!--//
function ToggleCommentMenu(element)
{
  element.className = 'selected';
  if (element.id == 'preview')
  {
    $('compose').className = '';    
    $('commentCompose').style.display = 'none';
    $('commentPreview').style.display = 'block';
    $('commentPreview').innerHTML = '<img src="<%=Utils.RelativeWebRoot %>pics/ajax-loader.gif" alt="Loading" />';
    var argument = $('commentPreview').innerHTML;
    AddComment(true);
  }
  else
  {
    $('preview').className = '';    
    $('commentPreview').style.display = 'none';
    $('commentCompose').style.display = 'block';    
  }
}

function EndShowPreview(arg, context)
{
  $('commentPreview').innerHTML = arg;
}

function AddComment(preview)
{
  var isPreview = preview == true;
  if (!isPreview)
  {
    $("btnSaveAjax").disabled = true;
    $("ajaxLoader").style.display = "inline";
    $("status").className = "";
    $("status").innerHTML = "<%=Resources.labels.savingTheComment %>";
  }
  
  var author = $("<%=txtName.ClientID %>").value;
  var email = $("<%=txtEmail.ClientID %>").value;
  var website = $("<%=txtWebsite.ClientID %>").value;
  var country = "";
  if ($("<%=ddlCountry.ClientID %>"))
    country = $("<%=ddlCountry.ClientID %>").value;
  var content = $("<%=txtContent.ClientID %>").value;
  var notify = $("cbNotify").checked;
  var captcha = $('<%=hfCaptcha.ClientID %>').value;
   
  var callback = isPreview ? EndShowPreview : AppendComment;
  var argument = author + "-|-" + email + "-|-" + website + "-|-" + country + "-|-" + content + "-|-" + notify + "-|-" + isPreview + "-|-" + captcha;
  
//  <%=Page.ClientScript.GetCallbackEventReference(this, "argument", "AppendComment", "'comment'") %>
  WebForm_DoCallback('ctl00$cphBody$CommentView1',argument, callback,'comment',null,false);
  
  if (!isPreview && typeof OnComment != "undefined")
    OnComment(author, email, website, country, content);
}

function AppendComment(args, context)
{
  if (context == "comment")
  {
    if ($("commentlist").innerHTML.length < 10)
      $("commentlist").innerHTML = "<h1 id='comment'><%=Resources.labels.comments %></h1>"
      
    $("commentlist").innerHTML += args;
    $("<%=txtContent.ClientID %>").value = "";
    $("ajaxLoader").style.display = "none";
    $("status").className = "success";
    $("status").innerHTML = "<%=Resources.labels.commentWasSaved %>";
  }
  
  $("btnSaveAjax").disabled = false;
}

var flagImage = $("<%= imgFlag.ClientID %>");

function CheckAuthorName(sender, args)
{
  args.IsValid = true;
  
  <% if (!Page.User.Identity.IsAuthenticated){ %>
  var author = "<%=Post.Author %>";
  var visitor = $("<%=txtName.ClientID %>").value;  
  args.IsValid = !Equal(author, visitor);
  <%} %>
}

function AddBbCode(v) {
  if (document.getSelection) // firefox
  {      
    tt = $("<%=txtContent.ClientID %>");
    var pretxt = tt.value.substring(0, tt.selectionStart);
    var therest = tt.value.substr(tt.selectionEnd);
    var sel = tt.value.substring(tt.selectionStart, tt.selectionEnd);
    tt.value = pretxt + "[" + v + "]" + sel + "[/" + v + "]" + therest;
  }
  else // IE
  {
    var str = document.selection.createRange().text;
    $("<%=txtContent.ClientID %>").focus();
    var sel = document.selection.createRange();
    sel.text = "[" + v + "]" + str + "[/" + v + "]";
  }

  ShowCommentPreview('livepreview', $("<%=txtContent.ClientID %>"));
  return;
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
     buttonID      : "btnSaveAjax"
}
</script>
<script id="cocomment-fetchlet" src="http://www.cocomment.com/js/enabler.js" type="text/javascript">
</script>
<%} %>
</asp:PlaceHolder>

<asp:label runat="server" id="lbCommentsDisabled" visible="false"><%=Resources.labels.commentsAreClosed %></asp:label>