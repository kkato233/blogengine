using System;
using System.Xml.Serialization;

/// <summary>
/// Summary description for ExtParameter
/// </summary>

public class ExtParameter
{
    string _name = string.Empty;
    string _value = string.Empty;

    [XmlElement]
    public string Name { get { return _name; } set { _name = value; } }
    
    [XmlElement]
    public string Value { get { return _value; } set { _value = value; } }

    public ExtParameter() { }
    public ExtParameter(string name, string val)
    {
        _name = name;
        _value = val;
    }
}

