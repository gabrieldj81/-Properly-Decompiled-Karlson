// Decompiled with JetBrains decompiler
// Type: Managers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
  public static Managers Instance { get; private set; }

  private void Start()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    SceneManager.LoadScene("MainMenu");
    Time.timeScale = 1f;
    Application.targetFrameRate = 240;
    QualitySettings.vSyncCount = 0;
  }
}
