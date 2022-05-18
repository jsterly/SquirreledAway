using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour
{
    private float moveSpeed = 10f;
    public float rotSpeed = 110f;
    public Rigidbody body;

    private bool isWandering = false;
    private bool isRoatingLeft = false;
    private bool isRoatingRight = false;
    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(wander());
        }
        
        if (isRoatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
       
        if (isRoatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }

        if (isWalking == true)
        {
            body.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
            //transform.position += (transform.forward * moveSpeed * Time.deltaTime); Doesn't move on y-axis
        }

    }

    IEnumerator wander()
    {
        //amount of time rotaiting 1,3
        int rotTime = Random.Range(1, 2);
        //time in between each time it rotates 1,4
        int rotateWait = Random.Range(1, 2);
        //Acts like a bool, determining if the roate will be leftward or rightward 1,2
        int rotateLorR = Random.Range(1, 2);
        //amount of time in between walking 1,4
        int walkWait = Random.Range(1, 2);
        //amount of time walking 1,5
        int walkTime = Random.Range(3, 20);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        if (rotateLorR == 1)
        {
            isRoatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRoatingLeft = false;
        }
        if (rotateLorR == 2)
        {
            isRoatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRoatingRight = false;
        }

        isWandering = false;
    }
}
