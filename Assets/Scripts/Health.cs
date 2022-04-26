using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHP;
    public float CurrentHP;

    public delegate void DeathActions();
    public event DeathActions OnDeath;

    public void TakeDamage(float damageToTake)
    {
        CurrentHP -= damageToTake;
        if(CurrentHP <= 0)
        {
            OnDeath();
        }
    }
}
