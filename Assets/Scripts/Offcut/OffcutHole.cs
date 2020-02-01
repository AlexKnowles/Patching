using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffcutHole : MonoBehaviour
{
    public PatchMaker Maker;    
    public Color BackgroundColor = new Color(233f/255f,203/255f,151/255f);
    private Transform _thisTransform;
    private GameObject _currentCutout;

    public void Create()
    {
        _currentCutout = Instantiate(Maker.CurrentPatch);

        SetupTransform();
        SetupLineRenderer();
        SetupTexture();
    }

    public void Clear()
    {
        if(_currentCutout == null)
            return;

        GameObject.Destroy(_currentCutout); 
        _currentCutout = null;
    }

    private void Start() 
    {
        _thisTransform = GetComponent<Transform>();
    }

    private void SetupTransform()
    {
        Transform cutoutTransform = _currentCutout.GetComponent<Transform>();
        cutoutTransform.SetParent(_thisTransform, true);
        cutoutTransform.position = new Vector3(cutoutTransform.position.x, cutoutTransform.position.y, 1);
    }
    private void SetupLineRenderer()
    {
        LineRenderer cutoutLineRender = _currentCutout.GetComponent<LineRenderer>();
        cutoutLineRender.enabled = true;

        cutoutLineRender.startWidth = 0.08f;
        cutoutLineRender.endWidth = 0.08f;

        cutoutLineRender.startColor = BackgroundColor;
        cutoutLineRender.endColor = BackgroundColor;
    }
    private void SetupTexture()
    {
        MeshRenderer cutoutRenderer = _currentCutout.GetComponent<MeshRenderer>();
        
        cutoutRenderer.material.SetColor("_Color", BackgroundColor);
    }
}
