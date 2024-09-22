using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUI : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float maxLength;

    private void Awake()
    {
        Transform tsf = transform;
        // tsf.localPosition = Vector3.Lerp(start, end, 0.5f);
        start = tsf.localPosition - new Vector3(0, 0, (maxLength * 1f));
        end = tsf.localPosition + new Vector3(0, 0, (maxLength * 1f));
    }
    
    public void SetChargeUI(float chargeRate)
    {
        Transform tsf = transform;
        Vector3 localScale = tsf.localScale;
        localScale.z = chargeRate * maxLength;
        tsf.localScale = localScale;
        
         Vector3 currentEnd = Vector3.Lerp(start, end, chargeRate);
         tsf.localPosition = Vector3.Lerp(start, currentEnd, 0.5f);
    }
}
