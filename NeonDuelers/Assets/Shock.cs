using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : CardAbility
{
    public float damage = 4f;
    public TargetType targetType;

    public override IEnumerator UseAbility() {
        DealDirectDamage(damage, targetType);
        yield return null;
    }
}
