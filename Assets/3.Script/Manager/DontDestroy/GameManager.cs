using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum StageName
{
    S1_1,
    S1_2,
    S1_3
};

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    public StageManager stageManager;

    // StartSceneManager���� ���
    [Header("�ٸ� Scene�鿡�� ���")]
    public GameObject transitionOut;
    public GameObject transitionIn;
    public GameObject blackBackGround;

    // MapScnen���� ��� (�� ���� ��)
    [Header("MapScnen���� ��� (�� ���� ��)")]
    public int summaryStar;

    [Header("StartScene���� ���")]
    public bool isOpenShutter = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        UIManager.Instance.Init();
        StartCoroutine(LoadScene_co(sceneName));
    }

    private IEnumerator LoadScene_co(string sceneName)
    {
        transitionIn.SetActive(false);
        SoundManager.Instance.PlaySE("UI_Screen_Out");
        transitionOut.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        blackBackGround.SetActive(true);
        yield return new WaitForSecondsRealtime(0.156f);
        LoadingScene.LoadScene(sceneName);
    }

    public void TransitionIn(bool isStage)
    {
        transitionOut.SetActive(false);
        StartCoroutine(TransitionIn_co(isStage));
    }

    public IEnumerator TransitionIn_co(bool isStage)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        blackBackGround.SetActive(false);
        SoundManager.Instance.PlaySE("UI_Screen_In");
        transitionIn.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        transitionIn.SetActive(false);
        if(isStage)
        {
            stageManager = FindObjectOfType<StageManager>();
            stageManager.StartGame();
        }
    }

}
