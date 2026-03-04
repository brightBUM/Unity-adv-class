using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform playerTransform;
    [SerializeField] float timeBWSpawns;
    [SerializeField] bool spawnActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine((SpawnEnemies()));
    }

    IEnumerator SpawnEnemies()
    {
        while(spawnActive)
        {
            yield return new WaitForSeconds(timeBWSpawns);
            var spawnPos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            var baseEnemyObject = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            baseEnemyObject.GetComponent<BaseEnemy>().Init(playerTransform);
        }

    }
}
