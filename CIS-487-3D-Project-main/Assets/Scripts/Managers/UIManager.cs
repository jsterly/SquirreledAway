using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image blinder;
    private Color blinderColor = Color.black;
    private float targetOpacity = 1f;

    public GameObject blinds;
    public bool blind = true;
    public bool isFullyBlind { get; set; } = false;
    public float blindOpacity { get => blinderColor.a; set => blinderColor.a = value; }

    public delegate void Notify();
    public static event Notify BlindComplete;

    // Start is called before the first frame update
    void Start()
    {
        blinder = blinds.GetComponent<Image>();
        blinder.color = blinderColor;
        blind = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetOpacity = blind ? 1f : 0f;
        float multiplier = blind ? 5f : 1f;

        blindOpacity = Mathf.Lerp(blindOpacity, targetOpacity, Time.deltaTime * multiplier);

        if (Approximate(blindOpacity, targetOpacity, 0.01f))
        {
            if (!isFullyBlind)
            {
                blinderColor.a = targetOpacity;
                isFullyBlind = true;
                BlindComplete();
            }
        } 
        else
        {
            isFullyBlind = false;
        }

        blinder.color = blinderColor;
    }

    private bool Approximate(float l, float r, float precision)
    {
        if (l <= r + precision && l >= r - precision)
            return true;

        return false;
    }
}
