using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : Object
{
    private ParticleSystem fire;

    public void Start()
    {
        fire = transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    override public void Burn() { }

    public void Activate()
    {
        if (!fire.isPlaying)
        {
            fire.Play();
        }
    }

    public void Deactivate()
    {
        if (fire.isPlaying)
        {
            fire.Stop();
        }
    }

    override public void ThrowToBin() { }
}

