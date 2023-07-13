using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlaySE(string name)
    {
        SoundManager.Instance.PlaySE(name);
    }

    public void PlayBGM(string name)
    {
        SoundManager.Instance.PlayBGM(name);
    }

    public void StopAllSE()
    {
        SoundManager.Instance.StopAllSE();
    }

    public void StopBGM()
    {
        SoundManager.Instance.StopBGM();
    }

    public void StopSE(string name)
    {
        SoundManager.Instance.StopSE(name);
    }
}
