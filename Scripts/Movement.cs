// Decompiled with JetBrains decompiler
// Type: Movement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using Audio;
using EZCameraShake;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
  public GameObject spawnWeapon;
  private float sensitivity = 50f;
  private float sensMultiplier = 1f;
  private bool dead;
  public PhysicMaterial deadMat;
  public Transform playerCam;
  public Transform orientation;
  public Transform gun;
  private float xRotation;
  public Rigidbody rb;
  private float moveSpeed = 4500f;
  private float walkSpeed = 20f;
  private float runSpeed = 10f;
  public bool grounded;
  public Transform groundChecker;
  public LayerMask whatIsGround;
  private bool readyToJump;
  private float jumpCooldown = 0.2f;
  private float jumpForce = 550f;
  private float x;
  private float y;
  private bool jumping;
  private bool sprinting;
  private bool crouching;
  public LineRenderer lr;
  private Vector3 grapplePoint;
  private SpringJoint joint;
  private Vector3 normalVector;
  private Vector3 wallNormalVector;
  private bool wallRunning;
  private Vector3 wallRunPos;
  private DetectWeapons detectWeapons;
  public ParticleSystem ps;
  private ParticleSystem.EmissionModule psEmission;
  private Collider playerCollider;
  public bool paused;
  public LayerMask whatIsGrabbable;
  private Rigidbody objectGrabbing;
  private Vector3 previousLookdir;
  private Vector3 grabPoint;
  private float dragForce = 700000f;
  private SpringJoint grabJoint;
  private LineRenderer grabLr;
  private Vector3 myGrabPoint;
  private Vector3 myHandPoint;
  private Vector3 endPoint;
  private Vector3 grappleVel;
  private float offsetMultiplier;
  private float offsetVel;
  private float distance;
  private float actualWallRotation;
  private float wallRotationVel;
  private float desiredX;
  private float wallRunRotation;
  private bool airborne;
  private bool touchingGround;
  public LayerMask whatIsHittable;
  private float desiredTimeScale = 1f;
  private float timeScaleVel;

  public static Movement Instance { get; private set; }

  private void Awake()
  {
    Movement.Instance = this;
    this.rb = this.GetComponent<Rigidbody>();
    MonoBehaviour.print((object) ("rb: " + (object) this.rb));
  }

  private void Start()
  {
    this.psEmission = this.ps.emission;
    this.playerCollider = this.GetComponent<Collider>();
    this.detectWeapons = (DetectWeapons) this.GetComponentInChildren(typeof (DetectWeapons));
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    this.readyToJump = true;
    this.wallNormalVector = Vector3.up;
    this.CameraShake();
    if ((UnityEngine.Object) this.spawnWeapon != (UnityEngine.Object) null)
      this.detectWeapons.ForcePickup(UnityEngine.Object.Instantiate<GameObject>(this.spawnWeapon, this.transform.position, Quaternion.identity));
    if (!(bool) (UnityEngine.Object) GameState.Instance)
      return;
    this.sensMultiplier = GameState.Instance.GetSensitivity();
  }

  private void LateUpdate()
  {
    if (this.dead || this.paused)
      return;
    this.DrawGrapple();
    this.DrawGrabbing();
    this.WallRunning();
  }

  private void FixedUpdate()
  {
    if (this.dead || Game.Instance.done || this.paused)
      return;
    this.Move();
  }

  private void Update()
  {
    this.MyInput();
    if (this.dead || Game.Instance.done || this.paused)
      return;
    this.Look();
    this.DrawGrabbing();
    this.UpdateTimescale();
    if ((double) this.transform.position.y >= -200.0)
      return;
    this.KillPlayer();
  }

  private void MyInput()
  {
    if (this.dead || Game.Instance.done)
      return;
    this.x = Input.GetAxisRaw("Horizontal");
    this.y = Input.GetAxisRaw("Vertical");
    this.jumping = Input.GetButton("Jump");
    this.sprinting = Input.GetKey(KeyCode.LeftShift);
    this.crouching = Input.GetKey(KeyCode.LeftControl);
    if (Input.GetKeyDown(KeyCode.LeftControl))
      this.StartCrouch();
    if (Input.GetKeyUp(KeyCode.LeftControl))
      this.StopCrouch();
    if (Input.GetKey(KeyCode.Mouse0))
    {
      if (this.detectWeapons.HasGun())
        this.detectWeapons.Shoot(this.HitPoint());
      else
        this.GrabObject();
    }
    if (Input.GetKeyUp(KeyCode.Mouse0))
    {
      this.detectWeapons.StopUse();
      if ((bool) (UnityEngine.Object) this.objectGrabbing)
        this.StopGrab();
    }
    if (Input.GetKeyDown(KeyCode.E))
      this.detectWeapons.Pickup();
    if (Input.GetKeyDown(KeyCode.Q))
      this.detectWeapons.Throw((this.HitPoint() - this.detectWeapons.weaponPos.position).normalized);
    if (!Input.GetKeyDown(KeyCode.Escape))
      return;
    this.Pause();
  }

  private void Pause()
  {
    if (this.dead)
      return;
    if (this.paused)
    {
      Time.timeScale = 1f;
      UIManger.Instance.DeadUI(false);
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      this.paused = false;
    }
    else
    {
      this.paused = true;
      Time.timeScale = 0.0f;
      UIManger.Instance.DeadUI(true);
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  private void UpdateTimescale()
  {
    if (Game.Instance.done || this.paused || this.dead)
      return;
    Time.timeScale = Mathf.SmoothDamp(Time.timeScale, this.desiredTimeScale, ref this.timeScaleVel, 0.15f);
  }

  private void GrabObject()
  {
    if ((UnityEngine.Object) this.objectGrabbing == (UnityEngine.Object) null)
      this.StartGrab();
    else
      this.HoldGrab();
  }

  private void DrawGrabbing()
  {
    if (!(bool) (UnityEngine.Object) this.objectGrabbing)
      return;
    this.myGrabPoint = Vector3.Lerp(this.myGrabPoint, this.objectGrabbing.position, Time.deltaTime * 45f);
    this.myHandPoint = Vector3.Lerp(this.myHandPoint, this.grabJoint.connectedAnchor, Time.deltaTime * 45f);
    this.grabLr.SetPosition(0, this.myGrabPoint);
    this.grabLr.SetPosition(1, this.myHandPoint);
  }

  private void StartGrab()
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 8f, (int) this.whatIsGrabbable);
    if (raycastHitArray.Length < 1)
      return;
    for (int index = 0; index < raycastHitArray.Length; ++index)
    {
      MonoBehaviour.print((object) ("testing on: " + (object) raycastHitArray[index].collider.gameObject.layer));
      if ((bool) (UnityEngine.Object) raycastHitArray[index].transform.GetComponent<Rigidbody>())
      {
        this.objectGrabbing = raycastHitArray[index].transform.GetComponent<Rigidbody>();
        this.grabPoint = raycastHitArray[index].point;
        this.grabJoint = this.objectGrabbing.gameObject.AddComponent<SpringJoint>();
        this.grabJoint.autoConfigureConnectedAnchor = false;
        this.grabJoint.minDistance = 0.0f;
        this.grabJoint.maxDistance = 0.0f;
        this.grabJoint.damper = 4f;
        this.grabJoint.spring = 40f;
        this.grabJoint.massScale = 5f;
        this.objectGrabbing.angularDrag = 5f;
        this.objectGrabbing.drag = 1f;
        this.previousLookdir = this.playerCam.transform.forward;
        this.grabLr = this.objectGrabbing.gameObject.AddComponent<LineRenderer>();
        this.grabLr.positionCount = 2;
        this.grabLr.startWidth = 0.05f;
        this.grabLr.material = new Material(Shader.Find("Sprites/Default"));
        this.grabLr.numCapVertices = 10;
        this.grabLr.numCornerVertices = 10;
        break;
      }
    }
  }

  private void HoldGrab()
  {
    this.grabJoint.connectedAnchor = this.playerCam.transform.position + this.playerCam.transform.forward * 5.5f;
    this.grabLr.startWidth = 0.0f;
    this.grabLr.endWidth = 0.0075f * this.objectGrabbing.velocity.magnitude;
    this.previousLookdir = this.playerCam.transform.forward;
  }

  private void StopGrab()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.grabJoint);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.grabLr);
    this.objectGrabbing.angularDrag = 0.05f;
    this.objectGrabbing.drag = 0.0f;
    this.objectGrabbing = (Rigidbody) null;
  }

  private void StartCrouch()
  {
    float num = 400f;
    this.transform.localScale = new Vector3(1f, 0.5f, 1f);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
    if ((double) this.rb.velocity.magnitude <= 0.10000000149011612 || !this.grounded)
      return;
    this.rb.AddForce(this.orientation.transform.forward * num);
    AudioManager.Instance.Play("StartSlide");
    AudioManager.Instance.Play("Slide");
  }

  private void StopCrouch()
  {
    this.transform.localScale = new Vector3(1f, 1.5f, 1f);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
  }

  private void StopGrapple()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.joint);
    this.grapplePoint = Vector3.zero;
  }

  private void StartGrapple()
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 70f, (int) this.whatIsGround);
    if (raycastHitArray.Length < 1)
      return;
    this.grapplePoint = raycastHitArray[0].point;
    this.joint = this.gameObject.AddComponent<SpringJoint>();
    this.joint.autoConfigureConnectedAnchor = false;
    this.joint.connectedAnchor = this.grapplePoint;
    this.joint.spring = 6.5f;
    this.joint.damper = 2f;
    this.joint.maxDistance = Vector2.Distance((Vector2) this.grapplePoint, (Vector2) this.transform.position) * 0.8f;
    this.joint.minDistance = Vector2.Distance((Vector2) this.grapplePoint, (Vector2) this.transform.position) * 0.25f;
    this.joint.spring = 4.5f;
    this.joint.damper = 7f;
    this.joint.massScale = 4.5f;
    this.endPoint = this.gun.transform.GetChild(0).position;
    this.offsetMultiplier = 2f;
  }

  private void DrawGrapple()
  {
    if (this.grapplePoint == Vector3.zero || (UnityEngine.Object) this.joint == (UnityEngine.Object) null)
    {
      this.lr.positionCount = 0;
    }
    else
    {
      this.lr.positionCount = 2;
      this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
      this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0.0f, ref this.offsetVel, 0.1f);
      int num1 = 100;
      this.lr.positionCount = num1;
      Vector3 position1 = this.gun.transform.GetChild(0).position;
      double num2 = (double) Vector3.Distance(this.endPoint, position1);
      this.lr.SetPosition(0, position1);
      this.lr.SetPosition(num1 - 1, this.endPoint);
      float num3 = (float) num2;
      float num4 = 1f;
      for (int index = 1; index < num1 - 1; ++index)
      {
        double num5 = (double) index / (double) num1;
        float num6 = (float) num5 * this.offsetMultiplier;
        float num7 = (float) (((double) Mathf.Sin(num6 * num3) - 0.5) * (double) num4 * ((double) num6 * 2.0));
        Vector3 normalized = (this.endPoint - position1).normalized;
        float num8 = Mathf.Sin((float) (num5 * 180.0 * (Math.PI / 180.0)));
        float num9 = Mathf.Cos((float) ((double) this.offsetMultiplier * 90.0 * (Math.PI / 180.0)));
        Vector3 position2 = position1 + (this.endPoint - position1) / (float) num1 * (float) index + ((Vector3) (num9 * num7 * Vector2.Perpendicular((Vector2) normalized)) + this.offsetMultiplier * num8 * Vector3.down);
        this.lr.SetPosition(index, position2);
      }
    }
  }

  private void WallRunning()
  {
    if (!this.wallRunning)
      return;
    this.rb.AddForce(-this.wallNormalVector * Time.deltaTime * this.moveSpeed);
  }

  private void FootSteps()
  {
    if (this.crouching || this.dead || !this.grounded && !this.wallRunning)
      return;
    float num1 = 1.2f;
    float num2 = this.rb.velocity.magnitude;
    if ((double) num2 > 20.0)
      num2 = 20f;
    this.distance += num2;
    if ((double) this.distance <= 300.0 / (double) num1)
      return;
    AudioManager.Instance.PlayFootStep();
    this.distance = 0.0f;
  }

  private void Move()
  {
    if (this.dead)
      return;
    this.grounded = Physics.OverlapSphere(this.groundChecker.position, 0.1f, (int) this.whatIsGround).Length != 0;
    if (!this.touchingGround)
      this.grounded = false;
    Vector2 velRelativeToLook = this.FindVelRelativeToLook();
    float x = velRelativeToLook.x;
    float y = velRelativeToLook.y;
    this.FootSteps();
    this.CounterMovement(this.x, this.y, velRelativeToLook);
    if (this.readyToJump && this.jumping)
      this.Jump();
    float num1 = this.walkSpeed;
    if (this.sprinting)
      num1 = this.runSpeed;
    if (this.crouching && this.grounded && this.readyToJump)
    {
      this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
    }
    else
    {
      if ((double) this.x > 0.0 && (double) x > (double) num1)
        this.x = 0.0f;
      if ((double) this.x < 0.0 && (double) x < -(double) num1)
        this.x = 0.0f;
      if ((double) this.y > 0.0 && (double) y > (double) num1)
        this.y = 0.0f;
      if ((double) this.y < 0.0 && (double) y < -(double) num1)
        this.y = 0.0f;
      float num2 = 1f;
      float num3 = 1f;
      if (!this.grounded)
        num2 = 0.5f;
      if (this.grounded && this.crouching)
        num3 = 0.0f;
      this.rb.AddForce(this.orientation.transform.forward * this.y * this.moveSpeed * Time.deltaTime * num2 * num3);
      this.rb.AddForce(this.orientation.transform.right * this.x * this.moveSpeed * Time.deltaTime * num2);
      this.SpeedLines();
    }
  }

  private void SpeedLines()
  {
    float num1 = Vector3.Angle(this.rb.velocity, this.playerCam.transform.forward) * 0.15f;
    if ((double) num1 < 1.0)
      num1 = 1f;
    float num2 = this.rb.velocity.magnitude / num1;
    if (this.grounded && !this.wallRunning)
      num2 = 0.0f;
    this.psEmission.rateOverTimeMultiplier = num2;
  }

  private void CameraShake()
  {
    float magnitude = this.rb.velocity.magnitude / 9f;
    CameraShaker.Instance.ShakeOnce(magnitude, 0.1f * magnitude, 0.25f, 0.2f);
    this.Invoke(nameof (CameraShake), 0.2f);
  }

  private void ResetJump()
  {
    this.readyToJump = true;
    MonoBehaviour.print((object) "reset");
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
    if (this.wallRunning)
      this.rb.AddForce(this.wallNormalVector * this.jumpForce * 1.5f);
    this.Invoke("ResetJump", this.jumpCooldown);
    if (this.wallRunning)
      this.wallRunning = false;
    AudioManager.Instance.PlayJump();
  }

  private void Look()
  {
    float num1 = Input.GetAxis("Mouse X") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
    float num2 = Input.GetAxis("Mouse Y") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
    this.desiredX = this.playerCam.transform.localRotation.eulerAngles.y + num1;
    this.xRotation -= num2;
    this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
    this.FindWallRunRotation();
    this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
    this.playerCam.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX, this.actualWallRotation);
    this.orientation.transform.localRotation = Quaternion.Euler(0.0f, this.desiredX, 0.0f);
  }

  private void FindWallRunRotation()
  {
    if (!this.wallRunning)
    {
      this.wallRunRotation = 0.0f;
    }
    else
    {
      Vector3 normalized = new Vector3(0.0f, this.playerCam.transform.rotation.y, 0.0f).normalized;
      Vector3 vector3 = new Vector3(0.0f, 0.0f, 1f);
      float num = 0.0f;
      double y = (double) this.playerCam.transform.rotation.eulerAngles.y;
      if ((double) Math.Abs(this.wallNormalVector.x - 1f) < 0.10000000149011612)
        num = 90f;
      else if ((double) Math.Abs(this.wallNormalVector.x - -1f) < 0.10000000149011612)
        num = 270f;
      else if ((double) Math.Abs(this.wallNormalVector.z - 1f) < 0.10000000149011612)
        num = 0.0f;
      else if ((double) Math.Abs(this.wallNormalVector.z - -1f) < 0.10000000149011612)
        num = 180f;
      double target = (double) Vector3.SignedAngle(new Vector3(0.0f, 0.0f, 1f), this.wallNormalVector, Vector3.up);
      this.wallRunRotation = (float) (-((double) Mathf.DeltaAngle((float) y, (float) target) / 90.0) * 15.0);
    }
  }

  private void CounterMovement(float x, float y, Vector2 mag)
  {
    if (!this.grounded)
      return;
    float num = 0.2f;
    if (this.crouching)
    {
      this.rb.AddForce(this.moveSpeed * Time.deltaTime * -this.rb.velocity.normalized * 0.045f);
    }
    else
    {
      if ((double) Math.Abs(x) < 0.05000000074505806 || (double) mag.x < 0.0 && (double) x > 0.0 || (double) mag.x > 0.0 && (double) x < 0.0)
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * Time.deltaTime * -mag.x * num);
      if ((double) Math.Abs(y) < 0.05000000074505806 || (double) mag.y < 0.0 && (double) y > 0.0 || (double) mag.y > 0.0 && (double) y < 0.0)
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * Time.deltaTime * -mag.y * num);
      if ((double) Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) <= 20.0)
        return;
      float y1 = this.rb.velocity.y;
      Vector3 vector3 = this.rb.velocity.normalized * 20f;
      this.rb.velocity = new Vector3(vector3.x, y1, vector3.z);
    }
  }

  public Vector2 FindVelRelativeToLook()
  {
    float num1 = Mathf.DeltaAngle(this.orientation.transform.eulerAngles.y, Mathf.Atan2(this.rb.velocity.x, this.rb.velocity.z) * 57.29578f);
    float num2 = 90f - num1;
    double magnitude = (double) this.rb.velocity.magnitude;
    float y = (float) magnitude * Mathf.Cos(num1 * ((float) Math.PI / 180f));
    return new Vector2((float) magnitude * Mathf.Cos(num2 * ((float) Math.PI / 180f)), y);
  }

  private void OnCollisionEnter(Collision other)
  {
    int layer = other.gameObject.layer;
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      CameraShaker.Instance.ShakeOnce(5.5f * GameState.Instance.cameraShake, 1.2f, 0.2f, 0.3f);
      if (this.wallRunning && (double) other.contacts[0].normal.y == -1.0)
      {
        MonoBehaviour.print((object) "ROOF");
        return;
      }
      this.wallNormalVector = other.contacts[0].normal;
      MonoBehaviour.print((object) ("nv: " + (object) this.wallNormalVector));
      AudioManager.Instance.PlayLanding();
      if ((double) Math.Abs(this.wallNormalVector.y) < 0.10000000149011612)
        this.StartWallRun();
      this.airborne = false;
    }
    if (layer != LayerMask.NameToLayer("Enemy") || this.grounded && !this.crouching)
      return;
    Vector3 velocity = this.rb.velocity;
    if ((double) velocity.magnitude < 3.0)
      return;
    Enemy component1 = (Enemy) other.transform.root.GetComponent(typeof (Enemy));
    if (!(bool) (UnityEngine.Object) component1 || component1.IsDead())
      return;
    UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
    RagdollController component2 = (RagdollController) other.transform.root.GetComponent(typeof (RagdollController));
    if (this.grounded && this.crouching)
    {
      component2.MakeRagdoll(this.rb.velocity * 1.2f * 34f);
    }
    else
    {
      RagdollController ragdollController = component2;
      velocity = this.rb.velocity;
      Vector3 dir = velocity.normalized * 250f;
      ragdollController.MakeRagdoll(dir);
    }
    Rigidbody rb = this.rb;
    velocity = this.rb.velocity;
    Vector3 force = velocity.normalized * 2f;
    rb.AddForce(force, ForceMode.Impulse);
    Enemy enemy = component1;
    velocity = this.rb.velocity;
    Vector3 dir1 = velocity.normalized * 2f;
    enemy.DropGun(dir1);
  }

  private void StartWallRun()
  {
    if (this.wallRunning)
      MonoBehaviour.print((object) "stopping since wallrunning");
    else if (this.touchingGround)
    {
      MonoBehaviour.print((object) "stopping since grounded");
    }
    else
    {
      MonoBehaviour.print((object) "got through");
      float num = 20f;
      this.wallRunning = true;
      this.rb.velocity = new Vector3(this.rb.velocity.x, 0.0f, this.rb.velocity.z);
      this.rb.AddForce(Vector3.up * num, ForceMode.Impulse);
    }
  }

  private void OnCollisionExit(Collision other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      if ((double) Math.Abs(this.wallNormalVector.y) < 0.10000000149011612)
      {
        MonoBehaviour.print((object) "oof");
        this.wallRunning = false;
        this.wallNormalVector = Vector3.up;
      }
      else
        this.touchingGround = false;
      this.airborne = true;
    }
    if (other.gameObject.layer != LayerMask.NameToLayer("Object"))
      return;
    this.touchingGround = false;
  }

  private void OnCollisionStay(Collision other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && (double) Math.Abs(other.contacts[0].normal.y) > 0.10000000149011612)
    {
      this.touchingGround = true;
      this.wallRunning = false;
    }
    if (other.gameObject.layer != LayerMask.NameToLayer("Object"))
      return;
    this.touchingGround = true;
  }

  public Vector3 GetVelocity() => this.rb.velocity;

  public float GetFallSpeed() => this.rb.velocity.y;

  public Vector3 GetGrapplePoint() => this.detectWeapons.GetGrapplerPoint();

  public Collider GetPlayerCollider() => this.playerCollider;

  public Transform GetPlayerCamTransform() => this.playerCam.transform;

  public Vector3 HitPoint()
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, (float) (int) this.whatIsHittable);
    if (raycastHitArray.Length < 1)
      return this.playerCam.transform.position + this.playerCam.transform.forward * 100f;
    if (raycastHitArray.Length > 1)
    {
      for (int index = 0; index < raycastHitArray.Length; ++index)
      {
        if (raycastHitArray[index].transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
          return raycastHitArray[index].point;
      }
    }
    return raycastHitArray[0].point;
  }

  public float GetRecoil() => this.detectWeapons.GetRecoil();

  public void KillPlayer()
  {
    if (Game.Instance.done)
      return;
    CameraShaker.Instance.ShakeOnce(3f * GameState.Instance.cameraShake, 2f, 0.1f, 0.6f);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    UIManger.Instance.DeadUI(true);
    Timer.Instance.Stop();
    this.dead = true;
    this.rb.freezeRotation = false;
    this.playerCollider.material = this.deadMat;
    this.detectWeapons.Throw(Vector3.zero);
    this.paused = false;
    this.ResetSlowmo();
  }

  public void Respawn() => this.detectWeapons.StopUse();

  public void Slowmo(float timescale, float length)
  {
    if (!GameState.Instance.shake)
      return;
    this.CancelInvoke(nameof (Slowmo));
    this.desiredTimeScale = timescale;
    this.Invoke("ResetSlowmo", length);
    AudioManager.Instance.Play("SlowmoStart");
  }

  private void ResetSlowmo()
  {
    this.desiredTimeScale = 1f;
    AudioManager.Instance.Play("SlowmoEnd");
  }

  public bool IsCrouching() => this.crouching;

  public bool HasGun() => this.detectWeapons.HasGun();

  public bool IsDead() => this.dead;

  public Rigidbody GetRb() => this.rb;
}
