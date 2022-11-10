using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{   
    public GameObject bullet;
    public float rayDistance = 15f;
    public float attackDelay;

    RaycastHit hit;
    private float _time = 0f;

    public void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        _time += Time.deltaTime;

        

        if(Physics.Raycast (transform.position, transform.forward, out hit, rayDistance))
        {
            if (_time > attackDelay)
            {
                _time = 0;
                Shoot();
            }
        }
        
    }

}
