using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturnTable : Table
{
    public GameObject dirtyPlate;

    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();

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
        Object objectToReturn = dirtyPlates.Pop();
        if(dirtyPlates.Count == 0)
        {
            placedObject = null;
        }
        return objectToReturn;
    }

    public void AddDirtyPlate()
    {
        // 10√ ?
        placedObject = Instantiate(dirtyPlate.GetComponent<Object>());
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0f, 0.005f + (dirtyPlates.Count * 0.001f), 0f);
        SoundManager.Instance.PlaySE("WashedPlate");
        ((Plate)placedObject).isDirty = true;
        dirtyPlates.Push(placedObject);
    }
}
