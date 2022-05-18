using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;


public class Collectible : MonoBehaviour
{
    public static int structureCollected;
    public static int foodCollected;
    public static int comfortCollected;
    public AudioSource pop;


    void Start()
    {
        structureCollected = 0;
        foodCollected = 0;
        comfortCollected = 0;
    }


    private void OnTriggerEnter(Collider other)
{

    if (other.CompareTag("structure"))
    {
        structureCollected++;
            pop.Play();
        Destroy(other.gameObject);
    }
    if (other.CompareTag("food"))
    {
        foodCollected++;
            pop.Play();
            Destroy(other.gameObject);
    }
    if (other.CompareTag("comfort"))
    {
        comfortCollected++;
            pop.Play();
            Destroy(other.gameObject);
    }

    if (other.CompareTag("collectible"))
    {
        //use this for the 
        /*if(other.GetComponent<Enemy>)
        {
            Destroy(other.gameObject);
        }*/
    }

    if (structureCollected >= 5 && foodCollected >= 5 && comfortCollected >= 5)
    {
        haveonw();
    }
}

 


   public void haveonw() {
        SceneManager.LoadScene(3);
    }

}
