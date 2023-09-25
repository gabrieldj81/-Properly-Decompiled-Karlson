// Decompiled with JetBrains decompiler
// Type: PrefabManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PrefabManager : MonoBehaviour
{
  public GameObject blood;
  public GameObject bulletDestroy;
  public GameObject muzzle;
  public GameObject explosion;
  public GameObject bulletHitAudio;
  public GameObject enemyHitAudio;
  public GameObject gunShotAudio;
  public GameObject objectImpactAudio;
  public GameObject thumpAudio;
  public GameObject destructionAudio;

  public static PrefabManager Instance { get; private set; }

  private void Awake() => PrefabManager.Instance = this;
}
