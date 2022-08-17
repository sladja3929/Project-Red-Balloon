using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPole : MonoBehaviour
{
    private Rigidbody rigid;

    public Vector3 _pushDirection;
    void Awake()
    {
        
        //https://dydvn.tistory.com/28
        
        rigid = GetComponent<Rigidbody>();
    }

    public float pushTime;
    public float pushPower;
    public bool isPushing = false;
    private void Push(GameObject obj)
    {
        
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();
        isPushing = true;
        
        float time = 0;

        while (true)
        {
            float dt = Time.deltaTime;
            time += dt;
            if (time  > pushTime) break;
            
            objRigid.AddForce(_pushDirection * pushPower);
            Debug.Log("Push");
            Debug.Log(_pushDirection * pushPower);
        }

        isPushing = false;
    }


        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isPushing)
            {
                Push(collision.gameObject);
            }
        }

}
