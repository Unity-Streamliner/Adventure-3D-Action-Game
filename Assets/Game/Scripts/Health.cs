using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    private CharacterController _characterController;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _characterController = GetComponent<CharacterController>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"{gameObject.name} took damage: {damage}, current health is: {CurrentHealth}");
    }
}
