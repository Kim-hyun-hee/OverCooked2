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
    [SerializeField] private float dashForce = 11f;
    private Quaternion finalRotation;

    public  AudioSource audioSource0;
    public  AudioSource audioSource1;

    [SerializeField] private AudioClip vanIn;
    [SerializeField] private AudioClip vanEngine;
    [SerializeField] private AudioClip vanOut;

    private bool isPlay = false;
    private bool isDashReady = true;

    private ObjectPool objectPool;
    private Coroutine running;
    [SerializeField] private Transform puffTransform;

    private void Start()
    {
        //vanAnimationController = FindObjectOfType<PlayerAnimationController>();
        TryGetComponent(out vanRb);
        objectPool = FindObjectOfType<ObjectPool>();
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
                audioSource0.clip = vanIn;
                audioSource0.Play();
                Dash();
                isDashReady = false;
                StartCoroutine(StartCooldown_co());
            }
        }

        if (vanRb.velocity != Vector3.zero)
        {
            if (!isPlay)
            {
                PlayPuff();
            }
        }
        else
        {
            if (isPlay)
            {
                StopPuff();
            }
        }
    }

    private IEnumerator StartCooldown_co()
    {
        yield return new WaitForSeconds(0.35f);
        moveSpeed = defaultSpeed;
        yield return new WaitForSeconds(0.65f);
        isDashReady = true;
    }

    private void Dash()
    {
        moveSpeed = dashForce;
        StartCoroutine(VanAudio_co());
    }

    private IEnumerator VanAudio_co()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource0.clip = vanOut;
        audioSource0.Play();
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
