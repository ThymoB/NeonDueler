using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUtils : MonoBehaviour
{
    public static Enemy GetEnemy(PlayerCharacter playerChar, TargetType targetType, float searchRange = Mathf.Infinity) {
        Enemy foundEnemy = null;
        switch (targetType) {
            case TargetType.Closest:
                foundEnemy = GetClosestEnemy(playerChar);
                break;
            case TargetType.Furthest:
                foundEnemy = GetFurthestEnemy(playerChar);
                break;
        }
        return foundEnemy;
    }


    public static Enemy GetClosestEnemy(PlayerCharacter playerChar, float searchRange = Mathf.Infinity) {
        Enemy closest = null;
        float distance = searchRange;
        Vector3 position = playerChar.transform.position;
        foreach (Enemy enemy in PlayArea.Instance.enemiesInArea) {
            if (enemy.isActive) {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance) {
                    closest = enemy;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public static Enemy GetFurthestEnemy(PlayerCharacter playerChar, float searchRange = Mathf.Infinity) { 
        Enemy furthest = null;
        float distance = 0;
        Vector3 position = playerChar.transform.position;
        foreach (Enemy enemy in PlayArea.Instance.enemiesInArea) {
            if (enemy.isActive) {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance > distance && curDistance<searchRange) {
                    furthest = enemy;
                    distance = curDistance;
                }
            }
        }
        return furthest;
    }

    public static Enemy GetRandomEnemy(PlayerCharacter playerChar, float searchRange = Mathf.Infinity) {
        Enemy random = null;
        float distance = searchRange;
        Vector3 position = playerChar.transform.position;
        foreach (Enemy enemy in PlayArea.Instance.enemiesInArea) {
            if (enemy.isActive) {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance) {
                    random = enemy;
                    distance = curDistance;
                }
            }
        }
        return random;
    }

    public static List<Enemy> GetAllEnemiesInArea(PlayerCharacter playerChar, Vector3 pos, float radius) {
        List<Enemy> foundEnemies = new List<Enemy>();
        Collider[] newCol = Physics.OverlapSphere(pos, radius, LayerMask.GetMask("Character"));
        foreach (Collider collider in newCol) {
            Enemy foundEnemy = collider.GetComponent<Enemy>();
            if (foundEnemy) foundEnemies.Add(foundEnemy);
        }
        return foundEnemies;
    }

}
