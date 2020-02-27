using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : CardAbility
{
    public float damage = 5;
    public float aoe = 5;
    public float stunDuration = 2f;

    public override IEnumerator UseAbility() {
        DealAOEDamage(damage, card.owner.playerChar.transform.position, aoe);
        Stun(stunDuration);
        yield return null;
    }
}
