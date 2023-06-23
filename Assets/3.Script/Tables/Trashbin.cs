using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin : Table
{
    public override bool PutObject(Object newObject)
    {
        newObject.ThrowToBin();
        return false;
    }
}
