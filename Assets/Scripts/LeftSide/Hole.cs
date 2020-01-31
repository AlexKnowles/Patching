using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape 
{
    Triangle,
    Square,
    Pentagon,
    Hexagon
}

public class Hole : MonoBehaviour
{
    public Material material;

    float width = 1;
    float height = 1;

    public Shape shapeType = Shape.Triangle;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh;

        switch(shapeType){
            case Shape.Triangle:
            default:
                mesh = GenerateTriangle();
                break;
        }

        GetComponent<MeshRenderer>().material = material;

        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    Mesh GenerateTriangle()
    {
        Mesh mesh = new Mesh();
        Vector2[] vertices2D = new Vector2[3];

        vertices2D[0] = new Vector2(-width, -height);
        vertices2D[1] = new Vector2(-width, height);
        vertices2D[2] = new Vector2(width, height);

        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
