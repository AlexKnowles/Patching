using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Material Material;
    public int Difficulty = 3;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = Material;

        GetComponent<MeshFilter>().mesh = GenerateShape();
    }
    
    Mesh GenerateShape()
    {
        float radius = 1f;
        List<Vector2> vertices2D = new List<Vector2>();

        for (int i = 0; i < Difficulty; i++){
            float angle = i * Mathf.PI * 2 / Difficulty;
            vertices2D.Add(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
        }

        return _getMeshFromVectors(vertices2D.ToArray());
    }

    private Mesh _getMeshFromVectors(Vector2[] vertices2D){
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
