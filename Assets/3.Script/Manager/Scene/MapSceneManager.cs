using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapSceneManager : MonoBehaviour
{
    private static MapSceneManager instance;
    public static MapSceneManager Instance { get { return instance; } }

    public GameObject pauseMenu;

    public Text totalStar;
    private int total = 0;

    // 스테이지 열릴 때 애니메이션 실행하는 순서
    [Header("스테이지 1-1")]
    public List<GameObject> orderedTiles1_1_1 = new List<GameObject>();
    public List<GameObject> orderedTiles1_1_2 = new List<GameObject>();
    public List<GameObject> orderedTiles1_1_3 = new List<GameObject>();
    public List<GameObject> orderedTiles1_1_4 = new List<GameObject>();
    public List<GameObject> orderedTiles1_1_5 = new List<GameObject>();
    public List<GameObject> orderedTiles1_1_6 = new List<GameObject>();

    [Header("스테이지 1-2")]
    public List<GameObject> orderedTiles1_2_1 = new List<GameObject>();
    public List<GameObject> orderedTiles1_2_2 = new List<GameObject>();
    public List<GameObject> orderedTiles1_2_3 = new List<GameObject>();
    public List<GameObject> orderedTiles1_2_4 = new List<GameObject>();
    public List<GameObject> orderedTiles1_2_5 = new List<GameObject>();
    public List<GameObject> orderedTiles1_2_6 = new List<GameObject>();

    [Header("스테이지 1-3")]
    public List<GameObject> orderedTiles1_3_1 = new List<GameObject>();
    public List<GameObject> orderedTiles1_3_2 = new List<GameObject>();
    public List<GameObject> orderedTiles1_3_3 = new List<GameObject>();
    public List<GameObject> orderedTiles1_3_4 = new List<GameObject>();

    public List<List<GameObject>> allTiles1_1 = new List<List<GameObject>>();
    public List<List<GameObject>> allTiles1_2 = new List<List<GameObject>>();
    public List<List<GameObject>> allTiles1_3 = new List<List<GameObject>>();

    public GameObject van;

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

    public delegate void OnOpenStage();
    public static event OnOpenStage OpenStage;

    void Start()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBGM("MapScreen");
        SoundManager.Instance.SetBGMVolume(1);
        GameManager.Instance.TransitionIn(false);

        for(int i = 0; i < Enum.GetValues(typeof(StageName)).Length; i++)
        {
            total += DBManager.Instance.playerInfo.stageInfos[i].star;
        }
        totalStar.text = total.ToString();

        SetTiles();

        if(!DBManager.Instance.playerInfo.stageInfos[0].isTileReverse)
        {
            OpenStage1_1();
        }

        OpenStage?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.GetUIStackCount() > 1)
            {
                GameObject ui = UIManager.Instance.PopUI();
                ui.SetActive(false);
            }
            else if (UIManager.Instance.GetUIStackCount() == 1)
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

        if (UIManager.Instance.GetUIStackCount() != 0)
        {
            Time.timeScale = 0;
        }
    }

    private void OnDisable()
    {
        OpenStage = null;
        StopAllCoroutines();
    }

    private void SetTiles()
    {
        allTiles1_1.Add(orderedTiles1_1_1);
        allTiles1_1.Add(orderedTiles1_1_2);
        allTiles1_1.Add(orderedTiles1_1_3);
        allTiles1_1.Add(orderedTiles1_1_4);
        allTiles1_1.Add(orderedTiles1_1_5);
        allTiles1_1.Add(orderedTiles1_1_6);

        allTiles1_2.Add(orderedTiles1_2_1);
        allTiles1_2.Add(orderedTiles1_2_2);
        allTiles1_2.Add(orderedTiles1_2_3);
        allTiles1_2.Add(orderedTiles1_2_4);
        allTiles1_2.Add(orderedTiles1_2_5);
        allTiles1_2.Add(orderedTiles1_2_6);

        allTiles1_3.Add(orderedTiles1_3_1);
        allTiles1_3.Add(orderedTiles1_3_2);
        allTiles1_3.Add(orderedTiles1_3_3);
        allTiles1_3.Add(orderedTiles1_3_4);
    }

    public void OpenStage1_1()
    {
        StartCoroutine(OpenStage1_1_co());
        DBManager.Instance.playerInfo.stageInfos[0].isTileReverse = true;
    }

    public void OpenStage1_2()
    {
        StartCoroutine(OpenStage1_2_co());
        DBManager.Instance.playerInfo.stageInfos[1].isTileReverse = true;
    }

    public void OpenStage1_3()
    {
        StartCoroutine(OpenStage1_3_co());
        DBManager.Instance.playerInfo.stageInfos[2].isTileReverse = true;
    }

    private IEnumerator OpenStage1_1_co()
    {
        for(int i = 0; i < allTiles1_1.Count; i++)
        {
            for(int j = 0; j < allTiles1_1[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_1_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private IEnumerator MovementTiles1_1_co(int i, int j)
    {
        var tween = allTiles1_1[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_1[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }

    private IEnumerator OpenStage1_2_co()
    {
        for (int i = 0; i < allTiles1_2.Count; i++)
        {
            for (int j = 0; j < allTiles1_2[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_2_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private IEnumerator MovementTiles1_2_co(int i, int j)
    {
        var tween = allTiles1_2[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_2[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }

    private IEnumerator OpenStage1_3_co()
    {
        for (int i = 0; i < allTiles1_3.Count; i++)
        {
            for (int j = 0; j < allTiles1_3[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_3_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private IEnumerator MovementTiles1_3_co(int i, int j)
    {
        var tween = allTiles1_3[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_3[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }
}
