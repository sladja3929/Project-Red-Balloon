using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ModeSelect : StateMachineBehaviour
{

    public KeyCode key1, key2;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("mode", 1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(key1))
        {
            animator.SetInteger("mode", 1);
            Debug.Log("Set mode Shoot");
        }
        else if (Input.GetKeyDown(key2))
        {
            animator.SetInteger("mode", 2);
            Debug.Log("Set Mode Bounce");
        }
    }
}
