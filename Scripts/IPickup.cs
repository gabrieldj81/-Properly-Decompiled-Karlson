// Decompiled with JetBrains decompiler
// Type: IPickup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public interface IPickup
{
  void Use(Vector3 attackDirection);

  bool IsPickedUp();
}
