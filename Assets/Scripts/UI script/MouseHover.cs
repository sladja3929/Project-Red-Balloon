using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public float scaleMultiplier = 1.1f;
    
    [FormerlySerializedAs("_image")] [SerializeField]
    private Image image;
    
    private float _originalScale;
    
    private void Awake()
    {
        if (image == null)
        {
            Debug.LogError("Image is not assigned");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        
        _originalScale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // _image.sprite = hoverSprite;
        transform.localScale = new Vector3(_originalScale * scaleMultiplier, _originalScale * scaleMultiplier, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (image == null) return;
        
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image == null) return;
        
        image.sprite = defaultSprite;
        image.transform.localScale = new Vector3(_originalScale, _originalScale, 1);
    }
    
    public void OnDisable()
    {
        if (image == null) return;
        
        image.sprite = defaultSprite;
        image.transform.localScale = new Vector3(_originalScale, _originalScale, 1);
    }
    
    public void OnEnable()
    {
        if (image == null) return;
        
        image.transform.localScale = new Vector3(_originalScale, _originalScale, 1);
        image.sprite = defaultSprite;
    }
    
    // image getter
    public Image GetImage()
    {
        return image;
    }
}
