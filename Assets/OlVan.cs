using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlVan : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float inputX;
    private float inputY;
    private float velocity = 10f;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Move();
        Rotate();
    }

    private void Move()
    {
        //_rigidbody.velocity = new Vector3(inputX, 0, inputY) * velocity;
        if(inputX != 0 || inputY != 0)
        {
            _rigidbody.velocity = transform.forward * velocity;
            return;
        }
        _rigidbody.velocity = Vector3.zero;
    }

    private void Rotate()
    {
        Quaternion lookdirection;
        if (inputX == 0 && inputY == 0)
        {
            lookdirection = transform.rotation;
        }
        else
        {
            Vector3 direction = new Vector3(inputX, 0, inputY) * velocity;
            lookdirection = Quaternion.LookRotation(direction);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookdirection, 0.025f);
    }

}
