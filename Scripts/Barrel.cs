// Decompiled with JetBrains decompiler
// Type: Barrel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Barrel : MonoBehaviour
{
  private bool done;

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
      return;
    Explosion componentInChildren = (Explosion) UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity).GetComponentInChildren(typeof (Explosion));
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    this.CancelInvoke();
    this.done = true;
    Bullet component = (Bullet) other.gameObject.GetComponent(typeof (Bullet));
    if (!(bool) (UnityEngine.Object) component || !component.player)
      return;
    componentInChildren.player = component.player;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
      return;
    this.done = true;
    this.Invoke("Explode", 0.2f);
  }

  private void Explode()
  {
    UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
