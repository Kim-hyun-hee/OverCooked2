using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTable : Table
{
    public GameObject plate;

    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();

    public override bool PutObject(Object newObject)
    {
        if(newObject is Plate && ((Plate)newObject).IsDirty())
        {
            // 설거지 하기
            // AddCleanPlate
            return true;
        }

        return false;
    }

    public override Object GetObject()
    {
        Object objectToReturn = dirtyPlates.Pop();
        if (dirtyPlates.Count == 0)
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
