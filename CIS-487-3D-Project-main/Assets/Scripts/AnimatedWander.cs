using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedWander : MonoBehaviour
{
    public float moveSpeed = 0f;
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
        if (isEating == false)
        {
            anim.SetBool("Eat_b", false);
        }

        if (isWandering == false)
        {
            //resets eat to false in case it was true last time
            anim.SetBool("Eat_b", false);

            StartCoroutine(wander());
        }

        if (isRoatingLeft == true)
        {
            //Stop animation while turning
            anim.SetFloat("Speed_f", 0f);
            //turn
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            //Start animation again
            //anim.SetFloat("Speed_f", moveSpeed);
        }

        if (isRoatingRight == true)
        {
            //Stop animation while turning
            anim.SetFloat("Speed_f", 0f);
            //turn
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            //Start animation again
            //anim.SetFloat("Speed_f", moveSpeed);
        }

        if (isWalking == true)
        {
            //resets eat to false in case it was true last time
            anim.SetBool("Eat_b", false);

            anim.SetFloat("Speed_f", moveSpeed);
            body.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
        }

        if (isEating == true)
        {
            anim.SetBool("Eat_b", true);
        }

        

        //anim.SetTrigger("trigger_run"); what'll be the trigger    //2
    }

    IEnumerator wander()
    {
        //movement options are 1) Idle, 2) Eat, 3) Walk, 4) Run
        int movement = Random.Range(2, 5);

        //Idle -- Is not active in current iteration
        if (movement == 1)
        {
            moveSpeed = 0f;
            anim.SetFloat("Speed_f", moveSpeed);

            //amount of time idleing
            int idleTime = Random.Range(5, 15);
            yield return new WaitForSeconds(idleTime);
        }

        //Eat
        if (movement == 2)
        {
            int eatTime = Random.Range(5, 15);

            isWandering = true;

            //amount of time eating
            isEating = true;
            yield return new WaitForSeconds(eatTime);
            anim.SetBool("Eat_b", false);
            isEating = false;

            anim.SetBool("Eat_b", false);

            isWandering = false;
        }

        //Walk and Run
        if (movement == 3 || movement == 4)
        {
            //Walk
            if (movement == 3)
            {
                moveSpeed = 5f;
            }

            //Run 
            if (movement == 4)
            {
                moveSpeed = 10f;
            }

            //amount of time rotaiting 1,3
            int rotTime = Random.Range(3, 6);
            //Acts like a bool, determining if the roate will be leftward or rightward (3 is not included)
            int rotateLorR = Random.Range(1, 3);
            //amount of time walking 1,5
            int walkTime = Random.Range(5, 15);

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

        //isWandering = false;
    }
}
