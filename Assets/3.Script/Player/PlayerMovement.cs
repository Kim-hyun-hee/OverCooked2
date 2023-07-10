using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 dir = Vector3.zero;
    private float moveSpeed = 10f;
    private PlayerAnimationController playerAnimationController;

    private bool isPlay = false;
    private ObjectPool objectPool;
    private Coroutine running;
    private Transform puffTransform;

    private void Start()
    {
        playerAnimationController = FindObjectOfType<PlayerAnimationController>();
        TryGetComponent(out playerRb);
        objectPool = FindObjectOfType<ObjectPool>();
        puffTransform = GameObject.FindGameObjectWithTag("PuffPos").transform;
    }

    private void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
    }

    //void OnMove(InputValue value)
    //{
    //    Vector2 input = value.Get<Vector2>();
    //    dir = new Vector3(input.x, 0f, input.y);
    //}

    private void FixedUpdate()
    {
        dir.Normalize();
        playerRb.velocity = dir * moveSpeed;
        transform.LookAt(transform.position + dir, Vector3.up);

        if (dir.magnitude > 0)
        {
            // Walk or Run animation
            playerAnimationController.Walk();
            if(!isPlay)
            {
                PlayPuff();
            }
        }
        else
        {
            // Idle animation
            playerAnimationController.Idle();
            if(isPlay)
            {
                StopPuff();
            }
        }
    }

    public void PlayPuff() // puff DeQueue
    {
        running = StartCoroutine(Puff_co());
    }

    public void StopPuff()
    {
        StopCoroutine(running);
        isPlay = false;
    }

    public bool IsPlay()
    {
        return isPlay;
    }

    private IEnumerator Puff_co()
    {
        isPlay = true;
        WaitForSeconds wfs = new WaitForSeconds(0.15f);
        while (true)
        {
            GameObject puff = objectPool.GetObject();
            puff.transform.position = puffTransform.position;
            yield return wfs;
        }
    }
}