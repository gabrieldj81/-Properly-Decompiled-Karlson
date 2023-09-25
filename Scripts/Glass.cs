// Decompiled with JetBrains decompiler
// Type: Glass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class Glass : MonoBehaviour
{
  public GameObject glass;
  public GameObject glassSfx;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
      return;
    UnityEngine.Object.Instantiate<GameObject>(this.glassSfx, this.transform.position, Quaternion.identity);
    this.glass.SetActive(true);
    this.glass.transform.parent = (Transform) null;
    this.glass.transform.localScale = Vector3.one;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
      PlayerMovement.Instance.Slowmo(0.3f, 1f);
    CameraShaker.Instance.ShakeOnce(5f, 3.5f, 0.3f, 0.2f);
  }
}
