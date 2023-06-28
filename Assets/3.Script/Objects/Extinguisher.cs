using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : Object
{
    private ParticleSystem fire;

    public void Start()
    {
        //fire = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    override public void Burn() { }

    public void Activate()
    {
        Debug.Log("소화기 분사");
        //if (!fire.isPlaying)
        //{
        //    fire.Play();
        //}
    }

    public void Deactivate()
    {
        Debug.Log("소화기 분사 끝");
        //if (fire.isPlaying)
        //{
        //    fire.Stop();
        //}
    }

    override public void ThrowToBin() { }
}

