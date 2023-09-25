// Decompiled with JetBrains decompiler
// Type: MainCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MainCamera : MonoBehaviour
{
  private void Awake()
  {
    if (!(bool) (UnityEngine.Object) SlowmoEffect.Instance)
      return;
    SlowmoEffect.Instance.NewScene(this.GetComponent<AudioLowPassFilter>(), this.GetComponent<AudioDistortionFilter>());
  }
}
