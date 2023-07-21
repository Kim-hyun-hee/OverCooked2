using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevel : MonoBehaviour
{
    public StageName stageName;
    private bool isClear;
    private bool isOpen;
    public Transform canvas;

    public GameObject defaultPanel;
    public GameObject stagePanel;
    public string sceneName;

    public GameObject ui;
    public GameObject ui_d;

    private void Start()
    {
        ui = Instantiate(stagePanel, canvas);
        ui.SetActive(false);

        ui_d = Instantiate(defaultPanel, canvas);
        ui_d.SetActive(false);

        isClear = DBManager.Instance.playerInfo.stageInfos[(int)stageName].isClear;
        isOpen = DBManager.Instance.playerInfo.stageInfos[(int)stageName].isOpen;
    }

    private GameObject SetStageLevelUI(GameObject ui)
    {
        StartCoroutine(SetPosition_co(ui));
        return ui;
    }

    private IEnumerator SetPosition_co(GameObject ui)
    {
        while(true)
        {
            ui.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 5f, Camera.main.WorldToScreenPoint(transform.position).y + 300f, Camera.main.WorldToScreenPoint(transform.position).z);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isClear || (!isClear && isOpen)) // Ŭ���� / Ŭ������ߴµ� ������
            {
                ui.SetActive(true);
                ui = SetStageLevelUI(ui);
            }
            else if(!isClear) // Ŭ���� ����
            {
                if(stageName == 0) // ù��° ���������� ui_d
                {
                    ui_d.SetActive(true);
                    ui_d = SetStageLevelUI(ui_d);
                }
                else // ���� �������� Ŭ���� ������ ui_d
                {
                    if(DBManager.Instance.playerInfo.stageInfos[(int)stageName - 1].isClear)
                    {
                        ui_d.SetActive(true);
                        ui_d = SetStageLevelUI(ui_d);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isClear && !isOpen)
        {
            if (ui_d.activeInHierarchy)
            {
                ui_d.SetActive(false);
                SoundManager.Instance.PlaySE("WorldMapRoad");
                ui.SetActive(true);
                ui = SetStageLevelUI(ui);
                isOpen = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isClear && isOpen)
        {
            if(ui.activeInHierarchy)
            {
                DBManager.Instance.playerInfo.stageInfos[(int)stageName].isOpen = isOpen;
                DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
                GameManager.Instance.LoadScene(sceneName);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isClear)
        {
            if (ui.activeInHierarchy)
            {
                DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
                GameManager.Instance.LoadScene(sceneName);
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (Input.GetKeyDown(KeyCode.Space) && !isClear && !isOpen)
    //    {
    //        if(ui_d.activeInHierarchy)
    //        {
    //            ui_d.SetActive(false);
    //            ui.SetActive(true);
    //            ui = SetStageLevelUI(ui);
    //            isOpen = true;
    //        }
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Space) && !isClear && isOpen)
    //    {
    //        DBManager.Instance.playerInfo.stageInfos[(int)stageName].isOpen = isOpen;
    //        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    //        GameManager.Instance.LoadScene(sceneName);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.Space) && isClear)
    //    {
    //        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    //        GameManager.Instance.LoadScene(sceneName);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ui.SetActive(false);
            ui_d.SetActive(false);
            StopAllCoroutines();
        }
    }
}
