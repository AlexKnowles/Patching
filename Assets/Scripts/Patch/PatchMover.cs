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
    private bool _hasStarted = false;
    private bool _gameOver = true;

    private void Start() 
    {
        _mouseDrag = new MouseDrag(StartMove, FinishMove);
        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterRestart(Restart);
    }

    private void Update() 
    {
        _mouseDrag.Update();

        if(!_patchDropped && _mouseDrag.IsDragging && Cutter.IsCutFinished() && !_gameOver) 
        {            
            UpdateMove();
        }   
    }

    public void Restart() 
    {
        _patchObjectTransform = Maker.CurrentPatch.GetComponent<Transform>();
        _patchDropped = false;
        _hasStarted = false;
        _gameOver = false;
    }
    public void GameOver()
    {
        _gameOver = true;
        FinishMove();
    }

    public void StartMove()
    {
        if(!Cutter.IsCutFinished() || _patchDropped || _gameOver)
            return;
        
        _hasStarted = true;

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
        if(!Cutter.IsCutFinished() || !_hasStarted)
            return;

        _patchDropped = true;

        Maker.SendPatchToBlanket();
    }
}
