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
            if (newObject is Ingredient) // ���� ������Ʈ�� ����϶�
            {
                if (placedObject is Plate) // ���� ���� �ø��� ���
                {
                    return ((Plate)placedObject).AddIngredient((Ingredient)newObject); // ��� �ö� �� �ִ��� ������ (������ �����ǿ� ���� �Ǵ�)
                }
            }
            // ���� ���� �������� / �������� �ȿ� �ִ� ��ᰡ ���� ���� �ö� �� ������ (������ �����ǿ� ���� �Ǵ�)
            else if (newObject is KitchenTool && placedObject is Plate && ((Plate)placedObject).AddIngredient(((KitchenTool)newObject).GetIngredient()))
            {
                ((KitchenTool)newObject).DeleteIngredient(); // ������������ ��� ����
                SoundManager.Instance.PlaySE("PickUp");
                return false;
            }
            // ���� ���� ���� / newObject���� �ִ� ��ᰡ placedObject�� �ִ� ��� ���� �ö� �� ������ (������ �����ǿ� ��������� ���� �Ǵ�)
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
