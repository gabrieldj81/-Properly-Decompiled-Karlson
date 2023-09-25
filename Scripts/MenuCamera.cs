// Decompiled with JetBrains decompiler
// Type: MenuCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
  private Vector3 startPos;
  private Vector3 options = new Vector3(0.0f, 3.6f, 8f);
  private Vector3 play = new Vector3(1f, 4.6f, 5.5f);
  private Vector3 about = new Vector3(1f, 5.5f, 5.5f);
  private Vector3 desiredPos;
  private Vector3 posVel;
  private Vector3 startRot;
  private Vector3 playRot;
  private Vector3 aboutRot;
  private Quaternion desiredRot;

  private void Start()
  {
    this.startPos = this.transform.position;
    this.desiredPos = this.startPos;
    this.options += this.startPos;
    this.play += this.startPos;
    this.about += this.startPos;
    CameraShaker.Instance.StartShake(1f, 0.04f, 0.1f);
    this.startRot = Vector3.zero;
    this.playRot = new Vector3(0.0f, 90f, 0.0f);
    this.aboutRot = new Vector3(-90f, 0.0f, 0.0f);
  }

  private void Update()
  {
    this.transform.position = Vector3.SmoothDamp(this.transform.position, this.desiredPos, ref this.posVel, 0.4f);
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.desiredRot, Time.deltaTime * 4f);
  }

  public void Options() => this.desiredPos = this.options;

  public void Main()
  {
    this.desiredPos = this.startPos;
    this.desiredRot = Quaternion.Euler(this.startRot);
  }

  public void Play()
  {
    this.desiredPos = this.play;
    this.desiredRot = Quaternion.Euler(this.playRot);
  }

  public void About()
  {
    this.desiredPos = this.about;
    this.desiredRot = Quaternion.Euler(this.aboutRot);
  }
}
