using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public float scaleMultiplier = 1.1f;
    
    private Image _image;
    private float _originalScale;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalScale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // _image.sprite = hoverSprite;
        transform.localScale = new Vector3(_originalScale * scaleMultiplier, _originalScale * scaleMultiplier, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _image.sprite = hoverSprite;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // _image.sprite = defaultSprite;
        transform.localScale = new Vector3(_originalScale, _originalScale, 1);
    }
}
