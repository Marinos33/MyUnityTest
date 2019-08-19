using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=64NblGkAabk&list=WL&index=2
public class ProceduralMeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

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
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; //nombre max de vertex ( 1 cube = 4 vertex)

        //boucle pour créer tous les vertex
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z); //un vertex
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6]; //tableau contenant le nombre de vertex afin de faire des triangle (3vertex par triangle)
        int vert = 0; //memorise le numéro du vertex
        int tris = 0; //memoreise le numero du triangle (3vertex)
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                //1er triangle
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                //2 triangle
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }


    }
    void UpdateMesh()
    {
        mesh.Clear(); // efface le mesh actuel (none)

        mesh.vertices = vertices; //set les vertex
        mesh.triangles = triangles; //set les triangles

        mesh.RecalculateNormals();
    }

    /*
     * //fonction afficher les point des vertex (debug)
    private void OnDrawGizmos()
    {

        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }*/
}

