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
            fire.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void Deactivate()
    {
        if (fire.isPlaying)
        {
            fire.Stop();
            fire.GetComponent<BoxCollider>().enabled = false;
        }
    }

    override public void ThrowToBin() { }
}

