using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject destroyVFX;
    [SerializeField] float vfxDestroyDelay;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinLifeTime =2f;
    bool destroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (!destroyed)
            {
                //destroy both
                Destroy(collision.gameObject);
                Destroy(this.gameObject);

                //spawn a collectible
                GameObject coinObject = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(coinObject, coinLifeTime);

                //spawn & destroy vfx
                GameObject vfxObject = Instantiate(destroyVFX, transform.position, Quaternion.identity);
                Destroy(vfxObject, vfxDestroyDelay);

                WaveSpawner.instance.enemiesAlive--;
                if (WaveSpawner.instance.enemiesAlive <= 0)
                {
                    WaveSpawner.instance.TriggerNewWave();
                }
                Debug.Log("enemiesAlive : " + WaveSpawner.instance.enemiesAlive);
                destroyed = true;
            }

        }
    }
    
}
