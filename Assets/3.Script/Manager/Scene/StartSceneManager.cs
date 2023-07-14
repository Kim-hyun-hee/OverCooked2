using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Text continueTxt;
    [SerializeField] private GameObject menubar;

    [SerializeField] private GameObject shutter;
    [SerializeField] private Animator shutterOpen;

    [SerializeField] private Transform cameraStartTransform;
    [SerializeField] private Transform cameraEndTransform;

    private void Start()
    {
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
            GameManager.Instance.TransitionIn();
            Camera.main.transform.position = new Vector3(cameraEndTransform.position.x, cameraEndTransform.position.y, cameraEndTransform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(0, 4.194f, 0);
            shutter.SetActive(false);
            menubar.SetActive(true);
            continueTxt.gameObject.SetActive(false);
        }
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
    }

    private void CameraMoving()
    {
        Camera.main.transform.DOMove(new Vector3(cameraEndTransform.position.x, cameraEndTransform.position.y, cameraEndTransform.position.z), 0.5f);
        Camera.main.transform.DORotate(new Vector3(0, 4.194f, 0), 0.5f);
    }
}
