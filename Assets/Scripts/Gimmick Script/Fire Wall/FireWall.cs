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
    public float coolDown;
    public float duration;

    [Header("For Debugging")]
    [SerializeField]
    private FireWallState curState;
    
    [SerializeField]
    private float timer;

    private ParticleSystem particle;
    private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private void Awake()
    {
        var tsf = transform;
        
        timer = 0;
        curState = FireWallState.Off;
        particle = GetComponent<ParticleSystem>();
        particle.trigger.AddCollider(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
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
        int numEnter = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        if(numEnter != 0) GameManager.instance.KillBalloon();
    }
}
