using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffcutHole : MonoBehaviour
{
    public PatchMaker Maker;    
    public Color BackgroundColor = new Color(0,0,0, 69/255f);
    public Material HoleMaterial;
    public GameObject ShadowBox;
    public LineRenderer Stencil;

    private Transform _thisTransform;
    private MeshRenderer _meshRenderer;
    private GameObject _currentCutout;

    public void Create()
    {
        _currentCutout = Instantiate(Maker.CurrentPatch);

        SetupTransform();
        SetupLineRenderer();
        SetupTexture();

        Stencil.enabled = false;
    }

    public void Clear()
    {
        if(_currentCutout == null)
            return;

        GameObject.Destroy(_currentCutout); 
        _currentCutout = null;
        Stencil.enabled = true;
    }

    private void Start() 
    {
        _thisTransform = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterBeforeTutorial(BeforeTutorial);
    }

    public void BeforeTutorial()
    {
        Show();
    }

    public void GameOver()
    {
        Clear();
        Hide();
    }

    private void SetupTransform()
    {
        Transform cutoutTransform = _currentCutout.GetComponent<Transform>();
        cutoutTransform.SetParent(_thisTransform, true);
        cutoutTransform.position = new Vector3(cutoutTransform.position.x, cutoutTransform.position.y, 0.1f);
    }
    private void SetupLineRenderer()
    {
        LineRenderer cutoutLineRender = _currentCutout.GetComponent<LineRenderer>();
        cutoutLineRender.enabled = true;

        cutoutLineRender.startWidth = 0.1f;
        cutoutLineRender.endWidth = 0.1f;

        cutoutLineRender.startColor = BackgroundColor;
        cutoutLineRender.endColor = BackgroundColor;
    }
    private void SetupTexture()
    {
        MeshRenderer cutoutRenderer = _currentCutout.GetComponent<MeshRenderer>();        
        cutoutRenderer.material = HoleMaterial;
    }

    private void Show()
    {
        ShadowBox.SetActive(true);
        _meshRenderer.enabled = true;
        Stencil.enabled = true;
    }
    private void Hide()
    {
        ShadowBox.SetActive(false);
        _meshRenderer.enabled = false;
        Stencil.enabled = false;
    }
}
