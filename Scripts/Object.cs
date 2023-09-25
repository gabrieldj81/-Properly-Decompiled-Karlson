// Decompiled with JetBrains decompiler
// Type: Object
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Object : MonoBehaviour
{
  private bool ready = true;
  private bool hitReady = true;

  private void OnCollisionEnter(Collision other)
  {
    float num1 = other.relativeVelocity.magnitude * 0.025f;
    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && this.hitReady && (double) num1 > 0.800000011920929)
    {
      this.hitReady = false;
      Vector3 normalized = this.GetComponent<Rigidbody>().velocity.normalized;
      UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
      ((RagdollController) other.transform.root.GetComponent(typeof (RagdollController))).MakeRagdoll(normalized * 350f);
      Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
      if ((bool) (UnityEngine.Object) component)
        component.AddForce(normalized * 1100f);
      ((Enemy) other.transform.root.GetComponent(typeof (Enemy))).DropGun(Vector3.up);
    }
    if (!this.ready)
      return;
    this.ready = false;
    AudioSource component1 = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.objectImpactAudio, this.transform.position, Quaternion.identity).GetComponent<AudioSource>();
    Rigidbody component2 = this.GetComponent<Rigidbody>();
    float num2 = 1f;
    if ((bool) (UnityEngine.Object) component2)
      num2 = component2.mass;
    if ((double) num2 < 0.30000001192092896)
      num2 = 0.5f;
    if ((double) num2 > 1.0)
      num2 = 1f;
    double volume = (double) component1.volume;
    if ((double) num1 > 1.0)
      num1 = 1f;
    component1.volume = num1 * num2;
    this.Invoke("GetReady", 0.1f);
  }

  private void GetReady() => this.ready = true;
}
