using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect AttackVFX;
    public ParticleSystem BeingHitVFX;
    public VisualEffect BeingHitSplashVFX;

    private float BeingHitSplashVFXDuration = 10f;

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

        Vector3 splashPosition = transform.position;
        splashPosition.y += 2f;
        VisualEffect newSplashVFX = Instantiate(BeingHitSplashVFX, splashPosition, Quaternion.identity);
        newSplashVFX.Play();
        Destroy(newSplashVFX, BeingHitSplashVFXDuration);
    }
}
