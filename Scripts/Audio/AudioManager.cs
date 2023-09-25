// Decompiled with JetBrains decompiler
// Type: Audio.AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Audio
{
  public class AudioManager : MonoBehaviour
  {
    public Sound[] sounds;
    public Sound[] footsteps;
    public Sound[] wallrun;
    public Sound[] jumps;
    public AudioLowPassFilter filter;
    private float desiredFreq = 500f;
    private float velFreq;
    private float freqSpeed = 0.2f;
    public bool muted;

    public static AudioManager Instance { get; set; }

    private void Awake()
    {
      AudioManager.Instance = this;
      foreach (Sound sound in this.sounds)
      {
        sound.source = this.gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.loop = sound.loop;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.bypassListenerEffects = sound.bypass;
      }
      foreach (Sound footstep in this.footsteps)
      {
        footstep.source = this.gameObject.AddComponent<AudioSource>();
        footstep.source.clip = footstep.clip;
        footstep.source.loop = footstep.loop;
        footstep.source.volume = footstep.volume;
        footstep.source.pitch = footstep.pitch;
        footstep.source.bypassListenerEffects = footstep.bypass;
      }
      foreach (Sound sound in this.wallrun)
      {
        sound.source = this.gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.loop = sound.loop;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.bypassListenerEffects = sound.bypass;
      }
      foreach (Sound jump in this.jumps)
      {
        jump.source = this.gameObject.AddComponent<AudioSource>();
        jump.source.clip = jump.clip;
        jump.source.loop = jump.loop;
        jump.source.volume = jump.volume;
        jump.source.pitch = jump.pitch;
        jump.source.bypassListenerEffects = jump.bypass;
      }
    }

    private void Update()
    {
    }

    public void MuteSounds(bool b)
    {
      AudioListener.volume = !b ? 1f : 0.0f;
      this.muted = b;
    }

    public void PlayButton()
    {
      if (this.muted)
        return;
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == "Button")
        {
          sound.source.pitch = 0.8f + Random.Range(-0.03f, 0.03f);
          break;
        }
      }
      this.Play("Button");
    }

    public void PlayPitched(string n, float v)
    {
      if (this.muted)
        return;
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == n)
        {
          sound.source.pitch = 1f + Random.Range(-v, v);
          break;
        }
      }
      this.Play(n);
    }

    public void MuteMusic()
    {
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == "Song")
        {
          sound.source.volume = 0.0f;
          break;
        }
      }
    }

    public void SetVolume(float v)
    {
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == "Song")
        {
          sound.source.volume = v;
          break;
        }
      }
    }

    public void UnmuteMusic()
    {
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == "Song")
        {
          sound.source.volume = 1.15f;
          break;
        }
      }
    }

    public void Play(string n)
    {
      if (this.muted && n != "Song")
        return;
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == n)
        {
          sound.source.Play();
          break;
        }
      }
    }

    public void PlayFootStep()
    {
      if (this.muted)
        return;
      this.footsteps[Random.Range(0, this.footsteps.Length - 1)].source.Play();
    }

    public void PlayLanding()
    {
      if (this.muted)
        return;
      this.wallrun[Random.Range(0, this.wallrun.Length - 1)].source.Play();
    }

    public void PlayJump()
    {
      if (this.muted)
        return;
      Sound jump = this.jumps[Random.Range(0, this.jumps.Length - 1)];
      if (!(bool) jump.source)
        return;
      jump.source.Play();
    }

    public void Stop(string n)
    {
      foreach (Sound sound in this.sounds)
      {
        if (sound.name == n)
        {
          sound.source.Stop();
          break;
        }
      }
    }

    public void SetFreq(float freq) => this.desiredFreq = freq;
  }
}
