// Decompiled with JetBrains decompiler
// Type: Respawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Respawn : MonoBehaviour
{
  public Transform respawnPoint;

  private void OnTriggerEnter(Collider other)
  {
    MonoBehaviour.print((object) other.gameObject.layer);
    if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
      return;
    Transform root = other.transform.root;
    root.transform.position = this.respawnPoint.position;
    root.GetComponent<Rigidbody>().velocity = Vector3.zero;
  }
}
