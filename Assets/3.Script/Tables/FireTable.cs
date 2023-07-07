using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireTable : Table
{
    public GameObject pot;
    public Image warning;
    public Transform uiTransform;

    private void Start()
    {
        fire = this.transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        placedObject = Instantiate(pot).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.0061f, 0.0f);
        uiTransform = GameObject.FindGameObjectWithTag("ObjectUI").transform;
        warning = transform.GetChild(3).GetComponent<Image>();
        warning.transform.SetParent(uiTransform);
        warning.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 15f, Camera.main.WorldToScreenPoint(transform.position).y - 20f, Camera.main.WorldToScreenPoint(transform.position).z);
        warning.gameObject.SetActive(false);
    }

    private void Update()
    {
        Cook();

        if (placedObject != null && ((KitchenTool)placedObject).HasIngredient())
        {
            // sound
        }
    }

    public void Cook()
    {
        if (placedObject != null && ((KitchenTool)placedObject).HasIngredient())
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

        if(placedObject != null && ((KitchenTool)placedObject).HasIngredient() && ((((KitchenTool)placedObject).GetIngredient()).GetState()) == State.COOKED
            && (((KitchenTool)placedObject).GetIngredient()).remainingOvercookTime <= (0.75f * (((KitchenTool)placedObject).GetIngredient()).overcookTime))
        {
            warning.gameObject.SetActive(true);
        }
        else if(placedObject != null && ((KitchenTool)placedObject).HasIngredient() && ((((KitchenTool)placedObject).GetIngredient()).GetState()) == State.OVERCOOKED)
        {
            warning.gameObject.SetActive(false);
        }
        else
        {
            warning.gameObject.SetActive(false);
        }
    }

    public override bool PutObject(Object newObject)
    {
        if (placedObject == null && newObject is KitchenTool) // 아무것도 없고 조리도구 올려놓기
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0061f, 0.0f);
            placedObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
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
