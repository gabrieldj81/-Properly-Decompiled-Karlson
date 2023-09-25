// Decompiled with JetBrains decompiler
// Type: EZCameraShake.CameraShaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace EZCameraShake
{
  [AddComponentMenu("EZ Camera Shake/Camera Shaker")]
  public class CameraShaker : MonoBehaviour
  {
    public static CameraShaker Instance;
    private static Dictionary<string, CameraShaker> instanceList = new Dictionary<string, CameraShaker>();
    public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);
    public Vector3 DefaultRotInfluence = new Vector3(1f, 1f, 1f);
    private Vector3 posAddShake;
    private Vector3 rotAddShake;
    private List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();

    private void Awake()
    {
      CameraShaker.Instance = this;
      CameraShaker.instanceList.Add(this.gameObject.name, this);
    }

    private void Update()
    {
      this.posAddShake = Vector3.zero;
      this.rotAddShake = Vector3.zero;
      for (int index = 0; index < this.cameraShakeInstances.Count && index < this.cameraShakeInstances.Count; ++index)
      {
        CameraShakeInstance cameraShakeInstance = this.cameraShakeInstances[index];
        if (cameraShakeInstance.CurrentState == CameraShakeState.Inactive && cameraShakeInstance.DeleteOnInactive)
        {
          this.cameraShakeInstances.RemoveAt(index);
          --index;
        }
        else if (cameraShakeInstance.CurrentState != CameraShakeState.Inactive)
        {
          this.posAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.PositionInfluence);
          this.rotAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.RotationInfluence);
        }
      }
      this.transform.localPosition = this.posAddShake;
      this.transform.localEulerAngles = this.rotAddShake;
    }

    public static CameraShaker GetInstance(string name)
    {
      CameraShaker cameraShaker;
      return CameraShaker.instanceList.TryGetValue(name, out cameraShaker) ? cameraShaker : (CameraShaker) null;
    }

    public CameraShakeInstance Shake(CameraShakeInstance shake)
    {
      this.cameraShakeInstances.Add(shake);
      return shake;
    }

    public CameraShakeInstance ShakeOnce(
      float magnitude,
      float roughness,
      float fadeInTime,
      float fadeOutTime)
    {
      if (!(bool) (UnityEngine.Object) GameState.Instance)
        return (CameraShakeInstance) null;
      if (!GameState.Instance.shake)
        return (CameraShakeInstance) null;
      CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
      cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
      cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
      this.cameraShakeInstances.Add(cameraShakeInstance);
      return cameraShakeInstance;
    }

    public CameraShakeInstance ShakeOnce(
      float magnitude,
      float roughness,
      float fadeInTime,
      float fadeOutTime,
      Vector3 posInfluence,
      Vector3 rotInfluence)
    {
      if (!GameState.Instance.shake)
        return (CameraShakeInstance) null;
      CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
      cameraShakeInstance.PositionInfluence = posInfluence;
      cameraShakeInstance.RotationInfluence = rotInfluence;
      this.cameraShakeInstances.Add(cameraShakeInstance);
      return cameraShakeInstance;
    }

    public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
    {
      if (!GameState.Instance.shake)
        return (CameraShakeInstance) null;
      CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
      cameraShakeInstance.PositionInfluence = this.DefaultPosInfluence;
      cameraShakeInstance.RotationInfluence = this.DefaultRotInfluence;
      cameraShakeInstance.StartFadeIn(fadeInTime);
      this.cameraShakeInstances.Add(cameraShakeInstance);
      return cameraShakeInstance;
    }

    public CameraShakeInstance StartShake(
      float magnitude,
      float roughness,
      float fadeInTime,
      Vector3 posInfluence,
      Vector3 rotInfluence)
    {
      CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
      cameraShakeInstance.PositionInfluence = posInfluence;
      cameraShakeInstance.RotationInfluence = rotInfluence;
      cameraShakeInstance.StartFadeIn(fadeInTime);
      this.cameraShakeInstances.Add(cameraShakeInstance);
      return cameraShakeInstance;
    }

    public List<CameraShakeInstance> ShakeInstances => new List<CameraShakeInstance>((IEnumerable<CameraShakeInstance>) this.cameraShakeInstances);

    private void OnDestroy() => CameraShaker.instanceList.Remove(this.gameObject.name);
  }
}
