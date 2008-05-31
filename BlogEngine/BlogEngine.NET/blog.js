﻿function $(id)
{
  return document.getElementById(id);
}

function SetFlag(iso)
{  
  if (iso.length > 0)
    flagImage.src = KEYwebRoot + "pics/flags/" + iso + ".png";
  else
    flagImage.src = KEYwebRoot + "pics/pixel.gif";
}

// Searches the blog based on the entered text and
// searches comments as well if chosen.
function Search(root)
{
  var input = $("searchfield");
  var check = $("searchcomments");
  
  var search = "search.aspx?q=" + encodeURIComponent(input.value);
  if (check != null && check.checked)
    search += "&comment=true";
  
  top.location.href = root + search;
  
  return false;
}

// Clears the search fields on focus.
function SearchClear(defaultText)
{
  var input = $("searchfield");
  if (input.value == defaultText)
    input.value = "";
  else if (input.value == "")
    input.value = defaultText;
}

function Rate(id, rating)
{
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
  else if (status == "HASRATED")
  {
    alert(KEYhasRated);
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
	    if (http.responseText.length > 0 && callback != null)
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
  var cal = $('calendarContainer');
  cal.innerHTML = args;
  months[context] = args;
}

function ToggleMonth(year)
{
  var monthList = $("monthList");
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

// Adds a trim method to all strings.
function Equal(first, second) 
{
  var f = first.toLowerCase().replace(new RegExp(' ','gi'), '');
  var s = second.toLowerCase().replace(new RegExp(' ','gi'), '');
	return f == s;
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
  var content = $('content');
  if (content == null)
    return;
    
  var links = content.getElementsByTagName('a')
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