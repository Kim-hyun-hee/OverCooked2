using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    private OrderManager orderManager;

    public GameObject ready;
    public GameObject go;

    public GameObject endPanel;

    public GameObject uiCrono;
    public Slider cronoSlider;
    private float hue;
    public float levelTime = 180.0f;
    public float remainingTime;
    private bool isRing = true;
    private bool is30Sound = false;
    private bool is10Sound = false;

    void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();

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
        yield return new WaitForSecondsRealtime(2f);
        ready.SetActive(false);
        go.SetActive(true);
        SoundManager.Instance.PlaySE("Go");
        yield return new WaitForSecondsRealtime(1f);
        go.SetActive(false);
        StartCoroutine(UpdateCrono_co());
        StartCoroutine(orderManager.UpdateNewOrder());
        StartCoroutine(orderManager.UpdateOrders());

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
                SoundManager.Instance.PlaySE("30SecondsGone");
                isRing = true;
            }
            else if (seconds == 30 && minutes != 0 && !isRing)
            {
                uiCrono.transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetTrigger("alram");
                SoundManager.Instance.PlaySE("30SecondsGone");
                isRing = true;
            }
            else if (seconds > 10 && seconds <= 30 && minutes == 0 && !isRing)
            {
                uiCrono.transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetTrigger("infalram");
                if(!is30Sound)
                {
                    SoundManager.Instance.PlaySE("30SecondsLeft");
                    is30Sound = true;
                }
                isRing = true;
            }
            else if (seconds <= 10 && minutes == 0 && !isRing)
            {
                if(!is10Sound)
                {
                    Debug.Log("10초 남았다");
                    SoundManager.Instance.PlaySE("10SecondsLeft");
                    is10Sound = true;
                }
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
                uiCrono.transform.GetChild(2).GetComponent<Text>().text = "00:00";
                break;
            }
            yield return null;
        }
        EndStage();
    }

    public void EndStage()
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        Debug.Log("게임 종료");
    }

}

