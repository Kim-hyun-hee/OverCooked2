using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Object placedObject;

    [SerializeField] private AudioSource fireSound;
    [SerializeField] protected List<GameObject> nearTables = new List<GameObject>();

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
        //fire.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        SpreadFire();
    }

    protected void SpreadFire()
    {
        if(fire.IsPlaying())
        {
            spreadFireCounter -= Time.deltaTime;
            if(spreadFireCounter <= 0)
            {
                for(int i = 0; i < nearTables.Count; i++)
                {
                    nearTables[i].GetComponent<Table>().ActivateFire();
                    nearTables[i].GetComponent<Table>().spreadFireCounter = 10;
                }
                spreadFireCounter = 10;
            }
        }
    }

    protected virtual void ActivateFire()
    {
        fire.ActivateFire();
        //fireSound.Play();

        //if (placedObject != null)
        //{
        //    if (!(this is IngredientTable))
        //    {
        //        placedObject.Burn();
        //    }
        //}
    }

    public virtual void ExtinguisFire()
    {
        fire.ExtinguisFire();
        //fireSound.Stop();
        // 탄 연기 여기서 테이블 위에 오브젝트(조리도구) 안에 재료(overcooked)일때 play
        if(HasObject() && placedObject is KitchenTool && ((KitchenTool)placedObject).HasIngredient())
        {
            ((KitchenTool)placedObject).PlayBurntSmoke();
        }
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

    public virtual bool PutObject(Object newObject) // 테이블 위에 newObject 들고 상호작용 키 눌렀을때 테이블 위에 newObject 두는지
    {
        //if (fire.IsPlaying()) // 불이 났으면
        //{
        //    return false;
        //}

        if (placedObject == null)
        {
            placedObject = newObject;
            placedObject.transform.SetParent(transform.GetChild(1));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.005f, 0.0f); // cutTable에서만 위치 이상함;
            return true;
        }
        else if (newObject is Ingredient) // 놓을 오브젝트가 재료일때
        {
            if (placedObject is Plate) // 접시 위에 올리는 경우
            {
                return ((Plate)placedObject).AddIngredient((Ingredient)newObject); // 재료 올라갈 수 있는지 없는지 (정해진 레시피에 따라 판단)
            }
            else if (placedObject is KitchenTool) // 조리도구 위에 올리는 경우
            {
                return ((KitchenTool)placedObject).AddIngredient((Ingredient)newObject); // 재료 올라갈 수 있는지 없는지 (정해진 조리방법에 따라 판단)
            }
        }
        // 재료 위에 조리도구 / 재료랑 조리도구랑 짝꿍일때 (정해진 조리방법에 따라 판단)
        else if (newObject is KitchenTool && placedObject is Ingredient && ((KitchenTool)newObject).AddIngredient(((Ingredient)placedObject)))
        {
            placedObject = null; // 재료랑 조리도구랑 합쳐서 손에 들고있음
            SoundManager.Instance.PlaySE("PickUp");
            return false;
        }
        // 접시 위에 조리도구 / 조리도구 안에 있는 재료가 접시 위에 올라갈 수 있을때 (정해진 레시피에 따라 판단)
        else if (newObject is KitchenTool && placedObject is Plate && ((Plate)placedObject).AddIngredient(((KitchenTool)newObject).GetIngredient()))
        {
            ((KitchenTool)newObject).DeleteIngredient(); // 조리도구에서 재료 빼기
            SoundManager.Instance.PlaySE("PickUp");
            return false;
        }
        // 재료 위에 접시 / 재료가 접시 위에 올라갈 수 있을때 (정해진 레시피에 따라 판단)
        else if (newObject is Plate && placedObject is Ingredient && ((Plate)newObject).AddIngredient((Ingredient)placedObject))
        {
            placedObject = null;
            SoundManager.Instance.PlaySE("PickUp");
            return false;
        }
        // 조리도구 위에 접시 / 조리도구 안에 있는 재료가 접시 위에 올라갈 수 있을때 (정해진 레시피와 조리방법에 따라 판단)
        else if (newObject is Plate && placedObject is KitchenTool && ((Plate)newObject).AddIngredient(((KitchenTool)placedObject).GetIngredient()))
        {
            ((KitchenTool)placedObject).DeleteIngredient(); // 조리도구 안에 있는 재료 빼기
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

        return false;
    }
}
