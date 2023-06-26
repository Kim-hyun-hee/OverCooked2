using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject poolingObject;
    [SerializeField]
    private int count = 15;
    private Queue<GameObject> poolingQueue;

    private void OnEnable()
    {
        poolingQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(poolingObject, transform.position, Quaternion.identity, transform);
            poolingQueue.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    public GameObject GetObject()
    {
        if (poolingQueue.Count > 0)
        {
            GameObject obj = poolingQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(poolingObject, transform.position, Quaternion.identity, transform);
            obj.SetActive(true);
            return obj;
        }
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolingQueue.Enqueue(obj);
    }
}


