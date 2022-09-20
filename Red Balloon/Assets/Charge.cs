using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Charge : StateMachineBehaviour
{
    private Slider _slider;
    
    public float chargeGauge;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("in charge State");
        
        chargeGauge = 0f;

        _slider = GameObject.FindWithTag("ChargeSlider").GetComponent<Slider>();
        
        animator.ResetTrigger("land");
    }

    [SerializeField] private float chargeSpeed;
    [SerializeField] private KeyCode chargeKey;
    private static readonly int ChargeGauge = Animator.StringToHash("chargeGauge");

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _slider.value = 0;
        
        
        animator.GetComponent<ShowArrow>().Hide();
    }
}
