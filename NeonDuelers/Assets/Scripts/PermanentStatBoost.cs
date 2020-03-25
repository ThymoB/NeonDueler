using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat {
    AttackSpeed,
    AttackDamage,
    AttackRange,
    MaxHealth,
    MaxEnergy,
    Armor,
    MoneyPerKill,
    EnergyRegen,
    CardDrawSpeed,
    MoveSpeed
}

public enum Modifier {
    Percentage,
    Flat
}

[System.Serializable]
public class PermanentStatBoost : MonoBehaviour
{
    public Stat stat;
    public float amount;
    public Modifier modifier;

    public void ApplyBoostToPlayerChar(PlayerCharacter playerCharacter) {

        if (modifier == Modifier.Flat) {
            switch (stat) {
                case Stat.AttackSpeed:
                    playerCharacter.attackCooldown -= amount;
                    break;
                case Stat.AttackDamage:
                    playerCharacter.attackDamage += amount;
                    break;
                case Stat.AttackRange:
                    playerCharacter.attackRange += amount;
                    break;
                case Stat.MaxHealth:
                    playerCharacter.maxHealth += Mathf.RoundToInt(amount);
                    break;
                case Stat.MaxEnergy:
                    playerCharacter.maxEnergy += Mathf.RoundToInt(amount);
                    break;
                case Stat.Armor:
                    playerCharacter.armor += amount;
                    break;
                case Stat.MoneyPerKill:
                    playerCharacter.flatBonusMoneyPerKill += amount;
                    break;
                case Stat.EnergyRegen:
                    playerCharacter.energyRegen += amount;
                    break;
                case Stat.CardDrawSpeed:
                    playerCharacter.player.drawInterval -= amount;
                    break;
                case Stat.MoveSpeed:
                    playerCharacter.nav.speed += amount;
                    break;
            }
        }
        else {
            switch (stat) {
                case Stat.AttackSpeed:
                    playerCharacter.attackCooldown = playerCharacter.attackCooldown * (1-amount);
                    break;
                case Stat.AttackDamage:
                    playerCharacter.attackDamage = playerCharacter.attackDamage * (1+amount);
                    break;
                case Stat.AttackRange:
                    playerCharacter.attackRange = playerCharacter.attackRange * (1+amount);
                    break;
                case Stat.MaxHealth:
                    playerCharacter.maxHealth = Mathf.RoundToInt(playerCharacter.maxHealth * (1 + amount));
                    break;
                case Stat.MaxEnergy:
                    playerCharacter.maxEnergy = Mathf.RoundToInt(playerCharacter.maxEnergy * (1 + amount));
                    break;
                case Stat.Armor:
                    playerCharacter.armor = playerCharacter.armor * (1+amount);
                    break;
                case Stat.MoneyPerKill:
                    playerCharacter.moneyPerKillModifier += amount;
                    break;
                case Stat.EnergyRegen:
                    playerCharacter.energyRegen = playerCharacter.energyRegen * (1 - amount);
                    break;
                case Stat.CardDrawSpeed:
                    playerCharacter.player.drawInterval = playerCharacter.player.drawInterval * (1-amount);
                    break;
                case Stat.MoveSpeed:
                    playerCharacter.nav.speed = playerCharacter.nav.speed * (1+amount);
                    break;
            }
        }
    }
}
