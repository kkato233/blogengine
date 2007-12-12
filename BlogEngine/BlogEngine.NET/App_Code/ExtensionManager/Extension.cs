using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using BlogEngine.Core;

/// <summary>
/// Summary description for Extension
/// </summary>

public class Extension
{
    #region Private members
    string _name = string.Empty;
    string _version = string.Empty;
    string _description = string.Empty;
    bool _enabled = true;
    ExtensionSettings _settings = null;
    #endregion

    #region Constructor
    public Extension() { }
    public Extension(string name, string version, string desc)
    {
        _name = name;
        _version = version;
        _description = desc;
        _settings = null; // new ExtensionSettings(_name);
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
    public bool Enabled { get { return _enabled; } set { _enabled = value; } }

    [XmlElement(IsNullable = true)]
    public ExtensionSettings Settings { get { return _settings; } set { _settings = value; } }

    #endregion

    public void SaveSettings(ExtensionSettings settings)
    {
        _settings = settings;
    }

}
