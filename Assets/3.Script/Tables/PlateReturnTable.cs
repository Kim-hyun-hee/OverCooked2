using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturnTable : Table
{
    public Plate dirtyPlate;

    private Stack<Plate> dirtyPlates = new Stack<Plate>();

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
        return dirtyPlates.Pop();
    }

    public void AddPlate()
    {
        // 10√ ?
        placedObject = Instantiate(dirtyPlate.GetComponent<Plate>());
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0f, 0.005f + (dirtyPlates.Count * 0.00f), 0f);
        dirtyPlates.Push(dirtyPlate);
    }
}
