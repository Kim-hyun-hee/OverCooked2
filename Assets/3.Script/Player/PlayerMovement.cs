using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 dir = Vector3.zero;
    private float moveSpeed = 4f;
    private PlayerAnimationController ani;

    private bool isPlay = false;
    private ObjectPool objectPool;
    private Coroutine running;
    private Transform puffPos;

    private void Start()
    {
        ani = FindObjectOfType<PlayerAnimationController>();
        TryGetComponent(out playerRb);
        objectPool = FindObjectOfType<ObjectPool>();
        puffPos = GameObject.FindGameObjectWithTag("PuffPos").transform;
    }

    private void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        dir.Normalize();
        playerRb.velocity = dir * moveSpeed;
        transform.LookAt(transform.position + dir, Vector3.up);

        if (dir.magnitude > 0)
        {
            // Walk or Run animation
            ani.Walk();
            if(!isPlay)
            {
                PlayPuff();
            }
        }
        else
        {
            // Idle animation
            ani.Idle();
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
            puff.transform.position = transform.position + new Vector3(0, 0.3f, 0);
            yield return wfs;
        }
    }
}