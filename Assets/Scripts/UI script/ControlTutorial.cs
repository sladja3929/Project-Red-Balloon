using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlTutorial : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private float _flickFrequency = 2f;
    [SerializeField] private SpriteRenderer[] sprites;
    
    private bool _rightClickTrigger = false;
    private bool _leftClickTrigger = false;
    private bool _spaceTrigger = false;
    private bool _cancelTrigger = false;
    
    private void Start()
    {
        foreach (var sprite in sprites)
        {
            Color color = sprite.color;
            color.a = 0;
            sprite.color = color;       
        }
        
        StartCoroutine(ShowTutorial());
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

        if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(0))
        {
            _cancelTrigger = true;
        }
    }

    private void ResetTrigger()
    {
        _rightClickTrigger = false;
        _leftClickTrigger = false;
        _spaceTrigger = false;
        _cancelTrigger = false;
    }

    private IEnumerator ShowTutorial()
    {
        yield return StartCoroutine(FadeImage(0, 0, 1, 0.5f));
        
        while(_rightClickTrigger == false)
        {
            // 0번과 1번 index를 Flick
            ChangeSprite(0, 1);
            yield return new WaitForSeconds(_flickFrequency);
            ChangeSprite(0, 0);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(0, 1, 0, 0.5f));
        
        // 2초간 2번 index를 보여준 후, 0번 인덱스로 Fade
        ChangeSprite(0, 2);
        yield return StartCoroutine(FadeImage(0, 0, 1, 0.5f));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeImage(0, 1, 0, 0.5f));
        
        // 3 : left click
        ChangeSprite(0, 3);
        yield return StartCoroutine(FadeImage(0, 0, 1, 0.5f));
        while(_leftClickTrigger == false)
        {
            ChangeSprite(0, 0);
            yield return new WaitForSeconds(_flickFrequency);
            ChangeSprite(0, 3);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(0, 1, 0, 0.5f));
        
        // 2초간 4번 index를 보여준 후, 0번 인덱스로 Fade
        ChangeSprite(0, 4);
        yield return StartCoroutine(FadeImage(0, 0, 1, 0.5f));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeImage(0, 1, 0, 0.5f));
        
        // 4 : space
        ChangeSprite(0, 5);
        yield return StartCoroutine(FadeImage(0, 0, 1, 0.5f));
        while(_spaceTrigger == false)
        {
            ChangeSprite(0, 6);
            yield return new WaitForSeconds(_flickFrequency);
            ChangeSprite(0, 5);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(0, 1, 0, 0.5f));
        
        //5 : cancel
        yield return StartCoroutine(FadeImage(1, 0, 1, 0.5f));
        yield return StartCoroutine(FadeImage(2, 0, 1, 0.5f));
        yield return StartCoroutine(FadeImage(3, 0, 1, 0.5f));
        
        while(_cancelTrigger == false)
        {
            ChangeSprite(3, 0);
            yield return new WaitForSeconds(_flickFrequency);
            ChangeSprite(3, 3);
            yield return new WaitForSeconds(_flickFrequency);
        }
        ResetTrigger();
        yield return StartCoroutine(FadeImage(1, 1, 0, 0.5f));
        yield return StartCoroutine(FadeImage(2, 1, 0, 0.5f));
        yield return StartCoroutine(FadeImage(3, 1, 0, 0.5f));
        
        // fin. image끄기
        Destroy(gameObject);
    }
    
    private IEnumerator FadeImage(int sprite, int from, int to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            sprites[sprite].color = new Color(1, 1, 1, Mathf.Lerp(from, to, percent));
            yield return null;
        }
    }


    public void ChangeSprite(int sprite, int image)
    {
        sprites[sprite].sprite = images[image];
    }
}
