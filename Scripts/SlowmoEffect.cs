// Decompiled with JetBrains decompiler
// Type: SlowmoEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SlowmoEffect : MonoBehaviour
{
  public Image blackFx;
  public PostProcessProfile pp;
  private ColorGrading cg;
  private float frequency;
  private float vel;
  private float hue;
  private float hueVel;
  private AudioDistortionFilter af;
  private AudioLowPassFilter lf;

  public static SlowmoEffect Instance { get; private set; }

  private void Start()
  {
    this.cg = this.pp.GetSetting<ColorGrading>();
    SlowmoEffect.Instance = this;
  }

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) this.af || !(bool) (UnityEngine.Object) this.lf)
      return;
    if (!Game.Instance.playing || !(bool) (UnityEngine.Object) Camera.main)
    {
      if ((double) this.cg.hueShift.value == 0.0)
        return;
      this.cg.hueShift.value = 0.0f;
    }
    else
    {
      float timeScale = Time.timeScale;
      float a = (float) ((1.0 - (double) timeScale) * 2.0);
      if ((double) a > 0.7)
        a = 0.7f;
      this.blackFx.color = new Color(1f, 1f, 1f, a);
      float target1 = PlayerMovement.Instance.GetActionMeter();
      float target2 = 0.0f;
      if ((double) timeScale < 0.89999997615814209)
      {
        target1 = 400f;
        target2 = -20f;
      }
      this.frequency = Mathf.SmoothDamp(this.frequency, target1, ref this.vel, 0.1f);
      this.hue = Mathf.SmoothDamp(this.hue, target2, ref this.hueVel, 0.2f);
      if ((bool) (UnityEngine.Object) this.af)
        this.af.distortionLevel = a * 0.2f;
      if ((bool) (UnityEngine.Object) this.lf)
        this.lf.cutoffFrequency = this.frequency;
      if ((bool) (UnityEngine.Object) this.cg)
        this.cg.hueShift.value = this.hue;
      if (Game.Instance.playing)
        return;
      this.cg.hueShift.value = 0.0f;
    }
  }

  public void NewScene(AudioLowPassFilter l, AudioDistortionFilter d)
  {
    this.lf = l;
    this.af = d;
  }
}
