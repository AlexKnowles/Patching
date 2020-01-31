using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatchCutter : MonoBehaviour
{
    public LineRenderer Renderer;
    public MeshFilter Filter;
    public bool CanCut = true;

    private bool _mouseBeingHeld = false;
    private List<Vector2> _points = new List<Vector2>();
    private bool _isCutting = false;

    private void Update() 
    {
        UpdateMouseBeingHeld();

        if(_mouseBeingHeld && _isCutting)
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
            if(CanCut)
                StartLine();

            _mouseBeingHeld = true;
        }
        else if(MouseHoldEnd())
        {
            FinishLine();
            ConvertLineToMesh();
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
        _isCutting = true;
        CanCut = false;
        
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
    private void FinishLine()
    {
        _isCutting = false;
        UpdateLine(_points.First());
    }

    private void ConvertLineToMesh()
    {
        Triangulator tr = new Triangulator(_points);
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[_points.Count];
        for (int i = 0; i < vertices.Length; i++) 
        {
            vertices[i] = new Vector3(_points[i].x, _points[i].y, 0);
        }
 
        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices.Reverse().ToArray();
        msh.RecalculateNormals();
        msh.RecalculateBounds();
 
        // Set up game object with mesh;
        Filter.mesh = msh;
    }
}
