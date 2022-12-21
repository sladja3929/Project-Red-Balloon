using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class JumpPole : MonoBehaviour
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
    public bool isPushing = false;
    IEnumerator Push(GameObject obj)
    {
        
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();
        isPushing = true;
        
        float time = 0;

        while (true)
        {
            float dt = 0.01f;
            time += dt;
            if (time  > pushTime) break;
            
            objRigid.AddForce(pushDirection * pushPower);
            yield return new WaitForSeconds(dt);
        }

        isPushing = false;
    }


        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isPushing)
            {
                StartCoroutine(Push(collision.gameObject));
            }
        }

}
