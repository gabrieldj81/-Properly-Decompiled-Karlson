// Decompiled with JetBrains decompiler
// Type: Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using Audio;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public bool changeCol;
  public bool player;
  private float damage;
  private float push;
  private bool done;
  private Color col;
  public bool explosive;
  private GameObject limbHit;
  private Rigidbody rb;

  private void Start() => this.rb = this.GetComponent<Rigidbody>();

  private void OnCollisionEnter(Collision other)
  {
    if (this.done)
      return;
    this.done = true;
    if (this.explosive)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      ((Explosion) UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, other.contacts[0].point, Quaternion.identity).GetComponentInChildren(typeof (Explosion))).player = this.player;
    }
    else
    {
      this.BulletExplosion(other.contacts[0]);
      UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletHitAudio, other.contacts[0].point, Quaternion.identity);
      int layer = other.gameObject.layer;
      if (layer == LayerMask.NameToLayer("Player"))
      {
        this.HitPlayer(other.gameObject);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else if (layer == LayerMask.NameToLayer("Enemy"))
      {
        if (this.col == Color.blue)
        {
          AudioManager.Instance.Play("Hitmarker");
          MonoBehaviour.print((object) "HITMARKER");
        }
        UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
        ((RagdollController) other.transform.root.GetComponent(typeof (RagdollController))).MakeRagdoll(-this.transform.right * 350f);
        if ((bool) (UnityEngine.Object) other.gameObject.GetComponent<Rigidbody>())
          other.gameObject.GetComponent<Rigidbody>().AddForce(-this.transform.right * 1500f);
        ((Enemy) other.transform.root.GetComponent(typeof (Enemy))).DropGun(Vector3.up);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        if (layer == LayerMask.NameToLayer(nameof (Bullet)))
        {
          if (other.gameObject.name == this.gameObject.name)
            return;
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
          UnityEngine.Object.Destroy((UnityEngine.Object) other.gameObject);
          this.BulletExplosion(other.contacts[0]);
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
  }

  private void HitPlayer(GameObject other) => PlayerMovement.Instance.KillPlayer();

  private void Update()
  {
    if (!this.explosive)
      return;
    this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
  }

  private void BulletExplosion(ContactPoint contact)
  {
    Vector3 point = contact.point;
    Vector3 normal = contact.normal;
    ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletDestroy, point + normal * 0.05f, Quaternion.identity).GetComponent<ParticleSystem>();
    component.transform.rotation = Quaternion.LookRotation(normal);
    component.startColor = Color.blue;
  }

  public void SetBullet(float damage, float push, Color col)
  {
    this.damage = damage;
    this.push = push;
    this.col = col;
    if (this.changeCol)
    {
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = col;
    }
    TrailRenderer componentInChildren = this.GetComponentInChildren<TrailRenderer>();
    if ((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null)
      return;
    componentInChildren.startColor = col;
    componentInChildren.endColor = col;
  }
}
