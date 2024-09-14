using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BirdStrike : Gimmick
{
    [Header("Setting")] 
    [SerializeField] private float birdSpeed;
    [SerializeField] private float maxDistance;
    
    [Header("Debugging")]
    [SerializeField] private Transform balloonTransform;
    [SerializeField] private bool birdFlying;
    private void Awake()
    {
        balloonTransform = GameObject.FindWithTag("Player").transform;
    }
    
    public override void Execute()
    {
        if (birdFlying) return;
        
        Debug.Log("Bird Strike Call");
        
        StartCoroutine(MoveCoroutine());
        
    }

    private IEnumerator MoveCoroutine()
    {
        birdFlying = true;
        Debug.Log("Bird Strike Coroutine Call");

        var tsf = transform;

        tsf.position = Camera.main.transform.position;
        tsf.LookAt(balloonTransform);
        
        var birdDirection = (balloonTransform.position - tsf.position).normalized;
        
        //rotate bird y 90
        tsf.Rotate(0, 90, 0);
        
        float totalMovedDistance = 0f;
        while (totalMovedDistance < maxDistance)
        {
            totalMovedDistance += birdSpeed * Time.deltaTime;
            tsf.position += birdDirection * (birdSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("닿았다");
        
        if (!col.CompareTag("Player")) return;
        
        Debug.Log("KillBalloon");

        GameManager.instance.KillBalloon();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("닿았다");
        
        if (!other.gameObject.CompareTag("Player")) return;
        
        Debug.Log("KillBalloon");

        GameManager.instance.KillBalloon();
    }
}
