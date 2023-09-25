// Decompiled with JetBrains decompiler
// Type: Grappler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grappler : Pickup
{
  private Transform tip;
  private bool grappling;
  public LayerMask whatIsGround;
  private Vector3 grapplePoint;
  private SpringJoint joint;
  private LineRenderer lr;
  private Vector3 endPoint;
  private float offsetMultiplier;
  private float offsetVel;
  public GameObject aim;
  private int positions = 100;
  private Vector3 aimVel;
  private Vector3 scaleVel;
  private Vector3 nearestPoint;

  private void Start()
  {
    this.tip = this.transform.GetChild(0);
    this.lr = this.GetComponent<LineRenderer>();
    this.lr.positionCount = this.positions;
    this.aim.transform.parent = (Transform) null;
    this.aim.SetActive(false);
  }

  public override void Use(Vector3 attackDirection)
  {
    if (this.grappling)
      return;
    this.grappling = true;
    Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
    Transform transform = PlayerMovement.Instance.transform;
    RaycastHit[] raycastHitArray = Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, (int) this.whatIsGround);
    if (raycastHitArray.Length < 1)
    {
      if (this.nearestPoint == Vector3.zero)
        return;
      this.grapplePoint = this.nearestPoint;
    }
    else
      this.grapplePoint = raycastHitArray[0].point;
    this.joint = transform.gameObject.AddComponent<SpringJoint>();
    this.joint.autoConfigureConnectedAnchor = false;
    this.joint.connectedAnchor = this.grapplePoint;
    this.joint.maxDistance = Vector2.Distance((Vector2) this.grapplePoint, (Vector2) transform.position) * 0.8f;
    this.joint.minDistance = Vector2.Distance((Vector2) this.grapplePoint, (Vector2) transform.position) * 0.25f;
    this.joint.spring = 4.5f;
    this.joint.damper = 7f;
    this.joint.massScale = 4.5f;
    this.endPoint = this.tip.position;
    this.offsetMultiplier = 2f;
    this.lr.positionCount = this.positions;
    AudioManager.Instance.PlayPitched("Grapple", 0.2f);
  }

  public override void OnAim()
  {
    if (this.grappling)
      return;
    Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
    List<RaycastHit> list = ((IEnumerable<RaycastHit>) Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, (int) this.whatIsGround)).ToList<RaycastHit>();
    if (list.Count > 0)
    {
      this.aim.SetActive(false);
      this.aim.transform.localScale = Vector3.zero;
    }
    else
    {
      int num1 = 50;
      int num2 = 10;
      float num3 = 0.035f;
      bool flag = list.Count > 0;
      for (int index1 = 0; index1 < num2 && !flag; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          double f = 6.2831854820251465 / (double) num1 * (double) index2;
          float num4 = Mathf.Cos((float) f);
          float num5 = Mathf.Sin((float) f);
          Vector3 vector3 = playerCamTransform.right * num4 + playerCamTransform.up * num5;
          list.AddRange((IEnumerable<RaycastHit>) Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward + vector3 * num3 * (float) index1, 70f, (int) this.whatIsGround));
        }
        if (list.Count > 0)
          break;
      }
      this.nearestPoint = this.FindNearestPoint(list);
      if (list.Count > 0 && !this.grappling)
      {
        this.aim.SetActive(true);
        this.aim.transform.position = Vector3.SmoothDamp(this.aim.transform.position, this.nearestPoint, ref this.aimVel, 0.05f);
        this.aim.transform.localScale = Vector3.SmoothDamp(this.aim.transform.localScale, 0.025f * list[0].distance * Vector3.one, ref this.scaleVel, 0.2f);
      }
      else
      {
        this.aim.SetActive(false);
        this.aim.transform.localScale = Vector3.zero;
      }
    }
  }

  private Vector3 FindNearestPoint(List<RaycastHit> hits)
  {
    Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
    Vector3 nearestPoint = Vector3.zero;
    float num = float.PositiveInfinity;
    for (int index = 0; index < hits.Count; ++index)
    {
      RaycastHit hit = hits[index];
      if ((double) hit.distance < (double) num)
      {
        hit = hits[index];
        num = hit.distance;
        hit = hits[index];
        nearestPoint = hit.collider.ClosestPoint(playerCamTransform.position + playerCamTransform.forward * num);
      }
    }
    return nearestPoint;
  }

  public override void StopUse()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.joint);
    this.grapplePoint = Vector3.zero;
    this.grappling = false;
  }

  private void LateUpdate() => this.DrawGrapple();

  private void DrawGrapple()
  {
    if (this.grapplePoint == Vector3.zero || (UnityEngine.Object) this.joint == (UnityEngine.Object) null)
    {
      this.lr.positionCount = 0;
    }
    else
    {
      this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
      this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0.0f, ref this.offsetVel, 0.1f);
      Vector3 position1 = this.tip.position;
      double num1 = (double) Vector3.Distance(this.endPoint, position1);
      this.lr.SetPosition(0, position1);
      this.lr.SetPosition(this.positions - 1, this.endPoint);
      float num2 = (float) num1;
      float num3 = 1f;
      for (int index = 1; index < this.positions - 1; ++index)
      {
        double num4 = (double) index / (double) this.positions;
        float num5 = (float) num4 * this.offsetMultiplier;
        float num6 = (float) (((double) Mathf.Sin(num5 * num2) - 0.5) * (double) num3 * ((double) num5 * 2.0));
        Vector3 normalized = (this.endPoint - position1).normalized;
        float num7 = Mathf.Sin((float) (num4 * 180.0 * (Math.PI / 180.0)));
        float num8 = Mathf.Cos((float) ((double) this.offsetMultiplier * 90.0 * (Math.PI / 180.0)));
        Vector3 position2 = position1 + (this.endPoint - position1) / (float) this.positions * (float) index + ((Vector3) (num8 * num6 * Vector2.Perpendicular((Vector2) normalized)) + this.offsetMultiplier * num7 * Vector3.down);
        this.lr.SetPosition(index, position2);
      }
    }
  }

  public Vector3 GetGrapplePoint() => this.grapplePoint;
}
