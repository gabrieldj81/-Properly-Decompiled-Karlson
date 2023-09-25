// Decompiled with JetBrains decompiler
// Type: GE_ToggleFullScreenUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class GE_ToggleFullScreenUI : MonoBehaviour
{
  private int m_DefWidth;
  private int m_DefHeight;

  private void Start()
  {
    this.m_DefWidth = Screen.width;
    this.m_DefHeight = Screen.height;
    if (Application.isEditor)
      return;
    if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
      this.gameObject.SetActive(true);
    else
      this.gameObject.SetActive(false);
  }

  private void Update()
  {
  }

  public void OnButton_ToggleFullScreen()
  {
    if (Application.isEditor)
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.GetComponent<Button>().interactable = false;
      foreach (Component component in this.transform)
        component.gameObject.SetActive(true);
    }
    else
    {
      Screen.fullScreen = !Screen.fullScreen;
      if (!Screen.fullScreen)
      {
        Resolution currentResolution = Screen.currentResolution;
        int width = currentResolution.width;
        currentResolution = Screen.currentResolution;
        int height = currentResolution.height;
        Screen.SetResolution(width, height, true);
      }
      else
        Screen.SetResolution(this.m_DefWidth, this.m_DefHeight, false);
    }
  }
}
