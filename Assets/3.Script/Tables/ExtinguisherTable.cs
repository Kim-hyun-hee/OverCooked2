using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherTable : Table
{
    public GameObject extinguisher;

    private void Start()
    {
        fire = this.transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        placedObject = Instantiate(extinguisher).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.005f, 0.0f);
    }
}
