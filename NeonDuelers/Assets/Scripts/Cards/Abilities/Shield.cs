using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : CardAbility {
    public TargetType targetType;
    public float duration = 5f;

    public override IEnumerator UseAbility() {
        MakeInvulnerable(targetType, duration);
        yield return null;
    }
}
