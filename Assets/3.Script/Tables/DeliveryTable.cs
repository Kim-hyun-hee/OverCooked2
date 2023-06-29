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
            Plate plate = (Plate)newObject;
            //OrderManager.Instance.AddCompletedRecipe(plate.GetRecipe());
            plate.ThrowToBin();
            Destroy(plate.gameObject);
            plateReturnTable.AddPlate();
            //audioManager.Play("Delivery");
            //money.Play();
            return true;
        }
        else
        {
            return false;
        }

    }
}
