using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartSceneManager : MonoBehaviour
{
    private bool isOpenShutter = false;

    [SerializeField] private Text continueTxt;
    [SerializeField] private GameObject menubar;

    [SerializeField] private GameObject shutter;
    [SerializeField] private Animator shutterOpen;

    [SerializeField] private Transform cameraStartTransform;
    [SerializeField] private Transform cameraEndTransform;

    private void Start()
    {
        //GameManager.Instance.startSceneManager = this;
        shutter.GetComponent<Animator>().TryGetComponent(out shutterOpen);
        Camera.main.transform.position = new Vector3(cameraStartTransform.position.x, cameraStartTransform.position.y, cameraStartTransform.position.z);
        Camera.main.transform.rotation = Quaternion.Euler(0, -4.053f, 0);
        shutter.SetActive(true);
        continueTxt.gameObject.SetActive(true);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isOpenShutter)
        {
            CameraMoving();// 카메라 무빙
            shutterOpen.SetTrigger("open"); // 셔터 열리는 애니메이션
            menubar.SetActive(true); // UI 등장
            continueTxt.gameObject.SetActive(false);
            isOpenShutter = true;
        }
    }

    private void CameraMoving()
    {
        Camera.main.transform.DOMove(new Vector3(cameraEndTransform.position.x, cameraEndTransform.position.y, cameraEndTransform.position.z), 0.5f);
        Camera.main.transform.DORotate(new Vector3(0, 4.194f, 0), 0.5f);
    }
}
