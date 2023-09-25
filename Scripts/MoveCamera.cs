// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MoveCamera : MonoBehaviour
{
  public Transform player;
  private Vector3 offset;
  private Camera cam;

  public static MoveCamera Instance { get; private set; }

  private void Start()
  {
    MoveCamera.Instance = this;
    this.cam = this.transform.GetChild(0).GetComponent<Camera>();
    this.cam.fieldOfView = GameState.Instance.fov;
    this.offset = this.transform.position - this.player.transform.position;
  }

  private void Update() => this.transform.position = this.player.transform.position;

  public void UpdateFov() => this.cam.fieldOfView = GameState.Instance.fov;
}
