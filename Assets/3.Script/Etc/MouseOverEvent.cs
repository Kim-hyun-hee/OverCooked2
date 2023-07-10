using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button btn;
    public Sprite sprite;
    public Sprite defaultSprite;

    private void Awake()
    {
        btn = transform.GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.image.sprite = sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.image.sprite = defaultSprite;
    }

    public void ImageChange()
    {
        btn.image.sprite = defaultSprite;
    }
}
