using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    private Table preTable;
    [SerializeField] private Object carriedObject;

    private void Update()
    {
        GetNearTable();
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

    private void GetNearTable()
    {
        if (nearTable != preTable) // nearTable이 갱신됐을때
        {
            if (preTable != null) // preTable이 null이 아니라면
            {
                if (preTable.GetComponentInParent<IngredientTable>() == null) // preTable 꺼주기
                {
                    preTable.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower", 0);
                }
                else
                {
                    preTable.transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].SetFloat("_EmissionPower", 0);
                }
            }
            preTable = nearTable;
        }

        bool isTable = false;
        foreach (RaycastHit hit in Physics.RaycastAll(transform.position, transform.forward, 1f))
        {
            if (hit.collider.gameObject.GetComponentInParent<Table>() != null)
            {
                nearTable = hit.collider.gameObject.GetComponentInParent<Table>();
                isTable = true;
                break;
            }
        }

        if(!isTable)
        {
            foreach (RaycastHit hit in Physics.RaycastAll(transform.position, transform.right, 1f))
            {
                if (hit.collider.gameObject.GetComponentInParent<Table>() != null)
                {
                    nearTable = hit.collider.gameObject.GetComponentInParent<Table>();
                    isTable = true;
                    break;
                }
            }
        }

        if (!isTable)
        {
            foreach (RaycastHit hit in Physics.RaycastAll(transform.position, -transform.right, 1f))
            {
                if (hit.collider.gameObject.GetComponentInParent<Table>() != null)
                {
                    nearTable = hit.collider.gameObject.GetComponentInParent<Table>();
                    isTable = true;
                    break;
                }
            }
        }

        if (!isTable)
        {
            nearTable = null;
        }

        if (nearTable != null)
        {
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
}
