using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] stars = new GameObject[3];

    [Header("성공 > 팁 > 실패 > 합계")]
    [SerializeField] private GameObject[] text = new GameObject[8];

    private readonly string[] resultText = new string[8];
    private int star = 0;

    [SerializeField] private Text id;

    void Start()
    {
        SoundManager.Instance.PlaySE("LevelVictorySound");
        StartCoroutine(ShowOnebyOne());

        resultText[0] = "배달된 주문 x " + string.Format("{0}", StageManager.Instance.successOrder);
        resultText[1] = string.Format("{0}", StageManager.Instance.successScore);

        resultText[2] = "팁";
        resultText[3] = string.Format("{0}", StageManager.Instance.tip);

        resultText[4] = "실패한 주문 x " + string.Format("{0}", StageManager.Instance.failOrder);
        resultText[5] = string.Format("{0}", StageManager.Instance.failOrder);

        resultText[6] = "합계";
        resultText[7] = string.Format("{0}", StageManager.Instance.totalScore - (StageManager.Instance.failOrder * 30));
        StageManager.Instance.totalScore = StageManager.Instance.totalScore - (StageManager.Instance.failOrder * 30);

        id.text = DBManager.Instance.playerInfo.id;

        AnimationEvent();
        SaveStageInfo();
        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //if (StageManager.Instance.totalScore >= StageManager.Instance.score[0])
            //{
            //    AnimationEvent();
            //}
            DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
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

    private void AnimationEvent()
    {
        if(!DBManager.Instance.playerInfo.stageInfos[(int)StageManager.Instance.stageName].isClear && StageManager.Instance.totalScore >= StageManager.Instance.score[0])
        {
            MapSceneManager.OpenStage += () => Debug.Log("이벤트에 애니메이션 추가 완료");
            if(StageManager.Instance.stageName == StageName.S1_1)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_1[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_1[0][0].transform.position.z);
                };

                MapSceneManager.OpenStage += () => MapSceneManager.Instance.OpenStage1_2();
            }
            else if(StageManager.Instance.stageName == StageName.S1_2)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_2[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_2[0][0].transform.position.z);
                };

                MapSceneManager.OpenStage += () => MapSceneManager.Instance.OpenStage1_3();
            }
            else if(StageManager.Instance.stageName == StageName.S1_3)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_3[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_3[0][0].transform.position.z);
                };
            }
        }
        else
        {
            Debug.Log("실패"); // 버스 위치 조정 만
            if (StageManager.Instance.stageName == StageName.S1_1)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_1[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_1[0][0].transform.position.z);
                };
            }
            else if (StageManager.Instance.stageName == StageName.S1_2)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_2[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_2[0][0].transform.position.z);
                };
            }
            else if (StageManager.Instance.stageName == StageName.S1_3)
            {
                MapSceneManager.OpenStage += () => { MapSceneManager.Instance.van.transform.position
                    = new Vector3(MapSceneManager.Instance.allTiles1_3[0][0].transform.position.x, 0.025f, MapSceneManager.Instance.allTiles1_3[0][0].transform.position.z);
                };
            }

        }
    }

    private IEnumerator Star()
    {
        for(int i = 0; i < 3; i++)
        {
            if(StageManager.Instance.score[i] <= StageManager.Instance.totalScore)
            {
                stars[i].SetActive(true);
                star += 1;
                string name = "RoundResults_Star_" + string.Format("{0}", i + 1);
                SoundManager.Instance.PlaySE(name);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            else
            {
                break;
            }
        }

        DBManager.Instance.playerInfo.stageInfos[(int)StageManager.Instance.stageName].star = star;
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

        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
        GameManager.Instance.LoadScene("MapScene");
    }

    private void SaveStageInfo()
    {
        if(StageManager.Instance.totalScore >= StageManager.Instance.score[0])
        {
            DBManager.Instance.playerInfo.stageInfos[(int)StageManager.Instance.stageName].isClear = true;
        }

        if(StageManager.Instance.totalScore > DBManager.Instance.playerInfo.stageInfos[(int)StageManager.Instance.stageName].highScore)
        {
            DBManager.Instance.playerInfo.stageInfos[(int)StageManager.Instance.stageName].highScore = StageManager.Instance.totalScore;
        }
    }
}
