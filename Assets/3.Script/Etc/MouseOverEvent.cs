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

    public bool isChangeTextColor;
    public Text text;
    public Color defaultColor;
    public Color color;

    private void Awake()
    {
        btn = transform.GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.image.sprite = sprite;
        if(isChangeTextColor)
        {
            text.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.image.sprite = defaultSprite;
        if (isChangeTextColor)
        {
            text.color = defaultColor;
        }
    }

    public void ImageChange()
    {
        btn.image.sprite = defaultSprite;
        btn.image.sprite = defaultSprite;
        if (isChangeTextColor)
        {
            text.color = defaultColor;
        }
    }
}
