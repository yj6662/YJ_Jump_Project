using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    public event Action onTakeDamage;

    private void Update()
    {
        if (stamina != null)
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        if (health != null)
        {
            health.Add(health.passiveValue * Time.deltaTime);
        }

        if (health.curValue <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (health != null)
        {
            health.Add(amount);
        }
    }

    public void TakeDamage(float amount)
    {
        if (health != null)
        {
            health.Subtract(amount);
        }
        onTakeDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("사망!");
        //TODO: 체크포인트에서 리스폰
    }
}

