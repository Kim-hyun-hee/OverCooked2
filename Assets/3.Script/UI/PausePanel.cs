using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject btn_stop;
    public GameObject stopUI;
    public GameObject restartUI;
    public GameObject controlUI;

    public OrderManager orderManager;

    private void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }

    public void Resume()
    {
        GameObject ui = UIManager.Instance.PopUI();
        ui.SetActive(false);
    }

    //public void ReStart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    Time.timeScale = 1;
    //}

    public void Control()
    {
        PopUpControl();
    }

    public void Stop()
    {
        PopUpStop();
    }

    public void PopUpStop()
    {
        UIManager.Instance.PushUI(stopUI);
    }

    public void PopUpControl()
    {
        UIManager.Instance.PushUI(controlUI);
    }

    public void PopUpRestart()
    {
        UIManager.Instance.PushUI(restartUI);
    }

    public void CancelBtn()
    {
        UIManager.Instance.CloseUI();
    }
}
