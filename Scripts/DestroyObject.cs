// Decompiled with JetBrains decompiler
// Type: DestroyObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DestroyObject : MonoBehaviour
{
  public float time;

  private void Start() => this.Invoke("DestroySelf", this.time);

  private void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
