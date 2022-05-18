using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidingScript : MonoBehaviour
{
    MeshCollider meshCollider;
    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
       // Debug.Log("Collision from box side:" + collision.gameObject.name);
        if (collision.gameObject.name == "Squirrel_01")
        {
            //Debug.Log("Collided with box");
            crowEnemy.hiding = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Squirrel_01")
        {
            crowEnemy.hiding = false;
        }
    }
}
