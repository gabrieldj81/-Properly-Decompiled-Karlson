// Decompiled with JetBrains decompiler
// Type: Timer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  private TextMeshProUGUI text;
  private float timer;
  private bool stop;

  public static Timer Instance { get; set; }

  private void Awake()
  {
    Timer.Instance = this;
    this.text = this.GetComponent<TextMeshProUGUI>();
    this.stop = false;
  }

  public void StartTimer()
  {
    this.stop = false;
    this.timer = 0.0f;
  }

  private void Update()
  {
    if (!Game.Instance.playing || this.stop)
      return;
    this.timer += Time.deltaTime;
    this.text.text = this.GetFormattedTime(this.timer);
  }

  public string GetFormattedTime(float f)
  {
    if ((double) f == 0.0)
      return "nan";
    string str1 = Mathf.Floor(f / 60f).ToString("00");
    string str2 = Mathf.Floor(f % 60f).ToString("00");
    string str3 = ((float) ((double) f * 100.0 % 100.0)).ToString("00");
    if (str3.Equals("100"))
      str3 = "99";
    return string.Format("{0}:{1}:{2}", (object) str1, (object) str2, (object) str3);
  }

  public float GetTimer() => this.timer;

  private string StatusText(float f)
  {
    if ((double) f < 2.0)
      return "very easy";
    if ((double) f < 4.0)
      return "easy";
    if ((double) f < 8.0)
      return "medium";
    if ((double) f < 12.0)
      return "hard";
    if ((double) f < 16.0)
      return "very hard";
    if ((double) f < 20.0)
      return "impossible";
    if ((double) f < 25.0)
      return "oh shit";
    return (double) f < 30.0 ? "very oh shit" : nameof (f);
  }

  public void Stop() => this.stop = true;

  public int GetMinutes() => (int) Mathf.Floor(this.timer / 60f);
}
