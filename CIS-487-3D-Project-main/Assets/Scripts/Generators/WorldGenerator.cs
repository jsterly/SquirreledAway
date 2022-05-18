using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldGenerator : MonoBehaviour
{
    private TerrainHeightMapGenerator terrainGenerator;
    private TerrainCollider tc;
    private TerrainData td;
    private Terrain t;

    private bool homePlaced = false;
    private bool loadExistingMap = false;
    private string TerrainSavePath = "Assets/TerrainSave.asset";

    [Header("Collectible Items")]
    public GameObject[] collectibles;

    [Header("World Props")]
    public GameObject[] trees;
    public GameObject[] grass;
    public GameObject[] rocks;
    public GameObject[] bushes;

    [Header("The Squirrel")]
    public GameObject squirrel;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
        PopulateTerrain();
    }

    private void GenerateTerrain()
    {
        terrainGenerator = GetComponent<TerrainHeightMapGenerator>();
        terrainGenerator.seed = Mathf.RoundToInt(Random.value * 100000f);
        terrainGenerator.offset = new Vector2(Mathf.RoundToInt((Random.value - 0.5f) * 100000f), Mathf.RoundToInt((Random.value - 0.5f) * 100000f));
        terrainGenerator.perlinScale = Random.Range(2.1f, 4.8f);
        terrainGenerator.heightScale = Random.Range(0.04f, 0.06f);
        terrainGenerator.octaves = Mathf.RoundToInt(Random.Range(1f, 10f));

        terrainGenerator.lacunarity = Random.Range(0.01f, 1.5f);
        terrainGenerator.lacunarity /= terrainGenerator.octaves;

        tc = GetComponent<TerrainCollider>();
        t = GetComponent<Terrain>();

        if (!loadExistingMap)
        {
            td = terrainGenerator.Generate();

#if UNITY_EDITOR
            if (AssetDatabase.Contains(td))
                AssetDatabase.DeleteAsset(TerrainSavePath);

            AssetDatabase.CreateAsset(td, TerrainSavePath);   
#endif
        }
        else
        {
#if UNITY_EDITOR
            td = AssetDatabase.LoadAssetAtPath<TerrainData>(TerrainSavePath);
#endif
        }

        t.terrainData = td;
        tc.terrainData = td;
        tag = "World";
    }

    private void CreateHome(GameObject homeTree)
    {
        homeTree.transform.localScale = Vector3.one * 10f;
        homeTree.name = "~~HOME~~";

        homePlaced = true;
        Debug.Log("Placed home in tree.");
    }

    private void PlaceSquirrel(GameObject homeTree)
    {
        bool squirrelPlaced = false;

        while (!squirrelPlaced)
        {
            Vector3 possiblePos = homeTree.transform.position + new Vector3(Mathf.Cos(Time.realtimeSinceStartup) * 10f, 10f, Mathf.Sin(Time.realtimeSinceStartup) * 10f);

            Physics.Raycast(possiblePos, Vector3.down, out RaycastHit hit);
            if (!hit.collider.CompareTag("World"))
                continue;

            squirrel.transform.position = hit.point + Vector3.up;

            var spawnPoint = new GameObject();
            spawnPoint.name = "SpawnPoint";
            spawnPoint.transform.position = squirrel.transform.position;

            squirrelPlaced = true;

            GameObject.Find("ReturnToHome").GetComponent<SquirrelHomePortal>().HouseTeleport = spawnPoint.transform;
        }

        Debug.Log("Placed squirrel around home.");
    }

    private void SpawnHomePortal(GameObject homeTree)
    {

        for (int i = 0; i < homeTree.transform.childCount; i++)
        {
            Transform childTransform = homeTree.transform.GetChild(i);

            if (!Physics.SphereCast(childTransform.position, 1f, childTransform.up, out RaycastHit hitInfo))
            {
                GameObject portalObj = childTransform.gameObject;
                portalObj.SetActive(true);

                SquirrelHomePortal portal = portalObj.GetComponent<SquirrelHomePortal>();
                portal.HouseTeleport = GameObject.Find("HomeSpawn").transform;

                return;
            }
        }

        for (int i = 0; i < homeTree.transform.childCount; i++)
        {
            Transform childTransform = homeTree.transform.GetChild(i);

            if (!Physics.Raycast(childTransform.position, childTransform.up, 1f))
            {
                GameObject portalObj = childTransform.gameObject;
                portalObj.SetActive(true);

                SquirrelHomePortal portal = portalObj.GetComponent<SquirrelHomePortal>();
                portal.HouseTeleport = GameObject.Find("HomeSpawn").transform;

                return;
            }
        }
    }

    private void PopulateTerrain()
    {
        GameObject[] allObjects = trees.Union(rocks).Union(bushes).Union(grass).ToArray();

        int worldPropSpawns = 3500;
        while (worldPropSpawns > 0)
        {
            Vector3 center = td.size / 2f;
            center.y = 0;

            float x = Random.Range(0, td.size.x);
            float normalizedX = x / td.size.x;

            float z = Random.Range(0, td.size.z);
            float normalizedZ = z / td.size.z;

            Vector3 position = new Vector3(x, 0, z);

            float distance = Vector3.Distance(position, center);
            float falloff = 1 - Mathf.Clamp(Mathf.Pow(1.8f - distance * 1.8f / center.x, 1.8f), 0, 1);

            if (Random.value >= falloff)
            {
                float y = t.SampleHeight(position);
                position.y = y;

                Vector3 terrainNormal = td.GetInterpolatedNormal(normalizedX, normalizedZ) * 10f;

                GameObject objectToSpawn = allObjects[Mathf.RoundToInt(Random.Range(0, allObjects.Length))];
                GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.FromToRotation(Vector3.up, terrainNormal), transform);
                spawnedObject.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), terrainNormal);
                spawnedObject.transform.localScale *= Random.Range(1f, 5f);

                if (rocks.Contains(objectToSpawn))
                    spawnedObject.tag = "World";

                if (trees.Contains(objectToSpawn))
                {
                    if (!homePlaced && distance < 100f)
                    {
                        CreateHome(spawnedObject);
                        SpawnHomePortal(spawnedObject);
                        PlaceSquirrel(spawnedObject);
                    }

                    spawnedObject.transform.position += Vector3.down * (spawnedObject.transform.localScale.magnitude * 0.05f);
                }

                worldPropSpawns--;
            }
        }

        int collectibleSpawns = 0;
        while(collectibleSpawns < 100)
        {
            Vector3 center = td.size / 2f;
            center.y = 0;

            float x = Random.Range(0, td.size.x);
            float normalizedX = x / td.size.x;

            float z = Random.Range(0, td.size.z);
            float normalizedZ = z / td.size.z;

            Vector3 position = new Vector3(x, 0, z);

            float distance = Vector3.Distance(position, center);
            float falloff = 1 - Mathf.Clamp(Mathf.Pow(1.8f - distance * 1.8f / center.x, 1.8f), 0, 1);

            if (Random.value >= falloff)
            {
                float y = t.SampleHeight(position);
                position.y = y;

                Ray r = new Ray(position, Vector3.down);
                Collider[] hits = Physics.OverlapBox(position, Vector3.one * 2f);

                if (!hits.Any(h => h.name.ToLower().Contains("terrain")))
                    continue;

                Vector3 terrainNormal = td.GetInterpolatedNormal(normalizedX, normalizedZ);
                GameObject objectToSpawn = collectibles[Mathf.RoundToInt(Random.Range(0, collectibles.Length))];
                GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.LookRotation(Vector3.right, terrainNormal), transform);
                
                collectibleSpawns++;
            }
        }
    }
}
