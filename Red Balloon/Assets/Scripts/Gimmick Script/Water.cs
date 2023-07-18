using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Water : Gimmick
{
    [SerializeField] private Vector3 streamVector;
    [SerializeField] private float streamPower;
    [SerializeField] private float floatingPower;
    private float _sizeOfBalloon = 2.25f;

    [SerializeField] private AudioClip fallSound;

    /// <summary>
    /// 물에 잠긴 비율을 계산하여 그에 따른 부력을 가하는 함수입니다.
    /// 계산의 편의성을 위해 풍선은 타원이 아닌 길이 2.25의 정사각형으로 잡고 계산합니다. (풍선 단축 길이 2 장축길이 2.5)
    /// </summary>
    /// <param name="balloon">풍선 게임오브젝트</param>
    private void floatOnWater(GameObject balloon)
    {
        if (!balloon.CompareTag("Player")) return;

        float waterSurface = transform.position.y + transform.localScale.y / 2; //물 표면의 y좌표
        float submergedRate = (waterSurface - (balloon.transform.position.y - _sizeOfBalloon / 2))
                              / _sizeOfBalloon; //물에 잠긴 비율
        if (submergedRate > 1f) submergedRate = 1f; //최대값은 1(100%)이므로 초과하면 1로 고정
        if (submergedRate <= 0f) return; //음수라면 잠기지 않은것이므로 return

        float floatingForce = floatingPower * submergedRate;

        balloon.GetComponent<Rigidbody>().AddForce(Time.deltaTime * Vector3.up * floatingForce);
    }

    /// <summary>
    /// 물의 유속과 풍선의 현재 속도를 비교하여, 차이 값을 계산 후 그에 비례하게 힘을 가하는 함수 입니다.
    /// </summary>
    /// <param name="balloon">풍선 오브젝트</param>
    private void DriftOverWater(GameObject balloon)
    {
        if (!balloon.CompareTag("Player")) return;
        Rigidbody playerRigid = balloon.GetComponent<Rigidbody>();

        Vector3 balloonVelocity = playerRigid.velocity;

        streamVector = streamVector.normalized * streamPower * 0.1f;
        Vector3 pushVector = streamVector.normalized *
                             (streamVector.magnitude -
                              Vector3.Dot(balloonVelocity, streamVector) / streamVector.magnitude);
        pushVector.y = 0;

        playerRigid.AddForce(Time.deltaTime * pushVector);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGimmickEnable) return;
        if (other.CompareTag("Player"))
        {
            floatOnWater(other.gameObject);
            DriftOverWater(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.SfxPlay("water fall sound", fallSound, other.transform.position);
        }
    }
}
