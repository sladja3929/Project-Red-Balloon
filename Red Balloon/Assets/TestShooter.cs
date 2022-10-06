using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    public Vector3 direction;

    public float power;

    public float startWaitTime;

    private BalloonShoot _balloonShoot;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Shoot), startWaitTime);
        _balloonShoot = GetComponent<BalloonShoot>();
    }

    private void Shoot()
    {
        _balloonShoot.SetMoveDirection(direction);
        _balloonShoot.StartMove(power);
    }
}
