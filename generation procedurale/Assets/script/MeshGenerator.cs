using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=eJEpeUH1EMg&list=WL&index=2&t=0s
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; // met le mesh custom (ce script) sur le meshfilter

        CreateShape();
        UpdateMesh();
    }


    void CreateShape()
    {
        //les vertex
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };

        //les triangles
        triangles = new int[]
        {
            0,1,2, //1er triangle
            1,3,2 // second triangle
        };
    }
    void UpdateMesh()
    {
        mesh.Clear(); // effece le mesh acteulle (none)

        mesh.vertices = vertices; //set les vertex
        mesh.triangles = triangles; //set les triangles

        mesh.RecalculateNormals();
    }
}
