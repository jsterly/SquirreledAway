using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableCollectable : MonoBehaviour
{
    public CollectableItem collectableItem;
    private Vector3 spawnPoint;
    private Vector3 groundPoint;
    private SphereCollider sCollider;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit);
        groundPoint = hit.point;
    }

    private void FixedUpdate()
    {
        var playerDistance = Vector3.Distance(player.transform.position, transform.position);
        if (playerDistance < 100)
        {
            // Debug.Log(playerDistance);

            float time = Time.fixedTime;
            float bob = Mathf.Sin(time * collectableItem.bobRate) * collectableItem.bobAmplitude;

            transform.position = groundPoint + (Vector3.up * collectableItem.height) + (Vector3.up * bob);
            transform.Rotate(Vector3.up * Time.fixedDeltaTime * collectableItem.rotationRate);
        }
    }
}
