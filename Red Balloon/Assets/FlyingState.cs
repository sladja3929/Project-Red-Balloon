using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class FlyingState : StateMachineBehaviour
{
    private float _time;

    public float stopCriterionVelocity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time = Time.deltaTime;
        _time = 0;
        
        
        
        Vector3 dirVec = new Vector3(0, 0, 1);
        Vector3 rotatedDirVec = animator.GetComponent<DragRotation>().GetDirection() * dirVec;
        
        animator.GetComponent<BallonShoot>().SetMoveDirection(rotatedDirVec);
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
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (animator.GetComponent<Rigidbody>().velocity.magnitude <= stopCriterionVelocity && _time > 0.5f)
        {
            animator.SetTrigger("land");
        }
    }
}
