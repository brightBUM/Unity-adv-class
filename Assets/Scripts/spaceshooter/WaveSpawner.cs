using System.Collections;
using TMPro;
using UnityEngine;

//spawns the enemies
//keeps tracks of the wave completion
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject waveUIObject;
    [SerializeField] TextMeshProUGUI waveNumUI;
    [SerializeField] Transform gridPosStart;
    [SerializeField] int enemiesinWave = 4;
    [SerializeField] float xOffset = 2f;
    [SerializeField] float yOffset = 2f;
    int currentWave = 0;
    public int enemiesAlive = 0;
    public static WaveSpawner instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TriggerNewWave();

    }
    public void TriggerNewWave()
    {
        //flash wave
        StartCoroutine(WaveFlash());
        //spawn enemies
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 spawnPos = gridPosStart.position;
                spawnPos += Vector3.right * i * xOffset;
                spawnPos += Vector3.down * j * yOffset;
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemiesAlive++;
            }

        }

        
    }

    IEnumerator WaveFlash()
    {
        //set wave no.
        currentWave++;
        waveNumUI.text = "Wave "+currentWave.ToString();

        for (int i=0;i<3;i++)
        {
            waveUIObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            waveUIObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

        }
        waveUIObject.SetActive(false);

        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
