// Decompiled with JetBrains decompiler
// Type: TextureScaling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4DD6ABB-CD77-4768-BBF0-6F1CFA0BDA0E
// Assembly location: C:\Users\gabri\Desktop\Karlson_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class TextureScaling : MonoBehaviour
{
  private Vector3 _currentScale;
  public float size = 1f;

  private void Start() => this.Calculate();

  private void Update() => this.Calculate();

  public void Calculate()
  {
    if (this._currentScale == this.transform.localScale || this.CheckForDefaultSize())
      return;
    this._currentScale = this.transform.localScale;
    Mesh mesh = this.GetMesh();
    mesh.uv = this.SetupUvMap(mesh.uv);
    mesh.name = "Cube Instance";
    if (this.GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode == TextureWrapMode.Repeat)
      return;
    this.GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
  }

  private Mesh GetMesh() => this.GetComponent<MeshFilter>().mesh;

  private Vector2[] SetupUvMap(Vector2[] meshUVs)
  {
    float x = this._currentScale.x * this.size;
    float num = this._currentScale.z * this.size;
    float y = this._currentScale.y * this.size;
    meshUVs[2] = new Vector2(0.0f, y);
    meshUVs[3] = new Vector2(x, y);
    meshUVs[0] = new Vector2(0.0f, 0.0f);
    meshUVs[1] = new Vector2(x, 0.0f);
    meshUVs[7] = new Vector2(0.0f, 0.0f);
    meshUVs[6] = new Vector2(x, 0.0f);
    meshUVs[11] = new Vector2(0.0f, y);
    meshUVs[10] = new Vector2(x, y);
    meshUVs[19] = new Vector2(num, 0.0f);
    meshUVs[17] = new Vector2(0.0f, y);
    meshUVs[16] = new Vector2(0.0f, 0.0f);
    meshUVs[18] = new Vector2(num, y);
    meshUVs[23] = new Vector2(num, 0.0f);
    meshUVs[21] = new Vector2(0.0f, y);
    meshUVs[20] = new Vector2(0.0f, 0.0f);
    meshUVs[22] = new Vector2(num, y);
    meshUVs[4] = new Vector2(x, 0.0f);
    meshUVs[5] = new Vector2(0.0f, 0.0f);
    meshUVs[8] = new Vector2(x, num);
    meshUVs[9] = new Vector2(0.0f, num);
    meshUVs[13] = new Vector2(x, 0.0f);
    meshUVs[14] = new Vector2(0.0f, 0.0f);
    meshUVs[12] = new Vector2(x, num);
    meshUVs[15] = new Vector2(0.0f, num);
    return meshUVs;
  }

  private bool CheckForDefaultSize()
  {
    if (this._currentScale != Vector3.one)
      return false;
    GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.GetComponent<MeshFilter>());
    this.gameObject.AddComponent<MeshFilter>();
    this.GetComponent<MeshFilter>().sharedMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) primitive);
    return true;
  }
}
