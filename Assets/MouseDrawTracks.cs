using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrawTracks : MonoBehaviour {
    public Camera _camera;
    public Shader _DrawSnowTracks;

    private RenderTexture _splatmap;
    private Material _snowMaterial, _drawMaterial;

    private RaycastHit _MouseHit;

    [Range(1, 500)]
    public float _brushSize;
    [Range(0,1)]
    public float _brushStrength;
    void Start() {
        _drawMaterial = new Material(_DrawSnowTracks);
        _drawMaterial.SetVector("_Color", Color.red);

        _snowMaterial = GetComponent<MeshRenderer>().material;
        _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatmap);
    }

    void Update() {
        if(Input.GetKey(KeyCode.Mouse0)) {
            if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _MouseHit)) {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_MouseHit.textureCoord.x, _MouseHit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }

    private void OnGUI() {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}
