using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuBtn : MonoBehaviour, IPointerEnterHandler
{
    public Button btn;
    public Sprite sprite;
    public Sprite defaultSprite;

    public GameObject bg;
    public GameObject[] otherBg = new GameObject[4];

    public bool isDown = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.image.sprite = sprite;
        bg.SetActive(true);
        isDown = true;

        for(int i = 0; i < otherBg.Length; i++)
        {
            if(otherBg[i].transform.parent.GetChild(1).GetComponent<MainMenuBtn>().isDown)
            {
                otherBg[i].transform.parent.GetChild(1).GetComponent<MainMenuBtn>().isDown = false;
                otherBg[i].transform.parent.GetChild(1).GetComponent<MainMenuBtn>().btn.image.sprite = defaultSprite;
                otherBg[i].SetActive(false);
                break;
            }
        }
    }
}
