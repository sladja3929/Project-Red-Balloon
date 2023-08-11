using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal enum FireWallState
{
    Up,
    Top,
    Down,
    Bottom
}

public class FireWall : Gimmick
{
    [SerializeField]
    private float moveTime;

    [SerializeField] private float stayTime;

    [Header("For Debugging")]
    
    [SerializeField]
    private FireWallState curState;
    
    [SerializeField]
    private float timer;
    
    [SerializeField]
    private float totalLength;
    
    [SerializeField]private float _startHeight;

    private void Awake()
    {
        var tsf = transform;
        
        timer = 0;
        curState = FireWallState.Bottom;
        totalLength = tsf.localScale.y;
        _startHeight = tsf.position.y - (totalLength * 0.5f);
    }
    
    private void Update()
    {
        var tsf = transform;
        if (isGimmickEnable is false)
        {
            var s = tsf.localScale;
            s.y = 0;
            tsf.localScale = s;
            
            return;
        }
        
        if ((timer -= Time.deltaTime) <= 0)
        {
            switch (curState)
            { 
                case FireWallState.Bottom:
                    timer = moveTime;
                    curState = FireWallState.Up;
                    break;
                case FireWallState.Up:
                    timer = stayTime;
                    curState = FireWallState.Top;
                    break;
                case FireWallState.Top:
                    timer = moveTime;
                    curState = FireWallState.Down;
                    break;
                case FireWallState.Down:
                    timer = stayTime;
                    curState = FireWallState.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var curLength = curState switch
        {
            FireWallState.Bottom => 0,
            FireWallState.Top => totalLength,
            FireWallState.Down => Mathf.Lerp(0, totalLength, timer / moveTime),
            FireWallState.Up => Mathf.Lerp(totalLength, 0, timer / moveTime),
            _ => throw new ArgumentOutOfRangeException()
        };
        var middlePosition = _startHeight + curLength * 0.5f;

        var pos = tsf.position;
        var scale = tsf.localScale;

        pos.y = middlePosition;
        scale.y = curLength;

        tsf.position = pos;
        tsf.localScale = scale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") is false) return; 
        GameManager.instance.KillBalloon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") is false) return; 
        GameManager.instance.KillBalloon();
    }
}
