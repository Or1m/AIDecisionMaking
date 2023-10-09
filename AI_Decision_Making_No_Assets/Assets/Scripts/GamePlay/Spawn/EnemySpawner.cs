using System.Linq;
using UnityEngine;

namespace GamePlay.Spawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform enemies;

        private Transform[] spawnPoints;

        public GameObject regluarEnemy;
        public GameObject mlEnemy;


        private void Awake()
        {
            spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }
        private void OnEnable() 
        {
            // Intentionally empty
        }

        public void SpawnRegularEnemies()
        {
            if (!enabled)
                return;

            SpawnEnemies(regluarEnemy);
        }
        public void SpawnMLEnemies()
        {
            if (!enabled)
                return;

            SpawnEnemies(mlEnemy);
        }

        private void SpawnEnemies(GameObject prefab)
        {
            var length = spawnPoints.Length;
            for (int i = 0; i < length; i++)
            {
                var spawnPoint = spawnPoints[i];
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, enemies);
            }
        }
    }
}
