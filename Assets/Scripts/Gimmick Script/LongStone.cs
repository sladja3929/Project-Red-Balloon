using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongStone : Gimmick
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDelay;
    [SerializeField] private float reloadDelay;
    [Range(1 ,3)]
    [SerializeField] private float lengthBetDirPos; //롱스톤이 튀어나오는 정도, 작을수록 많이나옴

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
        
        Vector3 dir = transform.InverseTransformPoint(GameManager.instance.GetBalloonPosition());
        dir.x = 0;
        dir = transform.TransformPoint(dir);
        transform.LookAt(dir);
        
        while (Vector3.Distance(transform.position, dir) > lengthBetDirPos)
        {
            transform.position = (transform.position + transform.forward * attackSpeed * Time.deltaTime);
            yield return null;
        }
        
        _particle.Stop();
        yield return new WaitForSeconds(reloadDelay);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        while ((transform.position - initPos).z > 0)
        {
            transform.position = (transform.position - transform.forward * attackSpeed / 10 * Time.deltaTime);
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