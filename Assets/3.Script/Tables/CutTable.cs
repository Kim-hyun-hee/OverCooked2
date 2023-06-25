using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTable : Table
{
    void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }
}
