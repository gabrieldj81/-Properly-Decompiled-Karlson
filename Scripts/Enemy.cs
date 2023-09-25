// Decompiled with JetBrains decompiler
// Type: Enemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
  private float hipSpeed = 3f;
  private float headAndHandSpeed = 4f;
  private Transform target;
  public LayerMask objectsAndPlayer;
  private NavMeshAgent agent;
  private bool spottedPlayer;
  private Animator animator;
  public GameObject startGun;
  public Transform gunPosition;
  private Weapon gunScript;
  public GameObject currentGun;
  private float attackSpeed;
  private bool readyToShoot;
  private RagdollController ragdoll;
  public Transform leftArm;
  public Transform rightArm;
  public Transform head;
  public Transform hips;
  public Transform player;
  private bool takingAim;

  private void Start()
  {
    this.ragdoll = (RagdollController) this.GetComponent(typeof (RagdollController));
    this.animator = this.GetComponentInChildren<Animator>();
    this.agent = this.GetComponent<NavMeshAgent>();
    this.GiveGun();
  }

  private void LateUpdate()
  {
    this.FindPlayer();
    this.Aim();
  }

  private void Aim()
  {
    if ((UnityEngine.Object) this.currentGun == (UnityEngine.Object) null || this.ragdoll.IsRagdoll() || !this.animator.GetBool("Aiming"))
      return;
    Vector3 vector3 = this.target.transform.position - this.transform.position;
    if ((double) Vector3.Angle(this.transform.forward, vector3) > 70.0)
      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(vector3), Time.deltaTime * this.hipSpeed);
    this.head.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector3), Time.deltaTime * this.headAndHandSpeed);
    this.rightArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector3), Time.deltaTime * this.headAndHandSpeed);
    this.leftArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector3), Time.deltaTime * this.headAndHandSpeed);
    if (!this.readyToShoot)
      return;
    this.gunScript.Use(this.target.position);
    this.readyToShoot = false;
    this.Invoke("Cooldown", this.attackSpeed + Random.Range(this.attackSpeed, this.attackSpeed * 5f));
  }

  private void FindPlayer()
  {
    this.FindTarget();
    if (!(bool) (UnityEngine.Object) this.agent || !(bool) (UnityEngine.Object) this.target)
      return;
    Vector3 normalized = (this.target.position - this.transform.position).normalized;
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.transform.position + normalized, normalized, (float) (int) this.objectsAndPlayer);
    if (raycastHitArray.Length < 1)
      return;
    bool flag = false;
    float num1 = 1001f;
    float num2 = 1000f;
    for (int index = 0; index < raycastHitArray.Length; ++index)
    {
      int layer = raycastHitArray[index].collider.gameObject.layer;
      if (!(raycastHitArray[index].collider.transform.root.gameObject.name == this.gameObject.name) && layer != LayerMask.NameToLayer("TransparentFX"))
      {
        if (layer == LayerMask.NameToLayer("Player"))
        {
          num1 = raycastHitArray[index].distance;
          flag = true;
        }
        else if ((double) raycastHitArray[index].distance < (double) num2)
          num2 = raycastHitArray[index].distance;
      }
    }
    if (!flag)
      return;
    if ((double) num2 < (double) num1 && (double) num1 != 1001.0)
    {
      this.readyToShoot = false;
      if (this.animator.GetBool("Running") && (double) this.agent.remainingDistance < 0.20000000298023224)
      {
        this.animator.SetBool("Running", false);
        this.spottedPlayer = false;
      }
      if (!this.spottedPlayer || !this.agent.isOnNavMesh || this.animator.GetBool("Running"))
        return;
      MonoBehaviour.print((object) "oof");
      this.takingAim = false;
      this.agent.destination = this.target.transform.position;
      this.animator.SetBool("Running", true);
      this.animator.SetBool("Aiming", false);
      this.readyToShoot = false;
    }
    else
    {
      if (this.takingAim || this.animator.GetBool("Aiming"))
        return;
      if (!this.spottedPlayer)
        this.spottedPlayer = true;
      this.Invoke("TakeAim", Random.Range(0.3f, 1f));
      this.takingAim = true;
    }
  }

  private void TakeAim()
  {
    this.animator.SetBool("Running", false);
    this.animator.SetBool("Aiming", true);
    this.CancelInvoke();
    this.Invoke("Cooldown", Random.Range(0.3f, 1f));
    if (!(bool) (UnityEngine.Object) this.agent || !this.agent.isOnNavMesh)
      return;
    this.agent.destination = this.transform.position;
  }

  private void GiveGun()
  {
    if ((UnityEngine.Object) this.startGun == (UnityEngine.Object) null)
      return;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.startGun);
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject.GetComponent<Rigidbody>());
    this.gunScript = (Weapon) gameObject.GetComponent(typeof (Weapon));
    this.gunScript.PickupWeapon(false);
    gameObject.transform.parent = this.gunPosition;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localRotation = Quaternion.identity;
    this.currentGun = gameObject;
    this.attackSpeed = this.gunScript.GetAttackSpeed();
  }

  private void Cooldown() => this.readyToShoot = true;

  private void FindTarget()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null || !(bool) (UnityEngine.Object) PlayerMovement.Instance)
      return;
    this.target = PlayerMovement.Instance.playerCam;
  }

  public void DropGun(Vector3 dir)
  {
    if ((UnityEngine.Object) this.gunScript == (UnityEngine.Object) null)
      return;
    this.gunScript.Drop();
    Rigidbody rigidbody = this.currentGun.AddComponent<Rigidbody>();
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    this.currentGun.transform.parent = (Transform) null;
    rigidbody.AddForce(dir, ForceMode.Impulse);
    float num = 10f;
    rigidbody.AddTorque(new Vector3((float) Random.Range(-1, 1), (float) Random.Range(-1, 1), (float) Random.Range(-1, 1)) * num);
    this.gunScript = (Weapon) null;
  }

  public bool IsDead() => this.ragdoll.IsRagdoll();
}
