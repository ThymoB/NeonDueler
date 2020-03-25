using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 10;
    public float health = 10f;
    public int maxEnergy = 5;
    public float energy = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float attackDamage = 1f;
    public bool isActive = true;
    public bool isAttacking;
    public bool canMove = true;
    public float moveSpeed = 2f;
    public int bounty = 5;
    public HealthBar healthBar;
    public NavMeshAgent nav;

    PlayerCharacter playerChar;

    private void Awake() {
        playerChar = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = attackRange;
        nav.speed = moveSpeed;
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private void Update() {
        if (isActive) {
            if (!InRange() && canMove) {
                //StopCoroutine(Attack());
                nav.SetDestination(playerChar.transform.position);
            }
            else {
                StartAttacking();
            }
        }

        if (nav.isStopped) {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, playerChar.transform.position - transform.position, 1f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public bool InRange() {
        return Vector3.Distance(playerChar.transform.position, transform.position) < attackRange;
    }

    public void StartAttacking() {
        if (!isAttacking) {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        playerChar.TakeDamage(attackDamage, DamageType.Auto);
        isAttacking = true;
        canMove = false;
        float cooldownLeft = attackCooldown;
        while (cooldownLeft > 0) {
            cooldownLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
        isAttacking = false;
    }

    public void HealDamage(float healing, PlayerCharacter healer) {
        health = Mathf.Clamp(health + healing, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    public void TakeDamage(float damage, DamageType damageType, PlayerCharacter attacker) {
        health -= damage;
        healthBar.SetHealth(health);
        DamageSystem.Instance.SpawnDamageNumbers(damage, damageType, transform.position);
        if (health <= 0) Die(attacker);
    }

    public void Die(PlayerCharacter killer) {
        isActive = false;
        bounty = Mathf.RoundToInt(killer.flatBonusMoneyPerKill * killer.moneyPerKillModifier);
        killer.player.UpdateMoney(bounty);
        PlayArea.Instance.enemiesInArea.Remove(this);
        EnemySpawner.Instance.EnemyKilled();
        Destroy(gameObject, 2f);
        gameObject.SetActive(false);
    }
}
