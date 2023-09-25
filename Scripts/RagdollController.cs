// Decompiled with JetBrains decompiler
// Type: RagdollController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{
  private CharacterJoint[] c;
  private Vector3[] axis;
  private Vector3[] anchor;
  private Vector3[] swingAxis;
  public GameObject hips;
  private float[] mass;
  public GameObject[] limbs;
  private bool isRagdoll;
  public Transform leftArm;
  public Transform rightArm;
  public Transform head;
  public Transform hand;
  public Transform hand2;

  private void Start() => this.MakeStatic();

  private void LateUpdate()
  {
  }

  public void MakeRagdoll(Vector3 dir)
  {
    if (this.isRagdoll)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.GetComponent<NavMeshAgent>());
    UnityEngine.Object.Destroy((UnityEngine.Object) this.GetComponent("NavTest"));
    this.isRagdoll = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.GetComponent<Rigidbody>());
    this.GetComponentInChildren<Animator>().enabled = false;
    for (int i = 0; i < this.limbs.Length; ++i)
    {
      this.AddRigid(i, dir);
      this.limbs[i].gameObject.layer = LayerMask.NameToLayer("Object");
      this.limbs[i].AddComponent(typeof (Object));
    }
  }

  private void AddRigid(int i, Vector3 dir)
  {
    GameObject limb = this.limbs[i];
    Rigidbody rigidbody = limb.AddComponent<Rigidbody>();
    rigidbody.mass = this.mass[i];
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.AddForce(dir);
    if (i == 0)
      return;
    CharacterJoint characterJoint = limb.AddComponent<CharacterJoint>();
    characterJoint.autoConfigureConnectedAnchor = true;
    characterJoint.connectedBody = this.FindConnectedBody(i);
    characterJoint.axis = this.axis[i];
    characterJoint.anchor = this.anchor[i];
    characterJoint.swingAxis = this.swingAxis[i];
  }

  private Rigidbody FindConnectedBody(int i)
  {
    int index = 0;
    if (i == 2)
      index = 1;
    if (i == 4)
      index = 3;
    if (i == 7)
      index = 6;
    if (i == 9)
      index = 8;
    if (i == 10)
      index = 5;
    return this.limbs[index].GetComponent<Rigidbody>();
  }

  private void MakeStatic()
  {
    int length = this.limbs.Length;
    this.c = new CharacterJoint[length];
    Rigidbody[] rigidbodyArray = new Rigidbody[length];
    this.mass = new float[length];
    for (int index = 0; index < this.limbs.Length; ++index)
    {
      rigidbodyArray[index] = this.limbs[index].GetComponent<Rigidbody>();
      this.mass[index] = rigidbodyArray[index].mass;
      this.c[index] = this.limbs[index].GetComponent<CharacterJoint>();
    }
    this.axis = new Vector3[length];
    this.anchor = new Vector3[length];
    this.swingAxis = new Vector3[length];
    for (int index = 0; index < this.c.Length; ++index)
    {
      if (!((UnityEngine.Object) this.c[index] == (UnityEngine.Object) null))
      {
        this.axis[index] = this.c[index].axis;
        this.anchor[index] = this.c[index].anchor;
        this.swingAxis[index] = this.c[index].swingAxis;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.c[index]);
      }
    }
    foreach (UnityEngine.Object @object in rigidbodyArray)
      UnityEngine.Object.Destroy(@object);
  }

  public bool IsRagdoll() => this.isRagdoll;
}
