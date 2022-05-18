using UnityEngine;

using System.Collections;

public class Player : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    public float speed = 1000.0f;
    public float turnSpeed = 400.0f;
    private Vector3 moveDirection = Vector3.zero;
    public bool climbining = false;
    public float gravity = 20.0f;
    public AudioSource moveSounds;

    private Camera maincam;

    public bool isInHouse { get; set; } = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        maincam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        moveSounds.enabled = !isInHouse;

        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("a"))
        {
            if (!moveSounds.isPlaying)
                moveSounds.Play();

            anim.SetInteger("AnimationPar", 1);

            float rot = maincam.transform.rotation.eulerAngles.y + (Input.GetKey("s") ? -180 : 0);

            if (Input.GetKey("w") || Input.GetKey("s"))
            {
                rot += (90 - (45 * Mathf.Abs(Input.GetAxis("Vertical")))) * Input.GetAxis("Horizontal") * Input.GetAxis("Vertical");
            }
            else if (Input.GetKey("a") || Input.GetKey("d"))
            {
                rot += (90 * Input.GetAxis("Horizontal"));
            }

            float rotdelta = rot - transform.rotation.eulerAngles.y;
            transform.Rotate(0, rotdelta, 0);
        }
        else
        {
            moveSounds.Stop();
            anim.SetInteger("AnimationPar", 0);
        }

        if (controller.isGrounded)
        {
            if (Input.GetAxis("Vertical") > .1 || Input.GetAxis("Vertical") < -.1)
                moveDirection = transform.forward * Mathf.Abs(Input.GetAxis("Vertical")) * speed;
            else
            {
                moveDirection = transform.forward * Mathf.Abs(Input.GetAxis("Horizontal")) * speed;
            }
        }

        Ray r = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(r, out RaycastHit hitInfo, 100))
        {
            if (hitInfo.transform.CompareTag("World"))
            {
                var correctedForward = Vector3.Cross(transform.right, hitInfo.normal);
                Quaternion groundRotation = Quaternion.LookRotation(correctedForward, hitInfo.normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, groundRotation, Time.deltaTime * 10f);
            }
        }

        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
    }
}
































