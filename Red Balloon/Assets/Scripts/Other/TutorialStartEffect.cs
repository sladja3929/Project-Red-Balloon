using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStartEffect : MonoBehaviour
{
    [SerializeField] private float WaitTime;
    [SerializeField] private Camera main;
    [SerializeField] private GameObject direction;
    //[SerializeField] private GameObject balloon;
    //[SerializeField] private GameObject waterfall;


    private float distance;
    // Start is called before the first frame update
    void Awake()
    {
        main.enabled = false;
        direction.SetActive(false);
        StartCoroutine(Wait());
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        direction.SetActive(true);
        main.enabled = true;        
        this.GetComponent<Camera>().enabled = false;
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
