using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFly : Gimmick
{
    public AnimationCurve moveCurve;
    public float flyTime;
    public float flyLength;
    public float flyHeight;

    public float coolDown;


    private Vector3 _startPosition;
    private Vector3 _forward;
    private void Awake()
    {
        _startPosition = transform.position;
        _forward = transform.forward;
    }

    private void Start()
    {
        Execute();
    }

    [ContextMenu("Execute")]    
    public override void Execute()
    {
        if (isGimmickEnable is false) return;

        StartCoroutine(FlyCoroutine());
    }

    private IEnumerator FlyCoroutine()
    {
        while (true)
        {
            float t = 0;
            while ((t += Time.deltaTime) < flyTime)
            {
                float percentage = t / flyTime;

                float forwardLength = Mathf.Lerp(0, flyLength, percentage);

                Vector3 pos = _startPosition + _forward * forwardLength;
                pos.y += moveCurve.Evaluate(percentage) * flyHeight;

                transform.position = pos;

                yield return null;
            }

            transform.position = _startPosition;

            yield return new WaitForSeconds(coolDown);
        }
    }
}
