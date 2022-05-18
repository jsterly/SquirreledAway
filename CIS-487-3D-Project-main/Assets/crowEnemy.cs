using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class crowEnemy : MonoBehaviour
{
    public GameObject Character; // Target Object to follow
    public float speed = 20f; // Enemy speed
    public GameObject attackUI;
    public GameObject attackBarUI;
    public GameObject featherPrefab;
    public float dropTime = 10.0f;
    private Vector3 directionOfCharacter;
    private Vector3 searchPoint;
    public AudioSource source;

    public StaminaBar attackBar;

    public static float attackTimer;
    public static float totalTime;

    public float personalTimer;
    private bool runTime;

    public Transform CharacterEnemy;
    private Vector3 directionOfEnemy;
    private Vector3 heightOffset;
    private Vector3 returnDirection;
    private Vector3 returnOffset;
    private Vector3 previousPosition;
    private Vector3 terrainHeightAdjustment;
    public bool patrolling, following, returning;
    public static bool hiding, chasing;
    private Vector3 startPoint;
    private Vector3 patrolRadius;
    public Rigidbody rb;
    public bool setSearchPoint;
    Animator animator;


    private Vector3 startHeight;


    // Start is called before the first frame update
    void Start()
    {
        setSearchPoint = true;
        attackTimer = 30;
        totalTime = attackTimer;
        personalTimer = totalTime;
        attackBar.SetMaxStamina(totalTime);
        // runTime = false;
        patrolRadius = new Vector3(5, 0, 0);
        heightOffset = new Vector3(0, 2, 0);
        terrainHeightAdjustment = new Vector3(0, 1, 0);
        returnOffset = new Vector3(1, 1, 1);
        startHeight = new Vector3(0, 20, 0);
        searchPoint = transform.position; //just so it doesn't give an error
        startPoint = transform.position;
        patrolling = true;
        hiding = false;
        following = false;
        returning = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
        StartCoroutine(featherWave());
        Character = GameObject.Find("Squirrel_01");

        Ray myRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(myRay, out RaycastHit hit))
        {
            //Debug.Log(hit.point);
            startPoint = hit.point + startHeight;
            transform.position = startPoint;
            previousPosition = transform.position;
            searchPoint = transform.position;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            attackBar.SetStamina(attackTimer);
            if (attackTimer < totalTime)
            {
                attackBarUI.SetActive(true);
            }
            else
            {
                attackBarUI.SetActive(false);
            }



            var distance = Vector3.Distance(Character.transform.position, transform.position);
            //Debug.Log(distance);
            if (patrolling)
            {
                Vector3 deltaPosition = transform.position - previousPosition;

                if (deltaPosition != Vector3.zero)
                {
                    // Same effect as rotating with quaternions, but simpler to read
                    transform.forward = deltaPosition;
                }
                // Recording current position as previous position for next frame
                previousPosition = transform.position;

                transform.RotateAround(startPoint + patrolRadius, Vector3.up, 20 * Time.deltaTime);
               // Debug.Log("Player location" + Character.transform.position);
            }
            if (distance <= 45 && distance > 2.1 && !hiding)
            {
                Debug.Log("Crow is attacking");


                patrolling = false;
                following = true;
                directionOfCharacter = Character.transform.position - transform.position + heightOffset;
                directionOfCharacter = directionOfCharacter.normalized;    // Get Direction to Move Towards
                                                                           //Look at direction of character
                transform.LookAt(Character.transform.position + heightOffset);
                transform.Translate(directionOfCharacter * 10 * Time.deltaTime, Space.World);

                //an attempt to make the crow adjust to the height of the terrain while following player
               

            }
            if (following && attackTimer > 0 && !hiding)
            {

                if (!source.isPlaying)
                    source.Play();
                //set crowattackUI to active
                attackUI.SetActive(true);
                attackTimer -= Time.deltaTime;
                personalTimer -= Time.deltaTime;
               // Debug.Log("Time:" + attackTimer);
            }
            if (attackTimer <= 0)
            {
                Debug.Log("You are dead");
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.LoadScene(2);

            }

            if (hiding)
            {
                attackUI.SetActive(false);
                if (personalTimer < totalTime)
                {
                    //search for squirrel
                    attackTimer += Time.deltaTime;
                    personalTimer += Time.deltaTime;
                    //Debug.Log("Time: " + attackTimer);

                    if (setSearchPoint)
                    {
                        searchPoint = transform.position;
                        setSearchPoint = false;
                    }
                    Vector3 deltaPosition = transform.position - previousPosition;

                    if (deltaPosition != Vector3.zero)
                    {
                        // Same effect as rotating with quaternions, but simpler to read
                        transform.forward = deltaPosition;
                    }
                    // Recording current position as previous position for next frame
                    previousPosition = transform.position;

                    transform.RotateAround(searchPoint + patrolRadius, Vector3.up, 40 * Time.deltaTime);
                }
                else
                {
                    setSearchPoint = true;
                    returning = true;
                    Debug.Log("Returning is true");
                }

                Debug.Log("Squirrel is hiding");
            }
            else
            {
                setSearchPoint = true;
                Debug.Log("Squirrel is not hiding");
            }
            if (attackTimer >= totalTime && returning && !patrolling)
            {
                //have crow stop chasing squirrel and return to patrol
                Debug.Log("Returning");
                following = false;
                returnDirection = startPoint - transform.position;
                returnDirection = returnDirection.normalized;
                transform.Translate(returnDirection * 10 * Time.deltaTime, Space.World);
                var distanceFromStart = Vector3.Distance(startPoint, transform.position);
                //Debug.Log("distance from start: " + distanceFromStart);
                transform.LookAt(startPoint);
                //Debug.Log(transform.position);
                if (distanceFromStart <= 2)
                {
                    Debug.Log("Patrolling again");
                    patrolling = true;
                    returning = false;
                }

            }

            // Debug.Log("Position of character" + Character.transform.position);
            // Debug.Log("Position of enemy" + transform.position);
        }
    }//closes update

    void dropFeather()
    {
        GameObject a = Instantiate(featherPrefab) as GameObject;
        a.transform.position = transform.position;
    }

    IEnumerator featherWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(dropTime);
            dropFeather();
        }
    }



}//closes class
