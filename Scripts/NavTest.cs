// Decompiled with JetBrains decompiler
// Type: NavTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
  private NavMeshAgent agent;

  private void Start() => this.agent = this.GetComponent<NavMeshAgent>();

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) PlayerMovement.Instance)
      return;
    Vector3 position = PlayerMovement.Instance.transform.position;
    if (!this.agent.isOnNavMesh)
      return;
    this.agent.destination = position;
    MonoBehaviour.print((object) "goin");
  }
}
