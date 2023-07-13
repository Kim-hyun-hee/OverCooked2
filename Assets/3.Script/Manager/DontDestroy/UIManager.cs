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
        GameObject ui = UIStack.Pop();
        //if(ui != null)
        //{
        //    ui.SetActive(false);
        //}
        return ui;
    }

    public void PushUI(GameObject ui)
    {
        UIStack.Push(ui);
        ui.SetActive(true);
    }

    //public void DropDown(GameObject ui, float value, float duration)
    //{
    //    ui.SetActive(true);
    //    ui.transform.DOLocalMoveY(value, duration).SetRelative();
    //}

    //public void CancelDropDown(GameObject ui, float value, float duration)
    //{
    //    ui.transform.DOLocalMoveY(value, duration).SetRelative();
    //    ui.SetActive(false);
    //}

    //public void MoveYDefaultLocalPos(GameObject ui, float y, float duration)
    //{
    //    ui.transform.DOLocalMoveY(y, duration);
    //}


    //private static UIManager instance;
    //public static UIManager Instance { get { return instance; } }

    //public GameObject EndMenu;
    //public GameObject PauseMenu;
    //public GameObject Intro;

    //public List<GameObject> panels = new List<GameObject>();

    ////public bool paused = false;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //    }
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        //if (paused)
    //        //{
    //        //    Time.timeScale = 1;
    //        //    PauseMenu.SetActive(false);
    //        //    //paused = false;
    //        //}
    //        //else
    //        //{
    //        //    Time.timeScale = 0;
    //        //    PauseMenu.SetActive(true);
    //        //    //paused = true;
    //        //}

    //        if (panels.Count > 1)
    //        {
    //            //panels[panels.Count - 1].SetActive(false);
    //            //panels.RemoveAt(panels.Count - 1);
    //        }
    //        else if (panels.Count == 1)
    //        {
    //            Time.timeScale = 1;
    //            panels[0].SetActive(false);
    //            panels.RemoveAt(panels.Count - 1);
    //            //paused = false;
    //        }
    //        else
    //        {
    //            Time.timeScale = 0;
    //            PauseMenu.SetActive(true);
    //            panels.Add(PauseMenu);
    //            //paused = true;
    //        }
    //    }
    //}

    //public void CloseUI(GameObject obj)
    //{
    //    obj.SetActive(false);
    //    panels.Remove(obj);
    //}
}

