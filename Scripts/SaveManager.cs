// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
  public PlayerSave state;

  public static SaveManager Instance { get; set; }

  private void Awake()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    SaveManager.Instance = this;
    this.Load();
  }

  public void Save() => PlayerPrefs.SetString("save", this.Serialize<PlayerSave>(this.state));

  public void Load()
  {
    if (PlayerPrefs.HasKey("save"))
      this.state = this.Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
    else
      this.NewSave();
  }

  public void NewSave()
  {
    this.state = new PlayerSave();
    this.Save();
    MonoBehaviour.print((object) "Creating new save file");
  }

  public string Serialize<T>(T toSerialize)
  {
    XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
    StringWriter stringWriter1 = new StringWriter();
    StringWriter stringWriter2 = stringWriter1;
    // ISSUE: variable of a boxed type
    __Boxed<T> o = (__Boxed<T>)(object)toSerialize;
    xmlSerializer.Serialize((TextWriter) stringWriter2, (object) o);
    return stringWriter1.ToString();
  }

  public T Deserialize<T>(string toDeserialize) => (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) new StringReader(toDeserialize));
}

internal class __Boxed<T>
{
}