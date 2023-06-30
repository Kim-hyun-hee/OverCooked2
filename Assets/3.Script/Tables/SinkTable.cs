using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTable : Table
{
    public GameObject plate;

    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();
    [SerializeField] private List<Object> cleanPlates = new List<Object>();

    public override bool PutObject(Object newObject)
    {
        if(newObject is Plate && ((Plate)newObject).IsDirty())
        {
            dirtyPlates.Push(newObject);
            // 물 안에 접시 SetActive(true)
            return true;
        }

        return false;
    }

    public override Object GetObject()
    {
        Object objectToReturn = cleanPlates[0];
        cleanPlates.RemoveAt(0);
        if (cleanPlates.Count == 0)
        {
            placedObject = null;
        }
        return objectToReturn;
    }

    private void AddCleanPlate()
    {
        //isDirty = false;
    }
}
