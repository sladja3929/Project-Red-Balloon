using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendIsland : Gimmick
{
    [SerializeField] private float descendDistance;
    [SerializeField] private float descendSpeed;
    [SerializeField] private float ascendSpeed;
    [SerializeField] private float delayTime;
    [SerializeField] private Color changeColor;
    
    private float timer;
    private bool onPlatform;
    private bool isMoving;
    private bool activate;
    private Vector3 initialPosition;
    private Renderer _renderer;
    private Color initialColor;
    
    void Start()
    {
        timer = 0;
        onPlatform = false;
        isMoving = false;
        activate = false;
        initialPosition = transform.localPosition;
        _renderer = GetComponent<Renderer>();
        _renderer.material = new Material(_renderer.material);
        initialColor = _renderer.material.color;
    }

    private void FixedUpdate()
    {
        if (isGimmickEnable && !isMoving && activate)
        {
            timer += Time.deltaTime;
            if(onPlatform) _renderer.material.color = Color.Lerp(initialColor, changeColor, timer / delayTime);
            
            if (timer >= delayTime)
            {
                if (onPlatform) StartCoroutine("Descend");
                else StartCoroutine("Ascend");
                timer = 0;
            }
        }
    }

    private IEnumerator Descend()
    {
        Vector3 curPosition = transform.localPosition;
        Vector3 targetPosition = curPosition + Vector3.down * descendDistance;
        isMoving = true;
        if(onPlatform) GameManager.instance.AimToFallForced();
        
        while (Vector3.Distance(curPosition, targetPosition) > 0.01f)
        {
            curPosition = Vector3.MoveTowards(curPosition, targetPosition, descendSpeed * Time.deltaTime);
            transform.localPosition = curPosition;
            yield return null;
        }

        transform.localPosition = targetPosition;
        isMoving = false;
    }

    private IEnumerator Ascend()
    {
        Vector3 curPosition = transform.localPosition;
        Vector3 targetPosition = initialPosition;
        isMoving = true;
        
        while (Vector3.Distance(curPosition, targetPosition) > 0.01f)
        {
            if (onPlatform)
            {
                targetPosition = transform.localPosition;
                break;
            }
            
            curPosition = Vector3.MoveTowards(curPosition, targetPosition, ascendSpeed * Time.deltaTime);
            transform.localPosition = curPosition;
            yield return null;
        }

        transform.localPosition = targetPosition;
        if (targetPosition == initialPosition) activate = false;
        isMoving = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onPlatform = true;
            activate = true;
            timer = 0;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onPlatform = false;
            if (transform.position == initialPosition) activate = false;
            _renderer.material.color = initialColor;
            timer = 0;
        }
    }
}
