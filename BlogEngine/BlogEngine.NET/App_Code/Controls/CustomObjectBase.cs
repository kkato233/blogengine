using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BlogEngine.Core;
using BlogEngine.Core.DataStore;

/// <summary>
/// CustomObjectBase - base for custom objects. All 
/// widget custom objects should inherit from this
/// base and only have serializable public members
/// </summary>
[Serializable()]
public abstract class CustomObjectBase
{
  public CustomObjectBase()
  {
    //
    // TODO: Add constructor logic here
    //
  }
}

public static class CustomObject
{
  public static object GetObject(string id, Type type)
  {
    WidgetSettings ws = new WidgetSettings(id, typeof(CustomObjectBase));
    Stream stm = (Stream)ws.GetSettings();
    object data = null;
    if (stm != null)
    {
      if (stm.GetType().Name == "FileStream")
      {
        XmlSerializer x = new XmlSerializer(type);
        data = x.Deserialize(stm);
        stm.Close();
      }
      else
      {
        BinaryFormatter bf = new BinaryFormatter();
        stm.Position = 0;
        data = bf.Deserialize(stm);
      }
    }
    return data;
  }
}
