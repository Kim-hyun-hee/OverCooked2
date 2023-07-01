using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryTable : Table
{
    public GameObject cleanPlate;

    [SerializeField] private Stack<Object> cleanPlates = new Stack<Object>();

    private void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }

    public override bool PutObject(Object newObject)
    {
        return false;
    }

    public override Object GetObject()
    {
        Object objectToReturn = cleanPlates.Pop();
        if (cleanPlates.Count == 0)
        {
            placedObject = null;
        }
        return objectToReturn;
    }


    public void AddCleanPlate()
    {
        placedObject = Instantiate(cleanPlate.GetComponent<Object>());
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0f, cleanPlates.Count * 0.001f, 0f);
        ((Plate)placedObject).isDirty = false;
        cleanPlates.Push(placedObject);
    }
}
