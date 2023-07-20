using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStartEffect : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private Camera main;
    [SerializeField] private GameObject direction;
    //[SerializeField] private GameObject balloon;
    //[SerializeField] private GameObject waterfall;


    private float _distance;
    // Start is called before the first frame update
    private void Awake()
    {
        main.enabled = false;
        direction.SetActive(false);
        StartCoroutine(WaitCoroutine());
    }
    
    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        
        direction.SetActive(true);
        main.enabled = true;        
        GetComponent<Camera>().enabled = false;
    }


    //private void Update()
    //{
    //    distance = Vector3.Distance(balloon.transform.position, waterfall.transform.position);
    //    if(distance < 500f)
    //    {
    //        SoundManager.Instance.SetBackgroundVolume(1);
    //    }
    //}
}
