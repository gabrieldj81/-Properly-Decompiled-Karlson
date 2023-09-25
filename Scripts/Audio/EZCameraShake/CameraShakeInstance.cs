// Decompiled with JetBrains decompiler
// Type: EZCameraShake.CameraShakeInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace EZCameraShake
{
  public class CameraShakeInstance
  {
    public float Magnitude;
    public float Roughness;
    public Vector3 PositionInfluence;
    public Vector3 RotationInfluence;
    public bool DeleteOnInactive = true;
    private float roughMod = 1f;
    private float magnMod = 1f;
    private float fadeOutDuration;
    private float fadeInDuration;
    private bool sustain;
    private float currentFadeTime;
    private float tick;
    private Vector3 amt;

    public CameraShakeInstance(
      float magnitude,
      float roughness,
      float fadeInTime,
      float fadeOutTime)
    {
      this.Magnitude = magnitude;
      this.fadeOutDuration = fadeOutTime;
      this.fadeInDuration = fadeInTime;
      this.Roughness = roughness;
      if ((double) fadeInTime > 0.0)
      {
        this.sustain = true;
        this.currentFadeTime = 0.0f;
      }
      else
      {
        this.sustain = false;
        this.currentFadeTime = 1f;
      }
      this.tick = (float) Random.Range(-100, 100);
    }

    public CameraShakeInstance(float magnitude, float roughness)
    {
      this.Magnitude = magnitude;
      this.Roughness = roughness;
      this.sustain = true;
      this.tick = (float) Random.Range(-100, 100);
    }

    public Vector3 UpdateShake()
    {
      this.amt.x = Mathf.PerlinNoise(this.tick, 0.0f) - 0.5f;
      this.amt.y = Mathf.PerlinNoise(0.0f, this.tick) - 0.5f;
      this.amt.z = Mathf.PerlinNoise(this.tick, this.tick) - 0.5f;
      if ((double) this.fadeInDuration > 0.0 && this.sustain)
      {
        if ((double) this.currentFadeTime < 1.0)
          this.currentFadeTime += Time.deltaTime / this.fadeInDuration;
        else if ((double) this.fadeOutDuration > 0.0)
          this.sustain = false;
      }
      if (!this.sustain)
        this.currentFadeTime -= Time.deltaTime / this.fadeOutDuration;
      if (this.sustain)
        this.tick += Time.deltaTime * this.Roughness * this.roughMod;
      else
        this.tick += Time.deltaTime * this.Roughness * this.roughMod * this.currentFadeTime;
      return this.amt * this.Magnitude * this.magnMod * this.currentFadeTime;
    }

    public void StartFadeOut(float fadeOutTime)
    {
      if ((double) fadeOutTime == 0.0)
        this.currentFadeTime = 0.0f;
      this.fadeOutDuration = fadeOutTime;
      this.fadeInDuration = 0.0f;
      this.sustain = false;
    }

    public void StartFadeIn(float fadeInTime)
    {
      if ((double) fadeInTime == 0.0)
        this.currentFadeTime = 1f;
      this.fadeInDuration = fadeInTime;
      this.fadeOutDuration = 0.0f;
      this.sustain = true;
    }

    public float ScaleRoughness
    {
      get => this.roughMod;
      set => this.roughMod = value;
    }

    public float ScaleMagnitude
    {
      get => this.magnMod;
      set => this.magnMod = value;
    }

    public float NormalizedFadeTime => this.currentFadeTime;

    private bool IsShaking => (double) this.currentFadeTime > 0.0 || this.sustain;

    private bool IsFadingOut => !this.sustain && (double) this.currentFadeTime > 0.0;

    private bool IsFadingIn => (double) this.currentFadeTime < 1.0 && this.sustain && (double) this.fadeInDuration > 0.0;

    public CameraShakeState CurrentState
    {
      get
      {
        if (this.IsFadingIn)
          return CameraShakeState.FadingIn;
        if (this.IsFadingOut)
          return CameraShakeState.FadingOut;
        return this.IsShaking ? CameraShakeState.Sustained : CameraShakeState.Inactive;
      }
    }
  }
}
