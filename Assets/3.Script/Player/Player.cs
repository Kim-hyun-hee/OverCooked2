using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    private CutTable cuttingTable;
    private WashTable washingTable;
    private Table preTable;

    private PlayerAnimationController playerAnimationController;
    [SerializeField] private Object carriedObject;

    private void Start()
    {
        playerAnimationController = FindObjectOfType<PlayerAnimationController>();
    }

    private void Update()
    {
        GetNearTable();
        TableInteraction();
        CutIngredient();
        WashDish();
        UseExtinguisher();

        if (carriedObject == null)
        {
            //playerAnimationController.StopCarry();
        }
        else
        {
            //playerAnimationController.Carry();
        }
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

            if(placedObject is Extinguisher)
            {
                placedObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
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
        if (nearTable != preTable) // nearTable�� ���ŵ�����
        {
            if (preTable != null) // preTable�� null�� �ƴ϶��
            {
                if (preTable is WashTable || preTable is DryTable)
                {
                    preTable = preTable.transform.parent.GetComponent<Table>();
                }

                if (preTable.GetComponentInParent<IngredientTable>() == null) // preTable ���ֱ�
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

        if(nearTable is SinkTable)
        {
            Transform washTable = ((SinkTable)nearTable).transform.GetChild(3);
            Transform dryTable = ((SinkTable)nearTable).transform.GetChild(2);
            
            if(Vector3.SqrMagnitude(transform.position - washTable.position) >= Vector3.SqrMagnitude(transform.position - dryTable.position))
            {
                nearTable = dryTable.gameObject.GetComponent<Table>();
            }
            else
            {
                nearTable = washTable.gameObject.GetComponent<Table>();
            }
        }
    }

    private void CutIngredient()
    {
        if(!IsCutting() && Input.GetKey(KeyCode.LeftControl) && nearTable is CutTable && ((CutTable)nearTable).HasCuttableObject()) // ó�� �ڸ���
        {
            cuttingTable = (CutTable)nearTable;
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true); // �÷��̾ ��� �ִ� knife Ȱ��ȭ
            playerAnimationController.Cut();
            //cuttingTable.SetKnifeState(false);
            cuttingTable.Cut();
        }
        else if(IsCutting() && (!Input.GetKey(KeyCode.LeftControl) || !(nearTable is CutTable) || !((CutTable)nearTable).HasCuttableObject())) // �ڸ��ٰ� ���⶧
        {
            playerAnimationController.StopCutting();
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(false);
            //cuttingTable.SetKnifeState(true);
            cuttingTable = null;
        }
        else if (IsCutting())
        {
            cuttingTable.Cut();
        }
    }

    private void WashDish()
    {
        if(!IsWashing() && Input.GetKey(KeyCode.LeftControl) && nearTable is WashTable && ((WashTable)nearTable).HasWashableObject())
        {
            washingTable = (WashTable)nearTable;
            // �ִϸ��̼�
            // Wash();
        }
    }

    private bool IsWashing()
    {
        return washingTable != null;
    }

    private bool IsCutting()
    {
        return cuttingTable != null;
    }

    private void UseExtinguisher()
    {
        if (carriedObject != null && carriedObject is Extinguisher)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //if (!audioManager.IsPlaying("Fire Extinguisher"))
                //    audioManager.Play("Fire Extinguisher");

                ((Extinguisher)carriedObject).Activate();
            }
            else
            {
                ((Extinguisher)carriedObject).Deactivate();
                //audioManager.Stop("Fire Extinguisher");
            }
        }
    }
}
