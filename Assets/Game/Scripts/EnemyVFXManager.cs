using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect AttackVFX;
    public ParticleSystem BeingHitVFX; 

    public void BurstFootStep()
    {
        FootStep.Play();
    }

    public void PlayAttackVFX()
    {
        AttackVFX.Play();
    }

    public void PlayBeingHitVFX(Vector3 attackerPosition)
    {
        Vector3 forceForward = transform.position - attackerPosition;
        forceForward.Normalize();
        forceForward.y = 0;
        BeingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
        BeingHitVFX.Play();
    }
}
