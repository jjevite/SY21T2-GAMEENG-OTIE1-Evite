using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : Skill
{
    public override void FindRange()
    {
        throw new System.NotImplementedException();
    }
    public override void Cast()
    {
        owner.GetComponent<Unit>().GameStats.Health.CurrentHP += 50;
        owner.GetComponent<Unit>().GameStats.Health.MaxHP += 50;
    }
}
