using UnityEngine;

//spawns the enemies
//keeps tracks of the wave completion
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform gridPosStart;
    [SerializeField] int enemiesinWave = 4;
    [SerializeField] float xOffset = 2f;
    [SerializeField] float yOffset = 2f;
    int currentWave;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWave = 1;
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; i++)
            {
                Vector3 spawnPos = gridPosStart.position;
                spawnPos += Vector3.right * i * xOffset;
                spawnPos += Vector3.down * j * yOffset;
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
