using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffect : Gimmick
{
    private ParticleSystem _particle;
    private void Awake()
    {
        _particle = transform.GetComponent<ParticleSystem>();
    }

    public override void GimmickOn()
    {
        if(true) _particle.Play();
    }
    
    public override void GimmickOff()
    {
        if(_particle.isPlaying) _particle.Stop();
    }
}
