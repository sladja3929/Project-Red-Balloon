using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
   private Rigidbody _rigid;

   [SerializeField] private float explosionPower;
   [SerializeField] private float explosionTime;

   void Awake()
   {
      _rigid = GetComponent<Rigidbody>();
   }
   
   void OnTriggerEnter(Collider col)
   {
      if (col.gameObject.CompareTag("Player") && !_isExploding)
      {
         StartCoroutine(Explode(col.gameObject));
      }
   }

   void OnCollisionEnter(Collision col)
   {
      if (col.gameObject.CompareTag("Player") && !_isExploding)
      {
         StartCoroutine(Explode(col.gameObject));
      }
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
