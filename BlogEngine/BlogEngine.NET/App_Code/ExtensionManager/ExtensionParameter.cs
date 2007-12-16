using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Collections.Generic;

/// <summary>
/// Summary description for ExtensionParameter
/// </summary>
public class ExtensionParameter
{
    #region Private members
    string _name = string.Empty;
    string _label = string.Empty;
    int _maxLength = 100;
    bool _required = false;
    bool _keyField = false;
    StringCollection _values = null;
    #endregion

    #region Public Serializable
    [XmlElement]
    public string Name { get { return _name; } set { _name = value; } }

    [XmlElement]
    public string Label { get { return _label; } set { _label = value; } }

    [XmlElement]
    public int MaxLength { get { return _maxLength; } set { _maxLength = value; } }

    [XmlElement]
    public bool Required { get { return _required; } set { _required = value; } }

    [XmlElement]
    public bool KeyField { get { return _keyField; } set { _keyField = value; } }

    [XmlElement]
    public StringCollection Values { get { return _values; } set { _values = value; } }
    #endregion

    #region Constructors
    public ExtensionParameter() { }

    public ExtensionParameter(string name)
    {
        _name = name;
    }
    #endregion

    #region Public Methods
    public void AddValue(string val)
    {
        if (_values == null)
            _values = new StringCollection();

        _values.Add(val);
    }

    public void DeleteValue(int rowIndex)
    {
        _values.RemoveAt(rowIndex);
    }
    #endregion
}

