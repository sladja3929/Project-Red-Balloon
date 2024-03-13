using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal enum FireWallState
{
    On,
    Off,
}

public class FireWall : Gimmick
{
    private const float SOME_THRESHOLD = 0.1f;

    public ParticleSystem particle;

    public float coolDown;
    public float duration;

    [Header("For Debugging")]
    [SerializeField]
    private FireWallState curState;
    
    [SerializeField]
    private float timer;

    private void Awake()
    {
        var tsf = transform;
        
        timer = 0;
        curState = FireWallState.Off;
        particle = GetComponent<ParticleSystem>();
    }
    
    private void Update()
    {
        switch (curState)
        { 
            case FireWallState.On:
            {
                timer += Time.deltaTime;
                if (timer >= duration)
                {
                    curState = FireWallState.Off;
                    particle.Stop();
                    timer = 0;
                }

                break;
            }
            case FireWallState.Off:
            {
                timer += Time.deltaTime;
                if (timer >= coolDown)
                {
                    curState = FireWallState.On;
                    particle.Play();
                    timer = 0;
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    void OnParticleTrigger()
    {
        // ParticleSystem의 트리거 이벤트가 발생했을 때 호출됩니다.
        // 이 함수 내에서는 트리거 이벤트를 처리하는 로직을 작성합니다.

        // 예를 들어, 플레이어가 파티클 시스템의 트리거 영역에 들어왔을 때 게임 매니저의 KillBalloon 함수를 호출하도록 할 수 있습니다.
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            if (Vector3.Distance(p.position, player.transform.position) < SOME_THRESHOLD)
            {
                GameManager.instance.KillBalloon();
                break;
            }
        }
    }
}
