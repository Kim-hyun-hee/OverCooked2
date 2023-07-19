using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPanel : MonoBehaviour
{
    public StageName stageName;
    public int[] score = new int[3];
    [Header("流立 技泼")]
    [SerializeField] private Image stageImage;
    [SerializeField] private Sprite[] images = new Sprite[Enum.GetValues(typeof(StageName)).Length];
    [SerializeField] private GameObject[] stars = new GameObject[3];
    [SerializeField] private Text[] scores = new Text[3];
    [Header("流立 技泼")]
    [SerializeField] private Text stage;
    [SerializeField] private Text id;
    [SerializeField] private Text highScore;

    //public static void Setting(StageName stageName, int[] score)
    //{

    //}

    private void OnEnable()
    {
        stage.text = stageName.ToString().Substring(1, 1) + "-" + stageName.ToString().Substring(3, 1);
        stageImage.sprite = images[(int)stageName];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].text = score[i].ToString();
            if((int)DBManager.Instance.playerInfo.stageInfos[(int)stageName].highScore >= score[i])
            {
                stars[i].SetActive(true);
            }
        }

        id.text = DBManager.Instance.playerInfo.id;
        highScore.text = DBManager.Instance.playerInfo.stageInfos[(int)stageName].highScore.ToString();
    }
}
