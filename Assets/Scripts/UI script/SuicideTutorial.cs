using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideTutorial : MonoBehaviour
{
    [SerializeField] private Sprite[] _tutorialImages;
    [SerializeField] private float _flickFrequency = 2f;
    
    private int _index;
    private SpriteRenderer _spriteRenderer;
    
    private bool trigger = false;
    
    private void Awake()
    {
        _index = 0;
        if(TryGetComponent(out _spriteRenderer))
        {
            _spriteRenderer.sprite = _tutorialImages[_index];
        }

        _spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            trigger = true;
        }
    }

    private void ResetTrigger()
    {
        trigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spriteRenderer.enabled = true;
            StartCoroutine(ShowTutorial());
        }
    }

    private IEnumerator ShowTutorial()
    {
        while(trigger == false)
        {
            SetIndex(0);
            yield return new WaitForSeconds(_flickFrequency);
            SetIndex(1);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(1, 0, 0.5f));
        
        // fin. image끄기
        gameObject.SetActive(false);
    }
    
    private IEnumerator FadeImage(int from, int to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            _spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(from, to, percent));
            yield return null;
        }
    }


    public void SetIndex(int index)
    {
        _index = index;
        _spriteRenderer.sprite = _tutorialImages[_index];
    }
}
