using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 dir = Vector3.zero;
    private float moveSpeed;
    private float defaultSpeed = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    private Quaternion finalRotation;

    private bool isPlay = false;
    private bool isDashReady = true;
    
    [SerializeField] private ObjectPool objectPool;
    private Coroutine running;
    [SerializeField] private Transform puffTransform;

    private void Start()
    {
        TryGetComponent(out playerRb);
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
        yield return new WaitForSeconds(0.8f);
        isDashReady = true;
    }

    private void Dash()
    {
        //playerRb.DOMove(transform.position + transform.forward * dashForce, 0.5f);
        SoundManager.Instance.PlaySE("Dash");
        moveSpeed = dashForce;
    }

    private void FixedUpdate()
    {
        dir.Normalize();
        playerRb.velocity = dir * moveSpeed;
        if(dir != Vector3.zero)
        {
            if ((Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x)) || (Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z)))
            {
                transform.Rotate(0, 1, 0);
            }
        }
        //transform.LookAt(transform.position + dir, Vector3.up);
        finalRotation = Quaternion.LookRotation(transform.forward + dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * 30);

        if (dir.magnitude > 0)
        {
            playerAnimationController.Walk();
            if(!isPlay)
            {
                PlayPuff();
            }
        }
        else
        {
            playerAnimationController.Idle();
            if(isPlay)
            {
                StopPuff();
            }
        }
    }

    public void PlayPuff()
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
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        while (true)
        {
            GameObject puff = objectPool.GetObject();
            puff.transform.position = puffTransform.position;
            yield return wfs;
        }
    }
}