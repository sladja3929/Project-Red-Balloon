using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Vector3 driftDirection;


    public float floatingPower;
    private float _sizeOfBalloon = 2.25f;
    /*
     * 물에 잠긴 비율을 계산하여 그에 따른 부력을 가하는 함수입니다.
     * 계산의 편의성을 위해 풍선은 타원이 아닌 길이 2.25의 정사각형으로 잡고 계산합니다. (풍선 단축 길이 2 장축길이 2.5)
     */
    private void floatOnWater(GameObject player)
    {
        if (!player.CompareTag("Player")) return;

        float waterSurface = transform.position.y + transform.localScale.y / 2; //물 표면의 y좌표
        float submergedRate = (waterSurface - (player.transform.position.y - _sizeOfBalloon / 2))
                              /_sizeOfBalloon; //물에 잠긴 비율
        if (submergedRate > 1f) submergedRate = 1f; //최대값은 1(100%)이므로 초과하면 1로 고정
        if (submergedRate <= 0f) return; //음수라면 잠기지 않은것이므로 return


        float floatingForce = floatingPower * submergedRate;
        
        player.GetComponent<Rigidbody>().AddForce(Time.deltaTime * Vector3.up * floatingForce);
    }

    private void DriftOverWater(GameObject player)
    {
        if (!player.CompareTag("Player")) return;
        
        player.GetComponent<Rigidbody>().AddForce(Time.deltaTime * driftDirection);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatOnWater(other.gameObject);
            DriftOverWater(other.gameObject);
        }
    }
}
