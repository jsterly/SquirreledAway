using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitWander : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotSpeed = 10f;
    public Rigidbody body;
    Animator anim;

    private bool isWandering = false;
    private bool isRoatingLeft = false;
    private bool isRoatingRight = false;
    private bool isWalking = false;
    private bool isEating = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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
            //Stop animation while turning
            anim.SetFloat("Speed_f", 0f);
            //turn
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }

        if (isRoatingRight == true)
        {
            //Stop animation while turning
            anim.SetFloat("Speed_f", 0f);
            //turn
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }

        if (isWalking == true)
        {
            anim.SetFloat("Speed_f", moveSpeed);
            body.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
        }
    }

    IEnumerator wander()
    {
        //movement options are 3) Walk, 4) Run
        int movement = Random.Range(3, 5);
        anim.SetInteger("Movement_i", movement);

        //Walk
        if (movement == 3)
        {
            moveSpeed = 7.5f;
        }

        //Run 
        if (movement == 4)
        {
            moveSpeed = 15f;
        }

        //amount of time rotaiting 1,3
        int rotTime = Random.Range(3, 6);
        //Acts like a bool, determining if the roate will be leftward or rightward (3 is not included)
        int rotateLorR = Random.Range(1, 3);
        //amount of time walking 1,5
        int walkTime = Random.Range(3, 15);

        isWandering = true;

        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;

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
