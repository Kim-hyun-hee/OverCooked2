using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Puff : MonoBehaviour
{
    private ObjectPool objectPool;
    private Sequence sequence;
    private void Start()
    {
        sequence = DOTween.Sequence();
        sequence.SetAutoKill(false);
        sequence.Append(transform.DOScale(0.1f, 0.01f));
        sequence.Append(transform.DOScale(0.0001f, 0.6f).SetEase(Ease.InCirc));
    }

    private void OnEnable()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        StartCoroutine(Change_co());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        sequence.Pause();
    }

    private IEnumerator Change_co()
    {
        sequence.Restart();
        yield return new WaitForSeconds(0.5f);
        objectPool.ReturnObject(gameObject);

    }
}
