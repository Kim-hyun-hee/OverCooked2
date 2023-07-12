using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropEvent : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{
    [SerializeField] private GameObject bg;
    [SerializeField] private List<GameObject> otherBg = new List<GameObject>();
    public Transform defaultTransform;

    private void Start()
    {
        defaultTransform = this.transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DropDown(bg, -60, 0.5f);
        for (int i = 0; i < otherBg.Count; i++)
        {
            UIManager.Instance.MoveDefaultPos(otherBg[i], defaultTransform, 0.01f);
            otherBg[i].SetActive(false);
        }
    }
}
