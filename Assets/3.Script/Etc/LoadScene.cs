using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void Load(string sceneName)
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadScene(sceneName);
    }
}
