using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowBalloon : MonoBehaviour
{
    [SerializeField] private Transform balloon;
    // Update is called once per frame
    private void Update()
    {
        transform.position = balloon.position;
    }
}
