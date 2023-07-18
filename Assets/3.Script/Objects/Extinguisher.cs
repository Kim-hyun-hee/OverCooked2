using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : Object
{
    private ParticleSystem fire;
    private AudioSource audioSource;
    private bool isSound = false;

    public void Start()
    {
        fire = transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>();
        TryGetComponent(out audioSource);
        StageManager.Instance.EndStage += SoundOff;
    }

    override public void Burn() { }

    private void SoundOff()
    {
        audioSource.Stop();
    }

    public void Activate()
    {
        if (!fire.isPlaying)
        {
            fire.Play();
            if(!isSound)
            {
                audioSource.Play();
                isSound = false;
            }
            fire.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void Deactivate()
    {
        if (fire.isPlaying)
        {
            fire.Stop();
            audioSource.Stop();
            fire.GetComponent<BoxCollider>().enabled = false;
        }
    }

    override public void ThrowToBin() { }
}

