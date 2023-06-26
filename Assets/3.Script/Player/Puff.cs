using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puff : MonoBehaviour
{
    [SerializeField] private GameObject puff;
    private bool isPlay = false;
    private ObjectPool objectPool;
    private Coroutine running;

    private void OnEnable()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        StartCoroutine(Puff_co());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Puff_co()
    {
        // ũ�� �۾����� ������� �ڷ�ƾ
        yield return null;
    }
}
