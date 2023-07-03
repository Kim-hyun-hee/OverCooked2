using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTable : Table
{
    void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }

    private void Update()
    {
        if(HasObject())
        {
            SetKnifeState(false);
        }
        else
        {
            SetKnifeState(true);
        }
    }

    public void StartCut()
    {
        if (placedObject != null)
        {
            ((Ingredient)placedObject).StartCut();

            //if (!audioManager.IsPlaying("Cutting"))
            //    audioManager.Play("Cutting");
        }
    }

    public void StopCut()
    {
        ((Ingredient)placedObject).StopCut();
    }

    public bool HasCuttableObject()
    {
        if (placedObject == null)
        {
            return false;
        }

        return ((Ingredient)placedObject).IsChoppable() && ((Ingredient)placedObject).GetState() == State.RAW;
    }

    public void SetKnifeState(bool state)
    {
        //if (!state)
        //    //smoke.Play();
        //else
        //{
        //    //smoke.Stop();
        //    //audioManager.Play("Delivery");
        //}

        transform.GetChild(1).GetChild(2).gameObject.SetActive(state);
    }

    public override bool PutObject(Object newObject)
    {
        if (placedObject == null && newObject is Ingredient)
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.005f, 0.0955f);
            return true;
        }
        else if (newObject is Plate && ((Plate)newObject).AddIngredient((Ingredient)placedObject))
        {
            placedObject = null;
            return false;
        }
        else
        {
            return false;
        }
    }
}
