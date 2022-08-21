using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    public Vector3 direction;

    public float power;

    public float startWaitTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Shoot), startWaitTime);
    }

    private void Shoot()
    {
        GetComponent<BallonShoot>().setMoveDirection(direction);
        GetComponent<BallonShoot>().StartMove(power);
    }
}
