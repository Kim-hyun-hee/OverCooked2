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
    public GameObject controlUI;

    public OrderManager orderManager;

    private void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        //UIManager.Instance.CloseUI(gameObject);
        //UIManager.Instance.paused = false;
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

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
        stopUI.SetActive(true);
        //UIManager.Instance.panels.Add(stopUI);
    }

    public void PopUpControl()
    {
        controlUI.SetActive(true);
    }
}
