using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginToStart : MonoBehaviour
{
    static string nextSceneName;

    public static void LoadStartScene(string sceneMame)
    {
        nextSceneName = sceneMame;
        SceneManager.LoadScene("LoginToStart");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess_co());
    }

    private IEnumerator LoadSceneProcess_co()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            if (op.progress < 0.9f)
            {
                Debug.Log("Loading...");
            }
            else
            {
                yield return new WaitForSecondsRealtime(3f);
                op.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }
}
