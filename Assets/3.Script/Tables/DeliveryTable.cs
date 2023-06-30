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
            OrderManager.Instance.AddCompletedRecipe(((Plate)newObject).GetRecipe());
            ((Plate)newObject).ThrowToBin();
            Destroy(((Plate)newObject).gameObject);
            plateReturnTable.AddDirtyPlate();
            //audioManager.Play("Delivery");
            //money.Play();
            return true;
        }
        else
        {
            // 접시가 필요하다! 
            return false;
        }

    }
}
