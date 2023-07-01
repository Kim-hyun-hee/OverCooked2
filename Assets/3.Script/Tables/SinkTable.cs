using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTable : Table
{
    public GameObject plate;
    [SerializeField] private Transform[] dirtyPlate = new Transform[3];
    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            dirtyPlate[i] = transform.GetChild(1).GetChild(i + 1);
        }
    }

    public override bool PutObject(Object newObject)
    {
        if(newObject is Plate && ((Plate)newObject).IsDirty())
        {
            dirtyPlates.Push(newObject);
            newObject.transform.SetParent(transform.GetChild(3).GetChild(0));
            newObject.gameObject.SetActive(false);
            if(dirtyPlates.Count != 4)
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
}
