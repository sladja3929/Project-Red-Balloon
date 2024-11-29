using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BrokenIsland : Gimmick
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float moveDistance = 10f;
    [SerializeField] private float moveDuration = 5f;
    [SerializeField] private float wait = 2f;
    
    // Original position and rotation
    private Vector3 originalPos;
    private Vector3 targetPos;
    private bool isMoving;
    private bool isOn;
    
    void Awake()
    {
        originalPos = transform.localPosition;
        targetPos = originalPos + transform.right * moveDistance;
        isMoving = false;
        isOn = false;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            isOn = true;
            if(!isMoving) Execute();
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isOn = false;
        }
    }
    
    public override void Execute()
    {
        if (!isGimmickEnable) return;

        isMoving = true;
        StartCoroutine("MoveObject");
    }
    
    IEnumerator MoveObject()
    {
        yield return new WaitForSeconds(delay);

        if (isOn)
        {
            Debug.Log("sss");
            GameManager.instance.AimToFallForced();
            
        }
        
        yield return StartCoroutine(MoveToPosition(targetPos, moveDuration));
        GameManager.instance.FallToAimForced();
        
        yield return new WaitForSeconds(wait);
        GameManager.instance.AimToFallForced();
        
        yield return StartCoroutine(MoveToPosition(originalPos, moveDuration));
        GameManager.instance.FallToAimForced();
        isMoving = false;
    }

    IEnumerator MoveToPosition(Vector3 destination, float duration)
    {
        float elapsed = 0f;
        Vector3 startingPos = transform.localPosition;

        while (elapsed < duration)
        {
            // Smoothly interpolate between the starting position and the destination
            transform.localPosition = Vector3.Lerp(startingPos, destination, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        transform.localPosition = destination;
    }
}
