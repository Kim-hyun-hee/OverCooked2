using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTable : Table
{
    [SerializeField] private Transform[] dirtyPlate = new Transform[3];
    private Stack<Object> dirtyPlates = new Stack<Object>();
    private DryTable dryTable;
    [SerializeField] private float remainingWashTime = 3;

    private void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        dryTable = FindObjectOfType<DryTable>();
    }

    public override bool PutObject(Object newObject)
    {
        if (newObject is Plate && ((Plate)newObject).IsDirty())
        {
            dirtyPlates.Push(newObject);
            newObject.transform.SetParent(transform.GetChild(1));
            newObject.gameObject.SetActive(false);
            if (dirtyPlates.Count != 4)
            {
                dirtyPlate[dirtyPlates.Count - 1].gameObject.SetActive(true);
            }
            return true;
        }

        return false;
    }

    public bool HasWashableObject()
    {
        return (dirtyPlates.Count != 0);
    }

    public void StartWash()
    {
        StartCoroutine(Wash_co());
    }

    private IEnumerator Wash_co()
    {
        while(true)
        {
            remainingWashTime -= Time.deltaTime;
            //slider.gameObject.SetActive(true);
            //slider.value = (chopTime - remainingChopTime) / chopTime;
            if (remainingWashTime <= 0)
            {
                if(dirtyPlates.Count < 4)
                {
                    dirtyPlate[dirtyPlates.Count - 1].gameObject.SetActive(false);
                }
                Destroy(transform.GetChild(1).GetChild(dirtyPlates.Count - 1).gameObject);
                dirtyPlates.Pop();
                dryTable.AddCleanPlate();
                break;
            }
            yield return null;
        }
        remainingWashTime = 3;
    }

    public void StopWash()
    {
        StopAllCoroutines();
    }
}

