using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{   
    [SerializeField] private GameObject bullet;
    [SerializeField] float rayDistance = 15f;
    [SerializeField] private float attackDelay;

    
    private bool _canAttack;

    private IEnumerator AttackCooldown(float cooldown)
    {
        _canAttack = false;
        yield return new WaitForSeconds(cooldown);
        _canAttack = true;
    }

    private void Shoot()
    {
        var transform1 = transform;
        Debug.DrawRay(transform1.position, transform1.forward * rayDistance, Color.red);


        if (!Physics.Raycast(transform.position, transform.forward, out var hit, rayDistance)) return;
        if (!hit.collider.CompareTag("Player")) return;

        Instantiate(bullet, transform1.position, transform1.rotation);
        StartCoroutine(AttackCooldown(attackDelay));
    }

    void Update()
    {
        if (_canAttack) Shoot();
    }
}
