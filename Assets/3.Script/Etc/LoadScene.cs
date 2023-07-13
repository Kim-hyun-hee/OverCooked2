using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void Load(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}
