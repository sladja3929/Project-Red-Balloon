using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : Gimmick
{
   private Rigidbody _rigid;

   [SerializeField] private float explosionPower;
   [SerializeField] private float explosionTime;

   private void Awake()
   {
      _rigid = GetComponent<Rigidbody>();
   }

   private void OnTriggerEnter(Collider col)
   {
      if (!isGimmickEnable) return;
      
      if (col.gameObject.CompareTag("Player") && !_isExploding)
      {
         StartCoroutine(Explode(col.gameObject));
      }
   }

   private void OnCollisionEnter(Collision col)
   {
      if (!isGimmickEnable) return;
      if (!col.gameObject.CompareTag("Player") || _isExploding) return;
      
      //플레이어가 닿아있을때만 활성화하는 기믹이므로 Execute함수를 오버라이딩 하지 않음.
      StartCoroutine(Explode(col.gameObject));
   }

   private bool _isExploding;
   private IEnumerator Explode(GameObject player)
   {
      _isExploding = true;
      
      Rigidbody playerRigid = player.GetComponent<Rigidbody>();
      
      Vector3 playerPoint = playerRigid.position;
      Vector3 objPoint = _rigid.position;

      Vector3 direction = (playerPoint - objPoint).normalized;

      float time = 0;
      
      while (true)
      {
         float dt = 0.01f;
         time += dt;
         if (time  > explosionTime) break;
            
         playerRigid.AddForce(direction * explosionPower);
         yield return new WaitForSeconds(dt);
      }
      
      _isExploding = false;
   }
}
