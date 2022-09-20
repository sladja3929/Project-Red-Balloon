using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;

    public void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    private float _time = 0f;
    public float attackDelay;

    void Update()
    {
        _time += Time.deltaTime;
        
        if (_time > attackDelay)
        {
            _time = 0;
            Shoot();
        }
    }
}
