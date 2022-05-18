using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHome : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public Transform wallLocation;
    public Transform floorLocation;
    private GameObject wall;
    private GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        wall = Instantiate(wallPrefab, wallLocation.position, Quaternion.identity);
        floor = Instantiate(floorPrefab, floorLocation.position, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Collectible.structureCollected == 1)
        {
            GameObject.Destroy(wall);
            var prefabwall = Resources.Load("Assets/House Models/House3 Variant.prefab");
            wall = (GameObject)Instantiate(Resources.Load("Assets/House Models/House3 Variant.prefab"), wallLocation.position, Quaternion.identity);
        }
    }
}
