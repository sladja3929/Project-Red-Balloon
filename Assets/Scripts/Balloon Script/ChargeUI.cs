using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUI : MonoBehaviour
{
    private Material[] _mats;
    
    private void Awake()
    {
        List<Material> materialList = new List<Material>();

        // 자신과 자식 오브젝트의 모든 Renderer 컴포넌트를 가져옴
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            materialList.AddRange(rend.materials);
        }
        
        _mats = materialList.ToArray();
    }
    
    public void SetChargeUI(float chargeRate)
    {
        foreach (var _mat in _mats)
        {
            _mat.SetFloat("_ChargeRate", chargeRate);
        }
    }
}
