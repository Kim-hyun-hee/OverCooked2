using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    [SerializeField] private Object carriedObject;

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Table>() != null)
        {
            // TODO: 하나로 특정 나중에 해주기
            nearTable = collision.gameObject.GetComponentInParent<Table>();
            if (nearTable.GetComponentInParent<IngredientTable>() == null)
            {
                nearTable.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower", 1);
            }
            else
            {
                nearTable.transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].SetFloat("_EmissionPower", 1);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Table>() != null)
        {
            if (collision.gameObject.GetComponentInParent<IngredientTable>() == null)
            {
                collision.gameObject.GetComponentInParent<Table>().transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower", 0);
            }
            else
            {
                collision.gameObject.GetComponentInParent<Table>().transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].SetFloat("_EmissionPower", 0);
            }
            nearTable = null;
        }
    }
}
