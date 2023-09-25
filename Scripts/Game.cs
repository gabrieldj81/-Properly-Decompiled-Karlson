// Decompiled with JetBrains decompiler
// Type: Game
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
  public bool playing;
  public bool done;

  public static Game Instance { get; private set; }

  private void Start()
  {
    Game.Instance = this;
    this.playing = false;
  }

  public void StartGame()
  {
    this.playing = true;
    this.done = false;
    Time.timeScale = 1f;
    UIManger.Instance.StartGame();
    Timer.Instance.StartTimer();
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    Time.timeScale = 1f;
    this.StartGame();
  }

  public void EndGame() => this.playing = false;

  public void NextMap()
  {
    Time.timeScale = 1f;
    int buildIndex = SceneManager.GetActiveScene().buildIndex;
    if (buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
    {
      this.MainMenu();
    }
    else
    {
      SceneManager.LoadScene(buildIndex + 1);
      this.StartGame();
    }
  }

  public void MainMenu()
  {
    this.playing = false;
    SceneManager.LoadScene(nameof (MainMenu));
    UIManger.Instance.GameUI(false);
    Time.timeScale = 1f;
  }

  public void Win()
  {
    this.playing = false;
    Timer.Instance.Stop();
    Time.timeScale = 0.05f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    UIManger.Instance.WinUI(true);
    float timer = Timer.Instance.GetTimer();
    int index = int.Parse(SceneManager.GetActiveScene().name[0].ToString() ?? "");
    int result;
    if (int.TryParse(SceneManager.GetActiveScene().name.Substring(0, 2) ?? "", out result))
      index = result;
    float time = SaveManager.Instance.state.times[index];
    if ((double) timer < (double) time || (double) time == 0.0)
    {
      SaveManager.Instance.state.times[index] = timer;
      SaveManager.Instance.Save();
    }
    MonoBehaviour.print((object) ("time has been saved as: " + Timer.Instance.GetFormattedTime(timer)));
    this.done = true;
  }
}
