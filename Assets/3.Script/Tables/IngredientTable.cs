using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : Table
{
    public GameObject ingredient;

    private void Start()
    {
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }

    private void Update()
    {
        SpreadFire();
    }


    override public Object GetObject()
    {
        Object objectToReturn = Instantiate(ingredient).GetComponent<Object>();
        return objectToReturn;
    }

    override public bool PutObject(Object Object) // 원작은 놓을 수 있긴 함,,
    {
        return false;
    }
}
