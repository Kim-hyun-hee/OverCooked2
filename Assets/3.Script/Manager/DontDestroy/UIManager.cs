using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    private Stack<GameObject> UIStack = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init() // 씬 전환될 때 사용
    {
        UIStack = new Stack<GameObject>();
    }

    public int GetUIStackCount()
    {
        return UIStack.Count;
    }

    public GameObject PopUI()
    {
        if(UIStack.Count > 0)
        {
            GameObject ui = UIStack.Pop();
            return ui;
        }
        return null;
    }

    public void PushUI(GameObject ui)
    {
        UIStack.Push(ui);
        ui.SetActive(true);
    }

    public void CloseAllUI()
    {
        while(UIStack.Count > 0)
        {
            GameObject ui = UIStack.Pop();
            ui.SetActive(false);
        }
    }

    public void CloseUI()
    {
        GameObject ui = UIStack.Pop();
        ui.SetActive(false);
    }
}

