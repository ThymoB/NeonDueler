using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public bool isAlive = true;
    public bool isAttacking;
    public bool canMove = true;

    public Transform target;

    public bool InRange() {
        return Vector3.Distance(target.position, transform.position) < attackRange;
    }

    public void StartAttacking() {
        if (!isAttacking) {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        Debug.Log(name + " attacked " + target.name + "!");
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
}
