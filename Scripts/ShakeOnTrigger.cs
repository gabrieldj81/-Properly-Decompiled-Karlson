// Decompiled with JetBrains decompiler
// Type: ShakeOnTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class ShakeOnTrigger : MonoBehaviour
{
  private CameraShakeInstance _shakeInstance;

  private void Start()
  {
    this._shakeInstance = CameraShaker.Instance.StartShake(2f, 15f, 2f);
    this._shakeInstance.StartFadeOut(0.0f);
    this._shakeInstance.DeleteOnInactive = true;
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!c.CompareTag("Player"))
      return;
    this._shakeInstance.StartFadeIn(1f);
  }

  private void OnTriggerExit(Collider c)
  {
    if (!c.CompareTag("Player"))
      return;
    this._shakeInstance.StartFadeOut(3f);
  }
}
