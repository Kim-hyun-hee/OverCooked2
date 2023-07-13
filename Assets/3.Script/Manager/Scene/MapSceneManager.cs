using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM("MapScreen");
        GameManager.Instance.transitionIn.SetActive(false);
        GameManager.Instance.transitionOut.SetActive(false);
        GameManager.Instance.blackBackGround.SetActive(false);
        SoundManager.Instance.PlaySE("UI_Screen_In");
        GameManager.Instance.transitionIn.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadScene("StartScene");
        }
    }
}