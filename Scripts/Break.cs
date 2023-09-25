// Decompiled with JetBrains decompiler
// Type: Break
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Break : MonoBehaviour
{
  public GameObject replace;
  private bool done;

  private void OnCollisionEnter(Collision other)
  {
    if (this.done || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
      return;
    Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
    if (!(bool) (UnityEngine.Object) component || (double) component.velocity.magnitude <= 18.0)
      return;
    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      if (!PlayerMovement.Instance.IsCrouching())
        return;
      PlayerMovement.Instance.Slowmo(0.35f, 0.8f);
      this.BreakDoor(component);
    }
    this.BreakDoor(component);
  }

  private void BreakDoor(Rigidbody rb)
  {
    Vector3 velocity = rb.velocity;
    float magnitude = velocity.magnitude;
    if ((double) magnitude > 20.0)
    {
      float num = magnitude / 20f;
      velocity /= num;
    }
    foreach (Rigidbody componentsInChild in UnityEngine.Object.Instantiate<GameObject>(this.replace, this.transform.position, this.transform.rotation).GetComponentsInChildren<Rigidbody>())
      componentsInChild.velocity = velocity * 1.5f;
    UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.destructionAudio, this.transform.position, Quaternion.identity);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    this.done = true;
  }
}
