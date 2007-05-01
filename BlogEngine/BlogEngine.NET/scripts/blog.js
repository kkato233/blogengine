// Sets a class to all external links
function StyleExternalLinks()
{
  var divs = document.getElementsByTagName("div");
  for (i = 0; i < divs.length; i++)
  {
    if (divs[i].className == "text")
    {
      var anchors = divs[i].getElementsByTagName("a");
     
      for (a = 0; a < anchors.length; a++)
      {
        if (anchors[a].href.substring(0,17) != location.href.substring(0,17))
          anchors[a].className += "external";
      }
    }
  }
}

// Opens the caller's default e-mail client
// with the subject filled if specified.
function SafeMail(name, domain, subject)
{
  if (subject != null)
    subject = "?subject=" + subject;
  location.href="mailto:" + name + "@" + domain + subject;
}

var _Regex = new RegExp("\\n","gi");
var _RegexUrl = new RegExp("((http://|www\\.)([A-Z0-9.-]{1,})\\.[0-9A-Z?&=\\-_\\./]{2,})", "gi");
var _Preview;
var _PreviewAuthor;
var _PreviewContent;
var _TxtName;

// Shows live preview of the comment being entered.
function ShowCommentPreview(target, sender)
{
  if (_Preview == null)
    _Preview = document.getElementById("livepreview");  
  
  if (_PreviewAuthor == null)
    _PreviewAuthor = GetElementByClassName(_Preview, "p", "author");
  
  if (_PreviewContent == null)
    _PreviewContent = GetElementByClassName(_Preview, "p", "content");
  
  if (_TxtName == null)
    _TxtName = document.getElementById("ctl00_cphBody_CommentView1_txtName");   
    
  if (!_PreviewAuthor)
    return;
    
  var body = sender.value.replace(_RegexUrl, "<a href=\"http://$1\" rel=\"nofollow\">$1</a>");
    
  _PreviewAuthor.innerHTML = _TxtName.value;
  _PreviewContent.innerHTML = body.replace(_Regex, "<br />");
}

function GetElementByClassName(parent, tag, className)
{
  var elements = parent.getElementsByTagName(tag);
  for (i = 0; i < elements.length; i++)
  {
    if (elements[i].className == className)
      return elements[i];
  }
}

// Searches the blog based on the entered text and
// searches comments as well if chosen.
function Search(root)
{
  var input = document.getElementById("searchfield");
  var check = document.getElementById("searchcomments");
  
  var search = "?q=" + escape(input.value);
  if (check != null && check.checked)
    search += "&comment=true";
  
  top.location.href = root + search;
  
  return false;
}

// Clears the search fields on focus.
function SearchClear(defaultText)
{
  var input = document.getElementById("searchfield");
  if (input.value == defaultText)
    input.value = "";
  else if (input.value == "")
    input.value = defaultText;
}