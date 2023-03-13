using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    public int Damage = 30;
    public string TargetTag;

    private Collider _damageCasterCollider;
    private List<Collider> _damageTargetList;

    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damageTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TargetTag && !_damageTargetList.Contains(other))
        {
            Character target = other.GetComponent<Character>();
            target?.ApplyDamage(Damage);
            PlayDamageVFX();

            _damageTargetList.Add(other);
        }
    }

    private void PlayDamageVFX()
    {
        RaycastHit hit;
        Vector3 originalPosition = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;
        bool isHit = Physics.BoxCast(originalPosition, _damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1<<6);
        if (isHit)
        {
            PlayerVFXManager playerVFXManager = transform.parent.GetComponent<PlayerVFXManager>();
            playerVFXManager?.PlaySlash(hit.point + new Vector3(0, 0.5f, 0));
        }
    }

    public void EnableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }

    public void OnDrawGizmos()
    {
        if (_damageCasterCollider == null)
        {
            _damageCasterCollider = GetComponent<Collider>();
        }
        RaycastHit hit;
        Vector3 originalPosition = transform.position + (-_damageCasterCollider.bounds.extents.z) * transform.forward;
        bool isHit = Physics.BoxCast(originalPosition, _damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, _damageCasterCollider.bounds.extents.z, 1<<6);
        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
}
