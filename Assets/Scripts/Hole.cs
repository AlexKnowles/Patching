using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public int Difficulty = 1;
    public List<Vector2> Vertices2D { get; private set; }

    private LineRenderer _lineRenderer;

    void Start()
    {
        Vertices2D = new List<Vector2>();
        _lineRenderer = GetComponent<LineRenderer>();

        GetComponent<MeshFilter>().mesh = GenerateShape();
        LoadVertsIntoLineRenderer();
    }

    private Mesh GenerateShape()
    {
        if(Difficulty == 0){
            Vertices2D.Add(new Vector2(0.5f, 1.0f));
            Vertices2D.Add(new Vector2(-1.30f, 0.0f));
            Vertices2D.Add(new Vector2(0.4f, -1.05f));
        } else {
            int spokes = 16;
            for (int i = 0; i < spokes; i++){
                int posNeg = UnityEngine.Random.value > 0.5 ? 1 : -1;
                float radius = 1f + (float)(((float)spokes*0.05f) * ((UnityEngine.Random.value * 0.4f) * (float)posNeg));
                float angle = i * Mathf.PI * 2 / (spokes);
                Vertices2D.Add(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
            }
        }

        return _getMeshFromVectors(Vertices2D.ToArray());
    }

    private Mesh _getMeshFromVectors(Vector2[] Vertices2D){
        Triangulator tr = new Triangulator(Vertices2D);
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[Vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(Vertices2D[i].x, Vertices2D[i].y, 0);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    
    private void LoadVertsIntoLineRenderer()
    {
        _lineRenderer.positionCount = 0;

        foreach(Vector2 vert in Vertices2D)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount++, new Vector3(vert.x, vert.y, 0));
        }

        _lineRenderer.SetPosition(_lineRenderer.positionCount++, new Vector3(Vertices2D[0].x, Vertices2D[0].y, 0));
    }
}
