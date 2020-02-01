﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape 
{
    Triangle,
    Square,
    Pentagon,
    Random
}

public class Hole : MonoBehaviour
{
    public Material material;
    public Shape shapeType = Shape.Triangle;

    public int difficulty = 1;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh;

        switch(shapeType){
            case Shape.Random:
                mesh = GenerateRandom();
                break;
            case Shape.Pentagon:
                mesh = GeneratePentagon();
                break;
            case Shape.Square:
                mesh = GenerateSquare();
                break;
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
        Vector2[] vertices2D = new Vector2[3];

        vertices2D[0] = new Vector2(0, 1);
        vertices2D[1] = new Vector2(-1, -1);
        vertices2D[2] = new Vector2(1, -1);

        return _getMeshFromVectors(vertices2D);
    }

    Mesh GenerateSquare()
    {
        Vector2[] vertices2D = new Vector2[4];

        vertices2D[0] = new Vector2(-1, -1);
        vertices2D[1] = new Vector2(-1, 1);
        vertices2D[2] = new Vector2(1, 1);
        vertices2D[3] = new Vector2(1, -1);

        return _getMeshFromVectors(vertices2D);
    }
    
    Mesh GeneratePentagon()
    {
        Vector2[] vertices2D = new Vector2[5];

        vertices2D[0] = new Vector2(0, -2);
        vertices2D[1] = new Vector2((float)-1.9, (float)-0.6);
        vertices2D[2] = new Vector2((float)-1.2, (float)1.6);
        vertices2D[3] = new Vector2((float)1.2, (float)1.6);
        vertices2D[4] = new Vector2((float)1.9, (float)-0.6);

        return _getMeshFromVectors(vertices2D);
    }

    Mesh GenerateRandom()
    {
        Vector2[] vertices2D = new Vector2[3];

        vertices2D[0] = new Vector2(0, 1);
        vertices2D[1] = new Vector2(-1, -1);
        vertices2D[2] = new Vector2(1, -1);

        return _getMeshFromVectors(vertices2D);
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
