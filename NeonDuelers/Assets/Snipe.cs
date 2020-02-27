using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : CardAbility    
{
    public float damage = 7f;
    public TargetType targetType;

    public override IEnumerator UseAbility() {
        DealDirectDamage(damage, targetType);
        yield return null;
    }
}
