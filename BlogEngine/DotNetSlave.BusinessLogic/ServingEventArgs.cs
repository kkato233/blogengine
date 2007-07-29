using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Core
{
  /// <summary>
  /// Used when a post is served to the output stream.
  /// </summary>
  public class ServingEventArgs : EventArgs
  {

    /// <summary>
    /// Creates a new instance of the class and applies the specified body.
    /// </summary>
    public ServingEventArgs(string body)
    {
      _Body = body;
    }

    private string _Body;
    /// <summary>
    /// Gets or sets the body of the post. If you change the Body, 
    /// then that change will be shown on the web page.
    /// </summary>
    public string Body
    {
      get { return _Body; }
      set { _Body = value; }
    }

  }
}
