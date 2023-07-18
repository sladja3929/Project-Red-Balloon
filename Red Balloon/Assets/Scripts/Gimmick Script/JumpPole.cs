using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class JumpPole : Gimmick
{
    private Rigidbody _rigid;

    public Vector3 pushDirection;
    void Awake()
    {
        //https://dydvn.tistory.com/28
        _rigid = GetComponent<Rigidbody>();
    }

    public float pushTime;
    public float pushPower;
    public bool isPushing;

    private IEnumerator Push(GameObject obj)
    {
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();
        isPushing = true;
        
        float time = 0;

        while (true)
        {
            const float DT = 0.01f;
            time += DT;
            if (time  > pushTime) break;
            
            objRigid.AddForce(pushDirection * pushPower);
            yield return new WaitForSeconds(DT);
        }

        isPushing = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isGimmickEnable) return;
        if (!collision.gameObject.CompareTag("Player") || isPushing) return;
        
        StartCoroutine(Push(collision.gameObject));
    }

}
