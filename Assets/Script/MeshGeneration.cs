using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    Mesh mesh;
    private int MeshScale;
    private Vector3[] vertices;
    private AnimationCurve heightCurve;
    public int[] triangles;

    public GameObject player;
    public GameObject tree_1;
    public GameObject Ground;
    public GameObject Grass;

    public int XSize;
    public int ZSize;

    public float offsetX;
    public float offsetZ;

    public float scale;
    public int octaves;
    public float lacunarity;

    public int seed;
    private Vector2[] octaveOffsets;
    private System.Random rng;

    private void SetProperties()
    {
        MeshScale = 10;
        XSize = 100;
        ZSize = 100;
        scale = 25;
        octaves = 5;
        lacunarity = 2;
    }

    void Start()
    {
        SetProperties();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMap();
    }

    public void CreateMap()
    {
        CreateShape();
        UpdateMesh();
    } 
    
    
    void CreateShape()
    {
        Vector2[] octaveOffsets = GetSeed();

        vertices = new Vector3[(XSize + 1) * (ZSize + 1)];

        for (int i = 0, z = 0; z <= ZSize; z++)
        {
            for (int x = 0; x <= XSize; x++)
            {
                float noiseHeight = GenerateNoiseHeight(z, x, octaveOffsets);
                //Debug.Log(noiseHeight);
                vertices[i] = new Vector3(x, noiseHeight, z);  
                //Debug.Log(vertices[i]);
                i++;
            }
        }
        
        triangles = new int[XSize *  ZSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < ZSize; z++)
        {
            for (int x = 0; x < XSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + XSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + XSize + 1;
                triangles[tris + 5] = vert + XSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        
    }
    /*
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    */
    private Vector2[] GetSeed()
    {
        seed = Random.Range(0, 10000);
        System.Random rng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];  
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = rng.Next(-100000, 100000);
            float offsetY = rng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }


    private float GenerateNoiseHeight(int z, int x, Vector2[] octaveOffsets)
    {
        float amplitude = 5;
        float frequency = 0.6f;
        float noiseHeight = 0;
        float persistance = 0.5f;
        for (int i = 0; i < octaves; i++)
        {
            float sampleX = x / scale * frequency + octaveOffsets[i].x;
            float sampleZ = z / scale * frequency + octaveOffsets[i].y;
            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
            noiseHeight += perlinValue * amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }
        return noiseHeight;
    }

    
    private void AddingAssets()
    {
        for(int i=0; i < vertices.Length; i++)
        {
            Vector3 WorldPoint = transform.TransformPoint(mesh.vertices[i]);
            var noiseHeight = WorldPoint.y;
            var spawnAboveTerrain = noiseHeight;
            if(Random.Range(1, 3) == 1)
            {
                GameObject SpawnTree = tree_1;
                float nextTree = Random.Range(-5, 5);
                Instantiate(SpawnTree, new Vector3(mesh.vertices[i].x * MeshScale + nextTree, spawnAboveTerrain, mesh.vertices[i].z * MeshScale + nextTree), Quaternion.identity);         
            }    
            /*   
            GameObject SpawnGrass = Grass;
            float nextGrass = Random.Range(-5, 5);
            Instantiate(SpawnGrass, new Vector3(mesh.vertices[i].x * MeshScale + nextGrass, spawnAboveTerrain, mesh.vertices[i].z * MeshScale + nextGrass), Quaternion.identity);
            */
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        gameObject.transform.localScale = new Vector3(MeshScale, MeshScale, MeshScale);

        AddingAssets();
        
    }

}

