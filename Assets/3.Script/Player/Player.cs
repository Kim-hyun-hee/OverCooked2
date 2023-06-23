using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    private Object carriedObject;

    void OnTriggerStay(Collider collision)
    {
        // Debug.Log(collision);
        if (collision.gameObject.GetComponentInParent<Table>() != null)
        {
            nearTable = collision.gameObject.GetComponentInParent<Table>();
            if (nearTable.GetComponentInParent<IngredientTable>() == null)
            {
                nearTable.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower", 1);
            }
            else
            {
                nearTable.transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].SetFloat("_EmissionPower", 1);
            }

            // �𼭸��϶� �Ĵٺ��� ���⿡ �ִ� ���̺� => nearTable
            // return;
            float dist = 10f;
            foreach (Collider col in Physics.OverlapSphere(transform.position, 1f))
            {
                if (col.gameObject.GetComponent<Table>() != null)
                {
                    // �Ÿ� ���ϱ�
                    // �Ÿ��� dist���� �۴ٸ� nearTable = col.gameObject.GetComponent<Table>();
                    // nearTable�̶� nearTable�� placedObject�� Renderer emmision ����
                }
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
        // Renderer ���󺹱�
    }

}
