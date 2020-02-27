using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public static PlayArea Instance;
    public BoxCollider playArea;
    public List<Enemy> enemiesInArea = new List<Enemy>();

    private void Awake() {
        if (!Instance) {
            Instance = this;
        }
    }
    void OnTriggerEnter(Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (!enemy) return;
        if (!enemiesInArea.Contains(enemy)) {
            enemiesInArea.Add(enemy);
            enemy.healthBar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemiesInArea.Contains(enemy)) {
            enemiesInArea.Remove(enemy);
            enemy.healthBar.gameObject.SetActive(false);
        }
    }
}
