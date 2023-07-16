using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject van;
    private void Update()
    {
        transform.position = new Vector3(van.transform.position.x, van.transform.position.y + 10f, van.transform.position.z - 5);
    }
}
