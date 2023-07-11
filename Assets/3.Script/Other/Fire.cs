using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private ParticleSystem fireParticles;
    private float fireHealth = 120f;

    void Awake()
    {
        fireParticles = GetComponent<ParticleSystem>();
    }

    public virtual void ActivateFire()
    {
        fireParticles.Play();
    }

    public virtual void ExtinguisFire()
    {
        fireParticles.Stop();
        fireHealth = 100;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Equals("FX_Steam_01"))
        {
            if (fireParticles.isPlaying)
            {
                fireHealth -= Time.deltaTime * 60;
                if (fireHealth <= 0)
                {
                    ExtinguisFire();
                    transform.parent.GetComponent<Table>().ExtinguisFire();
                }
            }
        }
    }

    public bool IsPlaying()
    {
        return fireParticles.isPlaying;
    }
}

