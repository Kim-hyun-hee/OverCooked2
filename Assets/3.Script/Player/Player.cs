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
            // 모서리일때 쳐다보는 방향에 있는 테이블 => nearTable
            // return;
            float dist = 10f;
            foreach (Collider col in Physics.OverlapSphere(transform.position, 1f))
            {
                if (col.gameObject.GetComponent<Table>() != null)
                {
                    // 거리 구하기
                    // 거리가 dist보다 작다면 nearTable = col.gameObject.GetComponent<Table>();
                    // nearTable이랑 nearTable의 placedObject의 Renderer emmision 변경
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nearTable = null;
        // Renderer 원상복구
    }

}
