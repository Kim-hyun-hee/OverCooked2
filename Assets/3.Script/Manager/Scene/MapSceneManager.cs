using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM("MapScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadScene("StartScene");
        }
    }
}
