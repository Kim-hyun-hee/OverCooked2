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
    [SerializeField] private List<GameObject> orderedTiles1_1_1 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_1_2 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_1_3 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_1_4 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_1_5 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_1_6 = new List<GameObject>();

    [Header("스테이지 1-1 구조물")]
    [SerializeField] private List<GameObject> stageObject1_1 = new List<GameObject>();

    [Header("스테이지 1-2")]
    [SerializeField] private List<GameObject> orderedTiles1_2_1 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_2_2 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_2_3 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_2_4 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_2_5 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_2_6 = new List<GameObject>();

    [Header("스테이지 1-2 구조물")]
    [SerializeField] private List<GameObject> stageObject1_2 = new List<GameObject>();

    [Header("스테이지 1-3")]
    [SerializeField] private List<GameObject> orderedTiles1_3_1 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_3_2 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_3_3 = new List<GameObject>();
    [SerializeField] private List<GameObject> orderedTiles1_3_4 = new List<GameObject>();

    [Header("스테이지 1-3 구조물")]
    [SerializeField] private List<GameObject> stageObject1_3 = new List<GameObject>();

    [SerializeField] public List<List<GameObject>> allTiles1_1 = new List<List<GameObject>>();
    [SerializeField] public List<List<GameObject>> allTiles1_2 = new List<List<GameObject>>();
    [SerializeField] public List<List<GameObject>> allTiles1_3 = new List<List<GameObject>>();

    // 타일 material
    [SerializeField] private Material sand;
    [SerializeField] private Material grass;

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
        SetTileMaterial();

        if (!DBManager.Instance.playerInfo.stageInfos[0].isTileReverse)
        {
            van.transform.position = new Vector3(18.21f, 0.025f, 18.04f);
            van.transform.eulerAngles = new Vector3(0, 60f, 0);
            OpenStage1_1();
        }

        OpenStage?.Invoke();
        Camera.main.transform.position = new Vector3(van.transform.position.x, van.transform.position.y + 7f, van.transform.position.z - 5);
    }

    private void SetTileMaterial()
    {

        if (DBManager.Instance.playerInfo.stageInfos[0].isTileReverse)
        {
            allTiles1_1[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            allTiles1_1[0][0].transform.GetChild(2).localPosition = new Vector3(0, 0, 0);
            allTiles1_1[0][0].transform.GetChild(2).localEulerAngles = new Vector3(0, -60f, 0);
            for (int i = 0; i < allTiles1_1.Count; i++)
            {
                for (int j = 0; j < allTiles1_1[i].Count; j++)
                {
                    allTiles1_1[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
                }
            }
        }
        else
        {
            allTiles1_1[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            for (int i = 0; i < allTiles1_1.Count; i++)
            {
                for (int j = 0; j < allTiles1_1[i].Count; j++)
                {
                    allTiles1_1[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = sand;
                }
            }
        }

        if (DBManager.Instance.playerInfo.stageInfos[1].isTileReverse)
        {
            allTiles1_2[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            allTiles1_2[0][0].transform.GetChild(2).localPosition = new Vector3(0, 0, 0);
            allTiles1_2[0][0].transform.GetChild(2).localEulerAngles = new Vector3(0, -60f, 0);
            for (int i = 0; i < allTiles1_2.Count; i++)
            {
                for (int j = 0; j < allTiles1_2[i].Count; j++)
                {
                    allTiles1_2[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
                }
            }
        }
        else
        {
            allTiles1_2[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            for (int i = 0; i < allTiles1_2.Count; i++)
            {
                for (int j = 0; j < allTiles1_2[i].Count; j++)
                {
                    allTiles1_2[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = sand;
                }
            }
        }

        if (DBManager.Instance.playerInfo.stageInfos[2].isTileReverse)
        {
            allTiles1_3[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            allTiles1_3[0][0].transform.GetChild(2).localPosition = new Vector3(0, 0, 0);
            allTiles1_3[0][0].transform.GetChild(2).localEulerAngles = new Vector3(0, -60f, 0);
            for (int i = 0; i < allTiles1_3.Count; i++)
            {
                for (int j = 0; j < allTiles1_3[i].Count; j++)
                {
                    allTiles1_3[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
                }
            }
        }
        else
        {
            allTiles1_3[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            for (int i = 0; i < allTiles1_3.Count; i++)
            {
                for (int j = 0; j < allTiles1_3[i].Count; j++)
                {
                    allTiles1_3[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = sand;
                }
            }
        }
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
        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    }

    public void OpenStage1_2()
    {
        StartCoroutine(OpenStage1_2_co());
        DBManager.Instance.playerInfo.stageInfos[1].isTileReverse = true;
        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    }

    public void OpenStage1_3()
    {
        StartCoroutine(OpenStage1_3_co());
        DBManager.Instance.playerInfo.stageInfos[2].isTileReverse = true;
        DBManager.Instance.SaveData(DBManager.Instance.playerInfo);
    }

    private IEnumerator OpenStage1_1_co()
    {
        van.GetComponent<MapVanController>().enabled = false; // van 컨트롤러 끄기
        Camera.main.GetComponent<CameraMovement>().enabled = false; // camera가 van 따라가도록 한 스크립트 끄기
        yield return new WaitForSecondsRealtime(0.5f);

        CameraMoving(allTiles1_1[0][0]);
        yield return new WaitForSecondsRealtime(2f);

        SoundManager.Instance.PlaySE("WorldMapExposed");
        for (int i = 0; i < allTiles1_1.Count; i++)
        {
            for(int j = 0; j < allTiles1_1[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_1_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
        allTiles1_1[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);

        CameraReturn(van);
        yield return new WaitForSecondsRealtime(2f);

        van.GetComponent<MapVanController>().enabled = true;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
    }

    private IEnumerator MovementTiles1_1_co(int i, int j)
    {
        var tween = allTiles1_1[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_1[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
        allTiles1_1[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }

    private IEnumerator OpenStage1_2_co()
    {
        van.GetComponent<MapVanController>().enabled = false; // van 컨트롤러 끄기
        Camera.main.GetComponent<CameraMovement>().enabled = false; // camera가 van 따라가도록 한 스크립트 끄기
        yield return new WaitForSecondsRealtime(0.5f);

        CameraMoving(allTiles1_2[0][0]);
        yield return new WaitForSecondsRealtime(2f);

        SoundManager.Instance.PlaySE("WorldMapExposed");
        for (int i = 0; i < allTiles1_2.Count; i++)
        {
            for (int j = 0; j < allTiles1_2[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_2_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
        allTiles1_2[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);

        CameraReturn(van);
        yield return new WaitForSecondsRealtime(2f);

        van.GetComponent<MapVanController>().enabled = true;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
    }

    private IEnumerator MovementTiles1_2_co(int i, int j)
    {
        var tween = allTiles1_2[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_2[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
        allTiles1_2[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }

    private IEnumerator OpenStage1_3_co()
    {
        van.GetComponent<MapVanController>().enabled = false; // van 컨트롤러 끄기
        Camera.main.GetComponent<CameraMovement>().enabled = false; // camera가 van 따라가도록 한 스크립트 끄기
        yield return new WaitForSecondsRealtime(0.5f);

        CameraMoving(allTiles1_3[0][0]);
        yield return new WaitForSecondsRealtime(2f);

        SoundManager.Instance.PlaySE("WorldMapExposed");
        for (int i = 0; i < allTiles1_3.Count; i++)
        {
            for (int j = 0; j < allTiles1_3[i].Count; j++)
            {
                StartCoroutine(MovementTiles1_3_co(i, j));
            }
            yield return new WaitForSecondsRealtime(0.2f);
        }
        allTiles1_3[0][0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);

        CameraReturn(van);
        yield return new WaitForSecondsRealtime(2f);
         
        van.GetComponent<MapVanController>().enabled = true;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
    }

    private IEnumerator MovementTiles1_3_co(int i, int j)
    {
        var tween = allTiles1_3[i][j].transform.DORotate(new Vector3(180, 0, 0), 0.3f);
        yield return tween.WaitForCompletion();
        allTiles1_3[i][j].transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = grass;
        allTiles1_3[i][j].transform.DOLocalMoveY(-0.01f, 0.2f);
    }

    private void CameraMoving(GameObject tile)
    {
        Camera.main.transform.DOMove(new Vector3 (tile.transform.position.x, tile.transform.position.y + 7f, tile.transform.position.z - 5), 1.8f);
    }

    private void CameraReturn(GameObject van)
    {
        Camera.main.transform.DOMove(new Vector3(van.transform.position.x, van.transform.position.y + 7f, van.transform.position.z - 5), 1.8f);
    }
}
