using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongStone : Gimmick
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float additionalDistance;
    [SerializeField] private float attackDelay;
    [SerializeField] private float reloadDelay;

    private ParticleSystem _particle;
    
    private Vector3 initPos;
    private Quaternion initRot;
    
    private bool canAttack;

    private void Start()
    {
        _particle = transform.parent.GetComponentInChildren<ParticleSystem>();
        canAttack = true;
        initPos = transform.position;
        initRot = transform.rotation;
    }

    private void Update()
    {
        if (!isGimmickEnable) return;
    }
    
    private IEnumerator Cooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        Vector3 targetPos = transform.InverseTransformPoint(GameManager.instance.GetBalloonPosition());
        targetPos.x = 0;
        targetPos = transform.TransformPoint(targetPos);
        transform.LookAt(targetPos);
        targetPos += transform.forward * additionalDistance;
        
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(currentPos, targetPos);
        float duration = distance / moveSpeed;
        float t = 0;
        
        while (t < duration)
        {
            transform.position = Vector3.Lerp(currentPos, targetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        
        _particle.Stop();
        yield return new WaitForSeconds(reloadDelay);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(transform.position, initPos);
        float duration = distance / (moveSpeed / 10);
        float t = 0f;
        
        while (t < duration)
        {
            transform.position = Vector3.Lerp(currentPos, initPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = initPos;
        transform.rotation = initRot;

        yield return new WaitForSeconds(attackDelay);
        _particle.Play();
        canAttack = true;
    }
    
    private void Shoot()
    {
        StartCoroutine(Attack());
    }

    public override void Execute()
    {
        if(canAttack) Shoot();
    }
}