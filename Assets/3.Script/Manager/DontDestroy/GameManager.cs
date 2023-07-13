using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    public bool isOpenShutter = false; // StartSceneManager에서 사용

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
        SoundManager.Instance.PlaySE("UI_Screen_Out");
        yield return new WaitForSecondsRealtime(0.8f);
        LoadingScene.LoadScene(sceneName);
    }

}
