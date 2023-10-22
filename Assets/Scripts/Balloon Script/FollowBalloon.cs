using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowBalloon : MonoBehaviour
{
    [SerializeField] private Transform balloon;

    public Vector3 pivot;
    // Update is called once per frame
    private void Update()
    {
        transform.position = balloon.position + pivot;
    }
}
