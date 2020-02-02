using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stencil : MonoBehaviour
{
    private Hole _hole;
    private Transform _thisTransform;
    private LineRenderer _lineRenderer;
    private bool _loadVertsLater;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _lineRenderer = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        if(_loadVertsLater && _hole.Vertices2D != null)
        {
            _loadVertsLater = false;
            LoadVertsIntoLineRenderer();
        }
    }

    public void GiveHole(GameObject holeObject)
    {
        _hole = holeObject.GetComponent<Hole>();

        if(_hole.Vertices2D != null)
            LoadVertsIntoLineRenderer();
        else
            _loadVertsLater = true;
    }    

    private void LoadVertsIntoLineRenderer()
    {
        _lineRenderer.positionCount = 0;

        foreach(Vector2 vert in _hole.Vertices2D)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount++, new Vector3(vert.x, vert.y, 0));
        }

        _lineRenderer.SetPosition(_lineRenderer.positionCount++, new Vector3(_hole.Vertices2D[0].x, _hole.Vertices2D[0].y, 0));
    }
}
