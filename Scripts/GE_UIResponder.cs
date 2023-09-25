// Decompiled with JetBrains decompiler
// Type: GE_UIResponder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class GE_UIResponder : MonoBehaviour
{
  public string m_PackageTitle = "-";
  public string m_TargetURL = "www.unity3d.com";

  private void Start()
  {
    GameObject gameObject = GameObject.Find("Text Package Title");
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    gameObject.GetComponent<Text>().text = this.m_PackageTitle;
  }

  private void Update()
  {
  }

  public void OnButton_AssetName() => Application.OpenURL(this.m_TargetURL);
}
