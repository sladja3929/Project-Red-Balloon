using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUI : MonoBehaviour
{
    private Material _mat;
    
    private void Awake()
    {
        _mat = transform.GetComponent<Renderer>().material;

    }
    
    public void SetChargeUI(float chargeRate)
    {
        _mat.SetFloat("_ChargeRate", chargeRate);
    }
}
