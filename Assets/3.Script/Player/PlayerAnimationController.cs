using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        TryGetComponent(out animator);
    }

    public void Walk()
    {
        animator.SetBool("Walk", true);
    }

    public void Idle()
    {
        animator.SetBool("Walk", false);
    }

    public void Carry()
    {
        animator.SetBool("Carry", true);
    }

    public void StopCarry()
    {
        animator.SetBool("Carry", false);
    }

    public void Cut()
    {
        animator.SetBool("Cut", true);
    }

    public void StopCutting()
    {
        animator.SetBool("Cut", false);
    }

    public void Wash()
    {
        animator.SetBool("Wash", true);
    }

    public void StopWash()
    {
        animator.SetBool("Wash", false);
    }

}

