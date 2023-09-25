// Decompiled with JetBrains decompiler
// Type: PlayUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class PlayUI : MonoBehaviour
{
  public TextMeshProUGUI[] maps;

  private void Start()
  {
    float[] times = SaveManager.Instance.state.times;
    for (int index = 0; index < this.maps.Length; ++index)
    {
      MonoBehaviour.print((object) ("i: " + (object) times[index]));
      this.maps[index].text = Timer.Instance.GetFormattedTime(times[index]);
    }
  }
}
