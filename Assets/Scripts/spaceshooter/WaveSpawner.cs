using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//spawns the enemies
//keeps tracks of the wave completion
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject waveUIObject;
    [SerializeField] TextMeshProUGUI waveNumUI;
    [SerializeField] Transform gridPosStart;
    [SerializeField] int enemiesinWave = 4;
    [SerializeField] float xOffset = 2f;
    [SerializeField] float yOffset = 2f;
    [SerializeField] float bombLifeTime = 2f;

    int currentWave = 0;
    public static WaveSpawner instance;
    public List<Enemy> enemies;
    Coroutine bombDropCoroutine;
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
    private void SpawnEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 spawnPos = gridPosStart.position;
                spawnPos += Vector3.right * i * xOffset;
                spawnPos += Vector3.down * j * yOffset;

                var enemyPrefab = ObjectPoolManager.Instance.Spawn(Random.Range(4, 7), spawnPos, Quaternion.identity);
                enemies.Add(enemyPrefab.GetComponent<Enemy>());
            }

        }

        if (bombDropCoroutine != null)
        {
            StopCoroutine(bombDropCoroutine);
            bombDropCoroutine = null;
        }
        bombDropCoroutine = StartCoroutine(BombDrop());
    }

    IEnumerator BombDrop()
    {

        while (true)
        {
            var enemy = enemies[Random.Range(0, enemies.Count)];
            GameObject bombObject = ObjectPoolManager.Instance.Spawn(3, enemy.transform.position, Quaternion.identity);
            var bombRB = bombObject.GetComponent<Rigidbody2D>();


            bombRB.gravityScale = Random.Range(0.5f, 1.0f);

            ObjectPoolManager.Instance.Despawn(bombObject, bombLifeTime);
            //spawn bombs
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count<=0)
        {
            if (bombDropCoroutine != null)
            {
                StopCoroutine(bombDropCoroutine);
                bombDropCoroutine = null;
            }
            //show interstitial ad
            LevelPlaySample.instance.interstitialAd.ShowAd();
            //on ad complete , then trigger new wave
            LevelPlaySample.instance.interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;
        }
    }
    private void InterstitialAd_OnAdClosed(Unity.Services.LevelPlay.LevelPlayAdInfo obj)
    {
        LevelPlaySample.instance.interstitialAd.LoadAd();

        TriggerNewWave();
        LevelPlaySample.instance.interstitialAd.OnAdClosed -= InterstitialAd_OnAdClosed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
