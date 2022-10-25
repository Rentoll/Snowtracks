using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour {

    private RenderTexture _splatmap;
    public Shader _DrawSnowTracks;
    private Material _drawMaterial;
    private Material _myMaterial;
    public GameObject _terrain;

    public Transform[] _wheel;
    RaycastHit _groundHit;
    int _layerMask;

    [Range(0, 2)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;

    void Start() {
        _layerMask = LayerMask.GetMask("Ground");
        _drawMaterial = new Material(_DrawSnowTracks);
        _myMaterial = _terrain.GetComponent<MeshRenderer>().material;
        _myMaterial.SetTexture("_Splat", _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    void Update() {
        for(int i = 0; i < _wheel.Length; i++) {
            if (Physics.Raycast(_wheel[i].position, Vector3.down, out _groundHit, 1f, _layerMask)) {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
