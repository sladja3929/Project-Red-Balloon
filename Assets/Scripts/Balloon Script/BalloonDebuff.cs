using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDebuff : MonoBehaviour
{
    private Vector3 _originalScale;
    private float _gauge;
    
    [SerializeField]
    private float maxSize;

    void Awake()
    {
        _originalScale = transform.localScale;
    }

    void Start()
    {
        _gauge = 0;
    }

    public void Heat(float heatGauge)
    {
        _gauge += heatGauge;
        _gauge = Mathf.Clamp01(_gauge);

        transform.localScale = _originalScale * (1 + maxSize * _gauge);

        if (_gauge > 1)
        {
            GameManager.instance.KillBalloon();
            _gauge = 0;
        }
    }
}
