// Decompiled with JetBrains decompiler
// Type: Weapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class Weapon : Pickup
{
  public float attackSpeed;
  public float damage;
  public TrailRenderer trailRenderer;

  public float MultiplierDamage { get; set; }

  public void Start() => this.MultiplierDamage = 1f;

  protected void Cooldown() => this.readyToUse = true;

  public float GetAttackSpeed() => this.attackSpeed;
}
