using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MappingUtilities
{
    public static TerrainData GenerateTerrain(this TerrainData terrainData, int seed, Vector2 offset, float perlinScale = 1, float heightScale = 1, int octaves = 1, float persistance = 1, float lacunarity = 1)
    {
        int resolution = terrainData.heightmapResolution;
        float[,] heights = GenerateHeights(resolution, seed, offset, perlinScale, heightScale, octaves, persistance, lacunarity);

        terrainData.SetHeights(0, 0, heights);

        return terrainData;
    }

    public static float[,] GenerateHeights(int resolution, int seed, Vector2 offset, float perlinScale = 1, float heightScale = 1, int octaves = 1, float persistance = 1, float lacunarity = 1)
    {
        perlinScale = Mathf.Clamp(perlinScale, 0.0001f, perlinScale);
        float halfRes = resolution / 2f;

        float[,] heights = new float[resolution, resolution];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float shiftedXCoord = (x - halfRes);
                float shiftedYCoord = (y - halfRes);

                float amplitude = 1;
                float frequency = 1;

                Vector2 centerPoint = new Vector2(resolution / 2f, resolution / 2f);
                Vector2 heightmapPoint = new Vector2(x, y);
                float distance = Vector2.Distance(centerPoint, heightmapPoint);

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = shiftedXCoord / perlinScale * frequency + octaveOffsets[i].x;
                    float yCoord = shiftedYCoord / perlinScale * frequency + octaveOffsets[i].y;

                    heights[x, y] += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                float falloff = Mathf.Clamp(Mathf.Pow(1.8f - distance * 1.5f / centerPoint.x, 1.8f), 0, 1);
                heights[x, y] *= heightScale;
                heights[x, y] *= falloff;
            }
        }

        return heights;
    }
}
