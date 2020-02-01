using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchMover : MonoBehaviour
{
    public PatchCutter Cutter;
    public Transform PatchObject;

    private MouseDrag _mouseDrag;
    private Vector3 _mousePostionRelativeToPatch;
    private bool _patchDropped = false;

    private void Start() 
    {
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

        _mousePostionRelativeToPatch = new Vector3(PatchObject.position.x - mouseInWorldSpace.x, PatchObject.position.y - mouseInWorldSpace.y, PatchObject.position.z);

        PatchObject.position =  new Vector3(mouseInWorldSpace.x, mouseInWorldSpace.y, PatchObject.position.z);         
    }
    public void UpdateMove()
    {
        Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PatchObject.position = _mousePostionRelativeToPatch + new Vector3(mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
    }
    public void FinishMove()
    {
        if(!Cutter.IsCutFinished())
            return;
        _patchDropped = true;
    }
}
