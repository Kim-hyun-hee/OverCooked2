using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Table nearTable;
    private Object carriedObject;

    void OnTriggerStay(Collider collision)
    {
        // Debug.Log(collision);
        if (collision.gameObject.GetComponentInParent<Table>() != null)
        {
            Debug.Log(collision.gameObject.GetComponentInParent<Table>().name);
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

    private void OnTriggerExit(Collider other)
    {
        nearTable = null;
        // Renderer ���󺹱�
    }

}
