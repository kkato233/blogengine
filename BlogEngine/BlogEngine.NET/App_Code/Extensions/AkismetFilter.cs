using System;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;

/// <summary>
/// Summary description for AkismetFilter
/// </summary>
[Extension("Aismet anti-spam comment filter", "1.0", "<a href=\"http://dotnetblogengine.net\">BlogEngine.NET</a>")]
public class AkismetFilter : ICustomFilter
{
    public AkismetFilter()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool Initialize(string site, string key)
    {
        return true;
    }

    public bool Check(Comment comment)
    {
        return false;
    }

    public void Report(Comment comment, bool isSpam)
    {
        
    }
}
