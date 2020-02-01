using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public int Difficulty = 1;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = GenerateShape();
    }
    
    Mesh GenerateShape()
    {
        List<Vector2> vertices2D = new List<Vector2>();
        int spokes = Difficulty + 3 < 6 ? Difficulty + 3 : 16;
        for (int i = 0; i < spokes; i++){
            int posNeg = UnityEngine.Random.value > 0.5 ? 1 : -1;
            float radius = 1f + (float)(((float)spokes*0.05f) * (UnityEngine.Random.value * (float)posNeg));
            float angle = i * Mathf.PI * 2 / (spokes);
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
