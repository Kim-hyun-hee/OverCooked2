using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StageDefaultPanel : MonoBehaviour
{
    public StageName stageName;
    [SerializeField] private Image stageImage;
    [SerializeField] private Sprite[] images = new Sprite[Enum.GetValues(typeof(StageName)).Length];
    [SerializeField] private Text stage;
    [SerializeField] private Text starNumber;
    [SerializeField] private string number;

    private void OnEnable()
    {
        stage.text = stageName.ToString().Substring(1, 1) + "-" + stageName.ToString().Substring(3, 1);
        stageImage.sprite = images[(int)stageName];
        starNumber.text = number;
    }
}
