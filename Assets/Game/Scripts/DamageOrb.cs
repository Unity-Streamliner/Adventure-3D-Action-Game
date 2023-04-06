using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public float Speed = 2f;
    public int Damage = 10;
    public ParticleSystem HitVFX;
    private Rigidbody _rigidbody;

    private void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && character.IsPlayer)
        {
            character.ApplyDamage(Damage, transform.position);
        }
        Instantiate(HitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
