using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private Text text;
    private int numText = 0;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the Arial font from the Unity Resources folder.
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        // Create Canvas GameObject.
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        CanvasScaler scalar = canvasGO.AddComponent<CanvasScaler>();
        scalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Text GameObject.
        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        // Set Text component properties.
        text = textGO.GetComponent<Text>();
        text.font = arial;
        text.text = "Press the space key to continue";
        text.fontSize = 48;
        text.alignment = TextAnchor.MiddleCenter;

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform;
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 300); //(x,y)
    }

    void Update()
    {
        // Press the space key to change the Text message.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (numText == 0)
            {
                text.text = "A storm has destroyed your home";
            }
            if (numText == 1)
            {
                text.text = "And, winter is fast approaching";
            }
            if (numText == 2)
            {
                text.text = "Collect sticks, feathers, and food, at least 5 of each, to rebuild and prepare";
            }
            if (numText == 3)
            {
                text.text = "Be careful not to get too close to the crows";
            }
            if (numText == 4)
            {
                text.text = "If they spot you, a red ! will appear over your head";
            }
            if (numText == 5)
            {
                text.text = "Hide in a bush until the danger bar goes away";
            }
            if (numText == 6)
            {
                text.text = "Or hide in your house, look for a hole in one of the big trees near the middle";
            }
            if (numText == 7)
            {
                text.text = "Now, get collecting!";
            }
            if(numText == 8)
            {
                text.fontSize = 30;
                text.text = "Controls: \n WASD for movement \n ESCAPE or P to pause \n E to interact"; 
            }
            if(numText == 9)
            {
                SceneManager.LoadScene("GeneratedWorldMap");
            }

            numText = numText + 1;
        }
    }
}
