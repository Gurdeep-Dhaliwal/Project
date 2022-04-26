using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public int mapDepth = 20;
    public int mapWidth = 256;
    public int mapHeight = 256;

    public float scale = 5f;
    public float offsetX = 10f;
    public float offsetY = 10f;
    // Start is called before the first frame update
    void Start()
    {
        offsetX = Random.Range(0.0f, 99999f);
        offsetY = Random.Range(0.0f, 99999f);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData){
        terrainData.heightmapResolution = mapWidth + 1;
        terrainData.size = new Vector3(mapWidth, mapDepth, mapHeight);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights(){
        float[,] heights = new float[mapWidth, mapHeight];
        for(int x = 0; x < mapWidth; x++){
            for(int y = 0; y < mapHeight; y++){
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y){
        float xCoord = (float)x / mapWidth * scale + offsetX;
        float yCoord = (float)y / mapHeight * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
