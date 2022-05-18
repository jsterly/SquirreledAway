using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherDrop : MonoBehaviour
{
    public float existingTimer;
    // Start is called before the first frame update
    void Start()
    {
        existingTimer = 90;
    }

    // Update is called once per frame
    void Update()
    {
        existingTimer -= Time.deltaTime;

        if(existingTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}
