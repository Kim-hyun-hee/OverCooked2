using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : Table
{
    public PlateReturnTable plateReturnTable;

    private void Start()
    {
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = this.transform.Find("FX_Fire_Big_01").GetComponent<Fire>();
        plateReturnTable = FindObjectOfType<PlateReturnTable>();
    }

    override public bool PutObject(Object newObject)
    {
        //if (fire.IsPlaying())
        //{
        //    return false;
        //}

        if (newObject is Plate && ((Plate)newObject).IsRecipe())
        {
            OrderManager.Instance.AddRecipe(((Plate)newObject).GetRecipe());
            //((Plate)newObject).ThrowToBin();
            //Destroy(((Plate)newObject).gameObject);
            //StartCoroutine(AddDirtyPlate_co());
            //audioManager.Play("Delivery");
            //money.Play();
            //return true;
        }
        else if(newObject is Plate && !((Plate)newObject).IsRecipe())
        {
            Debug.Log("틀림");
            // 콤보 초기화 해 줘야함 아마도?
        }
        else if (!(newObject is Plate))
        {
            Debug.Log("접시 필요!");
            return false;
        }

        ((Plate)newObject).ThrowToBin();
        Destroy(((Plate)newObject).gameObject);
        StartCoroutine(AddDirtyPlate_co());
        return true;

    }

    private IEnumerator AddDirtyPlate_co()
    {
        float remainingTime = 10f;
        while(true)
        {
            remainingTime -= Time.deltaTime;
            if(remainingTime <= 0)
            {
                break;
            }
            yield return null;
        }
        plateReturnTable.AddDirtyPlate();
    }
}
