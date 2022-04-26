using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoost : Skill
{
    public override void FindRange()
    {
        throw new System.NotImplementedException();
    }
    public override void Cast()
    {
        owner.GetComponent<Unit>().GameStats.AttackDamage += 25;
    }
}
