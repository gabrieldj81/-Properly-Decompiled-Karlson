// Decompiled with JetBrains decompiler
// Type: RandomSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RandomSfx : MonoBehaviour
{
  public AudioClip[] sounds;

  private void Awake()
  {
    AudioSource component = this.GetComponent<AudioSource>();
    component.clip = this.sounds[Random.Range(0, this.sounds.Length - 1)];
    component.playOnAwake = true;
    component.pitch = 1f + Random.Range(-0.3f, 0.1f);
    component.enabled = true;
  }
}
