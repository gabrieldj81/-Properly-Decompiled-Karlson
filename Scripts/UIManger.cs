// Decompiled with JetBrains decompiler
// Type: UIManger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class UIManger : MonoBehaviour
{
  public GameObject gameUI;
  public GameObject deadUI;
  public GameObject winUI;

  public static UIManger Instance { get; private set; }

  private void Awake() => UIManger.Instance = this;

  private void Start() => this.gameUI.SetActive(false);

  public void StartGame()
  {
    this.gameUI.SetActive(true);
    this.DeadUI(false);
    this.WinUI(false);
  }

  public void GameUI(bool b) => this.gameUI.SetActive(b);

  public void DeadUI(bool b) => this.deadUI.SetActive(b);

  public void WinUI(bool b)
  {
    this.winUI.SetActive(b);
    MonoBehaviour.print((object) "setting win UI");
  }
}
