#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Text;
using DotNetSlave.BlogEngine.BusinessLogic;

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

    #region Properties

    private bool _EnableCommentSearch;
    /// <summary>
    /// Gets or sets whether or not to display the checkbox.
    /// </summary>
    public bool EnableCommentSearch
    {
      get { return _EnableCommentSearch; }
      set { _EnableCommentSearch = value; }
    }

    private string _DefaultText;
    /// <summary>
    /// Gets or sets the text displayed in the textbox.
    /// </summary>
    public string DefaultText
    {
      get { return _DefaultText; }
      set { _DefaultText = value; }
    }

    private string _ButtonText;
    /// <summary>
    /// Gets or sets the text of the search button.
    /// </summary>
    public string ButtonText
    {
      get { return _ButtonText; }
      set { _ButtonText = value; }
    }

    private string _CommentLabeltText;
    /// <summary>
    /// Gets or sets the text of the label next to the checkbox.
    /// </summary>
    public string CommentLabeltText
    {
      get { return _CommentLabeltText; }
      set { _CommentLabeltText = value; }
    }

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
          sb.AppendFormat("<input type=\"text\" value=\"{0}\" id=\"searchfield\" onfocus=\"SearchClear('{1}')\" onblur=\"SearchClear('{1}')\" />", Context.Request.QueryString["q"] ?? DefaultText, DefaultText);
          sb.AppendFormat("<input type=\"submit\" value=\"{0}\" id=\"searchbutton\" onkeypress=\"this.onclick\" onclick=\"Search('{1}');\" />", ButtonText, Utils.RelativeWebRoot);

          if (EnableCommentSearch)
          {
            string check = Context.Request.QueryString["comment"] != null ? "checked=\"checked\"" : string.Empty;
            sb.AppendFormat("<br /><input type=\"checkbox\" id=\"searchcomments\" {0} />", check);
            if (!string.IsNullOrEmpty(CommentLabeltText))
              sb.AppendFormat("<label for=\"searchcomments\">{0}</label>", CommentLabeltText);
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