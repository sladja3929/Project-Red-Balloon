using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class setDirState : StateMachineBehaviour
{
    private ShowArrow sA;
    private DragRotation dr;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //화살표 표시
        sA = animator.GetComponent<ShowArrow>(); sA.Show();
        
        //카메라 컨트롤 타입 드래그로 변경
        CameraController.instance.onControll = CameraController.ControllType.Drag;
        dr = animator.GetComponent<DragRotation>();
        dr.onControll = true;

        

        animator.ResetTrigger("startCharge");
        animator.ResetTrigger("startShoot");
    }

    [SerializeField] private KeyCode chargeKey;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKey(chargeKey))
        {
            animator.SetTrigger("startCharge");
        }
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CameraController.instance.onControll = CameraController.ControllType.Stop;

        Vector3 dirVec = new Vector3(0, 0, 1);
        Vector3 rotatedDirVec = animator.GetComponent<DragRotation>().GetDirection() * dirVec;
        
        animator.GetComponent<BallonShoot>().setMoveDirection(rotatedDirVec);

        dr.onControll = false;

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
