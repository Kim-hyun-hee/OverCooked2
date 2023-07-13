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
        SoundManager.Instance.PlaySE("UI_Screen_In");
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess_co());
    }

    private IEnumerator LoadSceneProcess_co()
    {
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
                loadingBar.value = Mathf.Lerp(0.9f, 1f, time);

                if (loadingBar.value >= 1f)
                {
                    SoundManager.Instance.PlaySE("UI_Screen_Out");
                    yield return new WaitForSecondsRealtime(0.8f);
                    op.allowSceneActivation = true;
                    SoundManager.Instance.PlaySE("UI_Screen_In");
                    yield break;
                }
            }
            yield return null;
        }
    }
}
