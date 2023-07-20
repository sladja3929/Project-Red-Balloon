using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperMode : MonoBehaviour
{
    public float moveSpeed = 30;

    private Rigidbody _rigidbody;
    private CameraController _controller;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GameObject.Find("Viewer").GetComponent<CameraController>();
    }

    void Start()
    {
        _rigidbody.useGravity = false;
        _controller.onControll = CameraController.ControllType.LookAround;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position -= Vector3.up * (moveSpeed * Time.deltaTime);
        }

        transform.rotation = _controller.GetRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveSpeed = (moveSpeed += 10) % 50;
        }
    }

    private void OnDestroy()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;

        _controller.onControll = CameraController.ControllType.Stop;
    }
}
