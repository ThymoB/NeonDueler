using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : CardAbility {
    public float damage = 1f;
    public TargetType targetType;

    public override IEnumerator UseAbility() {
        DealDirectDamage(damage, targetType);
        yield return null;
    }
}
