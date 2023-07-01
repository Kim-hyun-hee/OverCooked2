using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTable : Table
{
    [SerializeField] private Transform[] dirtyPlate = new Transform[3];
    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();
    private DryTable dryTable;

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

    public void Wash()
    {
        dirtyPlate[dirtyPlates.Count - 1].gameObject.SetActive(false);
        Destroy(transform.GetChild(1).GetChild(dirtyPlates.Count - 1).gameObject);
        dirtyPlates.Pop();
        dryTable.AddCleanPlate();
    }
}

