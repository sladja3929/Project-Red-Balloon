using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveTime;
    [SerializeField] private Vector3 moveDir;

    private float _curTime;
    private void OnEnable()
    {
        _curTime = 0;        
        moveDir = moveDir.normalized;
    }

    private void FixedUpdate()
    {
        if(_curTime < moveTime)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
        _curTime += Time.deltaTime;
    }
}
