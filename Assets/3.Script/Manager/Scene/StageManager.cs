using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public GameObject ready;
    public GameObject go;

    public GameObject uiCrono;
    public Slider cronoSlider;
    private float hue;
    public float levelTime = 180.0f;
    public float remainingTime;
    private bool isRing = true;

    void Start()
    {
        remainingTime = levelTime;
        hue = (float)120 / 360;

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(true);
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        StartCoroutine(ReadyGo());
    }

    private IEnumerator ReadyGo()
    {
        ready.SetActive(true);
        SoundManager.Instance.PlaySE("Ready");
        yield return new WaitForSecondsRealtime(3f);
        ready.SetActive(false);
        StartCoroutine(UpdateCrono_co());
    }
    private IEnumerator UpdateCrono_co()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBGM("TheNeonCity");
        //if (remainingTime > 0 && cronoRunning)
        //{
        //    remainingTime -= Time.deltaTime;
        //}
        //else if (cronoRunning)
        //{
        //    cronoRunning = false;
        //    remainingTime = 0;
        //    Time.timeScale = 0;
        //    //UIManager.Instance.EndMenu.SetActive(true);
        //}
        while (true)
        {
            int minutes = (Mathf.CeilToInt(remainingTime) / 60);
            int seconds = Mathf.CeilToInt(remainingTime % 60);

            if (seconds == 60)
            {
                seconds = 0;
            }

            if (seconds == 0 && !isRing)
            {
                uiCrono.transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetTrigger("alram");
                isRing = true;
            }
            else if (seconds == 30 && minutes != 0 && !isRing)
            {
                uiCrono.transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetTrigger("alram");
                isRing = true;
            }
            else if (seconds <= 30 && minutes == 0 && !isRing)
            {
                uiCrono.transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetTrigger("infalram");
                isRing = true;
            }

            if (seconds != 0 && seconds != 30)
            {
                isRing = false;
            }

            string minutesString;
            string secondsString;

            if (minutes < 10)
            {
                minutesString = string.Format("0{0}", minutes);
            }
            else
            {
                minutesString = string.Format("{0}", minutes);
            }

            if (seconds < 10)
            {
                secondsString = string.Format("0{0}", seconds);
            }
            else
            {
                secondsString = string.Format("{0}", seconds);
            }

            uiCrono.transform.GetChild(2).GetComponent<Text>().text = minutesString + ":" + secondsString;
            cronoSlider.value = remainingTime / levelTime;
            float hue = this.hue;
            hue *= cronoSlider.value;
            cronoSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(hue, 1, 0.85f);
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 0)
            {
                break;
            }
            yield return null;
        }
    }
}
