using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Table nearTable;
    [SerializeField] private Object nearObject;
    private CutTable cuttingTable;
    private WashTable washingTable;
    private Table preTable;
    [SerializeField] private float throwForce;

    [SerializeField] private Transform floor;

    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private Object carriedObject;

    public bool isMove;
    [SerializeField] private Image indicator;

    private void Start()
    {
        throwForce = 8;
        //PlayerControlOn();
        StageManager.Instance.EndStage += PlayerControlOff;
    }

    public void PlayerControlOff()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Player>().enabled = false;
    }

    public void PlayerControlOn()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Player>().enabled = true;
    }

    public void PlayerMovementOff()
    {
        GetComponent<PlayerMovement>().enabled = false;
        indicator.gameObject.SetActive(false);
    }

    public void PlayerMovementOn()
    {
        GetComponent<PlayerMovement>().enabled = true;
        indicator.gameObject.SetActive(true);
    }

    private void Update()
    {
        //GetNearTable();
        GetNearObject();
        TableInteraction();
        CutIngredient();
        WashDish();
        UseExtinguisher();
        ThrowIngredient();

        if (carriedObject == null)
        {
            playerAnimationController.StopCarry();
        }
        else
        {
            playerAnimationController.Carry();
        }
    }

    private void PutObjectOnFloor()
    {
        if(carriedObject is Ingredient)
        {
            carriedObject.GetComponentInChildren<MeshCollider>().enabled = true;
            carriedObject.transform.SetParent(floor);
            carriedObject.gameObject.AddComponent<Rigidbody>();
            carriedObject = null;
        }
    }

    private void GetObjectFromFloor()
    {
        carriedObject = nearObject;
        carriedObject.GetComponentInChildren<MeshCollider>().enabled = false;
        Rigidbody rig = carriedObject.GetComponent<Rigidbody>();
        Destroy(rig);
        carriedObject.transform.SetParent(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1));
        carriedObject.transform.localPosition = new Vector3(0.0f, 0.0058f, 0.006f);
        carriedObject.transform.localRotation = Quaternion.identity;
    }

    private void GetNearObject()
    {
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position, 0.4f, transform.forward, 0.4f))
        {
            if (hit.collider.gameObject.GetComponentInParent<Object>() != null)
            {
                nearObject = hit.collider.gameObject.GetComponentInParent<Object>();
                Debug.DrawRay(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1).position, transform.forward * 10f, Color.red, Time.deltaTime);
                break;
            }
            else
            {
                nearObject = null;
                GetNearTable();
            }
        }
    }

    private void ThrowIngredient()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && carriedObject != null && carriedObject is Ingredient && isMove)
        {
            carriedObject.GetComponentInChildren<MeshCollider>().enabled = true;
            carriedObject.transform.SetParent(floor);
            carriedObject.gameObject.AddComponent<Rigidbody>();
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).forward * throwForce, ForceMode.Impulse);
            SoundManager.Instance.PlaySE("Throw");
            carriedObject = null;
        }
    }

    private void TableInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nearTable != null && isMove)
        {
            if (carriedObject == null && nearObject == null)
            {
                GetObjectFromTable();
            }
            else if (carriedObject == null && nearObject != null)
            {
                GetObjectFromTable();
                if(carriedObject == null && nearObject != null)
                {
                    GetObjectFromFloor();
                }
            }
            else
            {
                PutObjectOnTable();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && nearTable == null && isMove)
        {
            if (carriedObject == null && nearObject != null)
            {
                GetObjectFromFloor();
            }
            else if (carriedObject != null)
            {
                PutObjectOnFloor();
            }
        }
    }

    private void GetObjectFromTable()
    {
        Object placedObject = nearTable.GetObject();
        if (placedObject != null)
        {
            carriedObject = placedObject;
            carriedObject.GetComponentInChildren<MeshCollider>().enabled = false;
            carriedObject.transform.SetParent(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1));
            carriedObject.transform.localPosition = new Vector3(0.0f, 0.0058f, 0.006f);

            if (placedObject is Extinguisher)
            {
                placedObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            SoundManager.Instance.PlaySE("PickUp");
        }
    }

    private void PutObjectOnTable()
    {
        if (nearTable.PutObject(carriedObject))
        {
            SoundManager.Instance.PlaySE("PutDown");
            carriedObject.GetComponentInChildren<MeshCollider>().enabled = true;
            carriedObject = null;
        }
    }

    private void GetNearTable()
    {
        if (nearTable != preTable) // nearTable이 갱신됐을때
        {
            if (preTable != null) // preTable이 null이 아니라면
            {
                if (preTable is WashTable || preTable is DryTable)
                {
                    preTable = preTable.transform.parent.GetComponent<Table>();
                }

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
        if(!IsCutting() && Input.GetKey(KeyCode.LeftControl) && nearTable is CutTable && ((CutTable)nearTable).HasCuttableObject() && isMove) // 처음 자를때
        {
            cuttingTable = (CutTable)nearTable;
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true); // 플레이어가 들고 있는 knife 활성화
            playerAnimationController.Cut();
            cuttingTable.StartCut();
        }
        else if(IsCutting() && (!(nearTable is CutTable) || !((CutTable)nearTable).HasCuttableObject())) // 자르다가 멈출때
        {
            playerAnimationController.StopCutting();
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(false);
            cuttingTable.StopCut();
            cuttingTable = null;
        }
    }

    private void WashDish()
    {
        if(!IsWashing() && Input.GetKey(KeyCode.LeftControl) && nearTable is WashTable && ((WashTable)nearTable).HasWashableObject() && isMove)
        {
            washingTable = (WashTable)nearTable;
            washingTable.StartWash();
            playerAnimationController.Wash();
            // 애니메이션
            // Wash();
        }
        else if(IsWashing() && (!(nearTable is WashTable) || !((WashTable)nearTable).HasWashableObject())) // HasWashableObject()가 return (dirtyPlates.Count != 0); 이라서 문제임
        {
            washingTable.StopWash();
            playerAnimationController.StopWash();
            washingTable = null;
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
            if (Input.GetKey(KeyCode.LeftControl) && isMove)
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

    public void ChopSound()
    {
        SoundManager.Instance.PlaySE("KnifeChop");
    }

    public void WashSound()
    {
        SoundManager.Instance.PlaySE("Washing");
    }
}
