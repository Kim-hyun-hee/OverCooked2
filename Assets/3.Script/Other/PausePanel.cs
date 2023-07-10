using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject btn_stop;
    public GameObject stopUI;
    public GameObject controlUI;

    //private void Start()
    //{
    //    EventSystem.current.firstSelectedGameObject = first;
    //}

    void OnClick(InputValue value)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject; // ���õ� ��ư ��������
        current.GetComponent<Button>().onClick.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        transform.gameObject.SetActive(false);
        //paused = false;
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
        // ��ǲ �Ŵ��� �����̴� �� �ٲ��ְ� ������,,
    }

    public void PopUpControl()
    {
        controlUI.SetActive(true);
    }
}
