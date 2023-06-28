using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : Table
{
    public GameObject ingredient;
    public Object obj;
    private Animator ani;
    private void Start()
    {
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        TryGetComponent(out ani);
    }

    private void Update()
    {
        SpreadFire();
    }


    public override Object GetObject()
    {
        if(placedObject != null)
        {
            Object objectToReturn = placedObject;
            placedObject = null;
            return objectToReturn;
        }
        else
        {
            if(!fire.IsPlaying())
            {
                Object objectToReturn = Instantiate(ingredient).GetComponent<Object>();
                ani.SetTrigger("Open");
                return objectToReturn;
            }
            else
            {
                return null;
            }
        }
    }
}
