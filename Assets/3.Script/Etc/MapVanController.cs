using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MapVanController : MonoBehaviour
{
    private Rigidbody vanRb;
    private Vector3 dir = Vector3.zero;
    private float moveSpeed;
    private float defaultSpeed = 4f;
    [SerializeField] private float dashForce = 8f;
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
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");

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

    private void FixedUpdate()
    {
        dir.Normalize();
        if(dir.magnitude > 0)
        {
            if ((Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x)) || (Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z)))
            {
                transform.Rotate(0, 1, 0);
            }
            vanRb.velocity = dir * moveSpeed;
        }
        else
        {
            vanRb.velocity = Vector3.zero;
        }
    }

    private Vector3 eulers(Vector3 vector3)
    {
        throw new NotImplementedException();
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
