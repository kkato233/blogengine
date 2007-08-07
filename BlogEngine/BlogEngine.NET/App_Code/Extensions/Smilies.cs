#region using

using System;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using BlogEngine.Core;
using BlogEngine.Core.Web;

#endregion

/*
Plugin Name: Smilies
Plugin URI: http://cake.happytocode.com
Description: Changes smilies from text to image inside a comment post.
Version: 0.0.0.1
Author: John Knipper
Author URI: http://www.happytocode.com
*/

/// <summary>
/// Changes smilies from text to image inside a comment post.
/// </summary>
[Extension("Changes smilies from text to image inside a comment post.", "0.0.0.1", "www.happytocode.com")]
public class Smilies
{
  public Smilies()
  {
    Post.CommentServing += new EventHandler<ServingEventArgs>(Post_CommentServing);
  }

  void Post_CommentServing(object sender, ServingEventArgs e)
  {
    /*  It does support only the few smilies included by default in the FCKEditor.
        cool  (H)
        cry :'(
        embarassed  :$
        foot-in-mouth :|
        frown :(
        innocent  (A)
        kiss  (K)
        laughing  :D
        money-mouth ($)
        sealed  :-#
        smile :)
        surprised :-O
        tongue-out  :P
        undecided *-)
        wink ;)
        yell  8o|
     */
    if (!string.IsNullOrEmpty(e.Body))
    {
      //Characters other than . $ ^ { [ ( | ) * + ? \ match themselves.
      e.Body = Regex.Replace(e.Body, @"\(H\)", Smiley("cool", "Cool"));
      e.Body = Regex.Replace(e.Body, @":'\(", Smiley("cry", "Cry"));
      e.Body = Regex.Replace(e.Body, @":\$", Smiley("embarassed", "Embarassed"));
      e.Body = Regex.Replace(e.Body, @":\|", Smiley("foot-in-mouth", "Foot"));
      e.Body = Regex.Replace(e.Body, @":\(", Smiley("frown", "Frown"));
      e.Body = Regex.Replace(e.Body, @"\(A\)", Smiley("innocent", "Innocent"));
      e.Body = Regex.Replace(e.Body, @"\(K\)", Smiley("kiss", "Kiss"));
      e.Body = Regex.Replace(e.Body, @":D", Smiley("laughing", "Laughing"));
      e.Body = Regex.Replace(e.Body, @"\(\$\)", Smiley("money-mouth", "Money"));
      e.Body = Regex.Replace(e.Body, @":-#", Smiley("sealed", "Sealed"));
      e.Body = Regex.Replace(e.Body, @":\)", Smiley("smile", "Smile"));
      e.Body = Regex.Replace(e.Body, @":-O", Smiley("surprised", "Surprised"));
      e.Body = Regex.Replace(e.Body, @":P", Smiley("tongue-out", "Tong"));
      e.Body = Regex.Replace(e.Body, @"\*-\)", Smiley("undecided", "Undecided"));
      e.Body = Regex.Replace(e.Body, @";\)", Smiley("wink", "Wink"));
      e.Body = Regex.Replace(e.Body, @"8o\|", Smiley("yell", "Yell"));
    }
  }

  /// <summary>
  /// Does translate a smiley file name to the corresponding HTML.
  /// </summary>
  string Smiley(string name, string alt)
  {
    string smiley = string.Empty;

    if (!string.IsNullOrEmpty(name))
    {
      string path =
          HttpContext.Current.Server.MapPath(string.Format("~/admin/tiny_mce/plugins/emotions/images/smiley-{0}.gif", name));
      if (File.Exists(path))
      {
        smiley = string.Format("<img src=\"{0}admin/tiny_mce/plugins/emotions/images/smiley-{1}.gif\" class=\"flag\" alt=\"{2}\" />", Utils.RelativeWebRoot, name, alt);
      }
    }
    return smiley;
  }
}