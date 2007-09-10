var _Regex = new RegExp("\\n","gi");
var _RegexUrl = new RegExp("((http://|www\\.)([A-Z0-9.-]{1,})\\.[0-9A-Z?&#=\\-_\\./]{2,})", "gi");
var _Preview;
var _PreviewAuthor;
var _PreviewContent;
var _TxtName;

// Shows live preview of the comment being entered.
function ShowCommentPreview(target, sender)
{
  if (_Preview == null)
    _Preview = document.getElementById("livepreview");  
    
  if (_Preview == null)
    return;
  
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
  
  var _TxtWebsite = document.getElementById("ctl00_cphBody_CommentView1_txtWebsite");
  if( _TxtWebsite != null && _TxtWebsite.value.length > 0)
  {
    if (_TxtWebsite.value.indexOf("://") == -1)
      _TxtWebsite.value = "http://" + _TxtWebsite.value;
      
    _PreviewAuthor.innerHTML = "<a href=\"" + _TxtWebsite.value + "\">" + _PreviewAuthor.innerHTML + "</a>";
  }
}

function GetElementByClassName(parent, tag, className)
{
  if (parent == null)
    return;
    
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
  
  var search = "search.aspx?q=" + encodeURI(input.value);
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

function Rate(id, rating)
{
  //WebForm_DoCallback('ctl00$cphBody$ctl00', id + rating, RatingCallback, 'rating', null, false) 
  CreateCallback("rating.axd?id=" + id + "&rating=" + rating, RatingCallback);
}

function RatingCallback(response)
{
  var rating = response.substring(0, 1);
  var status = response.substring(1);
  
  if (status == "OK")
  {
    if (typeof OnRating != "undefined")
      OnRating(rating);
    
    alert("You rating has been registered. Thank you!");
  }  
  else
  {
    alert("An error occured while registering your rating. Please try again");
  }    
}

/// <summary>
/// Creates a client callback back to the requesting page
/// and calls the callback method with the response as parameter.
/// </summary>
function CreateCallback(url, callback)
{
  var http = GetHttpObject();
  http.open("GET", url, true);
  
  http.onreadystatechange = function() 
  {
	  if (http.readyState == 4) 
	  {
	    if (http.responseText.length > 0)
        callback(http.responseText);
	  }
  }
  
  http.send(null);
}

/// <summary>
/// Creates a XmlHttpRequest object.
/// </summary>
function GetHttpObject() 
{
    if (typeof XMLHttpRequest != 'undefined')
        return new XMLHttpRequest();
    
    try 
    {
        return new ActiveXObject("Msxml2.XMLHTTP");
    } 
    catch (e) 
    {
        try 
        {
            return new ActiveXObject("Microsoft.XMLHTTP");
        } 
        catch (e) {}
    }
    
    return false;
}

// Updates the calendar from client-callback
function UpdateCalendar(args, context)
{
  var cal = document.getElementById('calendarContainer');
  cal.innerHTML = args;
  months[context] = args;
}

function ToggleMonth(year)
{
  var monthList = document.getElementById("monthList");
  var years = monthList.getElementsByTagName("ul");
  for (i = 0; i < years.length; i++)
  {
    if (years[i].id == year)
    {
      var state = years[i].className == "open" ? "" : "open";
      years[i].className = state;
      break;
    }
  }
}

/*-----------------------------------------------------------------------------
                              XFN HIGHLIGHTER
-----------------------------------------------------------------------------*/

var xfnRelationships = ['friend', 'acquaintance', 'contact', 'met'
						            , 'co-worker', 'colleague', 'co-resident'
						            , 'neighbor', 'child', 'parent', 'sibling'
						            , 'spouse', 'kin', 'muse', 'crush', 'date'
						            , 'sweetheart', 'me']

// Applies the XFN tags of a link to the title tag
function HightLightXfn()
{
  var content = document.getElementById('content');
  if (content == null)
    return;
    
  var links = document.getElementsByTagName('a')
  for (i = 0; i < links.length; i++)
  {
    var link = links[i];
    var rel = link.getAttribute('rel');
    if (rel && rel != "nofollow") 
    {
      for (j = 0; j < xfnRelationships.length; j++)
      {
        if(rel.indexOf(xfnRelationships[j]) > -1)
        {
          link.title = 'XFN relationship: ' + rel;
          break;
        }
      }
    }
  }
}

// addLoadEvent()
// Adds event to window.onload without overwriting currently assigned onload functions.
// Function found at Simon Willison's weblog - http://simon.incutio.com/
function addLoadEvent(func)
{	
	var oldonload = window.onload;
	if (typeof window.onload != 'function')
	{
    	window.onload = func;
	} 
	else 
	{
		window.onload = function()
		{
			oldonload();
			func();
		}
	}
}

addLoadEvent(HightLightXfn);