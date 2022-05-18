using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeightMapGenerator : MonoBehaviour
{
    TerrainData td;

    public float perlinScale = 1;

    [Range(0, 1)]
    public float heightScale = 1;

    [Range(1, 10)]
    public int octaves = 1;
    public float persistance = 1;
    public float lacunarity = 1;

    [HideInInspector]
    public int seed = 0;

    public Vector2 offset;

    public TerrainData Generate()
    {
        td = new TerrainData
        {
            heightmapResolution = 33,
            baseMapResolution = 1024,
            size = new Vector3(1000, 500, 1000)
        };

        return td.GenerateTerrain(seed, offset, perlinScale, heightScale, octaves, persistance, lacunarity);
    }
}
