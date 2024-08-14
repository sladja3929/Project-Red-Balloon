using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : Gimmick
{
    private const float FALLING_TIME = 3.2f;
    [SerializeField] private GameObject stonePrefab;

    [SerializeField] private float degree;
    [SerializeField] private float fallingSpeed;
    
    [SerializeField] private float spawnInterval;


    private float t;
    private void Start()
    {
        t = 0;
    }
    
    private void Update()
    {
        if (!isGimmickEnable) return;
        
        t += Time.deltaTime;
        if (t >= spawnInterval && isGimmickEnable)
        {
            SpawnSingleStone();
            t = 0;
        }
    }

    [ContextMenu("돌떨어져유")]
    private void SpawnSingleStone()
    {
        // balloon 좌표 가져오기
        Vector3 balloonPos = GameManager.instance.GetBalloonPosition();
        Vector3 volcanoPosition = transform.position;

        // balloon과 화산의 좌표 연산해서 날아오는 방향 계산하기
        Vector3 direction = balloonPos - volcanoPosition;
        direction.y = 0;
        direction.Normalize();
        
        // 해당 방향에서 <변경 가능한 변수> 각도만큼 기울어진체로 방향 생성
        Vector3 normalVector = Vector3.Cross(Vector3.up, direction).normalized;
        Quaternion rotation = Quaternion.AngleAxis(degree, normalVector);
        direction = rotation * direction;


        float length = fallingSpeed * FALLING_TIME;
        Vector3 stoneSpawnPoint = balloonPos - direction * length;

        VolcanicStone stone = Instantiate(stonePrefab, stoneSpawnPoint, Quaternion.identity).GetComponent<VolcanicStone>();
        stone.Fall(direction, fallingSpeed);
    }
}
