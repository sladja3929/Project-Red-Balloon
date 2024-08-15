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
        Vector3 dir = target.transform.position - transform.position;
        rot.y = Quaternion.LookRotation(dir.normalized).eulerAngles.y + 90;
        if (100 < rot.y && rot.y < 180) rot.y = 100;
        else if (180 < rot.y && rot.y < 270) rot.y = 270;
        transform.rotation = Quaternion.Euler(rot);
    }
}
