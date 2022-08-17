using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class FlyingState : StateMachineBehaviour
{
    private float time;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = Time.deltaTime;
        time = 0;
        
        
        
        if (animator.GetComponent<BallonShoot>().StartMove(animator.GetFloat("chargeGauge")))
        {
            CameraController.instance.onControll = CameraController.ControllType.LookAround;
        }
        else
        {
            Debug.Log("발사실패");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (animator.GetComponent<Rigidbody>().velocity.magnitude == 0 && time > 0.5f)
        {
            animator.SetTrigger("land");
        }
    }
}
