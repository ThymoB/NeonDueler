using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType { Self, Closest, Furthest, Random }

public abstract class CardAbility : MonoBehaviour
{
    public abstract IEnumerator UseAbility();
    public ActionCard card;

    private bool isCasting;
    private float castTimeLeft;

    public void FireProjectile(Rigidbody projectile, float speed, TargetType targetType, out bool hit, out Collider hitTarget) {
        hit = false;
        hitTarget = null;
    }

    public void DealAOEDamage(float damage, Vector3 startPoint, float radius) {
        foreach (Enemy enemy in CombatUtils.GetAllEnemiesInArea(card.owner.playerChar, startPoint, radius)) {
            DealDirectDamage(damage, enemy);
        }
    }

    public void DealDirectDamage(float damage, TargetType targetType) {
        if (targetType != TargetType.Self) {
            Enemy target = CombatUtils.GetEnemy(card.owner.playerChar, targetType);
            if(target) target.TakeDamage(damage, DamageType.Energy, card.owner.playerChar);
        }
        else
            card.owner.playerChar.TakeDamage(damage, DamageType.Energy);
    }

    public void DealDirectDamage(float damage, Enemy enemy) {
        enemy.TakeDamage(damage, DamageType.Energy, card.owner.playerChar);
    }

    public void HealDamage(float healing, TargetType targetType) {
        if (targetType != TargetType.Self) {
            Enemy target = CombatUtils.GetEnemy(card.owner.playerChar, targetType);
            if (target) target.HealDamage(healing, card.owner.playerChar);
        }
        else
            card.owner.playerChar.HealDamage(healing);
    }

    public void Stun(float duration) {


    }

    public bool Casting(float time) {
        if (!isCasting) {
            castTimeLeft = time;
            isCasting = true;
            card.owner.playerChar.castBar.SetCastTime(time);
        }
        card.owner.playerChar.castBar.CastProgress(castTimeLeft);
        castTimeLeft -= Time.deltaTime;
        if (castTimeLeft <= 0) {
            isCasting = false;
        }
        card.owner.playerChar.isCasting = isCasting;
        card.owner.playerChar.castBar.gameObject.SetActive(isCasting);
        return isCasting;
    }

    public void MakeInvulnerable(TargetType targetType, float time) {

    }
}
