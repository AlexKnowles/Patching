using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchMover : MonoBehaviour
{
    public PatchMaker Maker;
    public PatchCutter Cutter;

    private Transform _patchObjectTransform;
    private MouseDrag _mouseDrag;
    private Vector3 _mousePostionRelativeToPatch;
    private bool _patchDropped = false;

    private void Start() 
    {
        _patchObjectTransform = Maker.CurrentPatch.GetComponent<Transform>();

        _mouseDrag = new MouseDrag(StartMove, FinishMove);
    }

    private void Update() 
    {
        _mouseDrag.Update();

        if(!_patchDropped && _mouseDrag.IsDragging && Cutter.IsCutFinished()) 
        {            
            UpdateMove();
        }   
    }

    public void StartMove()
    {
        if(!Cutter.IsCutFinished() || _patchDropped)
            return;
        
        Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _mousePostionRelativeToPatch = new Vector3(_patchObjectTransform.position.x - mouseInWorldSpace.x, 
                                                    _patchObjectTransform.position.y - mouseInWorldSpace.y, 
                                                    _patchObjectTransform.position.z);

        _patchObjectTransform.position =  new Vector3(mouseInWorldSpace.x, mouseInWorldSpace.y, _patchObjectTransform.position.z);         
    }
    public void UpdateMove()
    {
        Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _patchObjectTransform.position = _mousePostionRelativeToPatch + new Vector3(mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
    }
    public void FinishMove()
    {
        if(!Cutter.IsCutFinished())
            return;
        _patchDropped = true;
    }
}
