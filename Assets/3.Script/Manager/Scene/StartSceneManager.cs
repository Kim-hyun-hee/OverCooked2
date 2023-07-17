using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Text continueTxt;
    [SerializeField] private GameObject menubar;
    [SerializeField] private GameObject stopPanel;

    [SerializeField] private GameObject shutter;
    [SerializeField] private Animator shutterOpen;
    private bool isOpenShutter = false;

    [SerializeField] private Transform cameraStartTransform;
    [SerializeField] private Transform cameraEndTransform;

    private void Start()
    {
        Time.timeScale = 1;
        shutter.GetComponent<Animator>().TryGetComponent(out shutterOpen);
        SoundManager.Instance.PlayBGM("Frontend");
        SoundManager.Instance.SetBGMVolume(1);

        if (!isOpenShutter)
        {
            Camera.main.transform.position = new Vector3(cameraStartTransform.position.x, cameraStartTransform.position.y, cameraStartTransform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(0, -4.053f, 0);
            shutter.SetActive(true);
            menubar.SetActive(false);
            continueTxt.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.TransitionIn(false);
            Camera.main.transform.position = new Vector3(cameraEndTransform.position.x, cameraEndTransform.position.y, cameraEndTransform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(0, 4.194f, 0);
            shutter.SetActive(false);
            menubar.SetActive(true);
            continueTxt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isOpenShutter)
        {
            CameraMoving();// ī�޶� ����
            shutterOpen.SetTrigger("open"); // ���� ������ �ִϸ��̼�
            SoundManager.Instance.PlaySE("Food_Truck_Shutter");
            SoundManager.Instance.PlaySE("UI_PressStart");
            menubar.SetActive(true); // UI ����
            continueTxt.gameObject.SetActive(false);
            isOpenShutter = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isOpenShutter)
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
            }
            else
            {
                UIManager.Instance.PushUI(stopPanel);
                SoundManager.Instance.PlaySE("UI_Transition_In");
            }
        }
    }

    private void CameraMoving()
    {
        Camera.main.transform.DOMove(new Vector3(cameraEndTransform.position.x, cameraEndTransform.position.y, cameraEndTransform.position.z), 0.5f);
        Camera.main.transform.DORotate(new Vector3(0, 4.194f, 0), 0.5f);
    }
}
