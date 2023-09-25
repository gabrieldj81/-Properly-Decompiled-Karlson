// Decompiled with JetBrains decompiler
// Type: StartPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StartPlayer : MonoBehaviour
{
  private void Awake()
  {
    for (int index = this.transform.childCount - 1; index >= 0; --index)
    {
      MonoBehaviour.print((object) ("removing child: " + (object) index));
      this.transform.GetChild(index).parent = (Transform) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
