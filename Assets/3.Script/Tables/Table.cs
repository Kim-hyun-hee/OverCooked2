using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Object placedObject;

    [SerializeField] private AudioSource fireSound;
    protected AudioManager audioManager;

    [SerializeField] protected Fire fire;

    protected float fireHealth = 120f;
    private float spreadFireCounter = 12f;

    private void Start()
    {
        // fireSound
        // audioManager
        // fire
        fireSound = GetComponent<AudioSource>();
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        fire.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        SpreadFire();
    }

    protected void SpreadFire()
    {
        // ���� ������
        // �� ������
    }

    protected virtual void ActivateFire()
    {
        fire.ActivateFire();
        fireSound.Play();

        if (placedObject != null)
        {
            if (!(this is IngredientTable))
            {
                placedObject.Burn();
            }
        }
    }

    public virtual void ExtinguisFire()
    {
        fire.ExtinguisFire();
        fireSound.Stop();
        fireHealth = 100;
        spreadFireCounter = 5;
    }

    public bool HasObject()
    {
        return placedObject != null;
    }

    public virtual Object GetObject()
    {
        Object objectToReturn = placedObject;
        placedObject = null;
        return objectToReturn;
    }

    public virtual bool PutObject(Object newObject) // ���̺� ���� newObject ��� ��ȣ�ۿ� Ű �������� ���̺� ���� newObject �δ���
    {
        if (fire.IsPlaying()) // ���� ������
        {
            return false;
        }

        if (placedObject == null)
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, -0.001f, 0.0f);
            return true;
        }
        else if (newObject is Ingredient) // ���� ������Ʈ�� ����϶�
        {
            if (placedObject is Plate) // ���� ���� �ø��� ���
            {
                return ((Plate)placedObject).AddIngredient((Ingredient)newObject); // ��� �ö� �� �ִ��� ������ (������ �����ǿ� ���� �Ǵ�)
            }
            else if (placedObject is KitchenTool) // �������� ���� �ø��� ���
            {
                return ((KitchenTool)placedObject).AddIngredient((Ingredient)newObject); // ��� �ö� �� �ִ��� ������ (������ ��������� ���� �Ǵ�)
            }
        }
        // ��� ���� �������� / ���� ���������� ¦���϶� (������ ��������� ���� �Ǵ�)
        else if (newObject is KitchenTool && placedObject is Ingredient && ((KitchenTool)newObject).AddIngredient(((Ingredient)placedObject)))
        {
            placedObject = null; // ���� ���������� ���ļ� �տ� �������
            return false;
        }
        // ���� ���� �������� / �������� �ȿ� �ִ� ��ᰡ ���� ���� �ö� �� ������ (������ �����ǿ� ���� �Ǵ�)
        else if (newObject is KitchenTool && placedObject is Plate && ((Plate)placedObject).AddIngredient(((KitchenTool)newObject).GetIngredient()))
        {
            ((KitchenTool)newObject).DeleteIngredient(); // ������������ ��� ����
            return false;
        }
        // ��� ���� ���� / ��ᰡ ���� ���� �ö� �� ������ (������ �����ǿ� ���� �Ǵ�)
        else if (newObject is Plate && placedObject is Ingredient && ((Plate)newObject).AddIngredient((Ingredient)placedObject))
        {
            placedObject = null;
            return false;
        }
        // �������� ���� ���� /�������� �ȿ� �ִ� ��ᰡ ���� ���� �ö� �� ������ (������ �����ǿ� ��������� ���� �Ǵ�)
        else if (newObject is Plate && placedObject is KitchenTool && ((Plate)newObject).AddIngredient(((KitchenTool)placedObject).GetIngredient()))
        {
            ((KitchenTool)placedObject).DeleteIngredient(); // �������� �ȿ� �ִ� ��� ����
            return false;
        }
        return false;
    }
}
