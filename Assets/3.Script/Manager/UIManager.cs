using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public GameObject EndMenu;
    public GameObject PauseMenu;

    public List<GameObject> panels = new List<GameObject>();

    //public bool paused = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (paused)
            //{
            //    Time.timeScale = 1;
            //    PauseMenu.SetActive(false);
            //    //paused = false;
            //}
            //else
            //{
            //    Time.timeScale = 0;
            //    PauseMenu.SetActive(true);
            //    //paused = true;
            //}

            if (panels.Count > 1)
            {
                //panels[panels.Count - 1].SetActive(false);
                //panels.RemoveAt(panels.Count - 1);
            }
            else if (panels.Count == 1)
            {
                Time.timeScale = 1;
                panels[0].SetActive(false);
                panels.RemoveAt(panels.Count - 1);
                //paused = false;
            }
            else
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                panels.Add(PauseMenu);
                //paused = true;
            }
        }
    }

    public void CloseUI(GameObject obj)
    {
        obj.SetActive(false);
        panels.Remove(obj);
    }
}
