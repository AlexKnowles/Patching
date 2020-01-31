using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LineDrawer : MonoBehaviour
{
    public LineRenderer Renderer;

    private bool _mouseBeingHeld = false;
    private List<Vector2> _points = new List<Vector2>();

    private void Update() 
    {
        UpdateMouseBeingHeld();

        if(_mouseBeingHeld)
        {
            Vector2 mouseCurrentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(MouseMovedSinceLastUpdate(mouseCurrentWorldPosition))
            {
                UpdateLine(mouseCurrentWorldPosition);
            }
        }
    }

    private void UpdateMouseBeingHeld()
    {
        if(MouseHoldBegin())
        {
            StartLine();
            _mouseBeingHeld = true;
        }
        else if(MouseHoldEnd())
        {
            _mouseBeingHeld = false;
        }
    }
    private bool MouseHoldBegin() 
    {
        return (!_mouseBeingHeld && Input.GetMouseButtonDown(0));
    }
    private bool MouseHoldEnd() 
    {
        return (_mouseBeingHeld && Input.GetMouseButtonUp(0));
    }
    private bool MouseMovedSinceLastUpdate(Vector2 newPosition)
    {
        return (Vector2.Distance(newPosition, _points.Last()) > .1f);
    }

    private void StartLine()
    {
        Vector2 currentMousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _points.Clear();

        _points.Add(currentMousePositionInWorld);
        _points.Add(currentMousePositionInWorld);

        Renderer.positionCount = 2;
        Renderer.SetPosition(0, currentMousePositionInWorld);
        Renderer.SetPosition(1, currentMousePositionInWorld);
        
    }
    private void UpdateLine(Vector2 newPosition)
    {
        _points.Add(newPosition);
        Renderer.SetPosition(Renderer.positionCount++, newPosition);
    }
}
