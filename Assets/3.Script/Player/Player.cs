using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    [SerializeField] private Object carriedObject;

    private void Update()
    {
        TableInteraction();
    }

    private void TableInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nearTable != null)
        {
            if (carriedObject == null)
            {
                GetObjectFromTable();
            }
            else
            {
                PutObjectOnTable();
            }
        }
    }

    private void GetObjectFromTable()
    {
        Object placedObject = nearTable.GetObject();
        if (placedObject != null)
        {
            carriedObject = placedObject;
            carriedObject.transform.SetParent(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1));
            carriedObject.transform.localPosition = new Vector3(0.0f, 0.008f, 0.006f);
        }
    }

    private void PutObjectOnTable()
    {
        if (nearTable.PutObject(carriedObject))
        {
            carriedObject = null;
        }
    }

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
