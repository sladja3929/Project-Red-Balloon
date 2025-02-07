using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotate : MonoBehaviour
{
    private GameObject target;
    private Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        rot.x = 0;
        rot.z = 90;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        dir.Normalize();
        
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Euler(0f, -angle, 90f);
    }
}
