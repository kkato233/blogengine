using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using BlogEngine.Core;

/// <summary>
/// Summary description for ManagedExtension
/// </summary>

public class ManagedExtension
{
    #region Private members
    string _name = string.Empty;
    string _version = string.Empty;
    string _description = string.Empty;
    bool _enabled = true;
    string _author = string.Empty;
    string _adminPage = string.Empty;
    ExtensionSettings _settings = null;
    #endregion

    #region Constructor
    public ManagedExtension() { }
    public ManagedExtension(string name, string version, string desc, string author)
    {
        _name = name;
        _version = version;
        _description = desc;
        _author = author;
        _settings = null;
    }
    #endregion

    #region Public Serializable
    [XmlAttribute]
    public string Name { get { return _name; } set { _name = value; } }

    [XmlElement]
    public string Version { get { return _version; } set { _version = value; } }

    [XmlElement]
    public string Description { get { return _description; } set { _description = value; } }

    [XmlElement]
    public string Author { get { return _author; } set { _author = value; } }

    [XmlElement]
    public string AdminPage { get { return _adminPage; } set { _adminPage = value; } }

    [XmlElement]
    public bool Enabled { get { return _enabled; } set { _enabled = value; } }

    [XmlElement(IsNullable = true)]
    public ExtensionSettings Settings { get { return _settings; } set { _settings = value; } }

    #endregion

    public void SaveSettings(ExtensionSettings settings)
    {
        _settings = settings;
    }

}
