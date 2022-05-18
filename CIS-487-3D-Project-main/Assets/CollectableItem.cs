using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableItem", menuName = "ScriptableObjects/CollectableItem", order = 1)]
public class CollectableItem : ScriptableObject
{
    public float height = 1f;
    public float rotationRate = 20f;
    public float bobRate = 1f;
    public float bobAmplitude = 0.05f;
}
