// Decompiled with JetBrains decompiler
// Type: PlayerSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

public class PlayerSave
{
  public float[] times = new float[100];

  public bool cameraShake { get; set; } = true;

  public bool motionBlur { get; set; } = true;

  public bool slowmo { get; set; } = true;

  public bool graphics { get; set; } = true;

  public bool muted { get; set; }

  public float sensitivity { get; set; } = 1f;

  public float fov { get; set; } = 80f;

  public float volume { get; set; } = 0.75f;

  public float music { get; set; } = 0.5f;
}
