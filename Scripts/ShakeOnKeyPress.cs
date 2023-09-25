// Decompiled with JetBrains decompiler
// Type: ShakeOnKeyPress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class ShakeOnKeyPress : MonoBehaviour
{
  public float Magnitude = 2f;
  public float Roughness = 10f;
  public float FadeOutTime = 5f;

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.LeftShift))
      return;
    CameraShaker.Instance.ShakeOnce(this.Magnitude, this.Roughness, 0.0f, this.FadeOutTime);
  }
}
