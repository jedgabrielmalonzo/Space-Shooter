using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private GameObject enemyContainer; // Container for spawned enemies
    [SerializeField] private float spawnRate = 5f; // Time in seconds between enemy spawns
    [SerializeField] private GameObject _tripleShotPowerUpPrefab;
    [SerializeField] private GameObject _speedPowerUpPrefab;
    [SerializeField] private GameObject _shieldPowerUpPrefab;
    [SerializeField] private GameObject[] _powerups;
    private float spawnRangeX = 8f; // Range for random X spawn position
    private bool stopSpawning; // Flag to control spawning

    void Start()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnTripleShotPowerUpRoutine());
        StartCoroutine(SpawnSpeedPowerUpRoutine());
        StartCoroutine(SpawnShieldPowerUpRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (!stopSpawning) // Continue spawning until stopped
        {
            SpawnEnemy(); // Call to spawn an enemy
            yield return new WaitForSeconds(spawnRate); // Wait for the specified spawn rate
        }
    }

    private void SpawnEnemy()
    {
        // Generate a random X position for spawning
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, 7f, 0); // Spawn above the screen
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy
        newEnemy.transform.parent = enemyContainer.transform; // Parent the enemy to the container
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true; // Stop spawning when player dies
    }


    IEnumerator SpawnEnemyRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnTripleShotPowerUpRoutine()
  {
    while(!stopSpawning)
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
        Instantiate(_tripleShotPowerUpPrefab, posToSpawn, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(3,8));
    }
  }
    
    IEnumerator SpawnSpeedPowerUpRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            Instantiate(_speedPowerUpPrefab, posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f,8f));
        }
    }

    IEnumerator SpawnShieldPowerUpRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            Instantiate(_shieldPowerUpPrefab, posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f,8f));
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f),7,0);
            int randomPowerUp = Random.Range(0,2);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }
}
