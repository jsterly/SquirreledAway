using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Additive);
    }
}
