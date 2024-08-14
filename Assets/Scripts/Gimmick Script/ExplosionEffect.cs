using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(Rigidbody))]
public class ExplosionEffect : Gimmick
{
   private Rigidbody _rigid;
   //private Animator animator;

   [SerializeField] private float explosionPower;
   [SerializeField] private float explosionTime;
   [SerializeField] private AudioClip fxSound;

   private void Awake()
   {
      _rigid = GetComponent<Rigidbody>();
      //animator = GetComponent<Animator>();
   }

   private void OnTriggerEnter(Collider col)
   {
      if (!isGimmickEnable) return;
      
      if (col.gameObject.CompareTag("Player") && !_isExploding)
      {
         StartCoroutine(Explode(col.gameObject));   
         if (fxSound != null)
         {
            SoundManager.instance.SfxPlay("explodeSound", fxSound, transform.position);
         }         
      }
   }

   private void OnCollisionEnter(Collision col) 
   {
      if (!isGimmickEnable) return;
      if (!col.gameObject.CompareTag("Player") || _isExploding) return;
      
      //플레이어가 닿아있을때만 활성화하는 기믹이므로 Execute함수를 오버라이딩 하지 않음.
      StartCoroutine(Explode(col.gameObject));
      //animator.SetTrigger("Explosion");
      if (fxSound != null)
      {
            SoundManager.instance.SfxPlay("explodeSound", fxSound, transform.position);
      }
   }

   private bool _isExploding;
   private IEnumerator Explode(GameObject player)
   {
      Debug.Log("Start Explode");
      
      _isExploding = true;
      
      Rigidbody playerRigid = player.GetComponent<Rigidbody>();
      
      Vector3 playerPoint = playerRigid.position;
      Vector3 objPoint = _rigid.position;

      Vector3 direction = (playerPoint - objPoint).normalized;

      float time = 0;
      
      while (true)
      {
         const float dt = 0.01f;
         time += dt;
         if (time  > explosionTime) break;
            
         playerRigid.AddForce(direction * explosionPower, ForceMode.Acceleration);
         yield return new WaitForSeconds(dt);
      }
      
      // playerRigid.AddExplosionForce(explosionPower, transform.position, 10f, 0.5f, ForceMode.Impulse);
      
      _isExploding = false;

      yield break;
   }
}
