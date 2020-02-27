using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : CardAbility
{
    public TargetType targetType;
    public float healAmount;

    public override IEnumerator UseAbility() {
        HealDamage(healAmount, targetType);
        yield return null;
    }
}
