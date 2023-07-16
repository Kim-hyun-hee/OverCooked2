using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevel : MonoBehaviour
{
    public Transform canvas;

    public GameObject stagePanel;
    public string sceneName;

    public GameObject ui;

    private GameObject SetStageLevelUI()
    {
        GameObject ui = Instantiate(stagePanel, canvas);
        StartCoroutine(SetPosition_co(ui));
        // 스테이지별 정보(스테이지 이름, 사진, 점수 등등) 세팅해주기,,
        return ui;
    }

    private IEnumerator SetPosition_co(GameObject ui)
    {
        while(true)
        {
            ui.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 5f, Camera.main.WorldToScreenPoint(transform.position).y + 360f, Camera.main.WorldToScreenPoint(transform.position).z);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ui = SetStageLevelUI();
        ui.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadScene(sceneName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ui.SetActive(false);
        StopAllCoroutines();
    }
}
