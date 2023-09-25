// Decompiled with JetBrains decompiler
// Type: Actor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
  public Transform gunPosition;
  public Transform orientation;
  private float xRotation;
  private Rigidbody rb;
  private float accelerationSpeed = 4500f;
  private float maxSpeed = 20f;
  private bool crouching;
  private bool jumping;
  private bool wallRunning;
  protected float x;
  protected float y;
  private Vector3 wallNormalVector = Vector3.up;
  private bool grounded;
  public Transform groundChecker;
  public LayerMask whatIsGround;
  private bool readyToJump;
  private float jumpCooldown = 0.2f;
  private float jumpForce = 500f;

  private void Awake()
  {
    this.rb = this.GetComponent<Rigidbody>();
    this.OnStart();
  }

  private void FixedUpdate()
  {
    this.Movement();
    this.RotateBody();
  }

  private void LateUpdate() => this.Look();

  private void Update() => this.Logic();

  protected abstract void OnStart();

  protected abstract void Logic();

  protected abstract void RotateBody();

  protected abstract void Look();

  private void Movement()
  {
    this.grounded = Physics.OverlapSphere(this.groundChecker.position, 0.1f, (int) this.whatIsGround).Length != 0;
    Vector2 velRelativeToLook = this.FindVelRelativeToLook();
    float x = velRelativeToLook.x;
    float y = velRelativeToLook.y;
    this.CounterMovement(this.x, this.y, velRelativeToLook);
    if (this.readyToJump && this.jumping)
      this.Jump();
    if (this.crouching && this.grounded && this.readyToJump)
    {
      this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
    }
    else
    {
      if ((double) this.x > 0.0 && (double) x > (double) this.maxSpeed)
        this.x = 0.0f;
      if ((double) this.x < 0.0 && (double) x < -(double) this.maxSpeed)
        this.x = 0.0f;
      if ((double) this.y > 0.0 && (double) y > (double) this.maxSpeed)
        this.y = 0.0f;
      if ((double) this.y < 0.0 && (double) y < -(double) this.maxSpeed)
        this.y = 0.0f;
      this.rb.AddForce(Time.deltaTime * this.y * this.accelerationSpeed * this.orientation.transform.forward);
      this.rb.AddForce(Time.deltaTime * this.x * this.accelerationSpeed * this.orientation.transform.right);
    }
  }

  private void Jump()
  {
    if (!this.grounded && !this.wallRunning)
      return;
    Vector3 velocity = this.rb.velocity;
    this.rb.velocity = new Vector3(velocity.x, 0.0f, velocity.z);
    this.readyToJump = false;
    this.rb.AddForce((Vector3) (Vector2.up * this.jumpForce * 1.5f));
    this.rb.AddForce(this.wallNormalVector * this.jumpForce * 0.5f);
    this.Invoke("ResetJump", this.jumpCooldown);
    if (!this.wallRunning)
      return;
    this.wallRunning = false;
  }

  private void ResetJump() => this.readyToJump = true;

  protected void CounterMovement(float x, float y, Vector2 mag)
  {
    if (!this.grounded || this.crouching)
      return;
    float num = 0.2f;
    if ((double) x == 0.0 || (double) mag.x < 0.0 && (double) x > 0.0 || (double) mag.x > 0.0 && (double) x < 0.0)
      this.rb.AddForce((float) ((double) this.accelerationSpeed * (double) num * (double) Time.deltaTime * -(double) mag.x) * this.orientation.transform.right);
    if ((double) y == 0.0 || (double) mag.y < 0.0 && (double) y > 0.0 || (double) mag.y > 0.0 && (double) y < 0.0)
      this.rb.AddForce((float) ((double) this.accelerationSpeed * (double) num * (double) Time.deltaTime * -(double) mag.y) * this.orientation.transform.forward);
    if ((double) Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) <= 20.0)
      return;
    float y1 = this.rb.velocity.y;
    Vector3 vector3 = this.rb.velocity.normalized * 20f;
    this.rb.velocity = new Vector3(vector3.x, y1, vector3.z);
  }

  protected Vector2 FindVelRelativeToLook()
  {
    double y1 = (double) this.orientation.transform.eulerAngles.y;
    Vector3 velocity = this.rb.velocity;
    double target = (double) (Mathf.Atan2(velocity.x, velocity.z) * 57.29578f);
    float num1 = Mathf.DeltaAngle((float) y1, (float) target);
    float num2 = 90f - num1;
    double magnitude = (double) this.rb.velocity.magnitude;
    float y2 = (float) magnitude * Mathf.Cos(num1 * ((float) Math.PI / 180f));
    return new Vector2((float) magnitude * Mathf.Cos(num2 * ((float) Math.PI / 180f)), y2);
  }
}
