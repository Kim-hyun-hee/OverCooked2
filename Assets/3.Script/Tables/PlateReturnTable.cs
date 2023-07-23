using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturnTable : Table
{
    public GameObject dirtyPlate;

    [SerializeField] private Stack<Object> dirtyPlates = new Stack<Object>();

    private void Start()
    {
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
    }

    public override bool PutObject(Object newObject)
    {
        if (dirtyPlates.Count > 0 && !((Plate)placedObject).isDirty)
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
        Object objectToReturn = dirtyPlates.Pop();
        if(dirtyPlates.Count == 0)
        {
            placedObject = null;
        }
        return objectToReturn;
    }

    public void AddDirtyPlate()
    {
        // 10��?
        placedObject = Instantiate(dirtyPlate.GetComponent<Object>());
        placedObject.transform.SetParent(transform.GetChild(1));
        placedObject.transform.localPosition = new Vector3(0f, 0.005f + (dirtyPlates.Count * 0.001f), 0f);
        SoundManager.Instance.PlaySE("WashedPlate");
        if(StageManager.Instance.stageName == StageName.S1_3)
        {
            ((Plate)placedObject).isDirty = true;
        }
        dirtyPlates.Push(placedObject);
    }
}
