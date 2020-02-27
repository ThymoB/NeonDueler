using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : CardAbility
{
    public float damage = 10;
    public float castTime = 2f;
    public float projectileSpeed = 10f;
    public TargetType targetType;

    public override IEnumerator UseAbility() {
        while (Casting(castTime)){
            yield return null;
        }
        DealDirectDamage(damage, targetType);
        yield return null;
    }
}
