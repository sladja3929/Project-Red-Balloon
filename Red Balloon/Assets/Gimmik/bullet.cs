using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;

    public Rigidbody rigid;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigid.velocity = speed * Vector3.forward;
    }
}
