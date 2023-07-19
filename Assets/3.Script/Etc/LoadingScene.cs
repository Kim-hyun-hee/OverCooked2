using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    static string nextSceneName;
    [SerializeField] private Slider loadingBar;

    public static void LoadScene(string sceneMame)
    {
        nextSceneName = sceneMame;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        SoundManager.Instance.FadeBGM(0, 1.5f);
        StartCoroutine(LoadSceneProcess_co());
    }

    private IEnumerator LoadSceneProcess_co()
    {
        GameManager.Instance.TransitionIn(false);
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float time = 0f;
        while (!op.isDone)
        {
            if (op.progress < 0.9f)
            {
                loadingBar.value = op.progress;
            }
            else
            {
                time += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1f, time/2);

                if (loadingBar.value >= 1f)
                {
                    GameManager.Instance.transitionIn.SetActive(false);
                    SoundManager.Instance.PlaySE("UI_Screen_Out");
                    GameManager.Instance.transitionOut.SetActive(true);
                    yield return new WaitForSecondsRealtime(0.6f);
                    GameManager.Instance.blackBackGround.SetActive(true);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            yield return null;
        }
    }
}
