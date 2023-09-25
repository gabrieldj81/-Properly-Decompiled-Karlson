// Decompiled with JetBrains decompiler
// Type: DetectWeapons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using Audio;
using System.Collections.Generic;
using UnityEngine;

public class DetectWeapons : MonoBehaviour
{
  public Transform weaponPos;
  private List<GameObject> weapons;
  private bool hasGun;
  private GameObject gun;
  private global::Pickup gunScript;
  private float speed = 10f;
  private Quaternion desiredRot = Quaternion.Euler(0.0f, 90f, 0.0f);
  private Vector3 desiredPos = Vector3.zero;
  private Vector3 posVel;
  private float throwForce = 1000f;
  private Vector3 scale;

  public void Pickup()
  {
    if (this.hasGun || !this.HasWeapons())
      return;
    this.gun = this.GetWeapon();
    this.gunScript = (global::Pickup) this.gun.GetComponent(typeof (global::Pickup));
    if (this.gunScript.pickedUp)
    {
      this.gun = (GameObject) null;
      this.gunScript = (global::Pickup) null;
    }
    else
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gun.GetComponent<Rigidbody>());
      this.scale = this.gun.transform.localScale;
      this.gun.transform.parent = this.weaponPos;
      this.gun.transform.localScale = this.scale;
      this.hasGun = true;
      this.gunScript.PickupWeapon(true);
      AudioManager.Instance.Play("GunPickup");
      this.gun.layer = LayerMask.NameToLayer("Equipable");
    }
  }

  public void Shoot(Vector3 dir)
  {
    if (!this.hasGun)
      return;
    this.gunScript.Use(dir);
  }

  public void StopUse()
  {
    if (!this.hasGun)
      return;
    this.gunScript.StopUse();
  }

  public void Throw(Vector3 throwDir)
  {
    if (!this.hasGun || (bool) (UnityEngine.Object) this.gun.GetComponent<Rigidbody>())
      return;
    this.gunScript.StopUse();
    this.hasGun = false;
    Rigidbody rigidbody = this.gun.AddComponent<Rigidbody>();
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.maxAngularVelocity = 20f;
    rigidbody.AddForce(throwDir * this.throwForce);
    rigidbody.AddRelativeTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f) * 0.4f), ForceMode.Impulse);
    this.gun.layer = LayerMask.NameToLayer("Gun");
    this.gunScript.Drop();
    this.gun.transform.parent = (Transform) null;
    this.gun.transform.localScale = this.scale;
    this.gun = (GameObject) null;
    this.gunScript = (global::Pickup) null;
  }

  public void Fire(Vector3 dir) => this.gunScript.Use(dir);

  private void Update()
  {
    if (!this.hasGun)
      return;
    this.gun.transform.localRotation = Quaternion.Slerp(this.gun.transform.localRotation, this.desiredRot, Time.deltaTime * this.speed);
    this.gun.transform.localPosition = Vector3.SmoothDamp(this.gun.transform.localPosition, this.desiredPos, ref this.posVel, 1f / this.speed);
    this.gunScript.OnAim();
  }

  private void Start() => this.weapons = new List<GameObject>();

  private void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Gun") || this.weapons.Contains(other.gameObject))
      return;
    this.weapons.Add(other.gameObject);
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.CompareTag("Gun") || !this.weapons.Contains(other.gameObject))
      return;
    this.weapons.Remove(other.gameObject);
  }

  public GameObject GetWeapon()
  {
    if (this.weapons.Count == 1)
      return this.weapons[0];
    GameObject weapon1 = (GameObject) null;
    float num1 = float.PositiveInfinity;
    foreach (GameObject weapon2 in this.weapons)
    {
      float num2 = Vector3.Distance(this.transform.position, weapon2.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        weapon1 = weapon2;
      }
    }
    return weapon1;
  }

  public void ForcePickup(GameObject gun)
  {
    this.gunScript = (global::Pickup) gun.GetComponent(typeof (global::Pickup));
    this.gun = gun;
    if (this.gunScript.pickedUp)
    {
      gun = (GameObject) null;
      this.gunScript = (global::Pickup) null;
    }
    else
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) gun.GetComponent<Rigidbody>());
      this.scale = gun.transform.localScale;
      gun.transform.parent = this.weaponPos;
      gun.transform.localScale = this.scale;
      this.hasGun = true;
      this.gunScript.PickupWeapon(true);
      gun.layer = LayerMask.NameToLayer("Equipable");
    }
  }

  public float GetRecoil() => this.gunScript.recoil;

  public bool HasWeapons() => this.weapons.Count > 0;

  public bool IsGrappler() => (bool) (UnityEngine.Object) this.gun && (bool) (UnityEngine.Object) this.gun.GetComponent(typeof (Grappler));

  public Vector3 GetGrapplerPoint() => this.IsGrappler() ? ((Grappler) this.gun.GetComponent(typeof (Grappler))).GetGrapplePoint() : Vector3.zero;

  public global::Pickup GetWeaponScript() => this.gunScript;

  public bool HasGun() => this.hasGun;
}
