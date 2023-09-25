// Decompiled with JetBrains decompiler
// Type: ExplosiveBullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
  private Rigidbody rb;

  private void Start()
  {
    this.rb = this.GetComponent<Rigidbody>();
    UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, this.transform.position, Quaternion.identity);
  }

  private void OnCollisionEnter(Collision other)
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, this.transform.position, Quaternion.identity);
  }

  private void Update() => this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
}
