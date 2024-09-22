using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : Gimmick
{
    private const float FALLING_TIME = 3.2f;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private ParticleSystem gasEffect;
    
    [Space(10)]
    [SerializeField] private float degree;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float fallingDelay;
    
    [SerializeField] private float spawnInterval;
    
    private bool isSpawning = false;
    
    private float t;

    private void Start()
    {
        t = 0;
    }
    
    private void Update()
    {
        if (!isGimmickEnable) return;
        if (isSpawning) return;
        
        t += Time.deltaTime;
        if (t >= spawnInterval && isGimmickEnable)
        {
            isSpawning = true;
            
            ShakeCamera();
            GasEffect();
            Invoke(nameof(SpawnSingleStone), fallingDelay);
        }
    }

    private void ShakeCamera()
    {
        var virtualCamera = GameObject.Find("CM vcam1");
        if (virtualCamera != null)
        {
            StartCoroutine(ShakeCamera(virtualCamera.transform, 0.5f, 1.0f)); // Adjust the amount and time as needed
        }
    }
    
    private void GasEffect()
    {
        gasEffect.Play();
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
        
        isSpawning = false;
        t = 0;
    }

    private IEnumerator ShakeCamera(Transform cameraTransform, float amount, float time)
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < time)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;

            cameraTransform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}