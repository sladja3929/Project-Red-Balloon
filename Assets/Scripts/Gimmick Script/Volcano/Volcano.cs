using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : Gimmick
{
    private const float FALLING_TIME = 3.2f;
    [SerializeField] private GameObject stonePrefab;

    [SerializeField] private float degree;
    [SerializeField] private float fallingSpeed;
    
    public override void Execute()
    {
        SpawnSingleStone();
    }

    [ContextMenu("돌떨어져유")]
    private void SpawnSingleStone()
    {
        // balloon 좌표 가져오기
        var balloonPos = GameManager.instance.GetBalloonPosition();
        var volcanoPosition = transform.position;

        // balloon과 화산의 좌표 연산해서 날아오는 방향 계산하기
        Vector3 direction = balloonPos - volcanoPosition;
        direction.y = 0;
        direction.Normalize();
        
        // 해당 방향에서 <변경 가능한 변수> 각도만큼 기울어진체로 방향 생성
        var 법선벡터 = Vector3.Cross(Vector3.up, direction).normalized;
        var rotation = Quaternion.AngleAxis(degree, 법선벡터);
        direction = rotation * direction;


        var length = fallingSpeed * FALLING_TIME;
        var stoneSpawnPoint = balloonPos - direction * length;

        var stone = Instantiate(stonePrefab, stoneSpawnPoint, Quaternion.identity).GetComponent<VolcanicStone>();
        stone.Fall(direction, fallingSpeed);
    }
}
