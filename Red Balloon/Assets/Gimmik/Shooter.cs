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

    private float time = 0f;
    public float attackDelay;

    void Update()
    {
        time += Time.deltaTime;
        
        if (time > attackDelay)
        {
            time = 0;
            Shoot();
        }
    }
}
