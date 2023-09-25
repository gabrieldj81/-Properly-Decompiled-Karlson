// Decompiled with JetBrains decompiler
// Type: Pickup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class Pickup : MonoBehaviour, IPickup
{
  protected bool player;
  private bool thrown;
  public float recoil;
  private Transform outline;

  public bool pickedUp { get; set; }

  public bool readyToUse { get; set; }

  private void Awake()
  {
    this.readyToUse = true;
    this.outline = this.transform.GetChild(1);
  }

  private void Update()
  {
    int num = this.pickedUp ? 1 : 0;
  }

  public void PickupWeapon(bool player)
  {
    this.pickedUp = true;
    this.player = player;
    this.outline.gameObject.SetActive(false);
  }

  public void Drop()
  {
    this.readyToUse = true;
    this.Invoke("DropWeapon", 0.5f);
    this.thrown = true;
  }

  private void DropWeapon()
  {
    this.CancelInvoke();
    this.pickedUp = false;
    this.outline.gameObject.SetActive(true);
  }

  public abstract void Use(Vector3 attackDirection);

  public abstract void OnAim();

  public abstract void StopUse();

  public bool IsPickedUp() => this.pickedUp;

  private void OnCollisionEnter(Collision other)
  {
    if (!this.thrown)
      return;
    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    {
      UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
      ((RagdollController) other.transform.root.GetComponent(typeof (RagdollController))).MakeRagdoll(-this.transform.right * 60f);
      Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
      if ((bool) (UnityEngine.Object) component)
        component.AddForce(-this.transform.right * 1500f);
      ((Enemy) other.transform.root.GetComponent(typeof (Enemy))).DropGun(Vector3.up);
    }
    this.thrown = false;
  }
}
