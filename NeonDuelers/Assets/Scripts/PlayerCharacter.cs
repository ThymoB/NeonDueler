using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerCharacter : MonoBehaviour {
    public Player player;
    public int maxHealth = 100;
    public float health = 100;
    public int maxEnergy = 100;
    public int energy = 100;
    public float energyRegen = 2f;
    public bool isAlive = true;
    public bool canMove = true;
    public bool isAttacking;
    public float attackRange = 2f;
    public float attackCooldown;
    public float attackDamage = 5f;
    public Transform spawnPos;
    public bool isCasting;
    public bool isStunned;
    public HealthBar healthBar;
    public CastBar castBar;
    public TextMeshProUGUI energyText;

    Enemy closestEnemy;
    NavMeshAgent nav;

    private void Awake() {
        nav = GetComponent<NavMeshAgent>();
        InitPlayer();
    }

    public void InitPlayer() {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update() {
        if (isAlive && (player.playerState==PlayerState.Farming || player.playerState == PlayerState.Dueling)) {
            MoveChar();
        }
    }

    public bool CanMove() {
        if(!canMove || isCasting || isStunned) {
            return false;
        }
        return true;

    }

    public void MoveChar() {
        if (!InRange() && CanMove()) {
            if (closestEnemy) {
                nav.isStopped = false;
                nav.SetDestination(closestEnemy.transform.position);
            }
        }
        else {
            nav.isStopped = true;
            StartAttacking();
        }

        if (nav.isStopped && closestEnemy) {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, closestEnemy.transform.position - transform.position, 10f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }


    public bool InRange() {
        closestEnemy = CombatUtils.GetClosestEnemy(this);
        if (!closestEnemy) return false;
        return Vector3.Distance(closestEnemy.transform.position, transform.position) < attackRange;
    }

    public void StartAttacking() {
        if (!isAttacking) {
            StartCoroutine(Attack(closestEnemy));
        }
    }

    IEnumerator Attack(Enemy target) {
        isAttacking = true;
        target.TakeDamage(attackDamage, DamageType.Auto, this);
        if (!target.isActive) CombatUtils.GetClosestEnemy(this);
        canMove = false;
        float cooldownLeft = attackCooldown;
        while (cooldownLeft > 0) {
            cooldownLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
        isAttacking = false;
    }

    public void HealDamage(float healing) {
        health = Mathf.Clamp(health + healing, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    public void TakeDamage(float damage, DamageType damageType) {
        health -= damage;
        healthBar.SetHealth(health);
        DamageSystem.Instance.SpawnDamageNumbers(damage, damageType, transform.position);
        if (health <= 0) Die();
    }

    public void Die() {
        GameManager.Instance.FarmingRoundEnds(player);
        gameObject.SetActive(false);
    }

    public void ResetPosition() {
        nav.isStopped = true;
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;
    }

    public IEnumerator EnergyRegen() {
        float timeLeft = 1f;
        UpdateEnergyText();
        while (player.playerState == PlayerState.Farming || player.playerState == PlayerState.Dueling) {
            while (timeLeft > 0) {
                timeLeft -= Time.deltaTime * energyRegen;
                yield return new WaitForEndOfFrame();
            }
            ChangeEnergy(1);
            timeLeft = 1f;
            yield return null;
        }
    }

    public void ChangeEnergy(int amount) {
        energy = Mathf.Clamp(energy + amount, 0, maxEnergy); 
        UpdateEnergyText();
    }

    public void UpdateEnergyText() {
        energyText.text = energy.ToString();
    }
}
