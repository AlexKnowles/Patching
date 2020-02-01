using System;
using UnityEngine;

public class MouseDrag 
{    
    public bool IsDragging {get; private set;} = false;
    private Action _startDrag;
    private Action _endDrag;
    private Vector2 _lastPosition;

    public MouseDrag(Action startDrag, Action endDrag)
    {
        _startDrag = startDrag;
        _endDrag = endDrag;
    }
    
    public void Update()
    {
        if(BeginDrag())
        {
            IsDragging = true;
            _startDrag();
        }
        else if(EndDrag())
        {
            IsDragging = false;
            _endDrag();
        }
        
        _lastPosition = Input.mousePosition;
    }

    private bool BeginDrag() 
    {
        return (!IsDragging && Input.GetMouseButtonDown(0));
    }
    private bool EndDrag() 
    {
        return (IsDragging && Input.GetMouseButtonUp(0));
    }
}