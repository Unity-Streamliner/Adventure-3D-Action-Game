using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    private Character _character;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _character = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"{gameObject.name} took damage: {damage}, current health is: {CurrentHealth}");
        UpdateToDeadIfNotAlive();
    }

    private void UpdateToDeadIfNotAlive()
    {
        if (CurrentHealth <= 0)
        {
            _character.SwitchStateTo(Character.CharacterState.Dead);
        }
    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}
