using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public PlayArea playArea;
    NavMeshAgent nav;

    // Start is called before the first frame update
    private void Awake() {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy) {
            nav.SetDestination(closestEnemy.transform.position);
        }
    }

    Enemy FindClosestEnemy() {
        Enemy closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Enemy enemy in playArea.enemiesInArea) {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}
