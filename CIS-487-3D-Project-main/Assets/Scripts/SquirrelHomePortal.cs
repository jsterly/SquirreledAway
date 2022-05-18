using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelHomePortal : MonoBehaviour
{
    public Transform HouseTeleport;

    [SerializeField] private bool triggerActive = false;
    private bool isTeleporting = false;
    private GameObject PlayerObj;
    private Player Player;
    private CharacterController PlayerController;
    private UIManager UIManager;
    private Vector3 PlayerSpawnPos;

    private void Start()
    {
        PlayerObj = GameObject.Find("Squirrel_01");
        Player = PlayerObj.GetComponent<Player>();
        PlayerController = PlayerObj.GetComponent<CharacterController>();
        UIManager = GameObject.Find("UI").GetComponent<UIManager>();
        UIManager.BlindComplete += BlindComplete;
    }

    private void OnDestroy()
    {
        UIManager.BlindComplete -= BlindComplete;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerActive = false;
        }
    }

    public void SendToTarget()
    {
        PlayerController.transform.position = HouseTeleport.position;
        PlayerObj.transform.position = HouseTeleport.position;
    }

    private void BlindComplete()
    {
        if (isTeleporting)
        {
            SendToTarget();
            isTeleporting = false;
            UIManager.blind = false;
        }
    }

    private void Update()
    {
        if (triggerActive && Input.GetKeyDown(KeyCode.E))
        {
            isTeleporting = true;
            UIManager.blind = true;
        }
    }
}
