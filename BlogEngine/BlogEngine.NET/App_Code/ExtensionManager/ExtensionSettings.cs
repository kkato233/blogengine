using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Collections.Generic;

/// <summary>
/// Summary description for ExtensionSettings
/// </summary>
public class ExtensionSettings
{
    #region Private members
    string _extensionName = string.Empty;
    string _settingsHelp = string.Empty;
    char[] _delimiter = null;
    List<ExtensionParameter> _params = null;
    string _keyField = string.Empty;
    #endregion

    #region Constructors
    // required for serialization to work
    public ExtensionSettings() { }

    public ExtensionSettings(string extensionName)
    {
        _extensionName = extensionName;
        _delimiter = ",".ToCharArray();
    }
    #endregion

    #region Public members
    [XmlElement]
    public string ExtensionName { get { return _extensionName; } }

    [XmlElement]
    public string Help { get { return _settingsHelp; } set { _settingsHelp = value; } }

    [XmlElement]
    public char[] Delimiter { get { return _delimiter; } set { _delimiter = value; } }

    [XmlElement(IsNullable = true)]
    public List<ExtensionParameter> Parameters { get { return _params; } set { _params = value; } }

    [XmlElement]
    public string KeyField
    {
        get
        {
            string rval = string.Empty;
            foreach (ExtensionParameter par in _params)
            {
                if (par.KeyField == true)
                {
                    rval = par.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(rval))
            {
                rval = _params[0].Name;
            }
            return rval;
        }
        set
        {
            _keyField = value;
        }
    }
    #endregion

    #region Parameter methods

    public void AddParameter(string name)
    {
        AddParameter(name, name);
    }

    public void AddParameter(string name, string label)
    {
        AddParameter(name, label, 100);
    }

    public void AddParameter(string name, string label, int maxLength)
    {
        AddParameter(name, label, maxLength, false);
    }

    public void AddParameter(string name, string label, int maxLength, bool required)
    {
        AddParameter(name, label, maxLength, required, false);
    }

    public void AddParameter(string name, string label, int maxLength, bool required, bool keyfield)
    {
        if (_params == null)
            _params = new List<ExtensionParameter>();

        ExtensionParameter par = new ExtensionParameter(name);
        par.Label = label;
        par.MaxLength = maxLength;
        par.Required = required;
        par.KeyField = keyfield;

        _params.Add(par);
    }

    public bool IsRequiredParameter(string paramName)
    {
        foreach (ExtensionParameter par in _params)
        {
            if (par.Name == paramName)
            {
                if (par.Required == true)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }
    public bool IsKeyValueExists(string newVal)
    {
        foreach (ExtensionParameter par in _params)
        {
            if (par.Name == KeyField)
            {
                foreach (string val in par.Values)
                {
                    if (val == newVal)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    #endregion

    #region Values Methods

    public void AddValue(string parameterName, string val)
    {
        foreach (ExtensionParameter par in _params)
        {
            if (par.Name == parameterName)
            {
                par.AddValue(val);
                break;
            }
        }
    }

    public void AddValues(string[] values)
    {
        if (_params.Count > 0)
        {
            for (int i = 0; i < _params.Count; i++)
            {
                _params[i].AddValue(values[i]);
            }
        }
    }

    public void AddValues(StringCollection values)
    {
        if (_params.Count > 0)
        {
            for (int i = 0; i < _params.Count; i++)
            {
                _params[i].AddValue(values[i]);
            }
        }
    }

    public System.Data.DataTable GetDataTable()
    {
        System.Data.DataTable objDataTable = new System.Data.DataTable();
        foreach (ExtensionParameter p in _params)
        {
            objDataTable.Columns.Add(p.Name, string.Empty.GetType());
        }

        if (_params[0].Values != null)
        {
            for (int i = 0; i < _params[0].Values.Count; i++)
            {
                string[] row = new string[_params.Count];
                for (int j = 0; j < _params.Count; j++)
                {
                    row[j] = _params[j].Values[i];
                }
                objDataTable.Rows.Add(row);
            }
        }

        return objDataTable;
    }

    #endregion
}