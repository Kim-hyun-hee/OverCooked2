using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    private static MapSceneManager instance;
    public static MapSceneManager Instance { get { return instance; } }

    public GameObject pauseMenu;

    public Text totalStar;
    private int total = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public delegate void OnOpenStage();
    public static event OnOpenStage OpenStage;

    void Start()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBGM("MapScreen");
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(false);
        for(int i = 0; i < Enum.GetValues(typeof(StageName)).Length; i++)
        {
            total += DBManager.Instance.playerInfo.stageInfos[i].star;
        }
        totalStar.text = total.ToString();
        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
        //van스크립트 enabled false
        OpenStage?.Invoke(); // 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.GetUIStackCount() > 1)
            {
                GameObject ui = UIManager.Instance.PopUI();
                ui.SetActive(false);
            }
            else if (UIManager.Instance.GetUIStackCount() == 1)
            {
                GameObject ui = UIManager.Instance.PopUI();
                ui.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                UIManager.Instance.PushUI(pauseMenu);
            }
        }

        if (UIManager.Instance.GetUIStackCount() != 0)
        {
            Time.timeScale = 0;
        }
    }

    private void OnDisable()
    {
        OpenStage = null;
    }
}
