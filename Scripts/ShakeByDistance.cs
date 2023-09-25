// Decompiled with JetBrains decompiler
// Type: ShakeByDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class ShakeByDistance : MonoBehaviour
{
  public GameObject Player;
  public float Distance = 10f;
  private CameraShakeInstance _shakeInstance;

  private void Start() => this._shakeInstance = CameraShaker.Instance.StartShake(2f, 14f, 0.0f);

  private void Update() => this._shakeInstance.ScaleMagnitude = 1f - Mathf.Clamp01(Vector3.Distance(this.Player.transform.position, this.transform.position) / this.Distance);
}
