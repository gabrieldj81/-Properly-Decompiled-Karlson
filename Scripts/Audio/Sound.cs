// Decompiled with JetBrains decompiler
// Type: Audio.Sound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Audio
{
  [Serializable]
  public class Sound
  {
    public string name;
    public AudioClip clip;
    [Range(0.0f, 2f)]
    public float volume;
    [Range(0.0f, 2f)]
    public float pitch;
    public bool loop;
    public bool bypass;
    [HideInInspector]
    public AudioSource source;
  }
}
