using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareHoleGenerator : MonoBehaviour
{
    public Material material;

    float width = 1;
    float height = 1;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-width, -height);
        vertices[1] = new Vector3(-width, height);
        vertices[2] = new Vector3(width, height);
        vertices[3] = new Vector3(width, -height);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        GetComponent<MeshRenderer>().material = material;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
