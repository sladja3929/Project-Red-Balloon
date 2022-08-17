using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class chargeState : StateMachineBehaviour
{
    private Slider _slider;
    
    public float chargeGauge;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("in charge State");
        
        chargeGauge = 0f;

        _slider = GameObject.FindWithTag("ChargeSlider").GetComponent<Slider>();
        
        animator.ResetTrigger("land");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    [SerializeField] private float chargeSpeed;
    [SerializeField] private KeyCode chargeKey;
    private static readonly int ChargeGauge = Animator.StringToHash("chargeGauge");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetKey(chargeKey))
        {
            chargeGauge += chargeSpeed * Time.deltaTime;
            if (chargeGauge > 1f) chargeGauge = 1f;
        }
        else
        {
            animator.SetTrigger("startShoot");
        }
        //Debug.Log(chargeGauge);

        _slider.value = chargeGauge;
        
        animator.SetFloat(ChargeGauge, chargeGauge);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _slider.value = 0;
        
        
        animator.GetComponent<ShowArrow>().Hide();
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
