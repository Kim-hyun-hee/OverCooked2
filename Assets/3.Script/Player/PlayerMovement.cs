using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 dir = Vector3.zero;
    private float moveSpeed = 4f;
    private PlayerAnimationController ani;
    private Puff puff;

    private void Start()
    {
        ani = FindObjectOfType<PlayerAnimationController>();
        TryGetComponent(out playerRb);
        puff = transform.Find("puff").GetComponent<Puff>();
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
            if(puff.isStopped)
            {
                puff.Play();
            }
        }
        else
        {
            // Idle animation
            ani.Idle();
            if(puff.isPlaying)
            {
                puff.Stop();
            }
        }
    }
}