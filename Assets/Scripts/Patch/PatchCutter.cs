﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatchCutter : MonoBehaviour
{
    public PatchMaker Maker;
    public Material PatchMaterial;
    public OffcutHole OffcutHole;

    private LineRenderer _patchObjectLineRenderer;
    private MeshFilter _patchObjectMeshFilter;
    private MeshRenderer _patchObjectMeshRenderer;
    private MouseDrag _mouseDrag;
    private List<Vector2> _points = new List<Vector2>();
    private Transform _offcutHoleTransform;
    private Bounds _offcutHoleBounds;
    private bool _canCut = true;
    private bool _isCutting = false;
    private bool _gameOver = true;

    private void Start() 
    {
        _mouseDrag = new MouseDrag(StartCut, FinishCut);
        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterRestart(Restart);
    }

    private void Update() 
    {
        _mouseDrag.Update();

        if(_mouseDrag.IsDragging && _isCutting && !_gameOver)
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

    public void Restart()
    {
        NewPatch();
        StartCut();
    }

    public void GameOver()
    {
        _gameOver = true;
        FinishCut();
        OffcutHole.Clear();
    }

    public void NewPatch()
    {
        _offcutHoleTransform = OffcutHole.GetComponent<Transform>();
        _offcutHoleBounds = OffcutHole.GetComponent<MeshFilter>().mesh.bounds;
        _patchObjectLineRenderer = Maker.CurrentPatch.GetComponent<LineRenderer>();
        _patchObjectMeshFilter = Maker.CurrentPatch.GetComponent<MeshFilter>();
        _patchObjectMeshRenderer = Maker.CurrentPatch.GetComponent<MeshRenderer>();

        _gameOver = false;
        _canCut = true;
        _isCutting = false;

        OffcutHole.Clear();
    }
    private void StartCut()
    {
        if(!_canCut || _gameOver)
            return;

        _isCutting = true;
        _canCut = false;
        
        Vector2 currentMousePositionInWorld = ConstrainPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _points.Clear();

        _points.Add(currentMousePositionInWorld);
        _points.Add(currentMousePositionInWorld);

        _patchObjectLineRenderer.positionCount = 2;
        _patchObjectLineRenderer.SetPosition(0, currentMousePositionInWorld);
        _patchObjectLineRenderer.SetPosition(1, currentMousePositionInWorld);
        
    }
    private void UpdateCut(Vector2 newPosition)
    {
        newPosition = ConstrainPosition(newPosition);

        _points.Add(newPosition);
        _patchObjectLineRenderer.SetPosition(_patchObjectLineRenderer.positionCount++, newPosition);
    }
    private void FinishCut()
    {
        if(!_isCutting)
            return;

        _isCutting = false;

        UpdateCut(_points.First());
        ConvertLineToMesh();
        AddShadow();

        OffcutHole.Create();
    }

    private void ConvertLineToMesh()
    {
        Triangulator tr = new Triangulator(_points);
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[_points.Count];
        Vector2[] uvVerts = new Vector2[_points.Count];
        for (int i = 0; i < vertices.Length; i++) 
        {
            vertices[i] = new Vector3(_points[i].x, _points[i].y, 0);
            //TODO: This is almost right
            uvVerts[i] = Quaternion.Euler(0, 0, 90) * new Vector2(_points[i].x, _points[i].y);
        }
 
        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices.Reverse().ToArray();
        msh.uv = uvVerts;

        // Set up game object with mesh;
        _patchObjectMeshFilter.mesh = msh;
        _patchObjectMeshFilter.mesh.RecalculateNormals();
        _patchObjectMeshFilter.mesh.RecalculateBounds();
        _patchObjectMeshFilter.mesh.RecalculateTangents();
        _patchObjectMeshRenderer.material = PatchMaterial;
        _patchObjectMeshRenderer.material.mainTextureScale = new Vector2(0.15f, 0.15f);
 
    }

    private void AddShadow()
    {
        _patchObjectLineRenderer.startWidth = 0.08f;
        _patchObjectLineRenderer.endWidth = 0.08f;

        _patchObjectLineRenderer.startColor = new Color(0, 0, 0, 50f/255f);
        _patchObjectLineRenderer.endColor = new Color(0, 0, 0, 50f/255f);
    }

    private Vector3 ConstrainPosition(Vector3 desiredPosition)
    {
        float newX = desiredPosition.x;
        float newY = desiredPosition.y;
        float newZ = desiredPosition.z;

        Vector3 actualPosition = desiredPosition;
        Vector3 minInWorld = _offcutHoleTransform.TransformPoint(_offcutHoleBounds.min);
        Vector3 maxInWorld = _offcutHoleTransform.TransformPoint(_offcutHoleBounds.max);

        if(newX < minInWorld.x)
            newX = minInWorld.x;
        else if(newX > maxInWorld.x)
            newX = maxInWorld.x;

        if(newY < minInWorld.y)
            newY = minInWorld.y;
        else if(newY > maxInWorld.y)
            newY = maxInWorld.y;

        return new Vector3(newX, newY, newZ);
    }
}
