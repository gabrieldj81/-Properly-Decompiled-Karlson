// Decompiled with JetBrains decompiler
// Type: Grappling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Grappling : MonoBehaviour
{
  private LineRenderer lr;
  public LayerMask whatIsSickoMode;
  private Transform connectedTransform;
  private SpringJoint joint;
  private Vector3 offsetPoint;
  private Vector3 endPoint;
  private Vector3 ropeVel;
  private Vector3 desiredPos;
  private float offsetMultiplier;
  private float offsetVel;
  private bool readyToUse = true;

  public static Grappling Instance { get; set; }

  private void Start()
  {
    Grappling.Instance = this;
    this.lr = this.GetComponentInChildren<LineRenderer>();
    this.lr.positionCount = 0;
  }

  private void Update()
  {
    this.DrawLine();
    if ((UnityEngine.Object) this.connectedTransform == (UnityEngine.Object) null)
      return;
    Vector2 normalized = (Vector2) (this.connectedTransform.position - this.transform.position).normalized;
    double num1 = (double) Mathf.Atan2(normalized.y, normalized.x);
    int num2 = (UnityEngine.Object) this.joint == (UnityEngine.Object) null ? 1 : 0;
  }

  private void DrawLine()
  {
    if ((UnityEngine.Object) this.connectedTransform == (UnityEngine.Object) null || (UnityEngine.Object) this.joint == (UnityEngine.Object) null)
    {
      this.ClearLine();
    }
    else
    {
      this.desiredPos = this.connectedTransform.position + this.offsetPoint;
      this.endPoint = Vector3.SmoothDamp(this.endPoint, this.desiredPos, ref this.ropeVel, 0.03f);
      this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0.0f, ref this.offsetVel, 0.12f);
      int num1 = 100;
      this.lr.positionCount = num1;
      Vector3 position1 = this.transform.position;
      this.lr.SetPosition(0, position1);
      this.lr.SetPosition(num1 - 1, this.endPoint);
      float num2 = 15f;
      float num3 = 0.5f;
      for (int index = 1; index < num1 - 1; ++index)
      {
        double num4 = (double) index / (double) num1;
        float num5 = (float) num4 * this.offsetMultiplier;
        float num6 = (float) (((double) Mathf.Sin(num5 * num2) - 0.5) * (double) num3 * ((double) num5 * 2.0));
        Vector3 normalized = (this.endPoint - position1).normalized;
        float num7 = Mathf.Sin((float) (num4 * 180.0 * (Math.PI / 180.0)));
        float num8 = Mathf.Cos((float) ((double) this.offsetMultiplier * 90.0 * (Math.PI / 180.0)));
        Vector3 position2 = position1 + (this.endPoint - position1) / (float) num1 * (float) index + (Vector3) (num8 * num6 * Vector2.Perpendicular((Vector2) normalized) + this.offsetMultiplier * num7 * Vector2.down);
        this.lr.SetPosition(index, position2);
      }
    }
  }

  private void ClearLine() => this.lr.positionCount = 0;

  public void Use(Vector3 attackDirection)
  {
    if (!this.readyToUse)
      return;
    this.ShootRope(attackDirection);
    this.readyToUse = false;
  }

  public void StopUse()
  {
    if ((UnityEngine.Object) this.joint == (UnityEngine.Object) null)
      return;
    MonoBehaviour.print((object) "STOPPING");
    this.connectedTransform = (Transform) null;
    this.readyToUse = true;
  }

  private void ShootRope(Vector3 dir)
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.transform.position, dir, 10f, (int) this.whatIsSickoMode);
    GameObject gameObject = (GameObject) null;
    RaycastHit raycastHit1 = new RaycastHit();
    foreach (RaycastHit raycastHit2 in raycastHitArray)
    {
      gameObject = raycastHit2.collider.gameObject;
      if (gameObject.layer == LayerMask.NameToLayer("Player"))
      {
        gameObject = (GameObject) null;
      }
      else
      {
        raycastHit1 = raycastHit2;
        break;
      }
    }
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null || (UnityEngine.Object) raycastHit1.collider == (UnityEngine.Object) null)
      return;
    this.connectedTransform = raycastHit1.collider.transform;
    this.joint = this.gameObject.AddComponent<SpringJoint>();
    Rigidbody component = gameObject.GetComponent<Rigidbody>();
    this.offsetPoint = raycastHit1.point - raycastHit1.collider.transform.position;
    this.joint.connectedBody = gameObject.GetComponent<Rigidbody>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      this.joint.connectedAnchor = raycastHit1.point;
    else
      this.joint.connectedAnchor = this.offsetPoint;
    this.joint.autoConfigureConnectedAnchor = false;
    this.endPoint = this.transform.position;
    this.offsetMultiplier = 1f;
  }
}
