#region Using

using System;
using System.Web;
using System.Web.UI;
using System.IO;

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
  public class Script : Control
  {
    const string TAG = "<script type=\"text/javascript\" src=\"{0}\"></script>";

    private string _path;
    /// <summary>
    /// The relative path to the JavaScript file (.js).
    /// </summary>
    public string Path
    {
      get { return _path; }
      set { _path = value; }
    }

    /// <summary>
    /// Renders the control as a script tag.
    /// </summary>
    public override void RenderControl(HtmlTextWriter writer)
    {
      writer.Write(string.Format(TAG, Page.ResolveUrl(Path)));
      writer.Write(Environment.NewLine);
    }
  }
}