using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryTable : Table
{
    public GameObject cleanPlate;

    [SerializeField] private Stack<Object> cleanPlates = new Stack<Object>();

    private void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }

    public override bool PutObject(Object newObject)
    {
        if (cleanPlates.Count > 0)
        {
            if (newObject is Ingredient) // 놓을 오브젝트가 재료일때
            {
                if (placedObject is Plate) // 접시 위에 올리는 경우
                {
                    return ((Plate)placedObject).AddIngredient((Ingredient)newObject); // 재료 올라갈 수 있는지 없는지 (정해진 레시피에 따라 판단)
                }
            }
            // 접시 위에 조리도구 / 조리도구 안에 있는 재료가 접시 위에 올라갈 수 있을때 (정해진 레시피에 따라 판단)
            else if (newObject is KitchenTool && placedObject is Plate && ((Plate)placedObject).AddIngredient(((KitchenTool)newObject).GetIngredient()))
            {
                ((KitchenTool)newObject).DeleteIngredient(); // 조리도구에서 재료 빼기
                SoundManager.Instance.PlaySE("PickUp");
                return false;
            }
            // 접시 위에 접시 / newObject위에 있는 재료가 placedObject에 있는 재료 위에 올라갈 수 있을때 (정해진 레시피와 조리방법에 따라 판단)
            else if (newObject is Plate && placedObject is Plate && ((Plate)placedObject).AddIngredients(((Plate)newObject).GetIngredients()))
            {
                ((Plate)newObject).MoveRecipe();
                SoundManager.Instance.PlaySE("PutDown");
                return false;
            }
        }
        return false;
    }

    public override Object GetObject()
    {
        Object objectToReturn = cleanPlates.Pop();
        if (cleanPlates.Count == 0)
        {
            placedObject = null;
        }
        return objectToReturn;
    }


    public void AddCleanPlate()
    {
        SoundManager.Instance.PlaySE("WashedPlate");
        placedObject = Instantiate(cleanPlate.GetComponent<Object>());
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0f, cleanPlates.Count * 0.001f, 0f);
        ((Plate)placedObject).isDirty = false;
        cleanPlates.Push(placedObject);
    }
}
