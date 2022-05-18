/* DEBUG LATER :)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    IdleState iState = new IdleState();
    WalkState wState = new WalkState();
    RunState rState = new RunState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = iState;
        //??EHHHHHHHHHHHHHHHHHH, I"M NOT SO SURE ABOUT YOU \/
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //??EHHHHHHHHHHHHHHHHHH, I"M NOT SO SURE ABOUT YOU \/
        currentState.UpdateState(this);
    }
}
*/