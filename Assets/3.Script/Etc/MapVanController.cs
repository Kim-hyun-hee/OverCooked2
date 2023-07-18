using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MapVanController : MonoBehaviour
{
    private Rigidbody vanRb;

    private float inputX;
    private float inputY;

    private float moveSpeed;
    private float defaultSpeed = 4f;
    [SerializeField] private float dashForce = 10f;
    private Quaternion finalRotation;
    //private PlayerAnimationController vanAnimationController;

    private bool isPlay = false;
    private bool isDashReady = true;

    private ObjectPool objectPool;
    private Coroutine running;
    private Transform puffTransform;

    private void Start()
    {
        //vanAnimationController = FindObjectOfType<PlayerAnimationController>();
        TryGetComponent(out vanRb);
        moveSpeed = defaultSpeed;
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Move();
        Rotate();

        if (isDashReady)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Dash();
                StartCoroutine(StartCooldown_co());
            }
        }
    }

    private IEnumerator StartCooldown_co()
    {
        isDashReady = false;
        yield return new WaitForSeconds(0.2f);
        moveSpeed = defaultSpeed;
        isDashReady = true;
    }

    private void Dash()
    {
        moveSpeed = dashForce;
    }

    private void Move()
    {
        //_rigidbody.velocity = new Vector3(inputX, 0, inputY) * velocity;
        if (inputX != 0 || inputY != 0)
        {
            vanRb.velocity = transform.forward * moveSpeed;
            return;
        }
        vanRb.velocity = Vector3.zero;
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
            Vector3 direction = new Vector3(inputX, 0, inputY) * moveSpeed;
            lookdirection = Quaternion.LookRotation(direction);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookdirection, 0.025f);
    }


    //public void PlayPuff()
    //{
    //    running = StartCoroutine(Puff_co());
    //}

    //public void StopPuff()
    //{
    //    StopCoroutine(running);
    //    isPlay = false;
    //}

    //public bool IsPlay()
    //{
    //    return isPlay;
    //}

    //private IEnumerator Puff_co()
    //{
    //    isPlay = true;
    //    WaitForSeconds wfs = new WaitForSeconds(0.1f);
    //    while (true)
    //    {
    //        GameObject puff = objectPool.GetObject();
    //        puff.transform.position = puffTransform.position;
    //        yield return wfs;
    //    }
    //}
}
