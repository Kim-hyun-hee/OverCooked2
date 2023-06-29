using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTable : Table
{
    public GameObject pot;

    private void Start()
    {
        fire = this.transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        placedObject = Instantiate(pot).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.0061f, 0.0f);
    }

    private void Update()
    {
        Cook();

        if(placedObject != null && ((KitchenTool)placedObject).HasIngredient())
        {
            // sound
        }
    }

    public void Cook()
    {
        if(placedObject != null && !((KitchenTool)placedObject).Cook())
        {
            ActivateFire();
        }
    }

    public override bool PutObject(Object newObject)
    {
        if (placedObject == null && newObject is KitchenTool)
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0061f, 0.0f);
            return true;
        }
        else if (placedObject != null && newObject is Ingredient)
        {
            return ((KitchenTool)placedObject).AddIngredient((Ingredient)newObject);
        }
        else if (placedObject != null && newObject is Plate && ((Plate)newObject).AddIngredient(((KitchenTool)placedObject).GetIngredient()))
        {
            ((KitchenTool)placedObject).DeleteIngredient();
            return false;
        }
        return true;
    }
}
