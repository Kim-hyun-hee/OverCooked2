using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public StageName stageName;
    private static StageManager instance;
    public static StageManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public delegate void OnEndStage();
    public event OnEndStage EndStage;

    private OrderManager orderManager;

    public GameObject ready;
    public GameObject go;
    public GameObject end;

    public GameObject endMenu;
    public GameObject pauseMenu;

    public GameObject uiCrono;
    public Slider cronoSlider;
    private float hue;
    public float levelTime = 180.0f;
    public float remainingTime;

    private bool isRing = true;
    private bool is30Sound = false;
    private bool is10Sound = false;

    public int successOrder;
    public int successScore;
    public int tip;
    public int failOrder;
    public int totalScore;

    public Player player1;
    public Player player2;
    private bool isPlayerOne = true;

    // endPanel에서 별 조건 점수
    public int[] score = new int[3];

    void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
        SoundManager.Instance.audioSourceBgm.pitch = 1.0f;

        remainingTime = levelTime;
        hue = (float)120 / 360;

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(true);
        Time.timeScale = 0;

        player1.isMove = true;
        player1.PlayerMovementOn();
        player2.isMove = false;
        player2.PlayerMovementOff();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.GetUIStackCount() > 1)
            {
                GameObject ui = UIManager.Instance.PopUI();
                ui.SetActive(false);
            }
            else if(UIManager.Instance.GetUIStackCount() == 1)
            {
                GameObject ui = UIManager.Instance.PopUI();
                ui.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                UIManager.Instance.PushUI(pauseMenu);
            }
        }

        if(UIManager.Instance.GetUIStackCount() != 0)
        {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SoundManager.Instance.PlaySE("ChangeChef");
            if (isPlayerOne)
            {
                player1.isMove = false;
                player1.PlayerMovementOff();
                player2.isMove = true;
                player2.PlayerMovementOn();
                isPlayerOne = false;
            }
            else
            {
                player1.isMove = true;
                player1.PlayerMovementOn();
                player2.isMove = false;
                player2.PlayerMovementOff();
                isPlayerOne = true;
            }
        }

        // endMenu true 상태에서 스페이스바 누르면 로딩창 > 맵
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
                    SoundManager.Instance.audioSourceBgm.pitch = 1.2f;
                    is30Sound = true;
                }
                isRing = true;
            }
            else if (seconds <= 10 && minutes == 0 && !isRing)
            {
                if(!is10Sound)
                {
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
                SoundManager.Instance.audioSourceBgm.pitch = 1.0f;
                SoundManager.Instance.audioSourceBgm.Stop();
                break;
            }
            yield return null;
        }
        End();
    }

    public void End()
    {
        StopAllCoroutines();
        Time.timeScale = 0;

        EndStage?.Invoke();

        SoundManager.Instance.PlaySE("TImesUpSting");
        end.SetActive(true);
        StartCoroutine(EndPanel_Co());
    }

    public IEnumerator EndPanel_Co()
    {
        yield return new WaitForSecondsRealtime(4f);
        end.SetActive(false);
        endMenu.SetActive(true);
    }
}

