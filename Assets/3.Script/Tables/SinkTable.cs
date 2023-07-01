using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTable : Table
{

    public override bool PutObject(Object newObject)
    {
        return false;
    }

    public override Object GetObject()
    {
        return null;
    }
}
