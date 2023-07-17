using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBGM("MapScreen");
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(false);
    }
}
