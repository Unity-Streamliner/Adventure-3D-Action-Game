using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public ParticleSystem Blade01;
    public VisualEffect Slash;
    public VisualEffect Heal;


    public void UpdateFootStep(bool state)
    {
        if (state) 
        {
            footStep.Play();
        }
        else
        {
            footStep.Stop();
        }
    }

    public void PlayBlade01()
    {
        Blade01.Play();
    }

    public void PlaySlash(Vector3 position)
    {
        Slash.transform.position = position;
        Slash.Play();
    }

    public void PlayHealVFX()
    {
        Heal.Play();
    }
}
