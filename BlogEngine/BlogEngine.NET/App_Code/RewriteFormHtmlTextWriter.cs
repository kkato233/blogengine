using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// The RewriteFormHtmlTextWriter class implements Form action tag rewriting for rewritten pages 
/// on Mono.
/// </summary>
public class RewriteFormHtmlTextWriter : HtmlTextWriter
{

    public RewriteFormHtmlTextWriter(Html32TextWriter writer)
        : base(writer)
    {
        this.InnerWriter = writer.InnerWriter;
    }

    public RewriteFormHtmlTextWriter(System.IO.TextWriter writer)
        : base(writer)
    {
        this.InnerWriter = writer;
    }

    public override void WriteAttribute(string name, string value, bool fEncode)
    {
        // Mono has issues identifying relative paths when the url is rewritten,
        // so we need to place the full path in the form tag's action attribute
        // or postbacks won't work in rewritten pages.
        if (BlogEngine.Core.Utils.IsMono)
        {
            if (name == "action")
            {
                if (HttpContext.Current.Items["ActionAlreadyWritten"] == null)
                {
                    value = BlogEngine.Core.Utils.AbsoluteWebRoot + value;
                    HttpContext.Current.Items["ActionAlreadyWritten"] = true;
                }
            }
        }
        base.WriteAttribute(name, value, fEncode);
    }

}

