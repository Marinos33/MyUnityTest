
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256; //axe x
    public int height = 256; //axe y
    public int depth = 20; // axe z
    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public float speed = 5f;
    private void Start()
    {
        //valeur random 
        offsetX = Random.Range(0f, 999f);
        offsetY = Random.Range(0f, 9999f);
    }
    private void Update()
    {
        Terrain terrain = GetComponent<Terrain>();

        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        offsetX += Time.deltaTime * speed;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {

        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height); //set les valeur du terrain

        terrainData.SetHeights(0, 0, GenerateHeights()); //genere la hauteur et la set

        return terrainData;
    }

    //generation de la hauteur
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height]; //tableau de position x et y
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float) x / width * scale + offsetX;
        float yCoord = (float) y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord); // genere un map 2D
    }
}
