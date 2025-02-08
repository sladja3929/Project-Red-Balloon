using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperMode : MonoBehaviour
{
    public float moveSpeed = 60;

    private Rigidbody _rigidbody;
    private CameraController _controller;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GameObject.Find("Viewer").GetComponent<CameraController>();
    }

    private void Start()
    {
        _rigidbody.useGravity = false;
        _controller.onControll = CameraController.ControllType.LookAround;
    }
    
    // Update is called once per frame
    private void Update()
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
            moveSpeed = (moveSpeed += 20) % 100;
        }
    }

    private void OnDestroy()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = true;

        _controller.onControll = CameraController.ControllType.Stop;
    }
}