// Decompiled with JetBrains decompiler
// Type: Options
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
  public TextMeshProUGUI sens;
  public TextMeshProUGUI volume;
  public TextMeshProUGUI music;
  public TextMeshProUGUI fov;
  public TextMeshProUGUI[] sounds;
  public TextMeshProUGUI[] graphics;
  public TextMeshProUGUI[] shake;
  public TextMeshProUGUI[] slowmo;
  public TextMeshProUGUI[] blur;
  public Slider sensS;
  public Slider volumeS;
  public Slider musicS;
  public Slider fovS;

  private void OnEnable()
  {
    this.UpdateList(this.graphics, GameState.Instance.GetGraphics());
    this.UpdateList(this.shake, GameState.Instance.shake);
    this.UpdateList(this.slowmo, GameState.Instance.slowmo);
    this.UpdateList(this.blur, GameState.Instance.blur);
    this.sensS.value = GameState.Instance.GetSensitivity();
    this.volumeS.value = GameState.Instance.GetVolume();
    this.musicS.value = GameState.Instance.GetMusic();
    this.fovS.value = GameState.Instance.GetFov();
    MonoBehaviour.print((object) GameState.Instance.GetMusic());
    this.UpdateSensitivity();
    this.UpdateFov();
    this.UpdateVolume();
    this.UpdateMusic();
  }

  public void ChangeGraphics(bool b)
  {
    GameState.Instance.SetGraphics(b);
    this.UpdateList(this.graphics, b);
  }

  public void ChangeBlur(bool b)
  {
    GameState.Instance.SetBlur(b);
    this.UpdateList(this.blur, b);
  }

  public void ChangeShake(bool b)
  {
    GameState.Instance.SetShake(b);
    this.UpdateList(this.shake, b);
  }

  public void ChangeSlowmo(bool b)
  {
    GameState.Instance.SetSlowmo(b);
    this.UpdateList(this.slowmo, b);
  }

  public void UpdateSensitivity()
  {
    float s = this.sensS.value;
    GameState.Instance.SetSensitivity(s);
    this.sens.text = string.Format("{0:F2}", (object) s);
  }

  public void UpdateVolume()
  {
    float s = this.volumeS.value;
    AudioListener.volume = s;
    GameState.Instance.SetVolume(s);
    this.volume.text = string.Format("{0:F2}", (object) s);
  }

  public void UpdateMusic()
  {
    float s = this.musicS.value;
    GameState.Instance.SetMusic(s);
    this.music.text = string.Format("{0:F2}", (object) s);
  }

  public void UpdateFov()
  {
    float f = this.fovS.value;
    GameState.Instance.SetFov(f);
    this.fov.text = string.Concat((object) f);
  }

  private void UpdateList(TextMeshProUGUI[] list, bool b)
  {
    if (!b)
    {
      list[1].color = Color.white;
      list[0].color = (Color.clear + Color.white) / 2f;
    }
    else
    {
      list[1].color = (Color.clear + Color.white) / 2f;
      list[0].color = Color.white;
    }
  }
}
