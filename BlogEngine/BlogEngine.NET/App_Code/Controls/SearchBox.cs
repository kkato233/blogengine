#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Text;
using BlogEngine.Core;

#endregion

namespace Controls
{
  /// <summary>
  /// Includes a reference to a JavaScript.
  /// <remarks>
  /// This control is needed in order to let the src
  /// attribute of the script tage be relative to the root.
  /// </remarks>
  /// </summary>
  public class SearchBox : Control
  {

    static SearchBox()
    {
      BlogSettings.Changed += delegate { _Html = null; };
      Post.Saved += delegate { _Html = null; };
    }

    #region Properties

    private static string _Html;
    /// <summary>
    /// Gets the HTML to render.
    /// </summary>
    private string Html
    {
      get
      {
        if (_Html == null)
        {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("<div id=\"searchbox\">");
          sb.AppendFormat("<input type=\"text\" value=\"{0}\" id=\"searchfield\" onfocus=\"SearchClear('{1}')\" onblur=\"SearchClear('{1}')\" />", Context.Request.QueryString["q"] ?? BlogSettings.Instance.SearchDefaultText, BlogSettings.Instance.SearchDefaultText);
          sb.AppendFormat("<input type=\"button\" value=\"{0}\" id=\"searchbutton\" onclick=\"Search('{1}');\" />", BlogSettings.Instance.SearchButtonText, Utils.RelativeWebRoot);

          if (BlogSettings.Instance.EnableCommentSearch)
          {
            string check = Context.Request.QueryString["comment"] != null ? "checked=\"checked\"" : string.Empty;
            sb.AppendFormat("<br /><input type=\"checkbox\" id=\"searchcomments\" {0} />", check);
            if (!string.IsNullOrEmpty(BlogSettings.Instance.SearchCommentLabelText))
              sb.AppendFormat("<label for=\"searchcomments\">{0}</label>", BlogSettings.Instance.SearchCommentLabelText);
          }

          sb.AppendLine("</div>");
          _Html = sb.ToString();
        }

        return _Html;
      }
    }

    #endregion

    /// <summary>
    /// Renders the control as a script tag.
    /// </summary>
    public override void RenderControl(HtmlTextWriter writer)
    {
      writer.Write(Html);
    }
  }
}