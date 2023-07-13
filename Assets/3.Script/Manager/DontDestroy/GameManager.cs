using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    // StartSceneManager에서 사용
    public bool isOpenShutter = false;
    public GameObject transitionOut;
    public GameObject transitionIn;
    public GameObject blackBackGround;

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
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
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

    public void TransitionIn()
    {
        transitionIn.SetActive(false);
        transitionOut.SetActive(false);
        blackBackGround.SetActive(false);
        SoundManager.Instance.PlaySE("UI_Screen_In");
        StartCoroutine(TransitionIn_co());
    }

    public IEnumerator TransitionIn_co()
    {
        transitionIn.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        transitionIn.SetActive(false);
    }

}
