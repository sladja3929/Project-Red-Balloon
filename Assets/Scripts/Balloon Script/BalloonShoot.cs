using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BalloonShoot : MonoBehaviour
{
    public float moveSpeed;
    public float speedScale;
    public int step = 20;
    
    private Vector3 _moveDirection;

    public Vector3 GetMoveDir()
    {
        return _moveDirection;
    }

    
    public void SetMoveDirection(Vector3 moveDirection)
    {
        _moveDirection = moveDirection.normalized;
    }

    
    //그냥 한번에 AddForce -> 일정 이상 값이 커지면 물리 오류로 공이 하늘 끝으로 날아감
    //그래서 실제로 손으로 풍선을 밀듯, 특정 힘을 {push Time}초에 거쳐 지속적으로 가하는 방법을 선택
    //코루틴을 통해 0.01초마다 반복해서 힘을 가함
    public float pushTime; //Second

    private IEnumerator PushBalloon(float power)
    {
        int count = (int)(pushTime * 100);
        for (int i = 0; i < count; i++)
        {
            _rigidbody.AddForce(_moveDirection * (power * moveSpeed * speedScale));
            yield return new WaitForSeconds(0.01f);
        }
    }

    private float QuantizePower(float power)
    {
        return power = 1f / step >= power ? power : Mathf.Round(power * step) / (float)step;
    }
    
    public bool StartMove(float power)
    {
        SetRandomRotation();
        //Debug.LogError(power + " " + QuantizePower(power));
        StartCoroutine(PushBalloon(QuantizePower(power)));
        return true;
    }

    public float randomRotationSpeed;
    private void SetRandomRotation()
    {
        _rigidbody.angularVelocity = new Vector3
            (Random.value * randomRotationSpeed, 
            Random.value * randomRotationSpeed,
            Random.value * randomRotationSpeed);
    }

    private Rigidbody _rigidbody;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

}
