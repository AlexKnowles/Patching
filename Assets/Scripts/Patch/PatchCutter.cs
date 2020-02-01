using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatchCutter : MonoBehaviour
{
    public LineRenderer Renderer;
    public MeshFilter PatchObject;

    private MouseDrag _mouseDrag;
    private List<Vector2> _points = new List<Vector2>();
    private bool _canCut = true;
    private bool _isCutting = false;

    private void Start() 
    {
        _mouseDrag = new MouseDrag(StartCut, FinishCut);
    }

    private void Update() 
    {
        _mouseDrag.Update();

        if(_mouseDrag.IsDragging && _isCutting)
        {
            Vector2 mouseCurrentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if(Vector2.Distance(mouseCurrentWorldPosition, _points.Last()) > .1f)
            {
                UpdateCut(mouseCurrentWorldPosition);
            }
        }
    }

    public bool IsCutFinished()
    {
        return (!_canCut && !_isCutting);
    }

    private void StartCut()
    {
        if(!_canCut)
            return;

        _isCutting = true;
        _canCut = false;
        
        Vector2 currentMousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _points.Clear();

        _points.Add(currentMousePositionInWorld);
        _points.Add(currentMousePositionInWorld);

        Renderer.positionCount = 2;
        Renderer.SetPosition(0, currentMousePositionInWorld);
        Renderer.SetPosition(1, currentMousePositionInWorld);
        
    }
    private void UpdateCut(Vector2 newPosition)
    {
        _points.Add(newPosition);
        Renderer.SetPosition(Renderer.positionCount++, newPosition);
    }
    private void FinishCut()
    {
        _isCutting = false;
        UpdateCut(_points.First());
        ConvertLineToMesh();
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
        PatchObject.mesh = msh;
    }
}
