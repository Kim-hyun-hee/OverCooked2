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
        if (placedObject != null && ((KitchenTool)placedObject).HasIngredient() && ((KitchenTool)placedObject).Cook())
        {
            FireOn();
        }
        else
        {
            FireOff();
        }

        if(placedObject != null && !((KitchenTool)placedObject).Cook())
        {
            ActivateFire();
        }
    }

    public override bool PutObject(Object newObject)
    {
        if (placedObject == null && newObject is KitchenTool) // 아무것도 없고 조리도구 올려놓기
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0061f, 0.0f);
            return true;
        }
        else if (placedObject != null && newObject is Ingredient) // 조리도구 위에 재료
        {
            return ((KitchenTool)placedObject).AddIngredient((Ingredient)newObject);
        }
        else if (placedObject != null && newObject is Plate && ((Plate)newObject).AddIngredient(((KitchenTool)placedObject).GetIngredient())) // 조리도구 위에 접시
        {
            ((KitchenTool)placedObject).DeleteIngredient();
            return false;
        }
        return false;
    }

    private void FireOn()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }

    private void FireOff()
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
