using System;
using System.Xml.Serialization;
using System.Collections.Generic;

/// <summary>
/// Summary description for ExtensionSettings
/// </summary>
public class ExtensionSettings
{
    string _extensionName = string.Empty;
    string _settingsHelp = string.Empty;
    char[] _delimiter = null;
    List<ExtensionParameter> _params = null; // new List<ExtensionParameter>();

    [XmlElement]
    public string ExtensionName { get { return _extensionName; } }
    [XmlElement]
    public string SettingsHelp { get { return _settingsHelp; } set { _settingsHelp = value; } }
    [XmlElement]
    public char[] ParametersDelimiter { get { return _delimiter; } set { _delimiter = value; } }
    [XmlElement(IsNullable = true)]
    public List<ExtensionParameter> Parameters { get { return _params; } set { _params = value; } }

    // required for serialization to work
    public ExtensionSettings() { }

    public ExtensionSettings(string extensionName)
    {
        _extensionName = extensionName;
        _delimiter = ",".ToCharArray();
    }

    public void AddParameter(string name, string val)
    {
        if (_params == null)
            _params = new List<ExtensionParameter>();

        _params.Add(new ExtensionParameter(name, val));
    }

    public void RemoveParameter(string name)
    {
        foreach (ExtensionParameter p in _params)
        {
            if (p.Name == name)
            {
                _params.Remove(p);
                break;
            }
        }
    }

    public void UpdateParameter(string name, string val)
    {
        foreach (ExtensionParameter p in _params)
        {
            if (p.Name == name)
            {
                p.Value = val;
                break;
            }
        }
    }
}

public class ExtensionParameter
{
    string _name = string.Empty;
    string _value = string.Empty;

    [XmlElement]
    public string Name { get { return _name; } set { _name = value; } }

    [XmlElement]
    public string Value { get { return _value; } set { _value = value; } }

    public ExtensionParameter() { }
    public ExtensionParameter(string name, string val)
    {
        _name = name;
        _value = val;
    }
}

