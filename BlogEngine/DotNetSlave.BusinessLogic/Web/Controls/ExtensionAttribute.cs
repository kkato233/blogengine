using System;

namespace BlogEngine.Core.Web
{
  /// <summary>
  /// All extensions must decorate the class with this attribute.
  /// It is used for reflection.
  /// <remarks>
  /// When using this attribute, you must make sure
  /// to have a default constructor. It will be used to create
  /// an instance of the extension through reflection.
  /// </remarks>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ExtensionAttribute : System.Attribute
  {
    /// <summary>
    /// Creates an instance of the attribute and assigns a description.
    /// </summary>
    public ExtensionAttribute(string description, string version, string author)
    {
      _Description = description;
      _Version = version;
      _Author = author;
    }

    private string _Description;
    /// <summary>
    /// Gets the description of the extension.
    /// </summary>
    public string Description
    {
      get { return _Description; }
    }

    private string _Version;

    /// <summary>
    /// 
    /// </summary>
    public string Version
    {
      get { return _Version; }
    }

    private string _Author;

    /// <summary>
    /// 
    /// </summary>
    public string Author
    {
      get { return _Author; }
    }

  }

}