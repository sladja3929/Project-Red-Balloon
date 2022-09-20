using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJumpPoleAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _moveDirection;
    public float moveSpeed;
    private Vector3 _moveScale;
    private Vector3 _originalScale;
    void Awake()
    {
        _originalScale = transform.localScale;
        _moveDirection = GetComponent<JumpPole>().pushDirection;
        _moveScale = _moveDirection.normalized * moveSpeed;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<JumpPole>().isPushing)
        {
            transform.localScale += _moveScale;
        }
        else if(transform.localScale != _originalScale)
        {
            transform.localScale -= _moveScale;
        }
    }
}
