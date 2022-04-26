using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    ClassStat classStat;
    public float JumpHeight { get; set; }
    public int MovementRange { get; set; }



    public Health Health { get; set; }

    public float CurrentMP { get; set; }
    public float MaxMP { get; set; }
    public int Speed { get; set; }
    public int AttackDamage { get; set; }
    public int MagicDamage { get; set; }

    void Start()
    {
        Health = GetComponent<Health>();
        classStat = gameObject.GetComponent<Unit>().Class.Stats;

        JumpHeight = classStat.JumpHeight;
        MovementRange = classStat.MovementRange;

        Health.CurrentHP = classStat.HP;
        Health.MaxHP = classStat.HP;
        //MaxHP = classStat.HP;
        //CurrentHP = classStat.HP;
        MaxMP = classStat.MP;
        CurrentMP = classStat.MP;

        Speed = classStat.Speed;

        AttackDamage = classStat.AttackDamage;
        MagicDamage = classStat.MagicDamage;
    }
}
