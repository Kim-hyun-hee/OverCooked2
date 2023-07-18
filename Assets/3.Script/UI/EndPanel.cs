using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] star = new GameObject[3];

    [Header("성공 > 팁 > 실패 > 합계")]
    [SerializeField] private GameObject[] text = new GameObject[8];

    private readonly string[] resultText = new string[8];

    void Start()
    {
        SoundManager.Instance.PlaySE("LevelVictorySound");
        StartCoroutine(ShowOnebyOne());

        resultText[0] = "배달된 주문 x " + string.Format("{0}", StageManager.Instance.successOrder);
        resultText[1] = string.Format("{0}", StageManager.Instance.successScore);

        resultText[2] = "팁";
        resultText[3] = string.Format("{0}", StageManager.Instance.tip);

        resultText[4] = "실패한 주문 x " + string.Format("{0}", StageManager.Instance.failOrder);
        resultText[5] = "점수 어케 하지";

        resultText[6] = "합계";
        resultText[7] = string.Format("{0}", StageManager.Instance.totalScore);

        // 여기서 DB에 저장 하면 될거 같음!
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (StageManager.Instance.totalScore >= StageManager.Instance.score[0])
            {
                MapSceneManager.OpenStage += () => Debug.Log("이벤트에 추가 완료");
            }
            Time.timeScale = 1f;
            GameManager.Instance.LoadScene("MapScene");
        }
    }

    // RoundResults_Star_1
    // RoundResults_FinalScore
    // RoundResults_Description_1

    private IEnumerator ShowOnebyOne()
    {
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(Star());
        StartCoroutine(Text());
    }

    private IEnumerator Star()
    {
        for(int i = 0; i < 3; i++)
        {
            if(StageManager.Instance.score[i] <= StageManager.Instance.totalScore)
            {
                star[i].SetActive(true);
                string name = "RoundResults_Star_" + string.Format("{0}", i + 1);
                SoundManager.Instance.PlaySE(name);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            else
            {
                break;
            }
        }
    }

    private IEnumerator Text()
    {
        for (int i = 0; i < 6; i++)
        {
            text[i].GetComponent<Text>().text = resultText[i];
            text[i].SetActive(true);
            if(i % 2 == 0)
            {
                string name = "RoundResults_Description_" + string.Format("{0}", i);
                SoundManager.Instance.PlaySE(name);
            }
            yield return new WaitForSecondsRealtime(0.25f);
        }

        text[6].GetComponent<Text>().text = resultText[6];
        text[7].GetComponent<Text>().text = resultText[7];
        text[6].SetActive(true);
        text[7].SetActive(true);
        SoundManager.Instance.PlaySE("RoundResults_FinalScore");
        SoundManager.Instance.PlayBGM("RoundResults");
        yield return new WaitForSecondsRealtime(36f);

        if (StageManager.Instance.totalScore >= StageManager.Instance.score[0])
        {
            MapSceneManager.OpenStage += () => Debug.Log("이벤트에 추가 완료");
        }
        //Time.timeScale = 1f;
        GameManager.Instance.LoadScene("MapScene");
    }
}
