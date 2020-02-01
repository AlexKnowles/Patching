using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchMover : MonoBehaviour
{
    public PatchCutter Cutter;

    private MouseDrag _mouseDrag;

    private void Start() 
    {
        _mouseDrag = new MouseDrag(StartMove, FinishMove);
    }

    private void Update() 
    {
        _mouseDrag.Update();

        if(_mouseDrag.IsDragging && Cutter.IsCutFinished()) 
        {
            Debug.Log("Dragggg");
        }   
    }

    public void StartMove()
    {

    }
    public void UpdateMove()
    {
        
    }
    public void FinishMove()
    {
        
    }
}
