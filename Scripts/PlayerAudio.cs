// Decompiled with JetBrains decompiler
// Type: PlayerAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
  private Rigidbody rb;
  public AudioSource wind;
  public AudioSource foley;
  private float currentVol;
  private float volVel;

  private void Start() => this.rb = PlayerMovement.Instance.GetRb();

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.rb)
      return;
    float num1 = this.rb.velocity.magnitude;
    float num2;
    if (PlayerMovement.Instance.grounded)
    {
      if ((double) num1 < 20.0)
        num1 = 0.0f;
      num2 = (float) (((double) num1 - 20.0) / 30.0);
    }
    else
      num2 = (float) (((double) num1 - 10.0) / 30.0);
    if ((double) num2 > 1.0)
      num2 = 1f;
    this.currentVol = Mathf.SmoothDamp(this.currentVol, num2 * 1f, ref this.volVel, 0.2f);
    if (PlayerMovement.Instance.paused)
      this.currentVol = 0.0f;
    this.foley.volume = this.currentVol;
    this.wind.volume = this.currentVol * 0.5f;
  }
}
