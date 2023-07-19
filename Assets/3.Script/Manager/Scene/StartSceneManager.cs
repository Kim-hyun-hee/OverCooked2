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

    [SerializeField] private Transform cameraStartTransform;
    [SerializeField] private Transform cameraEndTransform;

    [SerializeField] private Text playerId;

    private void Start()
    {
        Time.timeScale = 1;
        shutter.GetComponent<Animator>().TryGetComponent(out shutterOpen);
        SoundManager.Instance.PlayBGM("Frontend");
        SoundManager.Instance.SetBGMVolume(1);

        if (!GameManager.Instance.isOpenShutter)
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

        playerId.text = DBManager.Instance.playerInfo.id + "님 환영합니다";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !GameManager.Instance.isOpenShutter)
        {
            CameraMoving();// 카메라 무빙
            shutterOpen.SetTrigger("open"); // 셔터 열리는 애니메이션
            SoundManager.Instance.PlaySE("Food_Truck_Shutter");
            SoundManager.Instance.PlaySE("UI_PressStart");
            menubar.SetActive(true); // UI 등장
            continueTxt.gameObject.SetActive(false);
            GameManager.Instance.isOpenShutter = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isOpenShutter)
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
