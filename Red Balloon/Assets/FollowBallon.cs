using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBallon : MonoBehaviour
{
    [SerializeField] private Transform ballon;
    // Update is called once per frame
    void Update()
    {
        transform.position = ballon.position;
    }
}
