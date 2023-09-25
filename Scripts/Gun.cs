// Decompiled with JetBrains decompiler
// Type: Gun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Gun : MonoBehaviour
{
  private Vector3 rotationVel;
  private float speed = 8f;
  private float posSpeed = 0.075f;
  private float posOffset = 0.004f;
  private Vector3 defaultPos;
  private Vector3 posVel;
  private Rigidbody rb;
  private float rotationOffset;
  private float rotationOffsetZ;
  private float rotVelY;
  private float rotVelZ;
  private Vector3 prevRotation;
  private Vector3 desiredBob;
  private float xBob = 0.12f;
  private float yBob = 0.08f;
  private float zBob = 0.1f;
  private float bobSpeed = 0.45f;

  public static Gun Instance { get; set; }

  private void Start()
  {
    Gun.Instance = this;
    this.defaultPos = this.transform.localPosition;
    this.rb = PlayerMovement.Instance.GetRb();
  }

  private void Update()
  {
    if ((bool) (UnityEngine.Object) PlayerMovement.Instance && !PlayerMovement.Instance.HasGun())
      return;
    this.MoveGun();
    Vector3 grapplePoint = PlayerMovement.Instance.GetGrapplePoint();
    Quaternion b = Quaternion.LookRotation((PlayerMovement.Instance.GetGrapplePoint() - this.transform.position).normalized);
    double rotationOffset = (double) this.rotationOffset;
    Quaternion rotation = this.transform.parent.rotation;
    double num = (double) Mathf.DeltaAngle(rotation.eulerAngles.y, this.prevRotation.y);
    this.rotationOffset = (float) (rotationOffset + num);
    if ((double) this.rotationOffset > 90.0)
      this.rotationOffset = 90f;
    if ((double) this.rotationOffset < -90.0)
      this.rotationOffset = -90f;
    this.rotationOffset = Mathf.SmoothDamp(this.rotationOffset, 0.0f, ref this.rotVelY, 0.025f);
    Vector3 vector3_1 = new Vector3(0.0f, this.rotationOffset, this.rotationOffset);
    Vector3 zero = Vector3.zero;
    if (grapplePoint == zero)
    {
      rotation = this.transform.parent.rotation;
      b = Quaternion.Euler(rotation.eulerAngles - vector3_1);
    }
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, Time.deltaTime * this.speed);
    Vector3 vector3_2 = (Vector3) (PlayerMovement.Instance.FindVelRelativeToLook() * this.posOffset);
    float y = PlayerMovement.Instance.GetFallSpeed() * this.posOffset;
    if ((double) y < -0.079999998211860657)
      y = -0.08f;
    this.transform.localPosition = Vector3.SmoothDamp(this.transform.localPosition, this.defaultPos - new Vector3(vector3_2.x, y, vector3_2.y) + this.desiredBob, ref this.posVel, this.posSpeed);
    rotation = this.transform.parent.rotation;
    this.prevRotation = rotation.eulerAngles;
  }

  private void MoveGun()
  {
    if (!(bool) (UnityEngine.Object) this.rb || !PlayerMovement.Instance.grounded)
      return;
    if ((double) Mathf.Abs(this.rb.velocity.magnitude) < 4.0)
      this.desiredBob = Vector3.zero;
    else
      this.desiredBob = new Vector3(Mathf.PingPong(Time.time * this.bobSpeed, this.xBob) - this.xBob / 2f, Mathf.PingPong(Time.time * this.bobSpeed, this.yBob) - this.yBob / 2f, Mathf.PingPong(Time.time * this.bobSpeed, this.zBob) - this.zBob / 2f);
  }

  public void Shoot()
  {
    float recoil = PlayerMovement.Instance.GetRecoil();
    this.transform.localPosition = this.transform.localPosition + (Vector3.up / 4f + Vector3.back / 1.5f) * recoil;
    this.transform.localRotation = Quaternion.Euler(-60f * recoil, 0.0f, 0.0f);
  }
}
