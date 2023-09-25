// Decompiled with JetBrains decompiler
// Type: Milk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Milk : MonoBehaviour
{
  private void Update() => this.transform.Rotate(new Vector3(1f, 1f, Mathf.PingPong(Time.time, 1f)), 0.5f);

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer != LayerMask.NameToLayer("Player") || PlayerMovement.Instance.IsDead())
      return;
    Game.Instance.Win();
    MonoBehaviour.print((object) "Player won");
  }
}
