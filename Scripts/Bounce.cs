// Decompiled with JetBrains decompiler
// Type: Bounce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Bounce : MonoBehaviour
{
  private void OnCollisionEnter(Collision other)
  {
    MonoBehaviour.print((object) "yeet");
    int num = (bool) (UnityEngine.Object) other.gameObject.GetComponent<Rigidbody>() ? 1 : 0;
  }
}
