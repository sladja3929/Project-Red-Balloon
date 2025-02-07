using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTutorial : MonoBehaviour
{
    [SerializeField] private Sprite[] _tutorialImages;
    [SerializeField] private float _flickFrequency = 2f;
    
    private int _index;
    private SpriteRenderer _spriteRenderer;
    
    private bool _rightClickTrigger = false;
    private bool _leftClickTrigger = false;
    private bool _spaceTrigger = false;
    
    private void Start()
    {
        _spriteRenderer.sprite = _tutorialImages[_index];
        StartCoroutine(ShowTutorial());
    }

    private void Awake()
    {
        _index = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // set trigger
        if (Input.GetMouseButtonDown(1))
        {
            _rightClickTrigger = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _leftClickTrigger = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spaceTrigger = true;
        }
    }

    private void ResetTrigger()
    {
        _rightClickTrigger = false;
        _leftClickTrigger = false;
        _spaceTrigger = false;
    }

    private IEnumerator ShowTutorial()
    {
        while(_rightClickTrigger == false)
        {
            // 0번과 1번 index를 Flick
            SetIndex(0);
            yield return new WaitForSeconds(_flickFrequency);
            SetIndex(1);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(1, 0, 0.5f));
        
        // 2초간 2번 index를 보여준 후, 0번 인덱스로 Fade
        SetIndex(2);
        yield return StartCoroutine(FadeImage(0, 1, 0.5f));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeImage(1, 0, 0.5f));
        
        // 3 : left click
        SetIndex(3);
        yield return StartCoroutine(FadeImage(0, 1, 0.5f));
        while(_leftClickTrigger == false)
        {
            SetIndex(0);
            yield return new WaitForSeconds(_flickFrequency);
            SetIndex(3);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(1, 0, 0.5f));
        
        // 2초간 4번 index를 보여준 후, 0번 인덱스로 Fade
        SetIndex(4);
        yield return StartCoroutine(FadeImage(0, 1, 0.5f));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeImage(1, 0, 0.5f));
        
        // 4 : space
        SetIndex(5);
        yield return StartCoroutine(FadeImage(0, 1, 0.5f));
        while(_spaceTrigger == false)
        {
            SetIndex(6);
            yield return new WaitForSeconds(_flickFrequency);
            SetIndex(5);
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
