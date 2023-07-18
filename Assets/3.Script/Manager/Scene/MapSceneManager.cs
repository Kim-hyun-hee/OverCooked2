using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    private static MapSceneManager instance;
    public static MapSceneManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public delegate void OnOpenStage();
    public static event OnOpenStage OpenStage;

    void Start()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBGM("MapScreen");
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(false);
        OpenStage?.Invoke();
    }
}
