// Decompiled with JetBrains decompiler
// Type: RangedWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using Audio;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
  public GameObject projectile;
  public float pushBackForce;
  public float force;
  public float accuracy;
  public int bullets;
  public float boostRecoil;
  private Transform guntip;
  private Rigidbody rb;
  private Collider[] projectileColliders;

  private new void Start()
  {
    base.Start();
    this.rb = this.GetComponent<Rigidbody>();
    this.guntip = this.transform.GetChild(0);
  }

  public override void Use(Vector3 attackDirection)
  {
    if (!this.readyToUse || !this.pickedUp)
      return;
    this.SpawnProjectile(attackDirection);
    this.Recoil();
    this.readyToUse = false;
    this.Invoke("GetReady", this.attackSpeed);
  }

  public override void OnAim()
  {
  }

  public override void StopUse()
  {
  }

  private void SpawnProjectile(Vector3 attackDirection)
  {
    Vector3 position = this.guntip.position - this.guntip.transform.right / 4f;
    Vector3 normalized = (attackDirection - position).normalized;
    List<Collider> colliderList = new List<Collider>();
    if (this.player)
      PlayerMovement.Instance.GetRb().AddForce(this.transform.right * this.boostRecoil, ForceMode.Impulse);
    for (int index = 0; index < this.bullets; ++index)
    {
      UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.muzzle, position, Quaternion.identity);
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectile, position, this.transform.rotation);
      Rigidbody componentInChildren = gameObject.GetComponentInChildren<Rigidbody>();
      this.projectileColliders = gameObject.GetComponentsInChildren<Collider>();
      this.RemoveCollisionWithPlayer();
      componentInChildren.transform.rotation = this.transform.rotation;
      Vector3 vector3 = normalized + (this.guntip.transform.up * Random.Range(-this.accuracy, this.accuracy) + this.guntip.transform.forward * Random.Range(-this.accuracy, this.accuracy));
      componentInChildren.AddForce(componentInChildren.mass * this.force * vector3);
      Bullet component = (Bullet) gameObject.GetComponent(typeof (Bullet));
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Color col = Color.red;
        if (this.player)
        {
          col = Color.blue;
          Gun.Instance.Shoot();
          if (component.explosive)
          {
            UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, this.transform.position, Quaternion.identity);
          }
          else
          {
            AudioManager.Instance.PlayPitched("GunBass", 0.3f);
            AudioManager.Instance.PlayPitched("GunHigh", 0.3f);
            AudioManager.Instance.PlayPitched("GunLow", 0.3f);
          }
          componentInChildren.AddForce(componentInChildren.mass * this.force * vector3);
        }
        else
          UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.gunShotAudio, this.transform.position, Quaternion.identity);
        component.SetBullet(this.damage, this.pushBackForce, col);
        component.player = this.player;
      }
      foreach (Collider collider1 in colliderList)
        Physics.IgnoreCollision(collider1, this.projectileColliders[0]);
      colliderList.Add(this.projectileColliders[0]);
    }
  }

  private void GetReady() => this.readyToUse = true;

  private void Recoil()
  {
  }

  private void RemoveCollisionWithPlayer()
  {
    Collider[] colliderArray;
    if (this.player)
      colliderArray = new Collider[1]
      {
        PlayerMovement.Instance.GetPlayerCollider()
      };
    else
      colliderArray = this.transform.root.GetComponentsInChildren<Collider>();
    for (int index1 = 0; index1 < colliderArray.Length; ++index1)
    {
      for (int index2 = 0; index2 < this.projectileColliders.Length; ++index2)
        Physics.IgnoreCollision(colliderArray[index1], this.projectileColliders[index2], true);
    }
  }
}
