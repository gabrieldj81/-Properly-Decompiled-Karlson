// Decompiled with JetBrains decompiler
// Type: Explosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using EZCameraShake;
using UnityEngine;

public class Explosion : MonoBehaviour
{
  public bool player;

  private void Start()
  {
    float message = Vector3.Distance(this.transform.position, PlayerMovement.Instance.gameObject.transform.position);
    MonoBehaviour.print((object) message);
    float num = 10f / message;
    if ((double) num < 0.10000000149011612)
      num = 0.0f;
    CameraShaker.Instance.ShakeOnce(20f * num * GameState.Instance.cameraShake, 2f, 0.4f, 0.5f);
    MonoBehaviour.print((object) ("ratio: " + (object) num));
  }

  private void OnTriggerEnter(Collider other)
  {
    int layer = other.gameObject.layer;
    Vector3 normalized = (other.transform.position - this.transform.position).normalized;
    float num = Vector3.Distance(other.transform.position, this.transform.position);
    if (other.gameObject.CompareTag("Enemy"))
    {
      if (other.gameObject.name != "Torso")
        return;
      RagdollController component1 = (RagdollController) other.transform.root.GetComponent(typeof (RagdollController));
      if (!(bool) (UnityEngine.Object) component1 || component1.IsRagdoll())
        return;
      component1.MakeRagdoll(normalized * 1100f);
      if (this.player)
        PlayerMovement.Instance.Slowmo(0.35f, 0.5f);
      Enemy component2 = (Enemy) other.transform.root.GetComponent(typeof (Enemy));
      if (!(bool) (UnityEngine.Object) component2)
        return;
      component2.DropGun(Vector3.up);
    }
    else
    {
      Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
      if (!(bool) (UnityEngine.Object) component)
        return;
      if ((double) num < 5.0)
        num = 5f;
      component.AddForce(normalized * 450f / num, ForceMode.Impulse);
      component.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10f);
      if (layer != LayerMask.NameToLayer("Player"))
        return;
      ((PlayerMovement) other.transform.root.GetComponent(typeof (PlayerMovement))).Explode();
    }
  }
}
