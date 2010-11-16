/* 
Author: John Dyer (http://johndyer.name/)
Player: MediaElementJS (http://mediaelementjs.com/)
*/

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Page=System.Web.UI.Page;

/// <summary>
/// Adds a HTML5 video/audio player to posts with Flash/Silverlight fallbacks
/// </summary>
[Extension("HTML5 Video/Audio Player", "1.0", @"<a href=""http://johndyer.me/"">John Dyer</a>")]
public class MediaElementPlayer
{

    #region Private members
    private const string _extensionName = "MediaElementPlayer";
    static protected ExtensionSettings _settings;
    private static Page _page;
    #endregion

    private const int _widthDefault = 480;
    private const int _heightDefault = 360;
	private const bool _enableAutoSize = false;
    private const string _folderDefault = "media";

    /// <summary>
    /// Adds a Flash video player to the post.
    /// </summary>
    public MediaElementPlayer()
    {
        Post.Serving += Post_Serving;
         InitSettings();
    }
    
    private void InitSettings() {

        ExtensionSettings initialSettings = new ExtensionSettings(GetType().Name);
        initialSettings.Help = @"
<p>Build on <a href=""http://mediaelement.js.com/"">MediaElement.js</a>, the HTML5 video/audio player.</p>

<ol>
	<li>Upload media files to your /media/ folder</li>
	<li>Add short code to your media: [video src=""myfile.mp4""] for video and [audio src=""myfile.mp3""] for audio</li>
	<li>Customize with the following parameters:
		<ul>
			<li><b>width</b>: The exact width of the video</li>
			<li><b>height</b>: The exact height of the video</li>
			<li><b>autoplay</b>: Plays the video as soon as the webpage loads</li>
		</ul>
	</li>
	<li>You can also specify multiple file formats and codecs 
		<ul>
			<li><b>mp4</b>: H.264 encoded MP4 file</li>
			<li><b>webm</b>: VP8/WebM encoded file</li>
			<li><b>ogg</b>: Theora/Vorbis encoded file</li>
		</ul>
	</li>
</ol>

<p>A complete example:<br />
[code mp4=""myfile.mp4"" webm=""myfile.webm"" ogg=""myfile.ogg"" width=""480"" height=""360""]
</p>

<p>Supported formats</p>
<ul>
	<li><b>MP4/MP3</b>: Native HTML5 for IE9, Safari, Chrome; Flash in IE8, Firefox, Opera</li>
	<li><b>WebM</b>: HTML5 for IE9, Chrome, Firefox, Opera; Flash in IE8 (coming in Flash 11)</li>
	<li><b>FLV</b>: Flash fallback</li>
	<li><b>WMV/WMA</b>: Silverlight fallback</li>
</ul>
";
        initialSettings.IsScalar = true;

		
        initialSettings.AddParameter("width", "Default Width");
        initialSettings.AddValue("width", _widthDefault.ToString());

        initialSettings.AddParameter("height", "Default Height");
        initialSettings.AddValue("height", _heightDefault.ToString());	

        initialSettings.AddParameter("folder", "Folder for Media (MP4, MP3, WMV, Ogg, WebM, etc.)");
        initialSettings.AddValue("folder", _folderDefault);     
        
        _settings = ExtensionManager.InitSettings(_extensionName, initialSettings);        
    }

    private void Post_Serving(object sender, ServingEventArgs e)
    {
        if (e.Location == ServingLocation.PostList  || e.Location == ServingLocation.SinglePost || e.Location == ServingLocation.Feed) {
	
			HttpContext context = HttpContext.Current;
			_page = (Page)context.CurrentHandler;
	
			string regex = @"(video|audio)";
			List<ShortCode> shortCodes = GetShortCodes(e.Body, regex);
	
			if (shortCodes.Count == 0)
				return;
				
			AddHeader();
			ProcessMediaTags(e, shortCodes);
			AddFooter();
			
		}
	}
	
	private void AddHeader() {
		string path = Utils.RelativeWebRoot + "Scripts/mediaelement/";
		
		AddJavaScript(path + "mediaelement.js");
		AddJavaScript(path + "mediaelementplayer.js");
		AddStylesheet(path + "mediaelementplayer.min.css");
    }
    
    private void AddJavaScript(string src)
    {
        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes["type"] = "text/javascript";
        script.Attributes["src"] = src;
        _page.Header.Controls.Add(script);
    }

    private void AddStylesheet(string href)
    {
        HtmlLink css = new HtmlLink();
        css.Attributes["type"] = "text/css";
        css.Attributes["rel"] = "stylesheet";
        css.Attributes["href"] = href;
        _page.Header.Controls.Add(css);
    }    

	private void AddFooter() {

		int width = 0;
		int height = 0;		

		if (!Int32.TryParse(_settings.GetSingleValue("width"), out width))
			width = _widthDefault;
		if (!Int32.TryParse(_settings.GetSingleValue("height"), out height))
			height = _heightDefault;      

		string startupScript = @"
<script type=""text/javascript"">
jQuery(document).ready(function($) {
	$('audio.mep, video.mep').mediaelementplayer({defaultVideoWidth:" + width.ToString() + ", defaultVideoHeight:" + height.ToString() + @"});
});
</script>	
";	
		_page.ClientScript.RegisterStartupScript(_page.GetType(), _extensionName, startupScript, false);
	}

	
	private void ProcessMediaTags(ServingEventArgs e, List<ShortCode> shortCodes) {
	
		// path to media
		string folder = _settings.GetSingleValue("folder");			
		string path = Utils.RelativeWebRoot + folder.TrimEnd(new char[] {'/'}) + "/";
		
		// override for feed
		if (e.Location == ServingLocation.Feed) {
			path = Utils.AbsoluteWebRoot + folder.TrimEnd(new char[] { '/' }) + "/";			
		}
					
		// do replacement for media
		foreach (ShortCode sc in shortCodes)
		{
			string tagName = sc.TagName;
			string src = sc.GetAttributeValue("src", "");
			string w = sc.GetAttributeValue("width", "");
			string h = sc.GetAttributeValue("height", "");

			string mp4 = sc.GetAttributeValue("mp4", "");
			string mp3 = sc.GetAttributeValue("mp3", "");
			string webm = sc.GetAttributeValue("webm", "");
			string ogg = sc.GetAttributeValue("ogg", "");
			
			string poster = sc.GetAttributeValue("poster", "");
			string autoplay = sc.GetAttributeValue("autoplay", "");
			string preload = sc.GetAttributeValue("preload", "");

			string code =
				"<" + tagName + " class=\"mep\" controls=\"controls\"" + ((src != "") ? " src=\"" + path + src + "\"" : "") + ((poster != "") ? " poster=\"" + path + poster + "\"" : "") + ((w != "") ? " width=\"" + w + "\"" : "") + ((h != "") ? " height=\"" + h + "\"" : "") + ((autoplay != "") ? " autoplay=\"autoplay\"" : "") + ((preload != "") ? " preload=\"" + preload + "\"" : "") + ">" +
					((mp4 != "") ? "<source src=\"" + path + mp4 + "\" type=\"video/mp4\" />" : "") +
					((mp3 != "") ? "<source src=\"" + path + mp3 + "\" type=\"audio/mp3\" />" : "") +
					((webm != "") ? "<source src=\"" + path + webm + "\" type=\"video/webm\" />" : "") +
					((ogg != "") ? "<source src=\"" + path + ogg + "\" type=\"video/ogg\" />" : "") + 
				
				"</" + tagName + ">";		
				
			e.Body = e.Body.Replace(sc.Text, code);
			
		}	
	}		
	
	
	
	public List<ShortCode> GetShortCodes(string input) {
		return GetShortCodes(input, @"\w+");
	}
	public List<ShortCode> GetShortCodes(string input, string regexMatchString) {

		List<ShortCode> shortCodes = new List<ShortCode>();

		// get the main tag [tag attr="value"]
		MatchCollection matches = Regex.Matches(input, @"\[(?<tag>" + regexMatchString + @")(?<attrs>[^\]]+)\]");
		
		foreach (Match match in matches) {
			
			string tagName = match.Groups["tag"].Value;
			string attributes = match.Groups["attrs"].Value;

			ShortCode sc = new ShortCode(tagName, match.Value);

			// parse the attributes
			// attr="value"
			MatchCollection attrMatches = Regex.Matches(attributes, @"(?<attr>\w+)=""(?<val>[^""]+)""");			
			
			foreach (Match attrMatch in attrMatches) {
				sc.Attributes.Add(attrMatch.Groups["attr"].Value, attrMatch.Groups["val"].Value);
			}

			shortCodes.Add(sc);
		}
		

		return shortCodes;
	}

	public class ShortCode {

		public ShortCode(): this("","") {
		}
		
		public ShortCode(string tagName, string text) {
			_tagName = tagName;
			_text = text;
			_attributes = new Dictionary<string, string>();
		}
		
		private string _tagName;
		private string _text;
		private Dictionary<string, string> _attributes;

		public string TagName { get { return _tagName; } set { _tagName = value; } }
		public string Text { get { return _text; } set { _text = value; } }
		public Dictionary<string, string> Attributes { get { return _attributes; } set { _attributes = value; } }

		public string GetAttributeValue(string attributeName, string defaultValue) {
			if (Attributes.ContainsKey(attributeName))
				return Attributes[attributeName];
			else
				return defaultValue;
		}
	}
	
	
	
}