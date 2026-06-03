using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject spawnVFX;
    [SerializeField] Transform playerTransform;
    [SerializeField] float timeBWSpawns;
    [SerializeField] bool spawnActive;
    int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine((SpawnEnemies()));
    }
    float radius = 10.0f;
    IEnumerator SpawnEnemies()
    {
        int c = 0;
        while(spawnActive)
        {
            yield return new WaitForSeconds(timeBWSpawns);

            bool spawnedOnNavMesh = false;
            for (int i = 0; i < 10; i++) // try multiple times
            {
                Vector2 randomCircle = Random.insideUnitCircle * radius;

                Vector3 randomPos = new Vector3(
                    playerTransform.position.x + randomCircle.x,
                    playerTransform.position.y,
                    playerTransform.position.z + randomCircle.y
                );

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
                {
                    var spawnPos = hit.position + Vector3.down * 2f;

                    Instantiate(spawnVFX, hit.position, spawnVFX.transform.rotation);

                    var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    enemy.GetComponent<BaseEnemy>().Init(playerTransform, hit.position);
                    spawnedOnNavMesh |= true;
                    break;
                }
            }

            if(!spawnedOnNavMesh)
            {
                Debug.LogWarning("couldnt find navmesh point , spawnin on player instead");
                // fallback (in case all attempts fail)
                Instantiate(spawnVFX, playerTransform.position, Quaternion.identity);

                var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                var enemy2 = Instantiate(enemyPrefab, playerTransform.position - Vector3.down * 2f, Quaternion.identity);
                enemy2.GetComponent<BaseEnemy>().Init(playerTransform, playerTransform.position);
            }
            

            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            //{
            //    var spawnPos = hit.position + Vector3.down * 2f;

            //    Instantiate(spawnVFX, hit.position, Quaternion.identity);

            //    var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            //    enemy.GetComponent<BaseEnemy>().Init(playerTransform, hit.position);
            //    c++;
            //}
            //else
            //{
            //    Debug.LogError("No samplePoint on NavMesh");
            //}

            
        }

    }

    public void SpawnOnTrigger()
    {
        Debug.Log("spawn enemy triggered");
        var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var spawnTransform = spawnPoints[count];
        Instantiate(spawnVFX, spawnTransform.position, spawnVFX.transform.rotation);
        var enemy = Instantiate(enemyPrefab, spawnTransform.position+ Vector3.down * 2f, Quaternion.identity);
        enemy.GetComponent<BaseEnemy>().Init(playerTransform, spawnTransform.position);
        count++;
    }
}
