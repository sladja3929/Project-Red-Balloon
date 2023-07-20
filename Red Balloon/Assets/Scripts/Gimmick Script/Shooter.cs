using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : Gimmick
{   
    [SerializeField] private GameObject bullet;
    [SerializeField] private float rayDistance = 15f;
    [SerializeField] private float attackDelay;

    [SerializeField]
    private bool canAttack;

    private IEnumerator AttackCooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private void Shoot()
    {
        var shooterTransform = transform;

        Instantiate(bullet, shooterTransform.position, shooterTransform.rotation);
        StartCoroutine(AttackCooldown(attackDelay));
    }

    private void Update()
    {
        if (!isGimmickEnable) return;
        if (canAttack) Shoot();
    }

    public override void Execute()
    {
        Shoot();
    }
}
