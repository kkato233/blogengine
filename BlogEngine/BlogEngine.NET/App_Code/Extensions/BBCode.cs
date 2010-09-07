#region using

using System.Data;

using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;

#endregion

/// <summary>
/// Converts BBCode to XHTML in the comments.
/// </summary>
[Extension("Converts BBCode to XHTML in the comments", "1.0", 
    "<a href=\"http://dotnetblogengine.net\">BlogEngine.NET</a>")]
public class BBCode
{
    #region Constants and Fields

    /// <summary>
    /// The settings.
    /// </summary>
    protected static ExtensionSettings Settings;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes static members of the <see cref="BBCode"/> class.
    /// </summary>
    static BBCode()
    {
        Comment.Serving += PostCommentServing;

        // create settings object. You need to pass exactly your
        // extension class name (case sencitive)
        var settings = new ExtensionSettings("BBCode");

        // -----------------------------------------------------
        // 1. Simple
        // -----------------------------------------------------
        // settings.AddParameter("Code");
        // settings.AddParameter("OpenTag");
        // settings.AddParameter("CloseTag");
        // -----------------------------------------------------
        // 2. Some more options
        // -----------------------------------------------------
        // settings.AddParameter("Code");
        // settings.AddParameter("OpenTag", "Open Tag");
        // settings.AddParameter("CloseTag", "Close Tag");

        //// describe specific rules applied to entering parameters. overrides default wording.
        // settings.Help = "Converts BBCode to XHTML in the comments. Close tag is optional.";
        // -----------------------------------------------------
        // 3. More options including import defaults
        // -----------------------------------------------------
        settings.AddParameter("Code", "Code", 20, true);
        settings.AddParameter("OpenTag", "Open Tag", 150, true);
        settings.AddParameter("CloseTag", "Close Tag");

        // describe specific rules for entering parameters
        settings.Help = "Converts BBCode to XHTML in the comments. Close tag is optional.";

        settings.AddValues(new[] { "b", "strong", string.Empty });
        settings.AddValues(new[] { "i", "em", string.Empty });
        settings.AddValues(new[] { "u", "span style=\"text-decoration:underline\"", "span" });
        settings.AddValues(new[] { "quote", "cite title=\"Quote\"", "cite" });

        // ------------------------------------------------------
        ExtensionManager.ImportSettings(settings);
        Settings = ExtensionManager.GetSettings("BBCode");
    }

    #endregion

    #region Methods

    /// <summary>
    /// Parses the specified body.
    /// </summary>
    /// <param name="body">The body string.</param>
    /// <param name="code">The code string.</param>
    /// <param name="tag">The tag string.</param>
    private static void Parse(ref string body, string code, string tag)
    {
        Parse(ref body, code, tag, tag);
    }

    /// <summary>
    /// Parses the BBCode into XHTML in a safe non-breaking manor.
    /// </summary>
    /// <param name="body">
    /// The body string.
    /// </param>
    /// <param name="code">
    /// The code string.
    /// </param>
    /// <param name="startTag">
    /// The start Tag.
    /// </param>
    /// <param name="endTag">
    /// The end Tag.
    /// </param>
    private static void Parse(ref string body, string code, string startTag, string endTag)
    {
        var start = body.IndexOf(string.Format("[{0}]", code));
        if (start <= -1)
        {
            return;
        }

        if (body.IndexOf(string.Format("[/{0}]", code), start) <= -1)
        {
            return;
        }

        body = body.Remove(start, code.Length + 2);
        body = body.Insert(start, string.Format("<{0}>", startTag));

        var end = body.IndexOf(string.Format("[/{0}]", code), start);

        body = body.Remove(end, code.Length + 3);
        body = body.Insert(end, string.Format("</{0}>", endTag));

        Parse(ref body, code, startTag);
    }

    /// <summary>
    /// Handles the CommentServing event of the Post control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="BlogEngine.Core.ServingEventArgs"/> instance containing the event data.</param>
    private static void PostCommentServing(object sender, ServingEventArgs e)
    {
        var body = e.Body;

        // retrieve parameters back as a data table
        // column = parameter
        var table = Settings.GetDataTable();
        foreach (DataRow row in table.Rows)
        {
            if (string.IsNullOrEmpty((string)row["CloseTag"]))
            {
                Parse(ref body, (string)row["Code"], (string)row["OpenTag"]);
            }
            else
            {
                Parse(ref body, (string)row["Code"], (string)row["OpenTag"], (string)row["CloseTag"]);
            }
        }

        e.Body = body;
    }

    #endregion
}