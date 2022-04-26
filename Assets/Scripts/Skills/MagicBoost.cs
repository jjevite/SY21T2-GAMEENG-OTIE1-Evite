using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoost : Skill
{
    public override void FindRange()
    {
        throw new System.NotImplementedException();
    }
    public override void Cast()
    {
        owner.GetComponent<Unit>().GameStats.MagicDamage += 50;
    }
}
