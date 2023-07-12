using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropEvent : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{
    [SerializeField] private GameObject bg;
    [SerializeField] private List<GameObject> otherBg = new List<GameObject>();
    public List<Transform> defaultTransforms = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < otherBg.Count; i++)
        {
            defaultTransforms.Add(otherBg[i].transform);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DropDown(bg, -60, 0.5f);
        for (int i = 0; i < otherBg.Count; i++)
        {
            UIManager.Instance.MoveDefaultPos(otherBg[i], defaultTransforms[i], 0.01f);
            otherBg[i].SetActive(false);
        }
    }
}
