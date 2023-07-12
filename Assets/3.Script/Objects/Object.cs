using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{

    public void Awake()
    {
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public abstract void ThrowToBin();

    public abstract void Burn();
}