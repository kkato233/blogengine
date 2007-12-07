using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.IO;
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
    string _pardesc = string.Empty;
    List<ExtParameter> _params = new List<ExtParameter>();
    #endregion

    #region Constructor
    public Extension() { }
    public Extension(string name, string version, string desc)
    {
        _name = name;
        _version = version;
        _description = desc;
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
    public string ParamsDescription { get { return _pardesc; } set { _pardesc = value; } }

    [XmlElement(IsNullable = true)]
    public List<ExtParameter> Parameters { get { return _params; } set { _params = value; } }
    #endregion

    public void SaveSettings(Dictionary<string, string[]> settings)
    {
        _params.Clear();
        foreach (string key in settings.Keys)
        {
            _params.Add(new ExtParameter(key, string.Join(",", settings[key])));
        }
    }

    [XmlIgnore]
    public Dictionary<string, string[]> Settings
    {
        get
        {
            Dictionary<string, string[]> settings = new Dictionary<string, string[]>();
            foreach (ExtParameter p in _params)
            {
                settings.Add(p.Name, p.Value.Split(','));
            }
            return settings;
        }
    }
}
