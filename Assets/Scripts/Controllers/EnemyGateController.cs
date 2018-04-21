using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class EnemyGateController : MonoBehaviour
    {
        // Properties
        // ---------------------------------------

        [SerializeField] private bool isActive = false; // Enemies appear only if the gate is active.
        [SerializeField] private float spawnInterval = 3.0f; // Number of seconds between enemies.
        [SerializeField] private float spawnDirection = 1.0f; // 1.0f = right, -1.0f = left.
        [SerializeField] private GameObject enemyModel;

        private float lastSpawnTime;
        private List<GameObject> spawnedEnemies = new List<GameObject>();
        
        // Private methods
        // ---------------------------------------
        
        private void Update()
        {
            if (GameController.instance.isPaused)
            {
                return;
            }
            
            if (!isActive || enemyModel == null)
            {
                return;
            }

            if (Time.time - lastSpawnTime > spawnInterval)
            {
                lastSpawnTime = Time.time;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            GameObject enemy = Instantiate(enemyModel, transform);
            enemy.transform.position = transform.position;
            
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.direction = spawnDirection;
            }
            
            spawnedEnemies.Add(enemy);
        }
    }
}