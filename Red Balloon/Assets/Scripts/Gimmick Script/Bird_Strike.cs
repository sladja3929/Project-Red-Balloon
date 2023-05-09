using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Strike : Gimmick
{
    [SerializeField] private Transform balloonTransform;
    [SerializeField] private float birdDistance;

    [SerializeField] private float birdFlyTime;

    [SerializeField] private bool birdFlying;
    private void Awake()
    {
        balloonTransform = GameObject.FindWithTag("Player").transform;
    }
    
    public override void Execute()
    {
        if (Camera.main == null) return;
        if (birdFlying) return;
        
        Debug.Log("Bird Strike Call");
        
        StartCoroutine(MoveCoroutine());
        
    }

    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        birdFlying = true;
        Debug.Log("Bird Strike Coroutine Call");
        var vec = balloonTransform.position - Camera.main.transform.position;
        var birdDirection = new Vector3(-vec.z, 0, vec.x).normalized;
        transform.position = balloonTransform.position - birdDirection * birdDistance;
        var birdSpeed = (birdDistance * 2) / birdFlyTime;

        float time = 0;
        while (time < birdFlyTime)
        {
            transform.position += birdDirection * (birdSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(0, -100, 0);
        birdFlying = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("닿았다");
        
        if (!col.CompareTag("Player")) return;
        
        Debug.Log("KillBalloon");

        GameManager.Instance.KillBalloon();
    }
}
