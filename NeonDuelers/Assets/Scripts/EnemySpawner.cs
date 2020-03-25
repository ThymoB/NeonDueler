using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public WaveBar waveBar;
    public static EnemySpawner Instance;
    public Transform[] spawnpoints;
    public int curWave = 1;
    public int enemiesThisWave = 20;
    public float timeBetweenWaves = 5f;
    public Enemy testEnemy;

    [Header("Cooldown")]
    public float spawnInterval = 2f;
    public float variance = 0.2f;
    public float spawnCooldown = 2f;
    public float spawnCooldownReductionOnDeath = 1f;

    int enemiesLeftThisWave = 20;
    int enemiesLeftToSpawn = 20;
    Transform playerTransform;

    private void Awake() {
        if (!Instance) {
            Instance = this;
        }
    }

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartSpawning(){
        curWave = 1;
        enemiesLeftToSpawn = enemiesThisWave;
        enemiesLeftThisWave = enemiesThisWave;
        waveBar.slider.maxValue = enemiesThisWave;
        waveBar.slider.value = enemiesLeftThisWave;
        waveBar.text.text = "Wave: " + curWave.ToString();
        StartCoroutine(SpawnEnemies(spawnInterval));
    }

    IEnumerator SpawnEnemies(float medInterval) {
        while (enemiesLeftToSpawn > 0) {
            spawnCooldown = medInterval * Random.Range(1 - variance, 1 + variance);
            SpawnEnemy(testEnemy);
            enemiesLeftToSpawn--;
            while (spawnCooldown > 0) {
                spawnCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    void SpawnEnemy(Enemy enemy) {
        Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
        Instantiate(enemy, spawnpoint.position, Quaternion.LookRotation(playerTransform.position - transform.position), transform);
    }

    public void EnemyKilled() {
        enemiesLeftThisWave--;
        spawnCooldown -= spawnCooldownReductionOnDeath;
        waveBar.slider.value = enemiesLeftThisWave;
        if (enemiesLeftThisWave <= 0) {
            WaveCleared();
        }
    }

    public void WaveCleared() {
        Debug.Log("Wave cleared!");
        StopCoroutine(SpawnEnemies(spawnInterval));
        Invoke("NewWave", timeBetweenWaves);

    }

    public void NewWave() {
        curWave++;
        if (GameManager.Instance.gameState == GameState.Fighting) {
            StartSpawning();
            Debug.Log("Spawning new Wave " + curWave);
        }
    }

}
